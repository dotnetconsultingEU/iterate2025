// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace dotnetconsulting.ServiceAndInterfaces;

public class SnailMailOrderService(/*[FromKeyedServices("email")]*/IPostageService PostageService) : IOrderService
{
    private readonly IPostageService postageService = PostageService;

    public string Sender { get; set; } = null!;

    public void PlaceOrder(string Article, int quantity)
    {
        Console.WriteLine($"Bestellen {quantity:N0} von {Article} via Post von {Sender}");
        Debug.Print($"Bestellen {quantity:N0} von {Article} via Post von {Sender}");

#pragma warning disable IDE0059 // Unnecessary assignment of a value
        Guid stampGuid = postageService.GetStamp(1.00m);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
    }
}

public class EMailOrderService(string SmtpHost) : IOrderService
{
    private readonly string smtpHost = SmtpHost;

    public string Sender { get; set; } = null!;

    public void PlaceOrder(string Article, int quantity)
    {
        Console.WriteLine($"Bestellen {quantity:N0} von {Article} via EMail ({smtpHost}) von {Sender}");
        Debug.Print($"Bestellen {quantity:N0} von {Article} via EMail ({smtpHost}) von {Sender}");
    }
}

public class GermanPostageService(IPayment Payment) : IPostageService
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly IPayment _payment = Payment;

    public Guid GetStamp(decimal Amount)
    {
        Console.WriteLine($"GetStamp {Amount:N2}");
        Debug.Print($"GetStamp {Amount:N2}");

        return Guid.NewGuid();
    }
}

public class PayPal : IPayment
{
    public void Pay(decimal amount)
    {
        throw new NotImplementedException();
    }
}

public class UseMissing(IMissing missing) : IUseMissing
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly IMissing _missing = missing;
}