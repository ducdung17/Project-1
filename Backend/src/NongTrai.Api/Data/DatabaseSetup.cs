using Microsoft.EntityFrameworkCore;

namespace NongTrai.Api.Data;

public static class DatabaseSetup
{
    public const string ConnectionName = "SqlServer";

    public static void Configure(DbContextOptionsBuilder options, IConfiguration config)
    {
        string? connection = config.GetConnectionString(ConnectionName);
        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException(
                $"Thiếu ConnectionStrings:{ConnectionName} trong appsettings.json.");

        options.UseSqlServer(connection, sql => sql.EnableRetryOnFailure(3));
    }
}
