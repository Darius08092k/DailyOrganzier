using DailyOrganzier.Services;
using DailyOrganzier.Services.Interfaces;
using DailyOrganzier.ViewModels;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace DailyOrganzier
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // 1. Register Game Engine (Service)
            builder.Services.AddSingleton<IStatsService, StatsService>();
            builder.Services.AddSingleton<IQuestPopupService, QuestPopupService>();

            // 2. Register Presentation Logic (ViewModels)
            builder.Services.AddTransient<MainViewModel>();

            // 3. Register UI (Views)
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
