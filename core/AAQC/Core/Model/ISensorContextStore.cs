using Model.Model;

namespace Core.Model
{
    public interface ISensorContextStore
    {
        void StoreContext(SensorContext sensorContext);
        SensorContext GetLastContext();
    }
}