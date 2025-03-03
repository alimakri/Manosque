using ComlineApp.Manager;
using ComLineCommon;
using ComLineConsoleApp;
using ComlineServices;
using Microsoft.Extensions.DependencyInjection;

// Options Ali
Global.WorkingDirectory_ServiceData = Global.WorkingDirectory_ServiceSystem = @"D:\Manosque\extra";

// Options Thierno
//ServiceData.WorkingDirectory = ServiceSystem.WorkingDirectory = @"D:\Manosque\extra";

ServiceSystem.Options.Add("Service", "System");
ServiceSystem.Options.Add("DisplayMode", "Normal");

ServiceData.ConnectionString = "Server=.\\SQLEXPRESS;Database=ManosqueBD;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

var serviceProvider = new ServiceCollection()
    .AddSingleton<CoreComline>()
    .BuildServiceProvider();
var comLine = serviceProvider.GetService<CoreComline>();

if (comLine == null) return;

// La console
comLine.Command.Prompts = [Global.Prompt];
var maConsole = new ConsoleComline(comLine);

// GO !
maConsole.Launch();