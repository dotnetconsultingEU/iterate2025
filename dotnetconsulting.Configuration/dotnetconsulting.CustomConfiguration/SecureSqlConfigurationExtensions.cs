// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace dotnetconsulting.CustomConfiguration;

// Erweiterungsmethoden für das bequeme Anfügen
public static class SqlConfigurationExtensions
{
    public static IConfigurationBuilder AddSqlDatabase(this IConfigurationBuilder builder, string ConnectionString)
    {
        IConfigurationSource source = new SqlDatabaseConfigurationSource(ConnectionString);
        builder.Sources.Add(source);
        return builder;
    }
}

// Configuration Source, zum speichern des ConnectionString
public class SqlDatabaseConfigurationSource(string ConnectionString) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        IConfigurationProvider configurationProvider = new SqlDatabaseConfigurationProvider(ConnectionString);
        return configurationProvider;
    }
}

// Kern für das Einlesen der Konfigurationswerte
public class SqlDatabaseConfigurationProvider(string ConnectionString) : IConfigurationProvider
{
    private readonly Dictionary<string, string?> values = [];

    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        // Eine Hierarchie wird nicht unterstützt
        return [];
    }

    private IChangeToken changeToken = null!;
    public IChangeToken GetReloadToken()
    {
        changeToken ??= new SqlDatabaseChangeToken();
        return changeToken;
    }

    public void Load()
    {
        using SqlConnection con = new(ConnectionString);
        con.Open();

        using SqlCommand cmd = con.CreateCommand();
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.CommandText = "SELECT [Key], [Value] FROM dbo.Configuration;";

        using SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            string key = dr.GetString(0);
            string value = (dr.IsDBNull(1) ? null : dr.GetString(1))!;
            values.Add(key, value);
        }
    }

    public void Set(string key, string? value)
    {
        if (!values.ContainsKey(key))
            throw new ArgumentOutOfRangeException(nameof(key));

        values[key] = value;
    }

    public bool TryGet(string key, out string value)
    {
        return values.TryGetValue(key, out value!);
    }
}

// Change Token für Änderungen
public class SqlDatabaseChangeToken : IChangeToken, IDisposable
{
    public bool HasChanged => false;

    public bool ActiveChangeCallbacks => false;

    public void Dispose()
    {
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    public IDisposable RegisterChangeCallback(Action<object> callback, object? state)
    {
        return this;
    }
}