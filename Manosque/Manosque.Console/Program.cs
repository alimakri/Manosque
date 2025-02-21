using ComlineApp.Manager;
using ComlineApp.Services;
using ComLineCommon;
using ComLineConsoleApp;
using Manosque.ServiceData;
using Microsoft.Extensions.DependencyInjection;

// Options
ServiceSystem.Options.Add("Service", "System");
ServiceSystem.Options.Add("DisplayMode", "Normal");

// Injection de dépendance : IServiceData = ServiceData de Manosque dans Comline
var serviceProvider = new ServiceCollection()
    .AddTransient<IServiceData>(provider => new ServiceData("Server=.\\SQLEXPRESS;Database=ManosqueBD;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"))
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