using Model.Model;

namespace Model.Interfaces
{
    public interface IApplicationStateStore
    {
        void StoreLatestActuatorState(ActuatorState actuatorState);
        void StoreLatestSensorContext(SensorContext context);
        ActuatorState GetLatestActuatorState();
        SensorContext GetLatestSensorContext();
    }
}