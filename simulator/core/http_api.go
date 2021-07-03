package core

import "github.com/gin-gonic/gin"

func (s *Simulator) ListSensors(context *gin.Context) {
	context.JSON(200, s.Sensors)
}

func (s *Simulator) ListActuators(context *gin.Context) {
	context.JSON(200, s.Actuators)
}
