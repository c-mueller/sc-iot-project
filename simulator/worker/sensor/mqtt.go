package sensor

import (
	"fmt"
	"github.com/c-mueller/sc-iot-project/simulator/model"
	mqtt "github.com/eclipse/paho.mqtt.golang"
)

func (w *Worker) onMQTTConnect(client mqtt.Client) {
	w.logger.Debugf("Connected Actuator Worker %q to MQTT Broker.", w.Sensor.Name)
}

func (w *Worker) onMQTTConnectionLost(client mqtt.Client, err error) {
	w.logger.WithError(err).Errorf("Actuator Worket %q lost connection to MQTT Broker. Reason: %s", w.Sensor.Name, err.Error())
}

func (w *Worker) initMQTT(config model.BrokerConfig) error {
	w.mqttConfig = config
	opts := mqtt.NewClientOptions()
	opts.SetClientID(fmt.Sprintf("sensorworker/%s", w.Sensor.Name))
	opts.AddBroker(fmt.Sprintf("tcp://%s:%d", config.Hostname, config.Port))

	opts.OnConnect = w.onMQTTConnect
	opts.OnConnectionLost = w.onMQTTConnectionLost

	w.brokerClient = mqtt.NewClient(opts)
	w.logger.Debugf("Attempting connection to MQTT Broker...")
	if token := w.brokerClient.Connect(); token.Wait() && token.Error() != nil {
		return token.Error()
	}

	w.logger.Debugf("Initialized MQTT Client for Actuator Worker %q", w.Sensor.Name)

	return nil
}
