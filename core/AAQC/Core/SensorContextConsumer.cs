using Core.AiPlanning;
using Model.Interfaces;
using Model.Model;

namespace Core
{
    public class SensorContextConsumer: ISensorContextConsumer
    {
        private readonly AiPlanner _planner;
        private readonly IApplicationStateStore _contextStore;

        public SensorContextConsumer(AiPlanner planner, IApplicationStateStore contextStore)
        {
            _planner = planner;
            _contextStore = contextStore;
        }

        public void Consume(SensorContext sensorContext)
        {
            // _contextStore.StoreLatestSensorContext(sensorContext);
            _planner.Initiate(sensorContext);
        }
    }
}