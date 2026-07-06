using DailyOrganzier.HelperClasses.Enums;
using DailyOrganzier.Models;
using DailyOrganzier.Models.Enums;
using DailyOrganzier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DailyOrganzier.HelperClasses
{
    public class DailyQuests : IDailyQuests
    {
        private readonly ILocalDatabaseService _databaseService;
        private readonly Random _random;

        public DailyQuests(ILocalDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _random = new Random();
        }

        public async Task AssignRandomQuestsAsync(int numberOfDailyQuests)
        {
            var allQuests = await _databaseService.GetQuestsAsync();
            var pool = allQuests.Where(q => q.IsSystemQuest).ToList();

            // Shuffle the pool
            for (int i = pool.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                var temp = pool[i];
                pool[i] = pool[j];
                pool[j] = temp;
            }

            var selected = pool.Take(numberOfDailyQuests).ToList();

            // Mark the selected quests as active for the day
            foreach (var quest in selected)
            {
                quest.IsActive = true;
                quest.IsCompleted = false;
                await _databaseService.SaveQuestAsync(quest);
            }
        }

        public async Task SeedSystemQuestsAsync()
        {
            try
            {
                var existingQuests = await _databaseService.GetQuestsAsync();
                var systemQuestCount = existingQuests.Count(q => q.IsSystemQuest);

                System.Diagnostics.Debug.WriteLine($"Seed Check: {systemQuestCount} system quests already exist");

                // Only seed the database if the system quests aren't already there
                if (!existingQuests.Any(q => q.IsSystemQuest))
                {
                    System.Diagnostics.Debug.WriteLine("Starting to seed system quests...");

                    var standardQuests = new List<Quest>
                    {
                        new Quest { Title = "Make bed", Type = QuestCategory.CHORE.ToString(), XpReward = 15, IsSystemQuest = true, IsActive = false },
                        new Quest { Title = "Clean setup", Type = QuestCategory.CHORE.ToString(), XpReward = 15, IsSystemQuest = true, IsActive = false },
                        new Quest { Title = "10 minute workout", Type = QuestCategory.FITNESS.ToString(), XpReward = 15, IsSystemQuest = true, IsActive = false },
                        new Quest { Title = "10 minute walk", Type = QuestCategory.FITNESS.ToString(), XpReward = 15, IsSystemQuest = true, IsActive = false },
                        new Quest { Title = "30 minute book reading", Type = QuestCategory.LEARNING.ToString(), XpReward = 15, IsSystemQuest = true, IsActive = false },
                        new Quest { Title = "30 minute learning", Type = QuestCategory.LEARNING.ToString(), XpReward = 15, IsSystemQuest = true, IsActive = false },
                        new Quest { Title = "10 minute mind relaxation", Type = QuestCategory.MINDFULNESS.ToString(), XpReward = 15, IsSystemQuest = true, IsActive = false }
                    };

                    int seedCount = 0;
                    foreach (var quest in standardQuests)
                    {
                        try
                        {
                            await _databaseService.SaveQuestAsync(quest);
                            seedCount++;
                            System.Diagnostics.Debug.WriteLine($"✓ Seeded: {quest.Title}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"✗ Failed to seed {quest.Title}: {ex.Message}");
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"Seed Complete: {seedCount}/{standardQuests.Count} quests seeded");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("System quests already seeded, skipping seed operation");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during seed: {ex.Message}");
                throw;
            }
        }
    }
}

    

