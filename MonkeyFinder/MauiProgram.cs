using Microsoft.Extensions.Logging;
using MonkeyFinder.MonkeyAPI;
using MonkeyFinder.Services.MonkeyServices;

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

            // Add HttpClient
            builder.Services.AddHttpClient<MonkeyHttpClient>(client =>
            {
                client.BaseAddress = new Uri("https://raw.githubusercontent.com/jamesmontemagno/");
            });

            // Add Services
            builder.Services.AddSingleton<IMonkeyService, MonkeyService>();

            // Add ViewModels
            builder.Services.AddSingleton<MonkeysViewModel>();

            // Add Pages
            builder.Services.AddSingleton<MainPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
