using Microsoft.EntityFrameworkCore;

namespace kusuri.Models;

public class AppDbContext : DbContext
{
    public DbSet<PrescriptionPrompt> PrescriptionPrompts { get; set; }
    public DbSet<Chat> Chats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Host=localhost;Port=8080;Database=kusuri;Username=postgres;Password=password",
            npgsqlOptions =>
            {
                npgsqlOptions.UseNodaTime()
                    .MigrationsHistoryTable("__EFMigrationsHistory", "kusuri");
            }
        );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set the default schema
        modelBuilder.HasDefaultSchema("kusuri");

        modelBuilder.Entity<PrescriptionPrompt>(entity =>
        {
            entity.ToTable("PrescriptionPrompt");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("Chat");
        });
    }
}
