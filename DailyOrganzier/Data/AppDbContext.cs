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
            // Ensure database directory exists before creating
            EnsureDatabaseDirectory();
            InitializeDatabase();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = GetDatabasePath();
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        /// <summary>
        /// Initializes the database, ensuring the schema is correct with all columns
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                // First ensure the database file exists
                Database.EnsureCreated();

                // Check if the Quests table has all required columns
                var connection = Database.GetDbConnection();
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "PRAGMA table_info(Quests);";
                var reader = command.ExecuteReader();

                var columns = new HashSet<string>();
                while (reader.Read())
                {
                    columns.Add(reader[1].ToString());
                }
                reader.Close();
                connection.Close();

                // Check if we need to migrate
                string[] requiredColumns = { "Id", "Title", "Type", "XpReward", "IsCompleted", "IsSystemQuest", "IsActive" };
                bool needsMigration = false;

                foreach (var column in requiredColumns)
                {
                    if (!columns.Contains(column))
                    {
                        System.Diagnostics.Debug.WriteLine($"Missing column: {column}");
                        needsMigration = true;
                        break;
                    }
                }

                if (needsMigration)
                {
                    System.Diagnostics.Debug.WriteLine("Database schema is outdated, recreating...");
                    RecreateDatabase();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Database schema is up to date");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        /// <summary>
        /// Recreates the database with the correct schema
        /// </summary>
        private void RecreateDatabase()
        {
            try
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
                System.Diagnostics.Debug.WriteLine("Database recreated successfully with correct schema");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error recreating database: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the database file path, creating the directory if needed
        /// </summary>
        private static string GetDatabasePath()
        {
            string appDataDir = FileSystem.AppDataDirectory;

            // Ensure the directory exists
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }

            return Path.Combine(appDataDir, "DailyOrganizer.db3");
        }

        /// <summary>
        /// Ensures the database directory exists before attempting to create the database
        /// </summary>
        private static void EnsureDatabaseDirectory()
        {
            try
            {
                string appDataDir = FileSystem.AppDataDirectory;
                if (!Directory.Exists(appDataDir))
                {
                    Directory.CreateDirectory(appDataDir);
                }

                System.Diagnostics.Debug.WriteLine($"Database directory: {appDataDir}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating database directory: {ex.Message}");
            }
        }
    }
}
