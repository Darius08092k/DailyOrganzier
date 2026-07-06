using CommunityToolkit.Maui.Core.Extensions;
using DailyOrganzier.HelperClasses.Enums;
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
        private readonly IDailyQuests _dailyQuests;
        private readonly ILocalDatabaseService _localDatabaseService;

        public MainViewModel(IStatsService statsService, IQuestPopupService questPopupService, IDailyQuests dailyQuests, ILocalDatabaseService localDatabaseService)
        {
            _statsService = statsService;
            _questPopupService = questPopupService;
            _dailyQuests = dailyQuests;
            _localDatabaseService = localDatabaseService;

            // Wire up the command to execute the service's method
            CompleteQuestCommand = new Command<Quest>(async (quest) =>
            {
                _statsService.CompleteQuest(quest);
            });

            AddQuestCommand = new Command(async () =>
            {
                // 1. Await the service directly and capture the result
                var newQuest = await _questPopupService.ShowAddQuestPopup();

                // 2. Check if the user actually created a quest (didn't hit cancel)
                if (newQuest != null)
                {
                    // 3. Add it to the list using the new async method
                    await _statsService.AddQuestAsync(newQuest);
                }
            });

            Task.Run(async () => await LoadQuestsAsync());
        }

        private async Task LoadQuestsAsync()
        {
            var savedQuests = await _localDatabaseService.GetQuestsAsync();
            // If the database is empty (first run), generate random quests and save them
            if(savedQuests == null || savedQuests.Count == 0)
            {
                var randomQuests = _dailyQuests.GetRandomQuest(3);
                foreach (var quest in randomQuests)
                {
                    MainThread.BeginInvokeOnMainThread(async () => 
                    {
                        await _statsService.AddQuestAsync(quest);
                    });
                }
            }
            else
            {
                // Load existing quests from the database
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _statsService.ActiveQuests.Clear();
                    foreach (var quest in savedQuests)
                    {
                        _statsService.ActiveQuests.Add(quest);
                    }
                });
            }
        }


    }
}
