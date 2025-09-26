using dotnetconsulting.GroundUp2.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace dotnetconsulting.GroundUp2.Services;

internal class MyService1 : IMyService
{
    private readonly IRepository _repository;
    private readonly MyServiceConfig _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MyService1> _logger;

    public MyService1(IServiceProvider services,
                      [FromKeyedServices("Ver1")] IRepository repository,
                      [FromKeyedServices("Ver2")] IRepository repository2,
                      IOptions<MyServiceConfig> options,
                      ILogger<MyService1> logger)
    {
        _config = options.Value;
        _repository = repository;
        _serviceProvider = services;
        _logger = logger;
    }

    public void Test(string par1)
    {
        _logger.LogDebug("Test({par1})", par1);

        try
        {
            string repositoryVer = "Ver1";

            // var repository = _serviceProvider.GetRequiredKeyedService<IRepository>(a);

            Debugger.Break();
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(MyService1EventIDs.Event1, ex, "Test()");

            throw;
        }
    }

    public int TestFunc()
    {
        throw new NotImplementedException();
    }
}