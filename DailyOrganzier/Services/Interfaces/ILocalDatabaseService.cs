using DailyOrganzier.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.Services.Interfaces
{
    public interface ILocalDatabaseService
    {
        public Task InitializeAsync();
        public Task<List<Quest>> GetQuestsAsync();
        public Task<int> SaveQuestAsync(Quest quest);
        public Task<int> DeleteQuestAsync(Quest quest);
    }
}
