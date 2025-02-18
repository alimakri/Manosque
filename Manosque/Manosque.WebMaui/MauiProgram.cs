using ComlineApp.Manager;
using ComlineApp.Services;
using Microsoft.Extensions.Logging;

namespace Manosque.WebMaui
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IServiceData, Manosque.ServiceData.ServiceData>();
            builder.Services.AddSingleton<CoreComline>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            ServiceSystem.Options.Add("Service", "Data");

            return builder.Build();
        }
    }
}
