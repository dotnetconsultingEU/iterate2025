// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace dotnetconsulting.SecretConfig;

public class SecretConfigProvider(FileConfigurationSource fileConfigurationSource) : FileConfigurationProvider(fileConfigurationSource)
{
    public override void Load(Stream stream)
    {
        var settings = JsonSerializer.Deserialize<SecretConfigObjects>(stream)!;

        // An dieser Stelle wäre es auch möglich, die Datei dynamisch zu analysieren
        // und die Konfiguration auszulesen und zu entschlüsseln.
        // Hier wird jedoch ein festes Format für die Konfiguration vorausgesetzt
        DecryptSettings(settings);

        // Die ermittelten Werte werden an dieser Stelle in die Struktur der
        // Konfiguration eingetragen. 'SECTIONNAME' könnte dabei ein Parameter sein
        Data.Add($"{SecretConfigExtensions.SECTIONNAME}:Email:User", settings.Email.User);
        Data.Add($"{SecretConfigExtensions.SECTIONNAME}:Email:Password", settings.Email.Password);
        Data.Add($"{SecretConfigExtensions.SECTIONNAME}:Email:Host", settings.Email.Host);
        Data.Add($"{SecretConfigExtensions.SECTIONNAME}:Email:Port", settings.Email.Port.ToString());
        Data.Add($"{SecretConfigExtensions.SECTIONNAME}:FileService:Path", settings.FileService.Path);
        Data.Add($"{SecretConfigExtensions.SECTIONNAME}:FileService:User", settings.FileService.User);
        Data.Add($"{SecretConfigExtensions.SECTIONNAME}:FileService:Password", settings.FileService.Password);
    }

    #region Dummy Implementation
    private static void DecryptSettings(SecretConfigObjects settings)
    {
        byte[] key = [1, 2, 3, 4, 5, 6, 7, 8];
        byte[] iv = [1, 2, 3, 4, 5, 6, 7, 8];

        settings.Email.Password = decrypt(settings.Email.Password);
        settings.FileService.Password = decrypt(settings.FileService.Password);

        string decrypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }
    }

    private static void EncryptSettings(SecretConfigObjects settings)
    {
        byte[] key = [1, 2, 3, 4, 5, 6, 7, 8];
        byte[] iv = [1, 2, 3, 4, 5, 6, 7, 8];

        settings.Email.Password = encrypt(settings.Email.Password);
        settings.FileService.Password = encrypt(settings.FileService.Password);

        string encrypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }
    }
    #endregion
}