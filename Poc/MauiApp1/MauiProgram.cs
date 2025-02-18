using ComlineApp.Manager;
using ComlineApp.Services;
using MauiApp1.ViewModels;
using Microsoft.Extensions.Logging;

namespace MauiApp1
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
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                    fonts.AddFont("FontAwesome6-Brands.otf", "FA6Brands");
                    fonts.AddFont("FontAwesome6-Regular.otf", "FA6Regular");
                });
            // Enregistrer ServiceApi en tant que singleton pour IServiceApi
            builder.Services.AddSingleton<IServiceApi>(sp => new ServiceApi(sp.GetRequiredService<ComlineData>()));
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
