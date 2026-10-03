# NongTrai Backend

ASP.NET Core 8 Web API + EF Core (SQLite / SQL Server).

## Run

```
cd src/NongTrai.Api
dotnet run
```

- Web: http://localhost:5180
- Swagger: http://localhost:5180/swagger

Database provider: `Database:Provider` in `appsettings.json` (`Sqlite` | `SqlServer`).

## Test

```
dotnet test
```

## API

| Method | Route |
|---|---|
| GET | /api/health |
| POST | /api/sessions |
| GET | /api/children |
| GET | /api/children/{id}/summary |
| GET | /api/children/{id}/sessions |
| DELETE | /api/children/{id} |

## Database

- `database/sqlserver/01_create_database.sql` – schema
- `database/sqlserver/02_seed_demo.sql` – demo data
- `database/sqlserver/03_reports.sql` – views, stored procedure, reports
