using Model.Model;

namespace Model.Interfaces
{
    public interface ISensorContextConsumer
    {
        public void Consume(SensorContext sensorContext);
    }
}