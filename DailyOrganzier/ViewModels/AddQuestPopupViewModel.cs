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
        public string ErrorType { get => _errorType; 
            set
            {
                _errorType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ErrorType"));
            }
        }

        private QuestCategory? _selectedCategory;
        public QuestCategory? SelectedCategory { get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedCategory"));
            }
        }

        private uint? _xpEntry;
        public uint? XpEntry
        {
            get => _xpEntry;
            set
            {
                _xpEntry = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("XpEntry"));
            }
        }

        private string _title;
        public string Title { get => _title; 
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            }
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible { get => _isErrorVisible;
            set {
                _isErrorVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsErrorVisible"));
            }
        }

        // Commands
        public ICommand SaveNewQuestCommand { get; }

        // Actions 
        public Action<Quest> ClosePopupAction { get; set; }


        public AddQuestPopupViewModel()
        {
            SaveNewQuestCommand = new Command(SaveNewQuest);
        }

        // TODO: Add validation for the TitleEntry and XpEntry fields, and display an error message if the input is invalid.
        private void SaveNewQuest(object obj)
        {
            if (Title == null || Title.Trim() == "" )
            {
                ErrorType = "Titleesesese cannot be empty.";
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
                Title = Title.Trim(),
                XpReward = (int)XpEntry,
                Type = SelectedCategory?.ToString(),
            };
            ClosePopupAction?.Invoke(newQuest);
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
