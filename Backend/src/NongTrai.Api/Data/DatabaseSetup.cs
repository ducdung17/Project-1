using Microsoft.EntityFrameworkCore;

namespace NongTrai.Api.Data;

public static class DatabaseSetup
{
    public const string Sqlite = "Sqlite";
    public const string SqlServer = "SqlServer";

    public static string ProviderName(IConfiguration config) =>
        string.Equals(config["Database:Provider"], SqlServer, StringComparison.OrdinalIgnoreCase) ? SqlServer : Sqlite;

    public static void Configure(DbContextOptionsBuilder options, IConfiguration config)
    {
        string provider = ProviderName(config);
        string? connection = config.GetConnectionString(provider);
        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException(
                $"Thiếu ConnectionStrings:{provider} trong appsettings.json (Database:Provider = {provider}).");

        if (provider == SqlServer)
            options.UseSqlServer(connection, sql => sql.EnableRetryOnFailure(3));
        else
            options.UseSqlite(connection);
    }
}
