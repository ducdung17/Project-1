using System;
using UnityEngine;

namespace NongTrai.Learning
{
    /// <summary>Lưu tiến độ học của từng bé dạng JSON trong PlayerPrefs (chạy được cả PC và WebGL).</summary>
    public class PlayerPrefsProgressStorage : IProgressStorage
    {
        public const string KeyPrefix = "nongtrai.progress.v1.";

        static string Key(string childId) => KeyPrefix + childId;

        public LearningProgress Load(string childId)
        {
            string key = Key(childId);
            if (!PlayerPrefs.HasKey(key))
                return null;
            try
            {
                return JsonUtility.FromJson<LearningProgress>(PlayerPrefs.GetString(key));
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Learning] Tiến độ của bé {childId} bị hỏng, sẽ tạo mới. Lỗi: {e.Message}");
                return null;
            }
        }

        public void Save(LearningProgress progress)
        {
            PlayerPrefs.SetString(Key(progress.childId), JsonUtility.ToJson(progress));
            PlayerPrefs.Save();
        }

        public void Delete(string childId)
        {
            PlayerPrefs.DeleteKey(Key(childId));
            PlayerPrefs.Save();
        }
    }
}
