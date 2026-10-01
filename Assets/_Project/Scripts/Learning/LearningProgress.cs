using System;
using System.Collections.Generic;

namespace NongTrai.Learning
{
    /// <summary>Mức độ thuộc bài của MỘT bé với MỘT con vật.</summary>
    [Serializable]
    public class MasteryRecord
    {
        public string animalId;
        /// <summary>0 = chưa biết gì, 1 = thuộc hoàn toàn.</summary>
        public float score;
        public int attempts;
        public int correct;
        public long lastSeenTicks;
    }

    /// <summary>Toàn bộ tiến độ học của một bé. Lưu dạng JSON, mỗi bé một bản.</summary>
    [Serializable]
    public class LearningProgress
    {
        public string childId;
        public int level = Difficulty.MinLevel;
        /// <summary>Kết quả các câu gần nhất (đúng ngay lần đầu = true), dùng để chỉnh độ khó.</summary>
        public List<bool> recent = new List<bool>();
        public List<MasteryRecord> records = new List<MasteryRecord>();
        public int totalAnswers;

        public MasteryRecord Find(string animalId)
        {
            foreach (MasteryRecord r in records)
                if (r.animalId == animalId)
                    return r;
            return null;
        }

        public MasteryRecord GetOrCreate(string animalId)
        {
            MasteryRecord r = Find(animalId);
            if (r != null) return r;
            r = new MasteryRecord { animalId = animalId };
            records.Add(r);
            return r;
        }
    }
}
