package actuator

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	mqtt "github.com/eclipse/paho.mqtt.golang"
	"github.com/sirupsen/logrus"
	"time"
)

type Worker struct {
	Actuator model.Actuator

	state         model.WorkerState
	interruptChan chan bool

	active        bool
	lastReceieved time.Time

	mqttConfig model.BrokerConfig

	ticker       *time.Ticker
	logger       *logrus.Entry
	brokerClient mqtt.Client
}

func (w *Worker) GetWorkerDeviceName() string {
	return w.Actuator.Name
}

func (w *Worker) GetState() model.WorkerState {
	return w.state
}

func (w *Worker) Init(logger *logrus.Entry, config model.BrokerConfig) error {
	w.state = model.Inactive
	w.logger = logger.WithField("actuator", w.Actuator.Name)
	w.active = false
	w.interruptChan = make(chan bool, 1)

	err := w.initMQTT(config)
	if err != nil {
		return err
	}

	w.logger.Debugf("Initialized Actuator Worker for sensor %q.", w.Actuator.Name)
	return nil
}

func (w *Worker) Start() error {
	go func() {
		w.state = model.Active

		w.logger.Debugf("Attempting connection to MQTT Broker...")
		if token := w.brokerClient.Connect(); token.Wait() && token.Error() != nil {
			w.logger.WithError(token.Error()).Errorf("Actuator Worker failed... Reason: %s", token.Error().Error())
			w.state = model.Inactive
			return
		}

		w.logger.Tracef("Actuator Worker %q awaits messages...", w.Actuator.Name)
		<-w.interruptChan
		w.state = model.Inactive
		w.logger.Tracef("Terminating Actuator Worker %q...", w.Actuator.Name)
	}()
	return nil
}

func (w *Worker) Stop() error {
	w.interruptChan <- true
	w.brokerClient.Disconnect(0)
	return nil
}

func (w *Worker) GetCurrentState() model.ActuatorState {
	return model.ActuatorState{
		ActuatorName: w.Actuator.Name,
		Actuator:     w.Actuator,
		Active:       w.active,
		LastUpdated:  w.lastReceieved,
	}
}
