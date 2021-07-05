package actuator

import (
	"fmt"
	"github.com/c-mueller/sc-iot-project/simulator/model"
	mqtt "github.com/eclipse/paho.mqtt.golang"
)

func (w *Worker) onMQTTMessage(client mqtt.Client, message mqtt.Message) {

}

func (w *Worker) onMQTTConnect(client mqtt.Client) {
	if token := client.Subscribe(w.mqttConfig.Topic, 0, w.onMQTTMessage); token.Wait() && token.Error() != nil {
		w.logger.WithError(token.Error()).Errorf("Actuator Worket %q lost connection to MQTT Broker. Reason: %s", w.Actuator.Name, token.Error().Error())
	}
	w.logger.Debugf("Connected Actuator Worker %q to MQTT Broker.", w.Actuator.Name)
}

func (w *Worker) onMQTTConnectionLost(client mqtt.Client, err error) {
	w.logger.WithError(err).Errorf("Actuator Worket %q lost connection to MQTT Broker. Reason: %s", w.Actuator.Name, err.Error())
}

func (w *Worker) initMQTT(config model.BrokerConfig) error {
	w.mqttConfig = config
	opts := mqtt.NewClientOptions()
	opts.SetClientID(fmt.Sprintf("actuatorworker/%s", w.Actuator.Name))
	opts.AddBroker(fmt.Sprintf("tcp://%s:%d", config.Hostname, config.Port))

	opts.OnConnect = w.onMQTTConnect
	opts.OnConnectionLost = w.onMQTTConnectionLost

	w.brokerClient = mqtt.NewClient(opts)

	w.logger.Debugf("Initialized MQTT Client for Actuator Worker %q", w.Actuator.Name)

	return nil
}
