using DailyOrganzier.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.Services.Interfaces
{
    public interface IQuestPopupService
    {
        Task<Quest> ShowAddQuestPopup();
    }
}
