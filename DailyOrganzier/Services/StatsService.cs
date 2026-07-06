using DailyOrganzier.Models;
using DailyOrganzier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DailyOrganzier.Services
{
    public class StatsService : IStatsService
    {
        private readonly ILocalDatabaseService _databaseService;

        public UserStats Stats { get; set; }
        public ObservableCollection<Quest> ActiveQuests { get; set; }

        public StatsService(ILocalDatabaseService databaseService)
        {
            _databaseService = databaseService;
            Stats = new UserStats();
            ActiveQuests = new ObservableCollection<Quest>();
        }

        public async Task InitializeAsync()
        {
            try
            {
                var questsFromDb = await _databaseService.GetQuestsAsync();
                foreach (var quest in questsFromDb)
                {
                    ActiveQuests.Add(quest);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading quests from database: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"Error saving quest to database: {ex.Message}");
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

    }
}
