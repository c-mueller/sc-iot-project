using Microsoft.AspNetCore.Mvc;
using Model.Interfaces;
using Model.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/sensors")]
    public class SensorController : ControllerBase
    {
        private readonly IApplicationStateStore _applicationStateStore;

        public SensorController(IApplicationStateStore applicationStateStore)
        {
            _applicationStateStore = applicationStateStore;
        }

        [HttpGet]
        public ActionResult<SensorContext> GetSensorData()
        {
            return Ok(_applicationStateStore.GetLatestSensorContext());
        }
    }
}