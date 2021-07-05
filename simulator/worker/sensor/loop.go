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
			w.logger.Debugf("Terminating Sensor Worker %q...", w.Sensor.Name)
			w.state = model.Inactive
			return
		}
	}
}

func (w *Worker) publishSensorData() {
	w.logger.Tracef("Sending Value %f to MQTT Broker", w.currentValue)

	message := model.SensorMessage{
		SensorName: w.Sensor.Name,
		Unit:       w.Sensor.Unit,
		Value:      w.currentValue,
		MeasuredAt: time.Now(),
	}
	messageJson, _ := json.Marshal(message)
	w.lastEmitted = message.MeasuredAt

	t := w.brokerClient.Publish(w.mqttConfig.Topic, 0, true, string(messageJson))
	go func() {
		_ = t.Wait()
		if t.Error() != nil {
			w.logger.WithError(t.Error()).Errorf("Publishing Sensor Data has failed. Reason: %s", t.Error().Error())
		}
	}()
}
