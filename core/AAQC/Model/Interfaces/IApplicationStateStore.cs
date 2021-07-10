using Model.Model;

namespace Model.Interfaces
{
    public interface IApplicationStateStore
    {
        void StoreLatestObjectState(ObjectState objectState);
        void StoreLatestSensorContext(SensorContext context);
        ObjectState GetLatestObjectState();
        SensorContext GetLatestSensorContext();
    }
}