using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DailyOrganzier.Models
{
    public class UserStats : INotifyPropertyChanged
    {
        public string UserId { get; set; }

        private int _currentLevel = 1;
        private int _currentXp = 0;
        private int _xpToNextLevel = 100;
        public int CurrentLevel
        {
            get => _currentLevel;
            set { _currentLevel = value; OnPropertyChanged(); }
        }

        public int CurrentXp
        {
            get => _currentXp;
            set { _currentXp = value; OnPropertyChanged(); OnPropertyChanged(nameof(XpProgress)); }
        }

        public int XpToNextLevel
        {
            get => _xpToNextLevel;
            set { _xpToNextLevel = value; OnPropertyChanged(); OnPropertyChanged(nameof(XpProgress)); }
        }

        public double XpProgress => (double)CurrentXp / XpToNextLevel;
        public UserStats()
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
