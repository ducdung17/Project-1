using Microsoft.EntityFrameworkCore;
using NongTrai.Api.Contracts;
using NongTrai.Api.Data;

namespace NongTrai.Api.Services;

public class SessionService
{
    readonly AppDbContext db;
    readonly TimeProvider clock;

    public SessionService(AppDbContext db, TimeProvider clock)
    {
        this.db = db;
        this.clock = clock;
    }

    public async Task<UploadResultDto> SaveAsync(SessionUploadDto dto, CancellationToken ct = default)
    {
        int? existingId = await db.Sessions
            .Where(s => s.ClientSessionId == dto.ClientSessionId)
            .Select(s => (int?)s.Id)
            .FirstOrDefaultAsync(ct);
        if (existingId is not null)
            return new UploadResultDto(existingId.Value, Duplicate: true);

        DateTime now = clock.GetUtcNow().UtcDateTime;

        Child? child = await db.Children
            .Include(c => c.Masteries)
            .FirstOrDefaultAsync(c => c.Id == dto.ChildId, ct);
        if (child is null)
        {
            child = new Child { Id = dto.ChildId, CreatedAt = now };
            db.Children.Add(child);
        }
        child.Nickname = dto.Nickname.Trim();
        child.AvatarId = dto.AvatarId;
        child.LastSeenAt = now;

        var session = new PlaySession
        {
            ClientSessionId = dto.ClientSessionId,
            ChildId = child.Id,
            Game = dto.Game,
            StartedAt = AsUtc(dto.StartedAt),
            EndedAt = AsUtc(dto.EndedAt),
            DifficultyLevel = dto.DifficultyLevel,
            QuestionCount = dto.Answers.Count(a => a.Correct),
            PerfectCount = dto.Answers.Count(a => a.Correct && a.FirstTry),
            Answers = dto.Answers.Select(a => new Answer
            {
                AnimalId = a.AnimalId,
                ChosenId = a.ChosenId,
                Correct = a.Correct,
                FirstTry = a.FirstTry,
                Seconds = a.Seconds,
            }).ToList(),
        };
        db.Sessions.Add(session);

        foreach (MasteryDto m in dto.Mastery ?? new List<MasteryDto>())
        {
            Mastery? row = child.Masteries.FirstOrDefault(x => x.AnimalId == m.AnimalId);
            if (row is null)
            {
                row = new Mastery { ChildId = child.Id, AnimalId = m.AnimalId };
                child.Masteries.Add(row);
            }
            row.Score = m.Score;
            row.UpdatedAt = now;
        }

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            if (!await IsDuplicateAsync(dto.ClientSessionId, ct)) throw;
            int id = await db.Sessions.Where(s => s.ClientSessionId == dto.ClientSessionId)
                .Select(s => s.Id).FirstAsync(ct);
            return new UploadResultDto(id, Duplicate: true);
        }

        return new UploadResultDto(session.Id, Duplicate: false);
    }

    async Task<bool> IsDuplicateAsync(string clientSessionId, CancellationToken ct)
    {
        db.ChangeTracker.Clear();
        return await db.Sessions.AnyAsync(s => s.ClientSessionId == clientSessionId, ct);
    }

    static DateTime AsUtc(DateTime t) => t.Kind switch
    {
        DateTimeKind.Utc => t,
        DateTimeKind.Local => t.ToUniversalTime(),
        _ => DateTime.SpecifyKind(t, DateTimeKind.Utc),
    };
}
