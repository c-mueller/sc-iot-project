package main

import (
	"fmt"
	"github.com/c-mueller/sc-iot-project/simulator/core"
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
	"gopkg.in/alecthomas/kingpin.v2"
)

var (
	httpPort     = kingpin.Flag("http-port", "HTTP port for the server").Short('p').Default("8080").Int()
	mqttHostFlag = kingpin.Flag("mqtt-host", "Hostname/IP of the MQQT Broker").Short('Q').Default("127.0.0.1").String()
	mqttPortFlag = kingpin.Flag("mqtt-port", "Port of the MQTT broker").Short('P').Default("1883").Int()
	jsonLog      = kingpin.Flag("json-log", "Log using json formatter").Bool()
)

var sensors = []model.Sensor{
	{
		Type:         model.Temperature,
		Place:        model.Indoors,
		Name:         "temperature-indoors",
		Unit:         "Celsius",
		InitialValue: 20,
	},
	{
		Type:         model.Temperature,
		Place:        model.Outdoors,
		Name:         "temperature-outdoors",
		Unit:         "Celsius",
		InitialValue: 20,
	},
	{
		Type:         model.AirParticle,
		Place:        model.Outdoors,
		Name:         "air-particles-outdoors",
		Unit:         "ppm",
		InitialValue: 50,
	},
	{
		Type:         model.AirParticle,
		Place:        model.Outdoors,
		Name:         "air-particles-indoors",
		Unit:         "ppm",
		InitialValue: 50,
	},
	{
		Type:         model.Humidity,
		Place:        model.Outdoors,
		Name:         "humidity-outdoors",
		Unit:         "Percent",
		InitialValue: 50,
	},
}

var actuators = []model.Actuator{
	{
		Type: model.Ventilation,
		Name: "ventilation",
	},
	{
		Type: model.Heater,
		Name: "heater",
	},
	{
		Type: model.AirPurifier,
		Name: "air-purifier",
	},
	{
		Type: model.AirConditioner,
		Name: "air_conditioner",
	},
}

var globalLogger *logrus.Entry

func init() {
	kingpin.Parse()
	gin.SetMode(gin.ReleaseMode)
}

func main() {
	if *jsonLog {
		logrus.SetFormatter(&logrus.JSONFormatter{})
	}
	logrus.StandardLogger().SetLevel(logrus.DebugLevel)
	globalLogger = logrus.NewEntry(logrus.StandardLogger()).WithField("module", "global")

	simulator := &core.Simulator{
		HttpEndpoint: fmt.Sprintf(":%d", *httpPort),
		MQTTHostname: *mqttHostFlag,
		MQTTPort:     *mqttPortFlag,
		Sensors:      sensors,
		Actuators:    actuators,
	}
	err:= simulator.Init(globalLogger.WithField("module", "simulator_root"))
	if err != nil {
		panic(err)
	}

	err = simulator.Run()
	if err != nil {
		panic(err)
	}
}
