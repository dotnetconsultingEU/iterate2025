using dotnetconsulting.GroundUp2.Interfaces;
using Microsoft.Extensions.Options;

namespace dotnetconsulting.GroundUp2.Services;

internal class MyServiceConfigValidator : IValidateOptions<MyServiceConfig>
{
    public MyServiceConfigValidator(IRepository repository)
    {
            
    }

    public ValidateOptionsResult Validate(string? name, MyServiceConfig options)
    {
        if (options.Port < 1 || options.Port > 65535)
        {
            return ValidateOptionsResult.Fail("Port 1..65535");
        }

        bool isHttps = options.Server.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase);
        if (!isHttps)
        {
            return ValidateOptionsResult.Fail("Server must start with https://");
        }
        return ValidateOptionsResult.Success;
    }
}
