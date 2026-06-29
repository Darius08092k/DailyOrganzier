using CommunityToolkit.Maui.Extensions;
using DailyOrganzier.Models;
using DailyOrganzier.ViewModels;
using DailyOrganzier.Views;

namespace DailyOrganzier
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();

            // Assign the injected ViewModel to the page's BindingContext
            BindingContext = viewModel;
        }

        
    }
}
