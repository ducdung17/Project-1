using System;
using UnityEngine;

namespace NongTrai.Profiles
{
    public class PlayerPrefsProfileStorage : IProfileStorage
    {
        public const string DefaultKey = "nongtrai.profiles.v1";

        readonly string key;

        public PlayerPrefsProfileStorage(string key = DefaultKey)
        {
            this.key = key;
        }

        public ProfileData Load()
        {
            if (!PlayerPrefs.HasKey(key))
                return null;

            string json = PlayerPrefs.GetString(key);
            try
            {
                return JsonUtility.FromJson<ProfileData>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Profiles] Dữ liệu hồ sơ bị hỏng, sẽ tạo mới. Lỗi: {e.Message}");
                return null;
            }
        }

        public void Save(ProfileData data)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }
    }
}
