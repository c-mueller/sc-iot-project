package core

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
)

type Simulator struct {
	HttpEndpoint string

	Sensors []model.Sensor
	Actuators []model.Actuator

	logger *logrus.Entry
	engine *gin.Engine
}

func (s *Simulator) Init(logger *logrus.Entry) error {
	s.logger = logger

	s.InitializeApi()

	return nil
}

func (s *Simulator) Run() error {
	return s.RunHttpServer()
}



