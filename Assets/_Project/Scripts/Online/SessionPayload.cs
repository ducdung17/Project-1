using System;
using System.Collections.Generic;

namespace NongTrai.Online
{
    [Serializable]
    public class AnswerPayload
    {
        public string animalId;
        public string chosenId;
        public bool correct;
        public bool firstTry;
        public float seconds;
    }

    [Serializable]
    public class MasteryPayload
    {
        public string animalId;
        public float score;
    }

    [Serializable]
    public class SessionPayload
    {
        public string clientSessionId;
        public string childId;
        public string nickname;
        public int avatarId;
        public string game;
        public string startedAt;
        public string endedAt;
        public int difficultyLevel;
        public List<AnswerPayload> answers = new List<AnswerPayload>();
        public List<MasteryPayload> mastery = new List<MasteryPayload>();
    }
}
