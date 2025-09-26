// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

#nullable disable

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace dotnetconsulting.ASPNETHost;

public class MyAppSettings
{
    [Required]
    public string ApiUrl { get; set; }
    public TimeSpan Timeout { get; set; }
    [Range(1, 10)]
    public int MaxRetryCount { get; set; }
}

[OptionsValidator]
public partial class MyAppSettingsValidator : IValidateOptions<MyAppSettings>
{
    ValidateOptionsResult IValidateOptions<MyAppSettings>.Validate(string name, MyAppSettings options)
    {
        //// Bei Bedarf auf Service Container zugreifen
        //var context = new ValidationContext(options);
        //var service1 = context.GetService<object>();
        //var service2 = context.GetRequiredService<object>();

        bool isHttps = options.ApiUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase);
        if (!isHttps)
        {
            return ValidateOptionsResult.Fail("ApiUrl must start with https://");
        }

        return ValidateOptionsResult.Success;
    }
}