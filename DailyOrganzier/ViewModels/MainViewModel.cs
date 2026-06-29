using DailyOrganzier.Models;
using DailyOrganzier.Services;
using DailyOrganzier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace DailyOrganzier.ViewModels
{
    public class MainViewModel
    {
        // Data exposed directly from the Service
        public UserStats Stats => _statsService.Stats;
        public ObservableCollection<Quest> ActiveQuests => _statsService.ActiveQuests;

        // Commands
        public ICommand CompleteQuestCommand { get; }

        private readonly IStatsService _statsService;
        public MainViewModel(IStatsService statsService)
        {
            _statsService = statsService;
            
            // Insert mock data
            _statsService.ActiveQuests = new ObservableCollection<Quest>
            {
                new Quest { Title = "Resolve a bug", Type = "CODING", XpReward = 50 },
                new Quest { Title = "Go for a run", Type = "FITNESS", XpReward = 75 },
                new Quest { Title = "Edit TikTok", Type = "CREATIVE", XpReward = 100 },
                new Quest { Title = "Clean Setup", Type = "CHORES", XpReward = 30 }
            };

            // Wire up the command to execute the service's method
            CompleteQuestCommand = new Command<Quest>(_statsService.CompleteQuest);
        }

    }
}
