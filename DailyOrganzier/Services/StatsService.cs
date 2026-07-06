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
                var savedQuests = await _databaseService.GetQuestsAsync();

                // If database is empty (first run), generate random quests
                if (savedQuests == null || savedQuests.Count == 0)
                {
                    var randomQuests = _dailyQuests.GetRandomQuest(3);
                    foreach (var quest in randomQuests)
                    {
                        await AddQuestAsync(quest);
                    }
                }
                else
                {
                    // Load existing quests from database
                    foreach (var quest in savedQuests)
                    {
                        MainThread.BeginInvokeOnMainThread(() => ActiveQuests.Add(quest));
                    }
                }
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

            // Save the deleted quest to database
            _ = _databaseService.DeleteQuestAsync(quest);
        }

        /// <summary>
        /// Helper method to log errors consistently across the service
        /// </summary>
        private static void LogError(string message, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"{message}: {ex.Message}");
        }
    }
}
