using System.Linq;

namespace NongTrai.Learning
{
    /// <summary>
    /// Tự điều chỉnh độ khó theo 6 câu gần nhất:
    /// - Đúng ngay từ lần đầu ≥ 80% (5/6 câu) -> lên mức.
    /// - ≤ 50% (3/6 câu trở xuống) -> xuống mức.
    /// Mức 1: 2 lựa chọn. Mức 2: 3 lựa chọn. Mức 3: 4 lựa chọn, đáp án nhiễu là con vật dễ nhầm.
    /// </summary>
    public static class Difficulty
    {
        public const int MinLevel = 1;
        public const int MaxLevel = 3;
        public const int Window = 6;
        public const float LevelUpAccuracy = 0.8f;
        public const float LevelDownAccuracy = 0.5f;

        public static int OptionCount(int level) => Clamp(level) + 1;

        public static bool UseSimilarDistractors(int level) => Clamp(level) >= MaxLevel;

        public static int Clamp(int level) => level < MinLevel ? MinLevel : (level > MaxLevel ? MaxLevel : level);

        /// <summary>Ghi nhận một câu trả lời (chỉ lần chọn đầu tiên) và đổi mức nếu cần. Trả về true nếu mức thay đổi.</summary>
        public static bool Register(LearningProgress progress, bool correctFirstTry)
        {
            progress.level = Clamp(progress.level);
            progress.recent.Add(correctFirstTry);
            while (progress.recent.Count > Window)
                progress.recent.RemoveAt(0);

            if (progress.recent.Count < Window)
                return false;

            float accuracy = progress.recent.Count(r => r) / (float)Window;
            if (accuracy >= LevelUpAccuracy && progress.level < MaxLevel)
            {
                progress.level++;
                progress.recent.Clear(); // bắt đầu đếm lại ở mức mới
                return true;
            }
            if (accuracy <= LevelDownAccuracy && progress.level > MinLevel)
            {
                progress.level--;
                progress.recent.Clear();
                return true;
            }
            return false;
        }
    }
}
