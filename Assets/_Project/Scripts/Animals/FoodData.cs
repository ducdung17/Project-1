using UnityEngine;

namespace NongTrai.Animals
{
    [CreateAssetMenu(fileName = "Food_", menuName = "NongTrai/Food Data", order = 1)]
    public class FoodData : ScriptableObject
    {
        [Tooltip("Mã cố định, chữ thường không dấu. Ví dụ: carrot")]
        [SerializeField] string id;
        [Tooltip("Tên hiển thị. Ví dụ: Cà rốt")]
        [SerializeField] string displayName;
        [SerializeField] Sprite sprite;
        [Tooltip("Giọng đọc tên thức ăn (không bắt buộc).")]
        [SerializeField] AudioClip nameVoice;

        public string Id => id;
        public string DisplayName => displayName;
        public Sprite Sprite => sprite;
        public AudioClip NameVoice => nameVoice;

        internal void InitForTests(string foodId, string name)
        {
            id = foodId;
            displayName = name;
        }
    }
}
