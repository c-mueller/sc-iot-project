package core

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
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
	return s.RunHttpServer()
}
