using DailyOrganzier.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Quest> Quests { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Prevent configuring if options are already set (e.g., via Dependency Injection)
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "DailyOrganizer.db3");

#if WINDOWS
                System.Diagnostics.Debug.WriteLine($"\n==================================\n[DB_PATH] SQLite File: {dbPath}\n==================================\n");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
#elif ANDROID || IOS || MACCATALYST
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quest>().HasData(
                new Quest { Id = 1, Title = "Make bed", Type = "CHORE", XpReward = 15, IsSystemQuest = true, UserId = null },
                new Quest { Id = 2, Title = "Clean setup", Type = "CHORE", XpReward = 15, IsSystemQuest = true, UserId = null },
                new Quest { Id = 3, Title = "10 minute workout", Type = "FITNESS", XpReward = 15, IsSystemQuest = true, UserId = null },
                new Quest { Id = 4, Title = "10 minute walk", Type = "FITNESS", XpReward = 15, IsSystemQuest = true, UserId = null },
                new Quest { Id = 5, Title = "30 minute book reading", Type = "LEARNING", XpReward = 15, IsSystemQuest = true, UserId = null },
                new Quest { Id = 6, Title = "30 minute learning", Type = "LEARNING", XpReward = 15, IsSystemQuest = true, UserId = null },
                new Quest { Id = 7, Title = "10 minute mind relaxation", Type = "MINDFULNESS", XpReward = 15, IsSystemQuest = true, UserId = null }
            );
        }
    }
}
