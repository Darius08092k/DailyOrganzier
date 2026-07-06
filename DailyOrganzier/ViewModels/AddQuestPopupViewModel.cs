using DailyOrganzier.Models;
using DailyOrganzier.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace DailyOrganzier.ViewModels
{
    public class AddQuestPopupViewModel : INotifyPropertyChanged
    {
        private string _errorType;
        public string ErrorType 
        { 
            get => _errorType; 
            set => SetProperty(ref _errorType, value, nameof(ErrorType));
        }

        private QuestCategory? _selectedCategory;
        public QuestCategory? SelectedCategory 
        { 
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value, nameof(SelectedCategory));
        }

        private uint? _xpEntry;
        public uint? XpEntry
        {
            get => _xpEntry;
            set => SetProperty(ref _xpEntry, value, nameof(XpEntry));
        }

        private string _title;
        public string Title 
        { 
            get => _title; 
            set => SetProperty(ref _title, value, nameof(Title));
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible 
        { 
            get => _isErrorVisible;
            set => SetProperty(ref _isErrorVisible, value, nameof(IsErrorVisible));
        }

        // Commands
        public ICommand SaveNewQuestCommand { get; }

        // Actions 
        public Action<Quest> ClosePopupAction { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public AddQuestPopupViewModel()
        {
            SaveNewQuestCommand = new Command(SaveNewQuest);
        }

        /// <summary>
        /// Helper method to set property values and raise PropertyChanged event
        /// </summary>
        private void SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // TODO: Add validation for the TitleEntry and XpEntry fields, and display an error message if the input is invalid.
        private void SaveNewQuest(object obj)
        {
            if (Title == null || Title.Trim() == "" )
            {
                ErrorType = "Title cannot be empty.";
                IsErrorVisible = true;
                return;
            }

            if(Title.Trim().All(char.IsDigit))
            {
                ErrorType = "Title cannot contain only digits.";
                IsErrorVisible = true;
                return;
            }

            if (XpEntry == null || XpEntry == 0)
            {
                ErrorType = "XP reward cannot be zero.";
                IsErrorVisible = true;
                return;
            }

            if (XpEntry > 15)
            {
                ErrorType = "Cannot add such high XP.";
                IsErrorVisible = true;
                return;
            }

            IsErrorVisible = false;
            var newQuest = new Quest
            {
                Id = Guid.NewGuid().ToString(),
                Title = Title.Trim(),
                XpReward = (int)XpEntry,
                Type = SelectedCategory?.ToString(),
            };
            ClosePopupAction?.Invoke(newQuest);
        }
    }
}
