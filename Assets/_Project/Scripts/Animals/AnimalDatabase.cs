using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NongTrai.Animals
{
    /// <summary>
    /// Danh sách tất cả con vật trong game. Các trò chơi chỉ cần giữ tham chiếu tới file này.
    /// Tạo bằng: chuột phải > Create > NongTrai > Animal Database.
    /// Kiểm tra dữ liệu: bấm dấu ⋮ ở góc phải component trong Inspector > "Kiểm tra dữ liệu".
    /// </summary>
    [CreateAssetMenu(fileName = "AnimalDatabase", menuName = "NongTrai/Animal Database", order = 2)]
    public class AnimalDatabase : ScriptableObject
    {
        static readonly Regex IdPattern = new Regex("^[a-z0-9]+(-[a-z0-9]+)*$");

        [SerializeField] List<AnimalData> animals = new List<AnimalData>();

        public IReadOnlyList<AnimalData> All => animals;
        public int Count => animals.Count;

        /// <summary>Tìm con vật theo mã. Trả về null nếu không có.</summary>
        public AnimalData Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            return animals.FirstOrDefault(a => a != null && a.Id == id);
        }

        /// <summary>Các con cùng nhóm "dễ nhầm" (không gồm chính nó). Rỗng nếu con này không thuộc nhóm nào.</summary>
        public IReadOnlyList<AnimalData> GetSimilar(AnimalData animal)
        {
            if (animal == null || string.IsNullOrEmpty(animal.SimilarGroup))
                return new List<AnimalData>();

            return animals
                .Where(a => a != null && a != animal && a.SimilarGroup == animal.SimilarGroup)
                .ToList();
        }

        /// <summary>
        /// Kiểm tra dữ liệu, trả về danh sách vấn đề (rỗng = dữ liệu tốt).
        /// Có unit test gọi hàm này trên database thật, nên nhập thiếu gì test sẽ báo.
        /// </summary>
        public List<string> Validate()
        {
            var problems = new List<string>();
            var seenIds = new HashSet<string>();

            for (int i = 0; i < animals.Count; i++)
            {
                AnimalData a = animals[i];
                if (a == null)
                {
                    problems.Add($"Ô số {i} trong danh sách đang trống.");
                    continue;
                }

                if (string.IsNullOrEmpty(a.Id))
                    problems.Add($"Ô số {i} ({a.name}): chưa có Id.");
                else if (!IdPattern.IsMatch(a.Id))
                    problems.Add($"Id \"{a.Id}\" không hợp lệ: chỉ dùng chữ thường không dấu, số và dấu gạch ngang.");
                else if (!seenIds.Add(a.Id))
                    problems.Add($"Id \"{a.Id}\" bị trùng.");

                if (string.IsNullOrWhiteSpace(a.NameVi)) problems.Add($"{a.Label}: thiếu tên tiếng Việt.");
                if (a.Sprite == null) problems.Add($"{a.Label}: thiếu hình.");
                if (a.Sound == null) problems.Add($"{a.Label}: thiếu tiếng kêu.");
                if (a.NameVoice == null) problems.Add($"{a.Label}: thiếu giọng đọc tên.");
                if (a.Food == null) problems.Add($"{a.Label}: chưa chọn thức ăn.");
            }

            // Một nhóm "dễ nhầm" chỉ có 1 con thì vô nghĩa, thường là do gõ sai tên nhóm.
            IEnumerable<IGrouping<string, AnimalData>> lonelyGroups = animals
                .Where(a => a != null && !string.IsNullOrEmpty(a.SimilarGroup))
                .GroupBy(a => a.SimilarGroup)
                .Where(g => g.Count() == 1);
            foreach (IGrouping<string, AnimalData> g in lonelyGroups)
                problems.Add($"Nhóm \"{g.Key}\" chỉ có 1 con ({g.First().Label}), cần ít nhất 2. Kiểm tra lỗi chính tả.");

            return problems;
        }

        [ContextMenu("Kiểm tra dữ liệu")]
        void ValidateFromMenu()
        {
            List<string> problems = Validate();
            if (problems.Count == 0)
            {
                Debug.Log($"[AnimalDatabase] Dữ liệu tốt: {Count} con vật, không có lỗi.", this);
                return;
            }

            foreach (string p in problems)
                Debug.LogWarning($"[AnimalDatabase] {p}", this);
        }

        internal void InitForTests(List<AnimalData> list)
        {
            animals = list;
        }
    }
}
