using DailyOrganzier.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.HelperClasses.Enums
{
    public interface IDailyQuests
    {
        Task SeedSystemQuestsAsync();
        Task AssignRandomQuestsAsync(int numberOfDailyQuests);
    }
}
