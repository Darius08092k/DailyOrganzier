using System;
using DailyOrganzier.Models;
using CommunityToolkit.Maui.Views;

namespace DailyOrganzier.Views;

// The explicit path below (CommunityToolkit.Maui.Views.Popup) is what fixes the 'Close' error.
public partial class AddQuestPopup : Popup<Quest>
{
    public AddQuestPopup()
    {
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync(null);
    }
    // TODO: Add validation for the TitleEntry and XpEntry fields, and display an error message if the input is invalid.
    // TODO: Add a check to ensure that the XpEntry field is a valid integer, and display an error message if it is not.
    // TODO: Add a check to ensure that the XpEntry field is not negative, and display an error message if it is.
    // TODO: Add a check to ensure that the TitleEntry field is not empty, and display an error message if it is.
    // TODO: Add a chekc to ensure that the XP is not too high, and display an error message if it is.
    private async void OnSaveClicked(object sender, EventArgs e)
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
            Type = CategoryPicker.SelectedItem.ToString(),
            XpReward = xp
        };

        await CloseAsync(newQuest);
    }
}