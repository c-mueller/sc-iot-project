using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Interfaces;
using Model.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/sensors")]
    public class SensorController : ControllerBase
    {
        
        private readonly ILogger<SensorController> _logger;
        private readonly IApplicationStateStore _applicationStateStore;
        
        public SensorController(ILogger<SensorController> logger, IApplicationStateStore applicationStateStore)
        {
            _logger = logger;
            _applicationStateStore = applicationStateStore;
        }

        [HttpGet]
        public ActionResult<SensorContext> GetSensorData()
        {
            return Ok(_applicationStateStore.GetLatestSensorContext());
        }
    }
}