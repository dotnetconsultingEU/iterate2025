
// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

#pragma warning disable CS8321 // Local function is declared but never used

using dotnetconsulting.CustomConfiguration;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static System.Console;
using dotnetconsulting.SecretConfig;

// Einfaches Beispiel
// SimpleExample();

// Mehere (Json-)Provider
// MultipleJsonProviders();

// Enviroment Beispiel
// EnviromentVarToSelectConfig(args);

// Vollständiges Beispiel
// FullExample(args);

// Custom Provider
// SqlCustomProvider();

// Secret Config
SecretConfiguration();

WriteLine("== Fertig ==");
ReadKey();

static void SimpleExample()
{
    Debugger.Break();

    // Konfiguration vorbereiten & Einsatz bereit machen
    IConfigurationBuilder builder = new ConfigurationBuilder()
        // Umgebungsvariablen hinzufügen
        .AddEnvironmentVariables()
        // Json-Datei hinzufügen
        .AddJsonFile("Config.json");

    // Konfiguration abschließen
    IConfigurationRoot config = builder.Build();

    // Zugriff
    string? windowHeight = config["App:Window:Height"];
    WriteLine($"windowHeight = {windowHeight}");
}

static void MultipleJsonProviders()
{
    Debugger.Break();

    // Konfiguration vorbereiten & Einsatz bereit machen
    IConfigurationRoot config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings2.json")
        .AddJsonFile("appsettings.Development.json")
        .Build();

    string? appName = config["AppName"];
    WriteLine($"AppName = {appName}");
}

static void EnviromentVarToSelectConfig(string[] args)
{
    Debugger.Break();

    // Konfiguration vorbereiten
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        // NuGet: Microsoft.Extensions.Configuration.EnvironmentVariables
        .AddCommandLine(args)
        .AddEnvironmentVariables();

    // Build() liest die Werte
    IConfigurationRoot config = configBuilder.Build();

    // Zugriff
    string? enviroment = config.GetValue("EnviromentName", "Production");
    WriteLine($"enviroment={enviroment}");

    configBuilder
        .SetBasePath(Directory.GetCurrentDirectory())
        // NuGet: Microsoft.Extensions.Configuration.Json
        .AddJsonFile($"appsettings.{config.GetValue("EnviromentName", "Production")}.json");

    config = configBuilder.Build();
    // ...
}

static void FullExample(string[] args)
{
    Debugger.Break();

    // Konfiguration vorbereiten
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        // NuGet: Microsoft.Extensions.Configuration.CommandLine
        .AddCommandLine(args);

    // Build() liest die Werte aller Quellen ein
    IConfigurationRoot config = configBuilder.Build();

    configBuilder
        .SetBasePath(Directory.GetCurrentDirectory())
                // NuGet: Microsoft.Extensions.Configuration.Json
                .AddJsonFile(@"\\Server\Share\globalconfig.json")
        .AddJsonFile($"appsettings.{config.GetValue("Env", "Production")}.json");

    config = configBuilder.Build();

    // Connectionstring abrufen...
    config = configBuilder.Build();
    string? connectionString = config["ConnectionStrings:Main"];

    // ...damit Konfiguration aus Datenbank aktivieren
    config = configBuilder
        // .AddSqlDatabase(connectionString)
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"App:Window:Height", "740"},
            {"App:Window:Width", "900"},
            {"App:Window:Top", "10"},
            {"App:Window:Left", "10"},
            {"App:Window:Title:Color", "Green" }
        })
        // NuGet: Microsoft.Extensions.Configuration.EnvironmentVariables
        .AddEnvironmentVariables()
        // NuGet: Microsoft.Extensions.Configuration.CommandLine
        .AddCommandLine(args)
        // NuGet: Microsoft.Extensions.Configuration.Ini
        .AddIniFile("Config.ini")
        // NuGet: Microsoft.Extensions.Configuration.Xml
        .AddXmlFile("Config.xml")
        // NuGet: Microsoft.Extensions.Configuration.AzureKeyVault
        //.AddAzureKeyVault($"https://{config["Vault"]}.vault.azure.net/",
        //                  config["ClientId"],
        //                  config["ClientSecret"])
        .Build();

    // Nicht definierter Wert
    int smptPort = config.GetValue("Smtp:Port", 25);

    // Einfacher Zugriff (ohne Binder)
    string? conString = config["ConnectionStrings:Main"];

    // JSON-File
    int c = config.GetValue("App:MainWindow:Top", 0);
    // Aus Ini-Datei
    string? wetter = config.GetValue<string>("IniSection:Wetter");
    // Aus XML-Datei
    // Konfiguration auf Enum mappen
    PaperOrientation paperOrientation = config.GetValue("setting:Orientation:value", PaperOrientation.Portrait);
    // Umgebungsvariabel
    string? systemdrive = config.GetValue("SystemDrive", "X:");
    // Programm-Argumente
    string? enviroment = config.GetValue("Env", "Production");
    // Aus der Datebank (Custom)
    string? color = config.GetValue<string>("Color");

    // Konfiguration auf POCOs mappen
    var appWindow = new AppWindows();
    config.GetSection("App:Window").Bind(appWindow);

    // Aus Private-Setter-Eigenschaften binden
    appWindow = config.GetSection("App:Window").Get<AppWindows>(
        o => o.BindNonPublicProperties = true
        );
}

static void SqlCustomProvider()
{
    Debugger.Break();

    // Konfiguration vorbereiten
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        // NuGet: Microsoft.Extensions.Configuration.CommandLine
        .AddJsonFile("appSettings2.json");

    // Build() liest die Werte aller Quellen ein
    IConfigurationRoot config = configBuilder.Build();

    // Connectionstring abrufen...
    string? connectionString = config["ConnectionStrings:Main"];

    // SqlCustomProvider erstellen
    configBuilder = new ConfigurationBuilder()
        .AddSqlDatabase(connectionString!);
    config = configBuilder.Build();

    // Werte abrufen
    string? config1 = config["SystemDrive"];
    string? config2 = config["Color"];

    WriteLine($"config1 = {config1}");
    WriteLine($"config2 = {config2}");
}

static void SecretConfiguration()
{
    // Debugger.Break();

    // Konfiguration vorbereiten
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddSecretConfig("secretappsettings.json");

    // Build() liest die Werte aller Quellen ein
    IConfigurationRoot config = configBuilder.Build();

    // Werte abrufen
    string? emailPassword = config["SecretConfig:Email:Password"];
    string? sharePassword = config["SecretConfig:FileService:Password"];

    Debugger.Break();
}

#region Misc
#nullable disable
public class AppWindows
{
    public int Height { get; set; }
    public int Width { get; set; }
    public int Top { get; set; }
    public int Left { get; set; }
    public Content Title { get; set; }

    public override string ToString()
    {
        return $"T:{Top}, L:{Left}, H:{Height}, W:{Width}, Title:Color:{Title.Color}";
    }

    public class Content
    {
        public string Color { get; set; }
    }
}

public enum PaperOrientation
{
    Landscape,
    Portrait,
}
#nullable restore
#endregion