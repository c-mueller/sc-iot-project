package sensorworker

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	mqtt "github.com/eclipse/paho.mqtt.golang"
	"github.com/sirupsen/logrus"
	"time"
)

type Worker struct {
	Sensor         model.Sensor
	UpdateInterval time.Duration

	state        model.WorkerState
	currentValue float64
	lastEmitted  time.Time

	mqttConfig model.BrokerConfig

	ticker       *time.Ticker
	logger       *logrus.Entry
	brokerClient mqtt.Client
}

func (w *Worker) Init(logger *logrus.Entry, config model.BrokerConfig) error {
	w.state = model.Inactive
	w.logger = logger.WithField("sensor", w.Sensor.Name)
	w.currentValue = w.Sensor.InitialValue
	w.ticker = time.NewTicker(w.UpdateInterval)

	err := w.initMQTT(config)
	if err != nil {
		return err
	}

	w.logger.Debugf("Initialized Sensor Worker for sensor %q.", w.Sensor.Name)

	return nil
}

func (w *Worker) Start() error {
	w.state = model.Active


	return nil
}

func (w *Worker) Stop() error {
	w.state = model.Inactive
	w.ticker.Stop()

	return nil
}

func (w *Worker) GetWorkerDeviceName() string {
	return w.Sensor.Name
}

func (w *Worker) GetState() model.WorkerState {
	return w.state
}

func (w *Worker) GetCurrentState() model.SensorState {
	return model.SensorState{
		SensorName:   w.Sensor.Name,
		Sensor:       w.Sensor,
		CurrentValue: w.currentValue,
		LastMeasured: w.lastEmitted,
	}
}

func (w *Worker) SetValue(value float64) {
	w.currentValue = value
}
