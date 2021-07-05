package core

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
	"os"
	"os/signal"
	"syscall"
	"time"
)

type Simulator struct {
	HttpEndpoint string
	MQTTHostname string
	MQTTPort     int

	Sensors   []model.Sensor
	Actuators []model.Actuator

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

		os.Exit(1)
	}()

	go func() {
		for _, worker := range s.sensorWorkers {
			err := worker.Start()
			if err != nil {
				s.logger.WithError(err).Errorf("Launching sensor worker %q failed. Reason: %s", worker.GetWorkerDeviceName(), err.Error())
				os.Exit(1)
			}
			time.Sleep(5 * time.Second)
		}
	}()

	return s.RunHttpServer()
}
