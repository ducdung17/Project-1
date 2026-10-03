using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace NongTrai.Online
{
    public class ProgressUploader : MonoBehaviour
    {
        const string QueueKey = "nongtrai.uploadqueue.v1";
        const int MaxQueued = 100;

        [Serializable]
        class Queue { public List<string> items = new List<string>(); }

        static ProgressUploader instance;

        ServerSettings settings;
        Queue queue;
        bool sending;
        float nextRetry;

        public static int PendingCount => instance != null ? instance.queue.items.Count : 0;

        public static string LastStatus { get; private set; } = "chưa gửi";

        public static void Enqueue(SessionPayload payload)
        {
            ProgressUploader uploader = Ensure();
            if (uploader == null) return;
            uploader.queue.items.Add(JsonUtility.ToJson(payload));
            int oldest = uploader.sending ? 1 : 0;
            while (uploader.queue.items.Count > MaxQueued) uploader.queue.items.RemoveAt(oldest);
            uploader.Save();
            uploader.TrySend();
        }

        static ProgressUploader Ensure()
        {
            if (instance != null) return instance;
            ServerSettings s = ServerSettings.Load();
            if (s == null || !s.enabled || string.IsNullOrEmpty(s.baseUrl)) return null;

            var go = new GameObject("ProgressUploader");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<ProgressUploader>();
            instance.settings = s;
            instance.Load();
            return instance;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void AutoStart()
        {
            ProgressUploader uploader = Ensure();
            if (uploader != null && uploader.queue.items.Count > 0) uploader.TrySend();
        }

        void Update()
        {
            if (!sending && queue.items.Count > 0 && Time.realtimeSinceStartup >= nextRetry)
                TrySend();
        }

        void TrySend()
        {
            if (!sending && queue.items.Count > 0) StartCoroutine(SendAll());
        }

        IEnumerator SendAll()
        {
            sending = true;
            string url = settings.baseUrl.TrimEnd('/') + "/api/sessions";

            while (queue.items.Count > 0)
            {
                string json = queue.items[0];
                using (var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
                {
                    req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                    req.downloadHandler = new DownloadHandlerBuffer();
                    req.SetRequestHeader("Content-Type", "application/json");
                    if (!string.IsNullOrEmpty(settings.apiKey)) req.SetRequestHeader("X-Api-Key", settings.apiKey);
                    req.timeout = settings.timeoutSeconds;

                    UnityWebRequestAsyncOperation op;
                    try
                    {
                        op = req.SendWebRequest();
                    }
                    catch (InvalidOperationException e)
                    {
                        LastStatus = "Lỗi: " + e.Message;
                        Debug.LogWarning("[ProgressUploader] " + e.Message +
                            " -> Project Settings > Player > Allow downloads over HTTP = Always allowed.");
                        break;
                    }
                    yield return op;

                    long code = req.responseCode;
                    if (req.result == UnityWebRequest.Result.Success)
                    {
                        LastStatus = "OK " + code;
                        queue.items.RemoveAt(0);
                        Save();
                    }
                    else if (code == 400)
                    {
                        LastStatus = "Bỏ lượt lỗi dữ liệu (400)";
                        Debug.LogWarning("[ProgressUploader] Server từ chối dữ liệu: " + req.downloadHandler.text + "\n" + json);
                        queue.items.RemoveAt(0);
                        Save();
                    }
                    else
                    {
                        LastStatus = "Lỗi: " + (code > 0 ? code.ToString() : req.error);
                        Debug.Log($"[ProgressUploader] Chưa gửi được ({LastStatus}), còn {queue.items.Count} lượt chờ.");
                        break;
                    }
                }
            }

            nextRetry = Time.realtimeSinceStartup + settings.retryEverySeconds;
            sending = false;
        }

        void Load()
        {
            string json = PlayerPrefs.GetString(QueueKey, "");
            queue = string.IsNullOrEmpty(json) ? new Queue() : (JsonUtility.FromJson<Queue>(json) ?? new Queue());
        }

        void Save()
        {
            PlayerPrefs.SetString(QueueKey, JsonUtility.ToJson(queue));
            PlayerPrefs.Save();
        }
    }
}
