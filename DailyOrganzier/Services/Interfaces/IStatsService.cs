using DailyOrganzier.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DailyOrganzier.Services.Interfaces
{
    public interface IStatsService
    {
        public UserStats Stats { get; set; }
        public ObservableCollection<Quest> ActiveQuests { get; set; }
        public void CompleteQuest(Quest quest);
    }
}
