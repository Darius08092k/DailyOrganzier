using DailyOrganzier.Data;
using DailyOrganzier.Models;
using DailyOrganzier.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.Services
{
    public class LocalDatabaseService : ILocalDatabaseService
    {
        private readonly AppDbContext _context;
        public LocalDatabaseService()
        {
            _context = new AppDbContext();
        }

        public async Task<List<Quest>> GetQuestsAsync()
        {
            return await _context.Quests.ToListAsync();
        }

        public async Task<int> SaveQuestAsync(Quest quest)
        {
            var existingQuest = await _context.Quests.FindAsync(quest.Id);

            if(existingQuest != null)
            {
                _context.Entry(existingQuest).CurrentValues.SetValues(quest);
            }
            else
            {
                _context.Quests.Add(quest);
            }

            return await _context.SaveChangesAsync();
        }


        public async Task<int> DeleteQuestAsync(Quest quest)
        {
            _context.Quests.Remove(quest);
            return await _context.SaveChangesAsync();
        }
    }
}
