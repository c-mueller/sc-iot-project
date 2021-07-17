using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Model.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/actuators")]
    public class ActuatorController : ControllerBase
    {
        private readonly IApplicationStateStore _applicationStateStore;

        public ActuatorController(IApplicationStateStore applicationStateStore)
        {
            _applicationStateStore = applicationStateStore;
        }

        [HttpGet]
        public ActionResult<List<ActuatorInfo>> GetActuatorData()
        {
            var latestActuatorState = _applicationStateStore.GetLatestActuatorState();

            var actuatorInfos = new List<ActuatorInfo>
            {
                new ActuatorInfo
                {
                    Name = "Ventilation", 
                    Active = latestActuatorState.IsVentilationActive.GetValueOrDefault()
                },
                new ActuatorInfo
                {
                    Name = "Heater", 
                    Active = latestActuatorState.IsHeaterActive.GetValueOrDefault()
                },
                new ActuatorInfo
                {
                    Name = "Air conditioner",
                    Active = latestActuatorState.IsAirConditionerActive.GetValueOrDefault()
                },
                new ActuatorInfo
                {
                    Name = "Air purifier", 
                    Active = latestActuatorState.IsAirPurifierActive.GetValueOrDefault()
                }
            };

            return Ok(actuatorInfos);
        }
    }

    [DataContract]
    public class ActuatorInfo
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "active")]
        public bool Active { get; set; }
    }
}