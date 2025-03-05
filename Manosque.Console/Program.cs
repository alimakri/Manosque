using ComlineApp.Manager;
using ComLineCommon;
using ComLineConsoleApp;
using ComlineServices;
using Microsoft.Extensions.DependencyInjection;

// WorkingDirectories
Global.WorkingDirectory_ServiceData = Global.WorkingDirectory_ServiceSystem = @"D:\Manosque\extra";  // ali
// Global.ServiceData.WorkingDirectory = Global.ServiceSystem.WorkingDirectory = @"D:\Manosque\extra"; // Thierno

// ServiceSystem
ServiceSystem.Options.Add("Service", "System");
ServiceSystem.Options.Add("DisplayMode", "Normal");

// ServiceData
ServiceData.ConnectionString = "Server=.\\SQLEXPRESS;Database=ManosqueBD;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

// ServiceApi
ServiceApi.Url = "https://makrisoft.net/";

// Singleton CoreComline instancié ici
var serviceProvider = new ServiceCollection()
    .AddSingleton<CoreComline>()
    .BuildServiceProvider();
var comLine = serviceProvider.GetService<CoreComline>();

if (comLine == null) return;

// Scenario
comLine.Command.Prompts = []; // "Execute-File -Name scenario9.ps1";

// GO !
new ConsoleComline(comLine).Launch();