using NongTrai.Api.Contracts;

namespace NongTrai.Api.Services;

public static class SessionValidator
{
    public static readonly string[] Games = { "sound", "shadow", "food" };
    public const int MaxIdLength = 64;
    public const int MaxNicknameLength = 32;
    public const int MaxAnswers = 100;
    public const int MaxMastery = 50;

    public static Dictionary<string, string[]> Validate(SessionUploadDto? dto)
    {
        var errors = new Dictionary<string, string[]>();
        void Add(string field, string message) => errors[field] = new[] { message };

        if (dto is null)
        {
            Add("body", "Thiếu dữ liệu.");
            return errors;
        }

        if (!IsId(dto.ClientSessionId)) Add(nameof(dto.ClientSessionId), "Bắt buộc, tối đa 64 ký tự.");
        if (!IsId(dto.ChildId)) Add(nameof(dto.ChildId), "Bắt buộc, tối đa 64 ký tự.");
        if (string.IsNullOrWhiteSpace(dto.Nickname) || dto.Nickname.Length > MaxNicknameLength)
            Add(nameof(dto.Nickname), $"Bắt buộc, tối đa {MaxNicknameLength} ký tự.");
        if (!Games.Contains(dto.Game)) Add(nameof(dto.Game), "Chỉ nhận: " + string.Join(", ", Games) + ".");
        if (dto.EndedAt < dto.StartedAt) Add(nameof(dto.EndedAt), "Giờ kết thúc phải sau giờ bắt đầu.");
        if (dto.DifficultyLevel is < 1 or > 3) Add(nameof(dto.DifficultyLevel), "Độ khó từ 1 đến 3.");

        if (dto.Answers is null || dto.Answers.Count == 0 || dto.Answers.Count > MaxAnswers)
            Add(nameof(dto.Answers), $"Cần từ 1 đến {MaxAnswers} câu trả lời.");
        else if (dto.Answers.Any(a => a is null || !IsId(a.AnimalId) || !IsId(a.ChosenId)
                                      || a.Seconds < 0 || a.Seconds > 3600
                                      || a.Correct != (a.AnimalId == a.ChosenId)))
            Add(nameof(dto.Answers), "Có câu trả lời sai định dạng (id rỗng, thời gian âm, hoặc cờ Correct không khớp).");

        if (dto.Mastery is not null)
        {
            if (dto.Mastery.Count > MaxMastery)
                Add(nameof(dto.Mastery), $"Tối đa {MaxMastery} con vật.");
            else if (dto.Mastery.Any(m => m is null || !IsId(m.AnimalId) || m.Score < 0 || m.Score > 1))
                Add(nameof(dto.Mastery), "Mức thuộc phải trong khoảng 0..1.");
        }

        return errors;
    }

    static bool IsId(string? s) => !string.IsNullOrWhiteSpace(s) && s.Length <= MaxIdLength;
}
