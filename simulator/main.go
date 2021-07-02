package main

import (
	util "github.com/c-mueller/sc-iot-project/simulator/utils"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
	"gopkg.in/alecthomas/kingpin.v2"
)

var (
	endpointFlag = kingpin.Flag("endpoint","HTTP Endpoint for the server").Default(":8080").String()
)

var sensors = []Sensor{
	{
		Type:         Temperature,
		Place:        Indoors,
		Name:         "temp_indoors",
		Unit:         "Celsius",
		CurrentValue: 20,
	},
	{
		Type:         Temperature,
		Place:        Outdoors,
		Name:         "temp_outdoors",
		Unit:         "Celsius",
		CurrentValue: 20,
	},
	{
		Type:         AirParticle,
		Place:        Outdoors,
		Name:         "air_particle_outdoors",
		Unit:         "ppm",
		CurrentValue: 50,
	},
	{
		Type:         AirParticle,
		Place:        Outdoors,
		Name:         "air_particle_indoors  ",
		Unit:         "ppm",
		CurrentValue: 50,
	},
	{
		Type:         Humidity,
		Place:        Outdoors,
		Name:         "humidity_outdoors",
		Unit:         "Percent",
		CurrentValue: 50,
	},

}

var logger *logrus.Entry

func init() {
	kingpin.Parse()
	gin.SetMode(gin.ReleaseMode)
}

func main() {
	logrus.StandardLogger().SetLevel(logrus.DebugLevel)
	logger = logrus.NewEntry(logrus.StandardLogger())

	engine := gin.New()
	engine.Use(gin.Recovery())
	engine.Use(util.LogEnricherMiddleware(logger))
	engine.Use(util.LogMiddleware(logger, true))

	apiRouteGroup := engine.Group("/api")
	apiRouteGroup.GET("/", func(context *gin.Context) {
		context.String(200, "Hello World")
	})

	logger.Infof("Listening on endpoint %q...", *endpointFlag)
	err := engine.Run(*endpointFlag)
	if err != nil {
		panic(err)
	}
}
