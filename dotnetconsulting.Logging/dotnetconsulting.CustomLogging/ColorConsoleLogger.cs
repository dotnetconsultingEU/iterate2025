// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Logging;
using static System.Console;

namespace dotnetconsulting.CustomLogging;

public class ColorConsoleLogger : ILogger, IDisposable
{
    private readonly ColorConsoleLoggerConfiguration _configuration;
    private readonly string _categoryName;

    private int indentLevel = 0;

    public ColorConsoleLogger(string categoryName,
                              ColorConsoleLoggerConfiguration Configuration)
    {
        _categoryName = categoryName;
        _configuration = Configuration;
    }
    public IDisposable BeginScope<TState>(TState state) where TState: notnull
    {
        string indent = new('\t', ++indentLevel);

        if (state != null)
        {
            ForegroundColor = _configuration.ScopeColor;
            WriteLine($"{indent}=== {state} ===");
        }

        return this;
    }

    public void Dispose()
    {
        indentLevel--;
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        // Alle Loglevel sind aktiv
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // LogLevel aktiv?
        if (!IsEnabled(logLevel))
            return;

        // Farbe einstellen
        switch (logLevel)
        {
            case LogLevel.Trace:
                ForegroundColor = _configuration.TraceColor;
                break;
            case LogLevel.Debug:
                ForegroundColor = _configuration.DebugColor;
                break;
            case LogLevel.Information:
                ForegroundColor = _configuration.InformationColor;
                break;
            case LogLevel.Warning:
                ForegroundColor = _configuration.WarningColor;
                break;
            case LogLevel.Error:
                ForegroundColor = _configuration.ErrorColor;
                break;
            case LogLevel.Critical:
                ForegroundColor = _configuration.CriticalColor;
                break;
            case LogLevel.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel));
        }

        // Meldung erzeugen & Ausgabe (ohne Buffer)
        string indent = new('\t', indentLevel);
        WriteLine($"{indent + formatter(state, exception)} [{_categoryName}]");
    }
}