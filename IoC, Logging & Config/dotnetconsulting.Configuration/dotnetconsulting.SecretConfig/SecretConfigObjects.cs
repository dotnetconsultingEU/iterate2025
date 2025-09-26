﻿// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu
#nullable disable

using System.Text.Json;

namespace dotnetconsulting.SecretConfig;

public class SecretConfigObjects
{
    public Email Email { get; set; }
    public FileService FileService { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this, jsonConfig);
    private static readonly JsonSerializerOptions jsonConfig = new() { WriteIndented = true };
}

public class Email
{
    public string User { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; } = 589;
    public override string ToString() => JsonSerializer.Serialize(this, jsonConfig);
    private static readonly JsonSerializerOptions jsonConfig = new() { WriteIndented = true };
}

public class FileService
{
    public string Path { get; set; }
    public string User { get; set; }
    public string Password { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this, jsonConfig);
    private static readonly JsonSerializerOptions jsonConfig = new() { WriteIndented = true };
}