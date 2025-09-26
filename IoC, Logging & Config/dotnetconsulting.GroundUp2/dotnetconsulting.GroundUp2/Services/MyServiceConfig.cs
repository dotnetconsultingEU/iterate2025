#nullable disable
using System.ComponentModel.DataAnnotations;

namespace dotnetconsulting.GroundUp2.Services;

internal class MyServiceConfig
{
    [Required, Range(1, 65535)]
    public int Port { get; set; } = 465;
    [Required]
    public string Server { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public bool UseSSL { get; set; } = true;
}