using Model.Model;

namespace Core.Model
{
    public interface IApplicationStateStore
    {
        void StorePddlObjectState(PddlObjectState objectState);
        void StoreSensorContext(SensorContext context);
        PddlObjectState GetLastPddlObjectState();
        SensorContext GetLastSensorContext();
    }
}