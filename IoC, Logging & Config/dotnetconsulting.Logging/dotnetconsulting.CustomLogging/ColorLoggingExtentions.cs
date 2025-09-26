﻿// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.CustomLogging;

namespace Microsoft.Extensions.Logging;

public static class ColorConsoleLoggingExtentions
{
    public static ILoggingBuilder AddLevelWithColorConsole(this ILoggingBuilder builder)
    {
        return AddLevelWithColorConsole(builder, null!);
    }

    public static ILoggingBuilder AddLevelWithColorConsole(this ILoggingBuilder builder,
                                                           Action<ColorConsoleLoggerConfiguration> configure)
    {
        // Standardkonfiguration erzeugen
        ColorConsoleLoggerConfiguration configuration = new();

        // Anpassen durch Action?
        configure?.Invoke(configuration);

        builder.AddProvider(new ColorConsoleLoggingProvider(configuration));
        return builder;
    }
}