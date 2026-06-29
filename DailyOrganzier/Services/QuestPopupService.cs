using CommunityToolkit.Maui.Extensions;
using DailyOrganzier.Models;
using DailyOrganzier.Services.Interfaces;
using DailyOrganzier.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyOrganzier.Services
{
    public class QuestPopupService : IQuestPopupService
    {
        

        async Task<Quest> IQuestPopupService.ShowAddQuestPopup()
        {
            var popup = new AddQuestPopup();

            var result = await Application.Current.MainPage.ShowPopupAsync(popup);

            if(result is Quest quest)
            {
                return quest;
            }
            else
            {
                return null;
            }
        }
    }
}
