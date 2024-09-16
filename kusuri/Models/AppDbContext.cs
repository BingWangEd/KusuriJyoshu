using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class AppDbContext : DbContext
{
    public DbSet<PrescriptionPrompt> PrescriptionPrompts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Host=localhost;Database=kusuri;Username=postgres;Password=password",
            npgsqlOptions =>
            {
                npgsqlOptions.UseNodaTime()
                    .MigrationsHistoryTable("__EFMigrationsHistory", "flatiron");
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
    }
}
