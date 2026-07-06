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
        private bool _isInitialized = false;

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
        }

        /// <summary>
        /// Initialize the view model with data from the database.
        /// This should be called from the View's Loaded event or from App startup.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;
            await _statsService.InitializeAsync();
        }

        /// <summary>
        /// Debug command to reset and reseed the database
        /// </summary>
        public async Task ResetDatabaseAsync()
        {
            _isInitialized = false; // Allow re-initialization
            await _statsService.ResetAndReseedDatabaseAsync();
        }
    }
}

