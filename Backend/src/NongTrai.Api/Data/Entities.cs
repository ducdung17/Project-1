namespace NongTrai.Api.Data;

public class Child
{
    public string Id { get; set; } = "";
    public string Nickname { get; set; } = "";
    public int AvatarId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastSeenAt { get; set; }

    public List<PlaySession> Sessions { get; set; } = new();
    public List<Mastery> Masteries { get; set; } = new();
}

public class PlaySession
{
    public int Id { get; set; }

    public string ClientSessionId { get; set; } = "";

    public string ChildId { get; set; } = "";
    public Child? Child { get; set; }

    public string Game { get; set; } = "";
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public int DifficultyLevel { get; set; }
    public int QuestionCount { get; set; }
    public int PerfectCount { get; set; }

    public List<Answer> Answers { get; set; } = new();
}

public class Answer
{
    public int Id { get; set; }
    public int PlaySessionId { get; set; }
    public PlaySession? PlaySession { get; set; }

    public string AnimalId { get; set; } = "";
    public string ChosenId { get; set; } = "";
    public bool Correct { get; set; }
    public bool FirstTry { get; set; }
    public float Seconds { get; set; }
}

public class Mastery
{
    public string ChildId { get; set; } = "";
    public Child? Child { get; set; }
    public string AnimalId { get; set; } = "";
    public float Score { get; set; }
    public DateTime UpdatedAt { get; set; }
}
