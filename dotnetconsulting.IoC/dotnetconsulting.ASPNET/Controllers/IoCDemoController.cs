// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.ASPNET.Code;
using dotnetconsulting.ServiceAndInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dotnetconsulting.ASPNET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IoCDemoController : ControllerBase
    {
        private readonly ILogger<IoCDemoController> _logger;
        private readonly IOrderService _snailOrderService;
        private readonly IOrderService _emailOrderService;

        public IoCDemoController(ILogger<IoCDemoController> logger,
                                 [FromKeyedServices("snail")]IOrderService snailOrderService,
                                 [FromKeyedServices("email")]IOrderService emailOrderService,
                                 TestInject? testInject = default)
        {
            _logger = logger;
            _snailOrderService = snailOrderService;
            _emailOrderService = emailOrderService;
            Debug.Print($"testInject={testInject}");
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Logger steht zur Verfügung
            _logger.LogInformation("Get() is called");

            // OrderServices ebenfalls
            _snailOrderService.PlaceOrder("Wattestäbchen", 10);
            _emailOrderService.PlaceOrder("Wattestäbchen", 10);

            return Ok();
        }
    }
}
