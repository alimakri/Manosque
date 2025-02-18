
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main(string[] args)
    {
        // Configurer le service provider
        var serviceProvider = new ServiceCollection()
            .AddTransient<IServiceData, ServiceData1>() 
            .AddTransient<ComLine>()
            .BuildServiceProvider();

        // Résoudre et exécuter ComLine
        var comLine = serviceProvider.GetService<ComLine>();

        var c = new Console(comLine);
        c.Launch();

        System.Console.ReadLine();
    }
}
public class Console
{
    public ComLine ComLine { get; set; }
    public Console(ComLine comline)
    {
        ComLine = comline;
    }
    public void Launch()
    {
        ComLine.Execute();

    }
}
public class ComLine
{
    public IServiceData MonServiceData { get; set; }
    public ComLine(IServiceData serviceData)
    {
        MonServiceData = serviceData;
    }
    public void Execute()
    {
        MonServiceData.Execute();
    }
}
public interface IServiceData
{
    void Execute();
}
public class ServiceData1 : ServiceDataBase, IServiceData
{
    public override void Execute()
    {
        System.Console.WriteLine("ServiceData1");
    }
}
public class ServiceData2 : ServiceDataBase, IServiceData
{
    public override void Execute()
    {
        System.Console.WriteLine("ServiceData2");
    }
}
public class ServiceDataBase
{
    public virtual void Execute()
    {
    }
}