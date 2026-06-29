using System;
using DailyOrganzier.Models;

namespace DailyOrganzier.Views;

// The explicit path below (CommunityToolkit.Maui.Views.Popup) is what fixes the 'Close' error.
public partial class AddQuestPopup : CommunityToolkit.Maui.Views.Popup
{
    public AddQuestPopup()
    {
        InitializeComponent();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(null);
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            return;
        }

        int xp = 10;
        if (!string.IsNullOrWhiteSpace(XpEntry.Text) && int.TryParse(XpEntry.Text, out int parsedXp))
        {
            xp = parsedXp;
        }

        var newQuest = new Quest
        {
            Id = Guid.NewGuid().ToString(),
            Title = TitleEntry.Text,
            XpReward = xp
        };

        Close(newQuest);
    }
}