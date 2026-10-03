using System;
using System.Collections.Generic;

namespace NongTrai.Learning
{
    [Serializable]
    public class MasteryRecord
    {
        public string animalId;
        public float score;
        public int attempts;
        public int correct;
        public long lastSeenTicks;
    }

    [Serializable]
    public class LearningProgress
    {
        public string childId;
        public int level = Difficulty.MinLevel;
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
