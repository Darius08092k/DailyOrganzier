using DailyOrganzier.Data;
using DailyOrganzier.Models;
using DailyOrganzier.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.Services
{
    public class LocalDatabaseService : ILocalDatabaseService
    {
        private readonly AppDbContext _context;
        public LocalDatabaseService()
        {
            _context = new AppDbContext();
            LogDatabaseInfo();
        }

        public async Task<List<Quest>> GetQuestsAsync()
        {
            try
            {
                var quests = await _context.Quests.ToListAsync();
                System.Diagnostics.Debug.WriteLine($"Retrieved {quests.Count} quests from database");
                return quests;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving quests: {ex.Message}");
                throw;
            }
        }

        public async Task<int> SaveQuestAsync(Quest quest)
        {
            try
            {
                var existingQuest = await _context.Quests.FindAsync(quest.Id);

                if(existingQuest != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Updating quest: {quest.Title} (ID: {quest.Id})");
                    _context.Entry(existingQuest).CurrentValues.SetValues(quest);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Saving new quest: {quest.Title} (ID: {quest.Id})");
                    _context.Quests.Add(quest);
                }

                int result = await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"SaveChangesAsync result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving quest: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteQuestAsync(Quest quest)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Deleting quest: {quest.Title} (ID: {quest.Id})");
                _context.Quests.Remove(quest);
                int result = await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"DeleteChangesAsync result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting quest: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Logs database connection info for debugging
        /// </summary>
        private void LogDatabaseInfo()
        {
            try
            {
                string appDataDir = FileSystem.AppDataDirectory;
                string dbPath = Path.Combine(appDataDir, "DailyOrganizer.db3");
                bool dbExists = File.Exists(dbPath);

                System.Diagnostics.Debug.WriteLine("========== DATABASE INFO ==========");
                System.Diagnostics.Debug.WriteLine($"App Data Directory: {appDataDir}");
                System.Diagnostics.Debug.WriteLine($"Database Path: {dbPath}");
                System.Diagnostics.Debug.WriteLine($"Database Exists: {dbExists}");
                System.Diagnostics.Debug.WriteLine($"Quest Count: {_context.Quests.Count()}");
                System.Diagnostics.Debug.WriteLine("===================================");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging database info: {ex.Message}");
            }
        }
    }
}
