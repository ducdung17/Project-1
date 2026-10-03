namespace NongTrai.Api.Contracts;

public record AnswerDto(string AnimalId, string ChosenId, bool Correct, bool FirstTry, float Seconds);

public record MasteryDto(string AnimalId, float Score);

public record SessionUploadDto(
    string ClientSessionId,
    string ChildId,
    string Nickname,
    int AvatarId,
    string Game,
    DateTime StartedAt,
    DateTime EndedAt,
    int DifficultyLevel,
    List<AnswerDto> Answers,
    List<MasteryDto> Mastery);

public record UploadResultDto(int SessionId, bool Duplicate);

public record ChildListItemDto(
    string Id, string Nickname, int AvatarId,
    DateTime LastSeenAt, int SessionCount, int MasteredCount);

public record GameStatDto(string Game, int Sessions, int Answers, double Accuracy, double AvgSeconds);

public record ConfusionDto(string AnimalId, string ChosenId, int Count);

public record DailyStatDto(DateOnly Day, int Sessions, int Answers, double Accuracy);

public record ChildSummaryDto(
    string Id, string Nickname, int AvatarId, DateTime CreatedAt, DateTime LastSeenAt,
    int SessionCount, int AnswerCount, double FirstTryAccuracy,
    List<MasteryDto> Mastery,
    List<GameStatDto> Games,
    List<ConfusionDto> TopConfusions,
    List<DailyStatDto> Last14Days);

public record SessionListItemDto(
    int Id, string Game, DateTime StartedAt, DateTime EndedAt,
    int DifficultyLevel, int QuestionCount, int PerfectCount, int AnswerCount);
