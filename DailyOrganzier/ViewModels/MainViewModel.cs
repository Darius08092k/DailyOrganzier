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

        public MainViewModel(IStatsService statsService, IQuestPopupService questPopupService)
        {
            _statsService = statsService;
            _questPopupService = questPopupService;

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
                    // 3. Add it to the list using the async method
                    await _statsService.AddQuestAsync(newQuest);
                }
            });

            // Initialize quests from database
            Task.Run(async () => await _statsService.InitializeAsync());
        }
    }
}
