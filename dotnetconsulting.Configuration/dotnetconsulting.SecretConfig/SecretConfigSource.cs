﻿// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace dotnetconsulting.SecretConfig;

public class SecretConfigSource : FileConfigurationSource
{
    public SecretConfigSource(IFileProvider FileProvider,
                                            string Path,
                                            bool Optional = false,
                                            bool ReloadOnChange = false) : base()
    {
        this.FileProvider = FileProvider;
        this.Path = Path;
        this.Optional = Optional;
        this.ReloadOnChange = ReloadOnChange;
    }

    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        FileProvider ??= builder.GetFileProvider();
        return new SecretConfigProvider(this);
    }
}