using ComlineApp.Manager;
using ComlineApp.Services;
using ComLineCommon;
using ComLineConsoleApp;
using Microsoft.Extensions.DependencyInjection;

// Options
ServiceSystem.Options.Add("Service", "System");
ServiceSystem.Options.Add("DisplayMode", "Normal");

// Injection de dépendance : IServiceData = ServiceData de Manosque dans Comline
var serviceProvider = new ServiceCollection()
    .AddTransient<IServiceData, Manosque.ServiceData.ServiceData>()
    .AddTransient<CoreComline>()
    .BuildServiceProvider();
var comLine = serviceProvider.GetService<CoreComline>();
if (comLine == null) return;

// La console
var maConsole = new ConsoleComline(comLine)
{
    // Initialiser le script de démarrage
    Prompts = [Global.Prompt]
};

// GO !
maConsole.Launch();