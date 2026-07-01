using CommunityToolkit.Maui.Views;
using DailyOrganzier.Models;
using DailyOrganzier.ViewModels;
using System;

namespace DailyOrganzier.Views;

// The explicit path below (CommunityToolkit.Maui.Views.Popup) is what fixes the 'Close' error.
public partial class AddQuestPopup : Popup<Quest>
{
    public AddQuestPopup()
    {
        InitializeComponent();

        var viewModel = new AddQuestPopupViewModel();
        viewModel.ClosePopupAction = async (quest) =>
        {
            await CloseAsync(quest);
        };
        BindingContext = viewModel;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync(null);
    }

    
}