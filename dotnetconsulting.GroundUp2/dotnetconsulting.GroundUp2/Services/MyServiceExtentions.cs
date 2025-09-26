using dotnetconsulting.GroundUp2.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace dotnetconsulting.GroundUp2.Services;

internal static class MyServiceExtentions
{
    extension(IServiceCollection serviceCollection)
    {
        internal IServiceCollection AddMyService(IConfigurationRoot configuration, string sectionName, string reVersion = "SQLServer")
        {
            serviceCollection.AddKeyedTransient<IRepository, Repository>("Ver1");
            serviceCollection.AddKeyedTransient<IRepository, Repository>("Ver2");
            serviceCollection.AddTransient<IMyService, MyService1>();
            serviceCollection.AddTransient<IRepository, Repository>();
            //serviceCollection.AddTransient<IMyService>((serviceProvider) =>
            //{
            //    IRepository r1 = serviceProvider.GetRequiredKeyedService<IRepository>("Ver1");
            //    IRepository r2 = serviceProvider.GetRequiredKeyedService<IRepository>("Ver2");

            //    IMyService result = new MyService1(serviceProvider, r1, r2);

            //    return result;
            //});

            serviceCollection.TryAddScoped<IConfiguration>(_ => configuration);
            serviceCollection.AddOptions<MyServiceConfig>()
                .ValidateDataAnnotations()
                .BindConfiguration(sectionName);
                // .ValidateOnStart(); ASP.NET Core App Startup

            serviceCollection.AddScoped<IValidateOptions<MyServiceConfig>, MyServiceConfigValidator>();

            // serviceCollection.Configure<MyServiceConfig>(configSection);

            return serviceCollection;
        }
    }
}
