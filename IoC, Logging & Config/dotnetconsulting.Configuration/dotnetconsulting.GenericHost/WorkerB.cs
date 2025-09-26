// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace dotnetconsulting.GenericHost;

internal class WorkerB(IOptions<MyAppSettings> MyAppSettings) 
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Without ValidateOnStart() OptionsValidationException is thrown when configuration is invalid
        var myAppSettings = MyAppSettings.Value;

        Console.WriteLine($"ApiUrl: {myAppSettings.ApiUrl}");
        Console.WriteLine($"Timeout: {myAppSettings.Timeout}");
        Console.WriteLine($"MaxRetryCount: {myAppSettings.MaxRetryCount}");

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"B: {DateTime.Now}");

            await Task.Delay(1000 * 60, cancellationToken);
        }
    }
}