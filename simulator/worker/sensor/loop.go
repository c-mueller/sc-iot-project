package sensor

import (
	"encoding/json"
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"time"
)

func (w *Worker) workerLoop() {
	w.publishSensorData()
	for {
		select {
		case <-w.ticker.C:
			w.publishSensorData()
		case <-w.interruptChan:
			w.logger.Debugf("Terminating Actuator Worker %q...", w.Sensor.Name)
			w.state = model.Inactive
			return
		}
	}
}

func (w *Worker) publishSensorData() {
	w.logger.Debugf("Sending Value %f to MQTT Broker", w.currentValue)

	message := model.SensorMessage{
		Location:   w.Sensor.Location,
		SensorType: w.Sensor.Type.String(),
		Value:      w.currentValue,
		Timestamp:  time.Now(),
	}
	messageJson, _ := json.Marshal(message)
	w.lastEmitted = message.Timestamp

	t := w.brokerClient.Publish(w.mqttConfig.Topic, 0, true, string(messageJson))
	go func() {
		_ = t.Wait()
		if t.Error() != nil {
			w.logger.WithError(t.Error()).Errorf("Publishing Actuator Data has failed. Reason: %s", t.Error().Error())
		}
	}()
}
