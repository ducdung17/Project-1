using System;
using System.Collections;
using NongTrai.Profiles;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NongTrai.MainMenu
{
    public class MainMenuScreen : MonoBehaviour
    {
        [Serializable]
        public class GameEntry
        {
            [Tooltip("Ô trò chơi.")]
            public Button button;
            [Tooltip("Tên scene của trò chơi, ví dụ GameSound. Để trống nếu chưa làm.")]
            public string sceneName;
        }

        [Header("Bé đang chơi")]
        [SerializeField] Image childAvatarBackground;
        [SerializeField] Image childAvatar;
        [SerializeField] TMP_Text childName;

        [Header("Trò chơi")]
        [SerializeField] GameEntry[] games = new GameEntry[0];

        [Header("Nút khác")]
        [SerializeField] Button albumButton;
        [SerializeField] string albumSceneName = "Album";
        [SerializeField] Button switchProfileButton;
        [SerializeField] string profileSceneName = "profile";

        [Header("Thông báo")]
        [Tooltip("Ô chữ nhỏ hiện 2 giây khi bấm vào trò chơi chưa có. Để TẮT sẵn trong scene.")]
        [SerializeField] GameObject toast;
        [SerializeField] TMP_Text toastText;
        [SerializeField] float toastSeconds = 2f;

        const string ComingSoonMessage = "Trò chơi này sắp có rồi, bé chờ nhé!";

        Coroutine toastRoutine;

        void Start()
        {
            ChildProfile child = ProfileManager.Current;
            if (child == null)
            {
                Debug.Log("[MainMenu] Chưa chọn bé, chuyển về màn chọn hồ sơ.");
                LoadProfileScene();
                return;
            }

            ShowChild(child);

            foreach (GameEntry entry in games)
            {
                if (entry.button == null) continue;
                string scene = entry.sceneName;
                entry.button.onClick.AddListener(() => OpenScene(scene));
            }

            if (albumButton != null)
                albumButton.onClick.AddListener(() => OpenScene(albumSceneName));
            if (switchProfileButton != null)
                switchProfileButton.onClick.AddListener(LoadProfileScene);

            if (toast != null)
                toast.SetActive(false);
        }

        void ShowChild(ChildProfile child)
        {
            childName.text = child.Nickname;

            AvatarCatalog.AvatarEntry avatar = ProfileManager.Instance.Avatars.Get(child.AvatarId);
            if (avatar == null) return;
            childAvatar.sprite = avatar.sprite;
            if (childAvatarBackground != null)
                childAvatarBackground.color = avatar.cardColor;
        }

        void LoadProfileScene()
        {
            if (!Application.CanStreamedLevelBeLoaded(profileSceneName))
            {
                Debug.LogError($"[MainMenu] Không tìm thấy scene \"{profileSceneName}\" trong Build Profiles. " +
                               "Kiểm tra ô Profile Scene Name trên MainMenuScreen.", this);
                return;
            }

            SceneManager.LoadScene(profileSceneName);
        }

        void OpenScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName) || !Application.CanStreamedLevelBeLoaded(sceneName))
            {
                ShowToast(ComingSoonMessage);
                return;
            }

            SceneManager.LoadScene(sceneName);
        }

        void ShowToast(string message)
        {
            if (toast == null) return;
            if (toastRoutine != null) StopCoroutine(toastRoutine);
            toastRoutine = StartCoroutine(ToastRoutine(message));
        }

        IEnumerator ToastRoutine(string message)
        {
            toastText.text = message;
            toast.SetActive(true);
            yield return new WaitForSecondsRealtime(toastSeconds);
            toast.SetActive(false);
            toastRoutine = null;
        }
    }
}
