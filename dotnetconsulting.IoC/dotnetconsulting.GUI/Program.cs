// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

#pragma warning disable CS8321 // Local function is declared but never used

using dotnetconsulting.GUI;
using dotnetconsulting.GUI.SmtpDemoServices;
using dotnetconsulting.ServiceAndInterfaces;
using dotnetconsulting.SMTPService;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using static System.Console;

// Nuget: Autofac.Extensions.DependencyInjection
// Nuget: Autofac

// IoC & DI konfigurieren
IServiceCollection serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);

// ConfigureServiceWithMissingDependency(serviceCollection);

// Demp App starten
ServiceProvider services = serviceCollection.BuildServiceProvider();

ISmtpService? smtpService = services.GetService(typeof(ISmtpService)) as ISmtpService;



Application application = new(services);
application.Run();

WriteLine("== Fertig ==");
ReadKey();


static void ConfigureServices(IServiceCollection serviceCollection)
{
    Debugger.Break();

    // ServiceCollection konfigurieren
    serviceCollection.AddSingleton<IPostageService, GermanPostageService>();
    serviceCollection.AddTransient<IOrderService, SnailMailOrderService>();
    serviceCollection.AddTransient<IPayment, PayPal>();
    
    //serviceCollection.AddSingleton<IOrderService, EMailOrderService>();
    //serviceCollection.AddScoped<IOrderService, EMailOrderService>();
    //serviceCollection.AddTransient<IOrderService, EMailOrderService>();

    serviceCollection.AddSingleton<IOrderService>(new EMailOrderService("127.0.0.1")
    {
        Sender = "tkansy@dotnetconsulting.eu"
    });

    // Komplexerer Ansatz für unterschiedliche SMTP-Server a la EF DbContext
    // Mocked SMTP-Service konfigurieren
    if (IsDevelopment())
        serviceCollection.AddSmtpServer<MockedSmtpService>(o =>
        {
            // Konkrete Werte aus Konfigration, Quelle jedoch beliebig und hier unwichtig
            o.SetHostAndPort("localhost", 25);
            o.Sender = "mocked@dotnetconsultng.eu";
        });
    else
        // Real SMTP-Service konfigurieren
        serviceCollection.AddSmtpServer<FastSmtpService>(o =>
        {
            // Konkrete Werte aus Konfigration, Quelle jedoch beliebig und hier unwichtig
            o.SetHostAndPort("192.168.1.1", 25);
            o.Sender = "produktion@dotnetconsultng.eu";
        });

    // Keyed Services
    serviceCollection.AddKeyedTransient<IOrderService, EMailOrderService>("email");
    serviceCollection.AddKeyedTransient<IOrderService, SnailMailOrderService>("snail");
}

static void ConfigureServiceWithMissingDependency(IServiceCollection serviceCollection)
#pragma warning restore CS8321 // Local function is declared but never used
{
    // Fügt, IUseMissing hinzu, von der eine Abhängigkeit (IMissing) fehlt
    serviceCollection.AddTransient<IUseMissing, UseMissing>();
}

static bool IsDevelopment()
{
    return Debugger.IsAttached && Environment.MachineName == "Skynet2";
}