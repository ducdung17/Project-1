using System;
using UnityEngine;

namespace NongTrai.Profiles
{
    /// <summary>
    /// Lưu hồ sơ dạng JSON trong PlayerPrefs.
    /// Chạy được trên cả PC (registry / file plist) lẫn WebGL (IndexedDB của trình duyệt),
    /// nên không cần viết code riêng cho từng nền tảng.
    /// </summary>
    public class PlayerPrefsProfileStorage : IProfileStorage
    {
        // Có số phiên bản trong key: nếu sau này đổi cấu trúc dữ liệu thì chuyển sang v2 và viết code chuyển đổi.
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
                // Dữ liệu hỏng thì không để game crash: báo lỗi và bắt đầu lại từ đầu.
                Debug.LogWarning($"[Profiles] Dữ liệu hồ sơ bị hỏng, sẽ tạo mới. Lỗi: {e.Message}");
                return null;
            }
        }

        public void Save(ProfileData data)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
            // Bắt buộc gọi Save() để WebGL ghi xuống IndexedDB ngay, tránh mất dữ liệu khi đóng tab.
            PlayerPrefs.Save();
        }
    }
}
