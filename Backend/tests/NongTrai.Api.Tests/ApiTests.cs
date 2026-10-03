using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using NongTrai.Api.Contracts;
using Xunit;

namespace NongTrai.Api.Tests;

public sealed class TestApi : WebApplicationFactory<Program>
{
    public const string Key = "test-key";
    readonly string dbPath = Path.Combine(Path.GetTempPath(), $"nongtrai-test-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Database:Provider"] = "Sqlite",
            ["ConnectionStrings:Sqlite"] = $"Data Source={dbPath}",
            ["Upload:ApiKey"] = Key,
        }));
    }

    public HttpClient CreateGameClient()
    {
        HttpClient client = CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", Key);
        return client;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        SqliteConnection.ClearAllPools();
        if (File.Exists(dbPath)) File.Delete(dbPath);
    }
}

public class ApiTests : IDisposable
{
    readonly TestApi api = new();
    readonly HttpClient game;

    public ApiTests() => game = api.CreateGameClient();

    public void Dispose() => api.Dispose();

    static SessionUploadDto Session(string sessionId = "s1", string childId = "kid-1", string game = "sound",
        List<AnswerDto>? answers = null, List<MasteryDto>? mastery = null) =>
        new(sessionId, childId, "Bé Na", 2, game,
            new DateTime(2026, 10, 1, 3, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 10, 1, 3, 5, 0, DateTimeKind.Utc),
            DifficultyLevel: 1,
            answers ?? new List<AnswerDto>
            {
                new("cat", "dog", Correct: false, FirstTry: true, Seconds: 3.2f),
                new("cat", "cat", Correct: true, FirstTry: false, Seconds: 5.0f),
                new("dog", "dog", Correct: true, FirstTry: true, Seconds: 2.1f),
            },
            mastery ?? new List<MasteryDto> { new("cat", 0.12f), new("dog", 0.35f) });

    [Fact]
    public async Task Health_ReturnsOk()
    {
        HttpResponseMessage res = await game.GetAsync("/api/health");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Upload_NewSession_CreatesChildAndSession()
    {
        HttpResponseMessage res = await game.PostAsJsonAsync("/api/sessions", Session());
        Assert.Equal(HttpStatusCode.Created, res.StatusCode);

        var kids = await game.GetFromJsonAsync<List<ChildListItemDto>>("/api/children");
        ChildListItemDto kid = Assert.Single(kids!);
        Assert.Equal("kid-1", kid.Id);
        Assert.Equal("Bé Na", kid.Nickname);
        Assert.Equal(1, kid.SessionCount);

        var sessions = await game.GetFromJsonAsync<List<SessionListItemDto>>("/api/children/kid-1/sessions");
        SessionListItemDto s = Assert.Single(sessions!);
        Assert.Equal(2, s.QuestionCount);
        Assert.Equal(1, s.PerfectCount);
        Assert.Equal(3, s.AnswerCount);
    }

    [Fact]
    public async Task Upload_SameSessionTwice_IsStoredOnce()
    {
        await game.PostAsJsonAsync("/api/sessions", Session());
        HttpResponseMessage again = await game.PostAsJsonAsync("/api/sessions", Session());

        Assert.Equal(HttpStatusCode.OK, again.StatusCode);
        UploadResultDto? result = await again.Content.ReadFromJsonAsync<UploadResultDto>();
        Assert.True(result!.Duplicate);

        var kids = await game.GetFromJsonAsync<List<ChildListItemDto>>("/api/children");
        Assert.Equal(1, kids![0].SessionCount);
    }

    [Fact]
    public async Task Upload_WithoutApiKey_IsRejected()
    {
        HttpClient stranger = api.CreateClient();
        HttpResponseMessage res = await stranger.PostAsJsonAsync("/api/sessions", Session());
        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }

    [Theory]
    [InlineData("chess")]
    [InlineData("")]
    public async Task Upload_UnknownGame_ReturnsBadRequest(string gameName)
    {
        HttpResponseMessage res = await game.PostAsJsonAsync("/api/sessions", Session(game: gameName));
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Upload_CorrectFlagNotMatchingChoice_ReturnsBadRequest()
    {
        var lying = new List<AnswerDto> { new("cat", "dog", Correct: true, FirstTry: true, Seconds: 1f) };
        HttpResponseMessage res = await game.PostAsJsonAsync("/api/sessions", Session(answers: lying));
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Summary_ComputesAccuracyAndConfusions()
    {
        await game.PostAsJsonAsync("/api/sessions", Session());

        ChildSummaryDto? sum = await game.GetFromJsonAsync<ChildSummaryDto>("/api/children/kid-1/summary");

        Assert.NotNull(sum);
        Assert.Equal(1, sum!.SessionCount);
        Assert.Equal(3, sum.AnswerCount);
        Assert.Equal(0.5, sum.FirstTryAccuracy);

        ConfusionDto top = Assert.Single(sum.TopConfusions);
        Assert.Equal(("cat", "dog", 1), (top.AnimalId, top.ChosenId, top.Count));

        GameStatDto sound = sum.Games.Single(g => g.Game == "sound");
        Assert.Equal(1, sound.Sessions);
        Assert.Equal(0, sum.Games.Single(g => g.Game == "food").Sessions);
        Assert.Equal(14, sum.Last14Days.Count);
    }

    [Fact]
    public async Task Upload_SecondSession_UpdatesMastery()
    {
        await game.PostAsJsonAsync("/api/sessions", Session());
        await game.PostAsJsonAsync("/api/sessions", Session(sessionId: "s2",
            mastery: new List<MasteryDto> { new("cat", 0.9f) }));

        ChildSummaryDto? sum = await game.GetFromJsonAsync<ChildSummaryDto>("/api/children/kid-1/summary");
        Assert.Equal(0.9f, sum!.Mastery.Single(m => m.AnimalId == "cat").Score);
        Assert.Equal(0.35f, sum.Mastery.Single(m => m.AnimalId == "dog").Score);

        var kids = await game.GetFromJsonAsync<List<ChildListItemDto>>("/api/children");
        Assert.Equal(1, kids![0].MasteredCount);
    }

    [Fact]
    public async Task Summary_UnknownChild_ReturnsNotFound()
    {
        HttpResponseMessage res = await game.GetAsync("/api/children/nobody/summary");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task DeleteChild_RemovesAllData()
    {
        await game.PostAsJsonAsync("/api/sessions", Session());

        HttpResponseMessage res = await game.DeleteAsync("/api/children/kid-1");

        Assert.Equal(HttpStatusCode.NoContent, res.StatusCode);
        var kids = await game.GetFromJsonAsync<List<ChildListItemDto>>("/api/children");
        Assert.Empty(kids!);
        HttpResponseMessage again = await game.PostAsJsonAsync("/api/sessions", Session());
        Assert.Equal(HttpStatusCode.Created, again.StatusCode);
    }
}
