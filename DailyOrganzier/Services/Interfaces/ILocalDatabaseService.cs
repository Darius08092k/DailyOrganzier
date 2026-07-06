using DailyOrganzier.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.Services.Interfaces
{
    public interface ILocalDatabaseService
    {
        Task<List<Quest>> GetQuestsAsync();
        Task<int> SaveQuestAsync(Quest quest);
        Task<int> DeleteQuestAsync(Quest quest);
    }
}
