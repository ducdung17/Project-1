using System.Linq;

namespace NongTrai.Learning
{
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
                progress.recent.Clear();
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
