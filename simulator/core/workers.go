package core

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	actuatorworker "github.com/c-mueller/sc-iot-project/simulator/worker/actuator"
	sensorworker "github.com/c-mueller/sc-iot-project/simulator/worker/sensor"
	"time"
)

func (s *Simulator) initializeSensorWorkers() error {
	s.logger.Infof("Initilializing Sensor Workers...")

	brokerConfig := model.BrokerConfig{
		Hostname: s.MQTTHostname,
		Port:     s.MQTTPort,
		Topic:    "sensors",
	}

	s.sensorWorkers = make(map[string]model.SensorWorker)
	for _, sensor := range s.Sensors {
		s.sensorWorkers[sensor.Name] = &sensorworker.Worker{
			Sensor:         sensor,
			UpdateInterval: time.Second * 15,
		}
		workerLogger := s.logger.WithField("module", "sensorworker")
		err := s.sensorWorkers[sensor.Name].Init(workerLogger, brokerConfig)
		if err != nil {
			s.logger.WithError(err).Errorf("Failed to initialize sensor worker for sensor %s. Reason: %s", sensor.Name, err.Error())
			return err
		}
	}

	s.logger.Infof("Initilaized %d Sensor Workers", len(s.sensorWorkers))
	return nil
}

func (s *Simulator) initializeActuatorWorkers() error {
	s.logger.Infof("Initilializing Actuator Workers...")

	s.actuatorWorkers = make(map[string]model.ActuatorWorker)
	for _, actuator := range s.Actuators {

		brokerConfig := model.BrokerConfig{
			Hostname: s.MQTTHostname,
			Port:     s.MQTTPort,
			Topic:    actuator.Topic,
		}

		s.actuatorWorkers[actuator.Name] = &actuatorworker.Worker{
			Actuator: actuator,
		}
		workerLogger := s.logger.WithField("module", "actuatorworker")
		err := s.actuatorWorkers[actuator.Name].Init(workerLogger, brokerConfig)
		if err != nil {
			s.logger.WithError(err).Errorf("Failed to initialize actuator worker for actuator %s. Reason: %s", actuator.Name, err.Error())
			return err
		}
	}

	s.logger.Infof("Initilaized %d Sensor Workers", len(s.sensorWorkers))
	return nil
}

func (s *Simulator) runWorkers() error {
	for _, worker := range s.sensorWorkers {
		err := worker.Start()
		if err != nil {
			s.logger.WithError(err).Errorf("Launching sensor worker %q failed. Reason: %s", worker.GetWorkerDeviceName(), err.Error())
			return err
		}
	}

	for _, worker := range s.actuatorWorkers {
		err := worker.Start()
		if err != nil {
			s.logger.WithError(err).Errorf("Launching actuator worker %q failed. Reason: %s", worker.GetWorkerDeviceName(), err.Error())
			return err
		}
	}
	time.Now().String()
	return nil
}
