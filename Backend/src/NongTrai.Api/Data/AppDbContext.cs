using Microsoft.EntityFrameworkCore;

namespace NongTrai.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Child> Children => Set<Child>();
    public DbSet<PlaySession> Sessions => Set<PlaySession>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Mastery> Masteries => Set<Mastery>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Child>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasMaxLength(64);
            e.Property(c => c.Nickname).HasMaxLength(32).IsRequired();
        });

        b.Entity<PlaySession>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.ClientSessionId).HasMaxLength(64).IsRequired();
            e.HasIndex(s => s.ClientSessionId).IsUnique();
            e.HasIndex(s => new { s.ChildId, s.StartedAt });
            e.Property(s => s.Game).HasMaxLength(16).IsRequired();
            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Sessions_Game", "Game IN ('sound', 'shadow', 'food')");
                t.HasCheckConstraint("CK_Sessions_Level", "DifficultyLevel BETWEEN 1 AND 3");
                t.HasCheckConstraint("CK_Sessions_Time", "EndedAt >= StartedAt");
            });
            e.HasOne(s => s.Child).WithMany(c => c.Sessions)
                .HasForeignKey(s => s.ChildId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<Answer>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.AnimalId).HasMaxLength(64).IsRequired();
            e.Property(a => a.ChosenId).HasMaxLength(64).IsRequired();
            e.HasOne(a => a.PlaySession).WithMany(s => s.Answers)
                .HasForeignKey(a => a.PlaySessionId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<Mastery>(e =>
        {
            e.HasKey(m => new { m.ChildId, m.AnimalId });
            e.Property(m => m.AnimalId).HasMaxLength(64);
            e.ToTable(t => t.HasCheckConstraint("CK_Masteries_Score", "Score >= 0 AND Score <= 1"));
            e.HasOne(m => m.Child).WithMany(c => c.Masteries)
                .HasForeignKey(m => m.ChildId).OnDelete(DeleteBehavior.Cascade);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder config)
    {
        config.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
    }

    public sealed class UtcDateTimeConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>
    {
        public UtcDateTimeConverter() : base(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) { }
    }
}
