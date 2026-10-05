# NongTrai Backend

ASP.NET Core 8 Web API + EF Core + SQL Server.

## Requirements

- .NET 8 SDK
- SQL Server running on `localhost` (Windows authentication).
  Connection string: `ConnectionStrings:SqlServer` in `src/NongTrai.Api/appsettings.json`.

## Run

Double-click `start.bat`, or:

```
cd src/NongTrai.Api
dotnet run
```

- Parent web page: http://localhost:5180
- Swagger: http://localhost:5180/swagger

On first start the server creates the `NongTrai` database and its tables automatically (empty, no demo data).
`database/sqlserver/01_create_database.sql` contains the same schema if you prefer to create it by hand in SSMS.

## Test

```
dotnet test
```

Each test creates a temporary SQL Server database (`NongTrai_Test_<guid>`) and drops it afterwards.
Set the environment variable `NONGTRAI_TEST_SQLSERVER` to use a server other than `localhost`.

## API

| Method | Route |
|---|---|
| GET | /api/health |
| POST | /api/sessions |
| GET | /api/children |
| GET | /api/children/{id}/summary |
| GET | /api/children/{id}/sessions |
| DELETE | /api/children/{id} |
