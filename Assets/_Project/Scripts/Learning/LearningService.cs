using System;
using System.Collections.Generic;
using System.Linq;
using NongTrai.Animals;

namespace NongTrai.Learning
{
    public class Question
    {
        public AnimalData Target { get; }
        public IReadOnlyList<AnimalData> Options { get; }
        public int Level { get; }

        public Question(AnimalData target, IReadOnlyList<AnimalData> options, int level)
        {
            Target = target;
            Options = options;
            Level = level;
        }
    }

    public class LearningService
    {
        readonly AnimalDatabase database;
        readonly IProgressStorage storage;
        readonly Random rng;
        readonly LearningProgress progress;
        string lastTargetId;

        public event Action Changed;

        public LearningService(AnimalDatabase database, string childId, IProgressStorage storage, Random rng = null)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.rng = rng ?? new Random();

            progress = storage.Load(childId) ?? new LearningProgress { childId = childId };
            progress.childId = childId;
            if (progress.recent == null) progress.recent = new List<bool>();
            if (progress.records == null) progress.records = new List<MasteryRecord>();
            progress.level = Difficulty.Clamp(progress.level);
        }

        public int Level => progress.level;
        public LearningProgress Progress => progress;

        public float GetMastery(string animalId) => progress.Find(animalId)?.score ?? 0f;

        public bool IsMastered(string animalId) => MasteryModel.IsMastered(GetMastery(animalId));

        List<string> AnimalIds() => database.All
            .Where(a => a != null && !string.IsNullOrEmpty(a.Id))
            .Select(a => a.Id)
            .Distinct()
            .ToList();

        string GroupOf(string id) => database.Get(id)?.SimilarGroup;

        public Question NextQuestion(Func<AnimalData, AnimalData, bool> isAllowedDistractor = null)
        {
            List<string> ids = AnimalIds();
            if (ids.Count < 2)
                throw new InvalidOperationException("Cần ít nhất 2 con vật trong AnimalDatabase.");

            string targetId = QuestionPicker.PickTarget(ids, GetMastery, lastTargetId, rng);
            lastTargetId = targetId;
            AnimalData target = database.Get(targetId);

            Func<string, bool> allowed = null;
            if (isAllowedDistractor != null)
                allowed = id => isAllowedDistractor(target, database.Get(id));

            List<string> distractors = QuestionPicker.PickDistractors(
                targetId, ids, Difficulty.OptionCount(progress.level) - 1, GroupOf,
                Difficulty.UseSimilarDistractors(progress.level), rng, allowed);

            distractors.Add(targetId);
            List<AnimalData> options = QuestionPicker.Shuffle(distractors, rng).Select(database.Get).ToList();
            return new Question(target, options, progress.level);
        }

        public bool RecordAnswer(Question question, string chosenId, float responseSeconds, bool firstTry)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            bool correct = chosenId == question.Target.Id;
            if (!firstTry)
                return correct;

            MasteryRecord record = progress.GetOrCreate(question.Target.Id);
            record.score = MasteryModel.Update(record.score, correct, responseSeconds);
            record.attempts++;
            if (correct) record.correct++;
            record.lastSeenTicks = DateTime.UtcNow.Ticks;

            progress.totalAnswers++;
            Difficulty.Register(progress, correct);
            storage.Save(progress);
            Changed?.Invoke();
            return correct;
        }
    }
}
