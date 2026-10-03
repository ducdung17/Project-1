using Microsoft.EntityFrameworkCore;
using NongTrai.Api.Contracts;
using NongTrai.Api.Data;

namespace NongTrai.Api.Services;

public class StatsService
{
    public const float MasteredThreshold = 0.8f;

    readonly AppDbContext db;
    readonly TimeProvider clock;

    public StatsService(AppDbContext db, TimeProvider clock)
    {
        this.db = db;
        this.clock = clock;
    }

    public Task<List<ChildListItemDto>> ListChildrenAsync(CancellationToken ct = default) =>
        db.Children
            .OrderByDescending(c => c.LastSeenAt)
            .Select(c => new ChildListItemDto(
                c.Id, c.Nickname, c.AvatarId, c.LastSeenAt,
                c.Sessions.Count,
                c.Masteries.Count(m => m.Score >= MasteredThreshold)))
            .ToListAsync(ct);

    public async Task<ChildSummaryDto?> GetSummaryAsync(string childId, CancellationToken ct = default)
    {
        Child? child = await db.Children.AsNoTracking()
            .Include(c => c.Masteries)
            .FirstOrDefaultAsync(c => c.Id == childId, ct);
        if (child is null) return null;

        var answers = await db.Answers.AsNoTracking()
            .Where(a => a.PlaySession!.ChildId == childId)
            .Select(a => new
            {
                a.AnimalId, a.ChosenId, a.Correct, a.FirstTry, a.Seconds,
                a.PlaySession!.Game, a.PlaySession.StartedAt,
            })
            .ToListAsync(ct);
        var sessions = await db.Sessions.AsNoTracking()
            .Where(s => s.ChildId == childId)
            .Select(s => new { s.Game, s.StartedAt })
            .ToListAsync(ct);

        var firstTries = answers.Where(a => a.FirstTry).ToList();

        List<GameStatDto> games = SessionValidator.Games
            .Select(g =>
            {
                var ft = firstTries.Where(a => a.Game == g).ToList();
                return new GameStatDto(
                    g,
                    sessions.Count(s => s.Game == g),
                    ft.Count,
                    Ratio(ft.Count(a => a.Correct), ft.Count),
                    ft.Count == 0 ? 0 : Math.Round(ft.Average(a => a.Seconds), 1));
            })
            .ToList();

        List<ConfusionDto> confusions = answers
            .Where(a => !a.Correct)
            .GroupBy(a => (a.AnimalId, a.ChosenId))
            .Select(g => new ConfusionDto(g.Key.AnimalId, g.Key.ChosenId, g.Count()))
            .OrderByDescending(c => c.Count).ThenBy(c => c.AnimalId)
            .Take(5)
            .ToList();

        DateOnly today = DateOnly.FromDateTime(clock.GetLocalNow().DateTime);
        List<DailyStatDto> days = Enumerable.Range(0, 14)
            .Select(i => today.AddDays(i - 13))
            .Select(day =>
            {
                var ft = firstTries.Where(a => LocalDay(a.StartedAt) == day).ToList();
                return new DailyStatDto(
                    day,
                    sessions.Count(s => LocalDay(s.StartedAt) == day),
                    ft.Count,
                    Ratio(ft.Count(a => a.Correct), ft.Count));
            })
            .ToList();

        return new ChildSummaryDto(
            child.Id, child.Nickname, child.AvatarId, child.CreatedAt, child.LastSeenAt,
            sessions.Count, answers.Count,
            Ratio(firstTries.Count(a => a.Correct), firstTries.Count),
            child.Masteries.OrderByDescending(m => m.Score).Select(m => new MasteryDto(m.AnimalId, m.Score)).ToList(),
            games, confusions, days);
    }

    public async Task<List<SessionListItemDto>?> ListSessionsAsync(string childId, int take, CancellationToken ct = default)
    {
        if (!await db.Children.AnyAsync(c => c.Id == childId, ct)) return null;
        take = Math.Clamp(take, 1, 100);
        return await db.Sessions.AsNoTracking()
            .Where(s => s.ChildId == childId)
            .OrderByDescending(s => s.StartedAt)
            .Take(take)
            .Select(s => new SessionListItemDto(
                s.Id, s.Game, s.StartedAt, s.EndedAt, s.DifficultyLevel,
                s.QuestionCount, s.PerfectCount, s.Answers.Count))
            .ToListAsync(ct);
    }

    public async Task<bool> DeleteChildAsync(string childId, CancellationToken ct = default)
    {
        Child? child = await db.Children.FindAsync(new object[] { childId }, ct);
        if (child is null) return false;
        db.Children.Remove(child);
        await db.SaveChangesAsync(ct);
        return true;
    }

    DateOnly LocalDay(DateTime utc) =>
        DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), clock.LocalTimeZone));

    static double Ratio(int part, int total) => total == 0 ? 0 : Math.Round((double)part / total, 3);
}
