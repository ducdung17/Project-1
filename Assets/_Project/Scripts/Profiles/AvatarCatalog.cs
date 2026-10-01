using System;
using System.Collections.Generic;
using UnityEngine;

namespace NongTrai.Profiles
{
    /// <summary>
    /// Danh sách con vật dùng làm avatar hồ sơ.
    /// Tạo bằng: chuột phải trong Project > Create > NongTrai > Avatar Catalog.
    /// LƯU Ý: avatarId chính là vị trí trong danh sách, nên chỉ THÊM vào cuối,
    /// không xóa hay đổi thứ tự, nếu không hồ sơ cũ sẽ hiển thị sai con vật.
    /// </summary>
    [CreateAssetMenu(fileName = "AvatarCatalog", menuName = "NongTrai/Avatar Catalog")]
    public class AvatarCatalog : ScriptableObject
    {
        [Serializable]
        public class AvatarEntry
        {
            public string displayName = "Con vật";
            public Sprite sprite;
            public Color cardColor = Color.white;
            [Tooltip("Tiếng kêu phát ra khi bé bấm vào thẻ (không bắt buộc).")]
            public AudioClip sound;
        }

        [SerializeField] List<AvatarEntry> avatars = new List<AvatarEntry>();

        public int Count => avatars.Count;

        public AvatarEntry Get(int avatarId)
        {
            return avatarId >= 0 && avatarId < avatars.Count ? avatars[avatarId] : null;
        }
    }
}
