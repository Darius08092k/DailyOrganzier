using DailyOrganzier.Models;
using DailyOrganzier.Services.Interfaces;
using DailyOrganzier.HelperClasses.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DailyOrganzier.Services
{
    public class StatsService : IStatsService
    {
        private readonly ILocalDatabaseService _databaseService;
        private readonly IDailyQuests _dailyQuests;

        public UserStats Stats { get; set; }
        public ObservableCollection<Quest> ActiveQuests { get; set; }

        public StatsService(ILocalDatabaseService databaseService, IDailyQuests dailyQuests)
        {
            _databaseService = databaseService;
            _dailyQuests = dailyQuests;
            Stats = new UserStats();
            ActiveQuests = new ObservableCollection<Quest>();
        }

        public async Task InitializeAsync()
        {
            try
            {

                // Check if a new day has started by comparing dates
                DateTime lastLoginDate = Preferences.Default.Get("LastLoginDate", DateTime.MinValue);
                bool isNewDay = lastLoginDate.Date < DateTime.Now.Date;

                if(isNewDay)
                {
                    Preferences.Default.Set("LastLoginDate", DateTime.Now);
                }

                // Load quests from the database
                await _dailyQuests.SeedSystemQuestsAsync();
                var savedQuests = await _databaseService.GetQuestsAsync();

                // Trigger if it's a new day, OR if no quests are active at all (e.g., very first launch)
                if (isNewDay || !savedQuests.Any(q => q.IsActive))
                {
                    foreach (var quest in savedQuests)
                    {
                        if (quest.IsSystemQuest)
                        {
                            quest.IsActive = false; // Send it back to the inactive pool
                            quest.IsCompleted = false;
                            await _databaseService.SaveQuestAsync(quest);
                        }
                        else
                        {
                            // Delete personal one-off quests permanently
                            await _databaseService.DeleteQuestAsync(quest);
                        }
                    }

                    // Assign 3 new quests from the pool
                    await _dailyQuests.AssignRandomQuestsAsync(3);

                    // Refresh the list after the updates
                    savedQuests = await _databaseService.GetQuestsAsync();
                }

                // Get all active, incomplete quests
                var activeQuests = savedQuests
                    .Where(q => q.IsActive && !q.IsCompleted)
                    .ToList();

                // Update the ObservableCollection on the main thread, all at once
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ActiveQuests.Clear();
                    foreach (var quest in activeQuests)
                    {
                        ActiveQuests.Add(quest);
                    }
                });
            }
            catch (Exception ex)
            {
                LogError("Error initializing quests from database", ex);
            }
        }

        public async Task AddQuestAsync(Quest quest)
        {
            try
            {
                ActiveQuests.Add(quest);
                await _databaseService.SaveQuestAsync(quest);
            }
            catch (Exception ex)
            {
                LogError("Error saving quest to database", ex);
                ActiveQuests.Remove(quest);
            }
        }

        public void CompleteQuest(Quest quest)
        {
            if(quest == null || !ActiveQuests.Contains(quest))
            {
                return;
            }

            // Remove the quest from the UI
            ActiveQuests.Remove(quest);

            // Add the xp to the stats
            Stats.CurrentXp += quest.XpReward;

            // Check the level up
            if(Stats.CurrentXp >= Stats.XpToNextLevel)
            {
                Stats.CurrentXp -= Stats.XpToNextLevel;
                Stats.CurrentLevel++;
                Stats.XpToNextLevel = (int)(Stats.XpToNextLevel * 3);
            }

            // Mark as completed and update the database instead of deleting
            quest.IsCompleted = true;
            _ = _databaseService.SaveQuestAsync(quest);
        }

        /// <summary>
        /// Helper method to log errors consistently across the service
        /// </summary>
        private static void LogError(string message, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"{message}: {ex.Message}");
        }

        /// <summary>
        /// Debug method to reset and re-seed the database
        /// WARNING: This deletes all data and starts fresh
        /// </summary>
        public async Task ResetAndReseedDatabaseAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("DEBUG: Starting database reset and reseed...");

                // Get all existing quests and delete them
                var allQuests = await _databaseService.GetQuestsAsync();
                foreach (var quest in allQuests)
                {
                    await _databaseService.DeleteQuestAsync(quest);
                }

                System.Diagnostics.Debug.WriteLine($"DEBUG: Deleted {allQuests.Count} quests");

                // Re-seed the database
                await _dailyQuests.SeedSystemQuestsAsync();

                // Re-initialize
                Stats = new UserStats();
                MainThread.BeginInvokeOnMainThread(() => ActiveQuests.Clear());

                await InitializeAsync();

                System.Diagnostics.Debug.WriteLine("DEBUG: Database reset and reseed complete!");
            }
            catch (Exception ex)
            {
                LogError("Error during database reset", ex);
            }
        }
    }
}

