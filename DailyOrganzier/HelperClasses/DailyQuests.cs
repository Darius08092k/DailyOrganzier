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
        private readonly List<Quest> _standardQuests;
        private readonly Random _random;

        public DailyQuests()
        {
            _random = new Random();

            // Insert mock data
            _standardQuests = new List<Quest>
            {
                new Quest { Title = "Make bed", Type = QuestCategory.CHORE.ToString(), XpReward = 15 },
                new Quest { Title = "Clean setup", Type = QuestCategory.CHORE.ToString(), XpReward = 15 },
                new Quest { Title = "10 minute workout", Type = QuestCategory.FITNESS.ToString(), XpReward = 15 },
                new Quest { Title = "10 minute walk", Type = QuestCategory.FITNESS.ToString(), XpReward = 15 },
                new Quest { Title = "30 minute book reading", Type = QuestCategory.LEARNING.ToString(), XpReward = 15 },
                new Quest { Title = "30 minute learning", Type = QuestCategory.LEARNING.ToString(), XpReward = 15 },
                new Quest { Title = "10 minute mind relaxation", Type = QuestCategory.MINDFULNESS.ToString(), XpReward = 15 }
            };
        }
        // Method to get a random quest from the list
        public List<Quest> GetRandomQuest(int numberOfDailyQuests)
        {
            // Make a copy of our master list
            var shuffledPool = new List<Quest>(_standardQuests);

            // Shuffle the list using Fisher-Yates shuffle algorithm
            for (int i = shuffledPool.Count - 1; i > 0; i--)
            {
                // Pick a random index from 0 to i
                int j = _random.Next(i + 1);

                // Swap the items at index i and index j
                var temp = shuffledPool[i];
                shuffledPool[i] = shuffledPool[j];
                shuffledPool[j] = temp;
            }

            return shuffledPool.Take(numberOfDailyQuests).ToList();
        }
    }
}

    

