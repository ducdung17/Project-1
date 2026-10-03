using UnityEngine;

namespace NongTrai.Animals
{
    public enum Habitat
    {
        Farm,
        Home,
        Forest,
        Water
    }

    [CreateAssetMenu(fileName = "Animal_", menuName = "NongTrai/Animal Data", order = 0)]
    public class AnimalData : ScriptableObject
    {
        [Header("Định danh")]
        [Tooltip("Mã cố định, chữ thường không dấu, ví dụ: cat. Tiến độ học của bé được lưu theo mã này, " +
                 "nên KHÔNG đổi mã sau khi đã có dữ liệu.")]
        [SerializeField] string id;
        [SerializeField] string nameVi;
        [SerializeField] string nameEn;

        [Header("Hình ảnh")]
        [Tooltip("PNG nền trong suốt.")]
        [SerializeField] Sprite sprite;

        [Header("Âm thanh")]
        [Tooltip("Tiếng kêu của con vật (meo meo, gâu gâu...).")]
        [SerializeField] AudioClip sound;
        [Tooltip("Giọng đọc tên, ví dụ \"Con mèo\".")]
        [SerializeField] AudioClip nameVoice;

        [Header("Kiến thức")]
        [SerializeField] FoodData food;
        [SerializeField] Habitat habitat = Habitat.Farm;
        [Tooltip("Nhóm các con dễ nhầm với nhau, ví dụ gia-cam cho gà và vịt. " +
                 "Ở mức khó, đáp án gây nhiễu được lấy trong cùng nhóm. Để trống nếu không có.")]
        [SerializeField] string similarGroup;

        public string Id => id;
        public string NameVi => nameVi;
        public string NameEn => nameEn;
        public Sprite Sprite => sprite;
        public AudioClip Sound => sound;
        public AudioClip NameVoice => nameVoice;
        public FoodData Food => food;
        public Habitat Habitat => habitat;
        public string SimilarGroup => similarGroup;

        public string Label => string.IsNullOrEmpty(nameVi) ? (string.IsNullOrEmpty(id) ? name : id) : $"{nameVi} ({id})";

        internal void InitForTests(string animalId, string vi, string group = null, FoodData foodData = null)
        {
            id = animalId;
            nameVi = vi;
            similarGroup = group;
            food = foodData;
        }
    }
}
