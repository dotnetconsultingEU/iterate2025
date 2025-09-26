// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

namespace dotnetconsulting.SMTPService;

public class SmtpServiceConfigBuilder
{
    private int Port { get; set; }
    private string Hostname { get; set; } = null!;
    public string Sender { get; set; } = null!;
    public bool UseSSL { get; set; }

    public void SetHostAndPort(string Hostname, int Port, bool UseSSL = true)
    {
        this.Hostname = Hostname;
        this.Port = Port;
        this.UseSSL = UseSSL;
    }

    public SmtpServiceConfig Build()
    {
        // Konfiguration prüfen
        if (Port < 1 || Port > 65535)
            throw new ArgumentOutOfRangeException("Port");

        SmtpServiceConfig config = new(Port, Hostname, Sender, default, UseSSL);

        return config;
    }
}