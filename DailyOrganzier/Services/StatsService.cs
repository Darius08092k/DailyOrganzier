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
        

        public UserStats Stats { get; set; }
        public ObservableCollection<Quest> ActiveQuests { get; set; }
        public StatsService()
        {
            Stats = new UserStats();
            ActiveQuests = new ObservableCollection<Quest>();
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

        }
    }
}
