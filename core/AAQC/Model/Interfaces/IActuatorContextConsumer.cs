using Model.Model;

namespace Model.Interfaces
{
    public interface IActuatorContextConsumer
    {
        public void Consume(ActuatorContext actuatorContext);
    }
}