using Core.AiPlanning;
using Model.Interfaces;
using Model.Model;

namespace Core
{
    public class SensorContextConsumer: ISensorContextConsumer
    {
        private AiPlanner _planner;
        
        public void Consume(SensorContext sensorContext)
        {
            _planner.Plan();
        }
    }
}