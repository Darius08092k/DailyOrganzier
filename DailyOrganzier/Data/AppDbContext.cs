using DailyOrganzier.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Quest> Quests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "DailyOrganizer.db3");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
