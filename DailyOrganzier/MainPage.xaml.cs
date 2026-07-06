using CommunityToolkit.Maui.Extensions;
using DailyOrganzier.Models;
using DailyOrganzier.ViewModels;
using DailyOrganzier.Views;

namespace DailyOrganzier
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            // Assign the injected ViewModel to the page's BindingContext
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Initialize the ViewModel with data from the database when the page appears
            if (_viewModel != null)
            {
                await _viewModel.InitializeAsync();
            }
        }
    }
}
