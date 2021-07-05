using Core.AiPlanning;
using Model.Interfaces;
using Model.Model;

namespace Core
{
    public class SensorContextConsumer: ISensorContextConsumer
    {
        private readonly AiPlanner _planner;

        public SensorContextConsumer(AiPlanner planner)
        {
            _planner = planner;
        }

        public void Consume(SensorContext sensorContext)
        {
            _planner.Initiate(sensorContext);
        }
    }
}