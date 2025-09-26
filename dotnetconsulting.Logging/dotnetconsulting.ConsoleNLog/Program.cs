﻿// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.ConsoleNLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using static System.Console;

// IoC & DI konfigurieren
IServiceCollection serviceCollection = new ServiceCollection();
IServiceProvider serviceProvider = ConfigureServices(serviceCollection);

// Demp App starten
Application application = serviceProvider.GetRequiredService<Application>();

application.Run();

WriteLine("== Fertig ==");
ReadKey();

static IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
{
    // IoC konfigurieren
    serviceCollection.AddTransient<Application>();

    // Factory
    serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();

    // NLog konfigurieren
    string configFileName = Path.Combine(AppContext.BaseDirectory, "nlog.config");
    NLog.LogManager.LoadConfiguration(configFileName);

    // Generischer Logger
    serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
    serviceCollection.AddLogging((builder) =>
    {
        builder.SetMinimumLevel(LogLevel.Trace)
               .AddConsole()
               .AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
    });

    // ServiceProvider erstellen und zurückgeben
    ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
    return serviceProvider;
}