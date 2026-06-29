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

        // Variables
        public string TodayDate => DateTime.Now.ToString("dd MMM yyyy").ToUpper();
        // Commands
        public ICommand CompleteQuestCommand { get; }
        public ICommand AddQuestCommand { get; }

        private readonly IStatsService _statsService;
        private readonly IQuestPopupService _questPopupService;

        public MainViewModel(IStatsService statsService, IQuestPopupService questPopupService)
        {
            _statsService = statsService;
            _questPopupService = questPopupService;

            // Insert mock data
            _statsService.ActiveQuests = new ObservableCollection<Quest>
            {
                new Quest { Title = "Make your bed", Type = "CHORES", XpReward = 5 },
                new Quest { Title = "Daily workout", Type = "FITNESS", XpReward = 10 },
                new Quest { Title = "Clean Setup", Type = "CHORES", XpReward = 10 }
            };

            // Wire up the command to execute the service's method
            CompleteQuestCommand = new Command<Quest>(_statsService.CompleteQuest);

            AddQuestCommand = new Command(async () =>
            {
                // 1. Await the service directly and capture the result
                var newQuest = await _questPopupService.ShowAddQuestPopup();

                // 2. Check if the user actually created a quest (didn't hit cancel)
                if (newQuest != null)
                {
                    // 3. Add it to the list
                    _statsService.ActiveQuests.Add(newQuest);
                }
            });
        }

    }
}
