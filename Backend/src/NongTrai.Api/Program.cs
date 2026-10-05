using Microsoft.EntityFrameworkCore;
using NongTrai.Api.Data;
using NongTrai.Api.Endpoints;
using NongTrai.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
    DatabaseSetup.Configure(options, sp.GetRequiredService<IConfiguration>()));

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<StatsService>();

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .WithMethods("GET", "POST", "DELETE")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    app.Logger.LogInformation("Database: SQL Server ({Database})", db.Database.GetDbConnection().Database);
}

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapNongTraiApi();

app.Run();

public partial class Program { }
