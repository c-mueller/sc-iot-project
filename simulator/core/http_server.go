package core

import (
	util "github.com/c-mueller/sc-iot-project/simulator/utils"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
)

func (s *Simulator) InitializeApi() {
	s.logger.Infof("Initializing API....")
	defer func() {
		s.logger.Infof("Initialized %d Routes...", len(s.engine.Routes()))
		for _, routeInfo := range s.engine.Routes() {
			s.logger.WithFields(logrus.Fields{
				"method":  routeInfo.Method,
				"path":    routeInfo.Path,
				"handler": routeInfo.Handler,
			}).Debugf("%s %s -> %s", routeInfo.Method, routeInfo.Path, routeInfo.Handler)
		}
	}()
	s.engine = gin.New()
	s.engine.Use(gin.Recovery())
	s.engine.Use(util.LogEnricherMiddleware(s.logger))
	s.engine.Use(util.LogMiddleware(s.logger, true))

	apiRouteGroup := s.engine.Group("/api")
	apiRouteGroup.GET("/sensors", s.ListSensors)
	apiRouteGroup.GET("/sensors/:name", s.GetSensor)
	apiRouteGroup.POST("/sensors/:name", s.SetSensorValue)
	apiRouteGroup.GET("/actuators", s.ListActuators)
}


func (s *Simulator) RunHttpServer() error {
	s.logger.Infof("Listening on endpoint %q...", s.HttpEndpoint)
	err := s.engine.Run(s.HttpEndpoint)
	if err != nil {
		return err
	}

	return nil
}