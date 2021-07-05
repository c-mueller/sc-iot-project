package core

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/c-mueller/sc-iot-project/simulator/sensorworker"
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
			UpdateInterval: time.Minute,
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
