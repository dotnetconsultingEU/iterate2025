// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.IoCConsole;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

// Konfiguration
// Microsoft.Extensions.Configuration
ConfigurationBuilder configurationBuilder = new();
// Microsoft.Extensions.Configuration.Json
configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("application.json");
IConfigurationRoot configurationRoot = configurationBuilder.Build();

#if DEBUG
var configDebug = configurationRoot.GetDebugView();
Debugger.Break();
#endif

// Service Container (IoC)
IServiceCollection serviceCollection = new ServiceCollection();

// Microsoft.Extensions.Options.ConfigurationExtensions
serviceCollection.Configure<TestConfig>(configurationRoot.GetSection("TestConfig"));

serviceCollection.AddScoped<IConfiguration>(_ => configurationRoot);
serviceCollection.AddOptions<TestConfig>()
    .BindConfiguration("TestConfig");

serviceCollection.AddTransient<TestService>();

// Logging
// Microsoft.Extensions.Logging
serviceCollection.AddLogging(l =>
{
    l.AddConfiguration(configurationRoot.GetSection("Logging"))
     .AddDebug()
     .AddConsole()
     .AddMyNLog();
});

IServiceProvider services = serviceCollection.BuildServiceProvider();

// Start
TestService test = services.GetRequiredService<TestService>();
