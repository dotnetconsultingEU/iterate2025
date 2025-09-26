// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschr�nkungen verwendet oder ver�ndert werden.
// Jedoch wird keine Garantie �bernommen, dass eine Funktionsf�higkeit mit aktuellen und 
// zuk�nftigen API-Versionen besteht. Der Autor �bernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgef�hrt wird.
// F�r Anregungen und Fragen stehe ich jedoch gerne zur Verf�gung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.ASPNETHost;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Console.WriteLine("=== Sources ===");
foreach (var source in builder.Configuration.Sources)
{
    if (source is MemoryConfigurationSource mcs)
    {
        Console.WriteLine($"[Program.cs] Memory: {source}");
    }
    else if (source is EnvironmentVariablesConfigurationSource evcs)
    {
        Console.WriteLine($"[Program.cs] Environment var: {evcs.Prefix}");
    }
    else if (source is JsonConfigurationSource jcs)
    {
        Console.WriteLine($"[Program.cs] Json: {jcs.Path}");
    }
    else if (source is CommandLineConfigurationSource ccs)
    {
        Console.WriteLine($"[Program.cs] CommandLine: {ccs.Args}");
    }
    else if (source is FileConfigurationSource fcs)
    {
        Console.WriteLine($"[Program.cs] File: {fcs.Path}");
    }
    else
        Console.WriteLine($"[Program.cs] Unkown: {source}");
}

Console.WriteLine("=== Values ===");
var configValues = builder.Configuration.GetDebugView();
Console.WriteLine(configValues);

Console.WriteLine("=== Single value by ':' notation===");
builder.Configuration.AddJsonFile("myAppSessions.json", optional: true, reloadOnChange: true);
builder.Configuration.AsEnumerable().ToList().ForEach(x => Console.WriteLine($"{x.Key}"));
string? configValue = builder.Configuration["Section1:SubSection1:SubSubSection1:Value1"];
Console.WriteLine($"[Progam.cs] configValue: {configValue ?? "<null>"}");

builder.Services.AddOptions<MyAppSettings>()
    .BindConfiguration("AppSettings")
    .ValidateDataAnnotations()    
    .ValidateOnStart()
    ;
// Manuelle Validierung
// builder.Services.AddSingleton<IValidateOptions<MyAppSettings>, MyAppSettingsValidator>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", (IOptions<MyAppSettings> MyAppSettings) => $"Hello World! ({MyAppSettings.Value.ApiUrl})");

app.Run();
