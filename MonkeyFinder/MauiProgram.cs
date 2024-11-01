using Microsoft.Extensions.Logging;
using MonkeyFinder.MonkeyAPI;
using MonkeyFinder.Services.MonkeyServices;
using MonkeyFinder.Views;

namespace MonkeyFinder
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register HttpClient
            builder.Services.AddHttpClient<MonkeyHttpClient>(client =>
            {
                client.BaseAddress = new Uri("https://raw.githubusercontent.com/jamesmontemagno/");
            });

            // Register Services
            builder.Services.AddSingleton<IMonkeyService, MonkeyService>();

            // Register ViewModels
            builder.Services.AddSingleton<MonkeysViewModel>();
            builder.Services.AddTransient<MonkeyDetailsViewModel>();

            // Register Pages
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<DetailsPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
