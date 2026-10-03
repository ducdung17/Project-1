using NongTrai.Api.Contracts;
using NongTrai.Api.Services;

namespace NongTrai.Api.Endpoints;

public static class ApiEndpoints
{
    public const string ApiKeyHeader = "X-Api-Key";

    public static void MapNongTraiApi(this WebApplication app)
    {
        RouteGroupBuilder api = app.MapGroup("/api");

        api.MapGet("/health", () => Results.Ok(new { status = "ok" }))
            .WithSummary("Kiểm tra server còn sống");

        api.MapPost("/sessions", async (SessionUploadDto dto, HttpRequest request,
                IConfiguration config, SessionService sessions, CancellationToken ct) =>
            {
                if (!HasValidKey(request, config))
                    return Results.Unauthorized();

                Dictionary<string, string[]> errors = SessionValidator.Validate(dto);
                if (errors.Count > 0)
                    return Results.ValidationProblem(errors);

                UploadResultDto result = await sessions.SaveAsync(dto, ct);
                return result.Duplicate
                    ? Results.Ok(result)
                    : Results.Created($"/api/sessions/{result.SessionId}", result);
            })
            .WithSummary("Game gửi kết quả một lượt chơi (tự tạo/cập nhật hồ sơ bé)");

        api.MapGet("/children", (StatsService stats, CancellationToken ct) => stats.ListChildrenAsync(ct))
            .WithSummary("Danh sách các bé");

        api.MapGet("/children/{id}/summary", async (string id, StatsService stats, CancellationToken ct) =>
                await stats.GetSummaryAsync(id, ct) is { } summary ? Results.Ok(summary) : Results.NotFound())
            .WithSummary("Tổng quan học tập của một bé");

        api.MapGet("/children/{id}/sessions", async (string id, int? take, StatsService stats, CancellationToken ct) =>
                await stats.ListSessionsAsync(id, take ?? 20, ct) is { } list ? Results.Ok(list) : Results.NotFound())
            .WithSummary("Các lượt chơi gần đây của một bé");

        api.MapDelete("/children/{id}", async (string id, HttpRequest request, IConfiguration config,
                StatsService stats, CancellationToken ct) =>
            {
                if (!HasValidKey(request, config)) return Results.Unauthorized();
                return await stats.DeleteChildAsync(id, ct) ? Results.NoContent() : Results.NotFound();
            })
            .WithSummary("Xóa một bé và toàn bộ dữ liệu của bé");
    }

    static bool HasValidKey(HttpRequest request, IConfiguration config)
    {
        string? expected = config["Upload:ApiKey"];
        if (string.IsNullOrEmpty(expected)) return true;
        return request.Headers.TryGetValue(ApiKeyHeader, out var given) && given == expected;
    }
}
