// See https://aka.ms/new-console-template for more information
using dotnetconsulting.GroundUp2.Interfaces;
using dotnetconsulting.GroundUp2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using dotnetconsulting.GroundUp2;

// Konfiguration
ConfigurationBuilder configurationBuilder = new();

configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile($"app2.json");
string env = Environment.UserName;
configurationBuilder.AddJsonFile($"app2.{env}.json", optional: true);
configurationBuilder.AddJsonFile("appSettings.json", optional: false, reloadOnChange: false);
configurationBuilder.AddJsonFile("MyServiceConfig.json", optional: false, reloadOnChange: false);
// configurationBuilder.AddEnvironmentVariables();
configurationBuilder.AddCommandLine(args);

#if DEBUG
configurationBuilder.AddUserSecrets<Program>();
#endif

IConfigurationRoot configurationRoot = configurationBuilder.Build();

#if DEBUG
//var configDebug = configurationRoot.GetDebugView();
//Debugger.Break();
#endif

// Service Container (IoC)
IServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddTransient<IMyService, MyService1>();
// serviceCollection.AddTransient<IRepository, Repository>();
serviceCollection.AddTransient<IMyService, MyService1>();

string reVersion = configurationRoot.GetValue("UsedRepositry", "SQLServer");
IConfigurationSection configurationSection = configurationRoot.GetSection("MyService");

serviceCollection.AddMyService(configurationRoot, "MyService", reVersion);

// serviceCollection.Configure<MyServiceConfig>(configurationRoot.GetSection("MyService"));

serviceCollection.AddLogging(l =>
{
    l.AddConfiguration(configurationRoot.GetSection("Logging"));
    l.AddDebug();
    l.AddConsole();
    l.AddMyNLog();
});
// ...

IServiceProvider services = serviceCollection.BuildServiceProvider();

IMyService myService = services.GetRequiredService<IMyService>();


myService.Test("Hallo");

Console.WriteLine("Hello, World!");
