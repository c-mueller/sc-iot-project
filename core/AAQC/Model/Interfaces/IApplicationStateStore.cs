using Model.Model;

namespace Model.Interfaces
{
    public interface IApplicationStateStore
    {
        void StorePddlObjectState(PddlObjectState objectState);
        void StoreSensorContext(SensorContext context);
        PddlObjectState GetLastPddlObjectState();
        SensorContext GetLastSensorContext();
    }
}