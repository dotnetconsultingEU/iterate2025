// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.ServiceAndInterfaces;
using Microsoft.Extensions.DependencyInjection;
using dotnetconsulting.SMTPService;
using dotnetconsulting.GUI.SmtpDemoServices;
using System.Diagnostics;

namespace dotnetconsulting.GUI;

public class Application(IServiceProvider services)
{
    private readonly IServiceProvider services = services;

    public void Run()
    {
        Debugger.Break();

        Console.WriteLine("== Bestellungen ==");

        IOrderService orderService = services.GetRequiredService<IOrderService>();

        orderService.PlaceOrder("Wattestäbchen", 10);
        orderService.PlaceOrder("Taschentuch", 1);
        orderService.PlaceOrder("Ölfaß", 10);

        orderService.Sender = "tkansy@dotnetconsulting.eu";

        Console.WriteLine();

        // Unterschiedliche SMTP-Services nutzen, je nach Environment
        Console.WriteLine("== MockedSmtpService ==");
        // Besser GetRequiredService()?
        MockedSmtpService? mockedSmtpService = services.GetService<MockedSmtpService>();
        mockedSmtpService?.Send("ente@hausen.de", "Wichtig!", "...");

        Console.WriteLine("== FastSmtpService ==");
        FastSmtpService? fastSmtpService = services.GetService<FastSmtpService>();
        fastSmtpService?.Send("ente@hausen.de", "Wichtig!", "...");

        // Via Interface. Leider nein. ISmtpService ist nicht registiert
        Console.WriteLine("== ISmtpService ==");

        // Liefert NULL, wenn der Service nicht geliefert warden kann
        ISmtpService? iSmtpService = services.GetService<ISmtpService>();
        iSmtpService?.Send("ente@hausen.de", "Wichtig!", "...");

        // Wirft eine Exception (System.InvalidOperationException) statt NULL zuliefern
        iSmtpService = services.GetRequiredService<ISmtpService>();
        iSmtpService.Send("ente@hausen.de", "Wichtig!", "...");

        // Diese Komponente hat eine fehlende Abhängigkeit => Exception (immer!)
#pragma warning disable IDE0059 // Unnecessary assignment of a value
        IUseMissing? useMissing = services.GetService<IUseMissing>();
#pragma warning restore IDE0059 // Unnecessary assignment of a value

        // Scope erzeugen (via ASP.NET Core als Request)
        using (IServiceScope scope = services.CreateScope())
        {
            ISmtpService? smtpServiceScope = scope.ServiceProvider.GetService<ISmtpService>();
            IOrderService? orderServiceScope = services.GetService<IOrderService>();

            // ...
        }

        // Keyed Services
#pragma warning disable IDE0059 // Unnecessary assignment of a value
        var keyedIOrderService1 = services.GetRequiredKeyedService<IOrderService>("email");
        var keyedIOrderService2 = services.GetRequiredKeyedService<IOrderService>("snail");
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        Debugger.Break();
    }
}