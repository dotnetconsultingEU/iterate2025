// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace dotnetconsulting.SecretConfig;

public static class SecretConfigExtensions
{
    internal static string SECTIONNAME = "SecretConfig";

    public static IConfigurationBuilder AddSecretConfig(this IConfigurationBuilder builder, string path)
    {
        return AddSecretConfig(builder, provider: null!, path: path, optional: false, reloadOnChange: false);
    }

    public static IConfigurationBuilder AddSecretConfig(this IConfigurationBuilder builder, string path, bool optional)
    {
        return AddSecretConfig(builder, provider: null!, path: path, optional: optional, reloadOnChange: false);
    }

    public static IConfigurationBuilder AddSecretConfig(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
    {
        return AddSecretConfig(builder, provider: null!, path: path, optional: optional, reloadOnChange: reloadOnChange);
    }

    public static IConfigurationBuilder AddSecretConfig(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
    {
        if (provider == null && Path.IsPathRooted(path))
        {
            provider = new PhysicalFileProvider(Path.GetDirectoryName(path)!);
            path = Path.GetFileName(path);
        }

        var source = new SecretConfigSource(provider!, path, optional, reloadOnChange);

        builder.Add(source);
        return builder;
    }

    public static IServiceCollection AddSecretConfig(this IServiceCollection configurationSection, IConfiguration configurationRoot)
    {
        IConfigurationSection secretSection = configurationRoot.GetSection(SECTIONNAME);
        configurationSection.Configure<SecretConfigObjects>(secretSection);

        return configurationSection;
    }
}