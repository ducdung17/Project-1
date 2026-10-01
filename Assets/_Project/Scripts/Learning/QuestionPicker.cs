using System;
using System.Collections.Generic;
using System.Linq;

namespace NongTrai.Learning
{
    /// <summary>
    /// Chọn câu hỏi và đáp án gây nhiễu. Làm việc trên Id (chuỗi) nên test được không cần Unity.
    /// </summary>
    public static class QuestionPicker
    {
        /// <summary>Trọng số tối thiểu: con đã thuộc vẫn thỉnh thoảng được hỏi lại để ôn.</summary>
        public const double MinWeight = 0.1;

        /// <summary>
        /// Chọn con vật để hỏi bằng random có trọng số: trọng số = (1 - điểm thuộc) + 0.1.
        /// Con chưa thuộc (điểm 0) có trọng số 1.1, con đã thuộc hẳn (điểm 1) chỉ 0.1, tức ít gặp hơn 11 lần.
        /// Không hỏi lại đúng con vừa hỏi (trừ khi chỉ có 1 con).
        /// </summary>
        public static string PickTarget(IReadOnlyList<string> ids, Func<string, float> scoreOf, string lastId, Random rng)
        {
            if (ids == null || ids.Count == 0)
                throw new ArgumentException("Cần ít nhất 1 con vật.", nameof(ids));

            List<string> candidates = ids.Count == 1 ? ids.ToList() : ids.Where(id => id != lastId).ToList();
            double[] weights = candidates.Select(id => 1.0 - Clamp01(scoreOf(id)) + MinWeight).ToArray();

            double roll = rng.NextDouble() * weights.Sum();
            for (int i = 0; i < candidates.Count; i++)
            {
                roll -= weights[i];
                if (roll < 0)
                    return candidates[i];
            }
            return candidates[candidates.Count - 1];
        }

        /// <summary>
        /// Chọn đáp án gây nhiễu (không trùng nhau, không gồm con đúng).
        /// preferSimilar = true: lấy các con cùng nhóm "dễ nhầm" trước (gà thì nhiễu bằng vịt).
        /// isAllowed: loại bớt con không hợp lệ (ví dụ trò "Cho bạn ăn": bỏ con ăn cùng thức ăn với con đúng).
        /// </summary>
        public static List<string> PickDistractors(
            string targetId,
            IReadOnlyList<string> ids,
            int count,
            Func<string, string> groupOf,
            bool preferSimilar,
            Random rng,
            Func<string, bool> isAllowed = null)
        {
            List<string> pool = ids
                .Where(id => id != targetId && (isAllowed == null || isAllowed(id)))
                .Distinct()
                .ToList();
            count = Math.Max(0, Math.Min(count, pool.Count));

            var result = new List<string>();
            if (preferSimilar)
            {
                string group = groupOf(targetId);
                if (!string.IsNullOrEmpty(group))
                {
                    foreach (string id in Shuffle(pool.Where(id => groupOf(id) == group), rng))
                    {
                        if (result.Count >= count) break;
                        result.Add(id);
                    }
                }
            }

            foreach (string id in Shuffle(pool, rng))
            {
                if (result.Count >= count) break;
                if (!result.Contains(id)) result.Add(id);
            }
            return result;
        }

        /// <summary>Xáo trộn Fisher–Yates, trả về danh sách mới.</summary>
        public static List<T> Shuffle<T>(IEnumerable<T> source, Random rng)
        {
            List<T> list = source.ToList();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                T tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
            return list;
        }

        static double Clamp01(float v) => Math.Max(0.0, Math.Min(1.0, v));
    }
}
