using System;

namespace NongTrai.Learning
{
    public static class MasteryModel
    {
        public const float FastSeconds = 4f;
        public const float GainFast = 0.35f;
        public const float GainSlow = 0.20f;
        public const float LossFactor = 0.40f;
        public const float MasteredThreshold = 0.8f;

        public static float Update(float score, bool correct, float responseSeconds)
        {
            score = Clamp01(score);
            if (correct)
            {
                float gain = responseSeconds <= FastSeconds ? GainFast : GainSlow;
                return Clamp01(score + (1f - score) * gain);
            }
            return Clamp01(score * (1f - LossFactor));
        }

        public static bool IsMastered(float score) => score >= MasteredThreshold;

        static float Clamp01(float v) => Math.Max(0f, Math.Min(1f, v));
    }
}
