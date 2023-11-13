using RatLevelerBot.Models;
using Microsoft.EntityFrameworkCore;

namespace RatLevelerBot.Services;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Chat> Chats => Set<Chat>();

    public DbSet<Level> Levels => Set<Level>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base (options) => Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>().HasMany(e => e.UserLevels)
            .WithOne(e => e.Chat)
            .HasForeignKey(e => e.ChatId)
            .IsRequired();

        modelBuilder.Entity<Level>().HasData(
            new Level { Id = 1, Value = 0, Name = "Крыска нулевка", Exp = 0 },
            new Level { Id = 2, Value = 1, Name = "Мелкая крыска", Exp = 0 },
            new Level { Id = 3, Value = 2, Name = "Крыска милашка", Exp = 0 }
        );
    }
}

