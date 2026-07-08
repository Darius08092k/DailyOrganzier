using DailyOrganzier.Data;
using DailyOrganzier.Models;
using DailyOrganzier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DailyOrganzier.Services
{
    public class LocalDatabaseService : ILocalDatabaseService
    {
        private readonly AppDbContext _appDbContext;
        public LocalDatabaseService()
        {
            _appDbContext = new AppDbContext();
        }

        public async Task<List<Quest>> GetQuestsAsync()
        {
            return await _appDbContext.Quests.ToListAsync();
        }

        public async Task InitializeAsync()
        {
            await _appDbContext.Database.EnsureCreatedAsync();
        }


        public async Task<int> DeleteQuestAsync(Quest quest)
        {
            _appDbContext.Quests.Remove(quest);
            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> SaveQuestAsync(Quest quest)
        {
            if (quest.Id != 0)
            {
                // If the ID is not 0, it already exists in the database. Update it.
                var existingQuest = await _appDbContext.Quests.FindAsync(quest.Id);
                if (existingQuest != null)
                {
                    _appDbContext.Entry(existingQuest).CurrentValues.SetValues(quest);
                }
            }
            else
            {
                // If the ID is 0, it's a new custom quest. Add it.
                _appDbContext.Quests.Add(quest);
            }

            return await _appDbContext.SaveChangesAsync();
        }
    }
}
