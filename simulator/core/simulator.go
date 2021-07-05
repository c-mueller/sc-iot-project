package core

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
	"os"
	"os/signal"
	"syscall"
)

type Simulator struct {
	Config       model.SimulatorConfig

	sensorWorkers   map[string]model.SensorWorker
	actuatorWorkers map[string]model.ActuatorWorker

	logger *logrus.Entry
	engine *gin.Engine
}

func (s *Simulator) Init(logger *logrus.Entry) error {
	s.logger = logger

	s.InitializeApi()
	err := s.initializeSensorWorkers()
	if err != nil {
		return err
	}

	err = s.initializeActuatorWorkers()
	if err != nil {
		return err
	}

	return nil
}

func (s *Simulator) Run() error {
	sigc := make(chan os.Signal, 1)
	signal.Notify(sigc, syscall.SIGINT)
	go func() {
		<-sigc

		for _, worker := range s.sensorWorkers {
			err := worker.Stop()
			if err != nil {
				s.logger.WithError(err).Errorf("Termination of Sensor Worker %q failed. Reason: %s", worker.GetWorkerDeviceName(), err.Error())
			}
		}
		for _, worker := range s.actuatorWorkers {
			err := worker.Stop()
			if err != nil {
				s.logger.WithError(err).Errorf("Termination of Actuator Worker %q failed. Reason: %s", worker.GetWorkerDeviceName(), err.Error())
			}
		}

		os.Exit(1)
	}()

	err := s.runWorkers()
	if err != nil {
		return err
	}

	return s.RunHttpServer()
}
