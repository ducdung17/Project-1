using System;
using NongTrai.Animals;
using NongTrai.Learning;
using NongTrai.Profiles;

namespace NongTrai.Online
{
    public class SessionRecorder
    {
        readonly SessionPayload payload;

        public SessionPayload Payload => payload;

        public SessionRecorder(string game, ChildProfile child, DateTime? startedUtc = null)
        {
            payload = new SessionPayload
            {
                clientSessionId = Guid.NewGuid().ToString("N"),
                childId = child?.Id,
                nickname = child?.Nickname,
                avatarId = child?.AvatarId ?? 0,
                game = game,
                startedAt = (startedUtc ?? DateTime.UtcNow).ToString("o"),
            };
        }

        public void Add(string animalId, string chosenId, bool firstTry, float seconds)
        {
            payload.answers.Add(new AnswerPayload
            {
                animalId = animalId,
                chosenId = chosenId,
                correct = animalId == chosenId,
                firstTry = firstTry,
                seconds = seconds,
            });
        }

        public void Finish(LearningService learning, AnimalDatabase database, DateTime? endedUtc = null)
        {
            Complete(learning, database, endedUtc);
            if (payload.childId != null && payload.answers.Count > 0)
                ProgressUploader.Enqueue(payload);
        }

        public SessionPayload Complete(LearningService learning, AnimalDatabase database, DateTime? endedUtc = null)
        {
            payload.endedAt = (endedUtc ?? DateTime.UtcNow).ToString("o");
            payload.difficultyLevel = learning != null ? learning.Level : 1;
            payload.mastery.Clear();
            if (learning != null && database != null)
            {
                foreach (AnimalData animal in database.All)
                {
                    if (animal == null) continue;
                    payload.mastery.Add(new MasteryPayload { animalId = animal.Id, score = learning.GetMastery(animal.Id) });
                }
            }
            return payload;
        }
    }
}
