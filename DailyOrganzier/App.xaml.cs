using DailyOrganzier.Services;
using DailyOrganzier.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DailyOrganzier
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();

            // Initialize the database service on app startup
            try
            {
                var statsService = IPlatformApplication.Current?.Services.GetService<IStatsService>();
                if (statsService is StatsService service)
                {
                    await service.InitializeAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing database on app start: {ex.Message}");
            }
        }
    }
}
