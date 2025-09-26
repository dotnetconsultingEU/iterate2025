using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace dotnetconsulting.ASPNETHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IOptions<MyAppSettings> MyAppSettings,
                                           ILogger<WeatherForecastController> logger) : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            // Without ValidateOnStart() OptionsValidationException is thrown when configuration is invalid
            var myAppSettings = MyAppSettings.Value;

            Console.WriteLine($"ApiUrl: {myAppSettings.ApiUrl}");
            Console.WriteLine($"Timeout: {myAppSettings.Timeout}");
            Console.WriteLine($"MaxRetryCount: {myAppSettings.MaxRetryCount}");

            return [.. Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })];
        }
    }
}