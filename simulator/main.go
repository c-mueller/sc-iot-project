package main

import (
	"fmt"
	"github.com/c-mueller/sc-iot-project/simulator/core"
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
	"gopkg.in/alecthomas/kingpin.v2"
	"gopkg.in/yaml.v2"
	"io/ioutil"
)

var (
	generateConfigFlag = kingpin.Flag("gen-config", "Generate default config file").Bool()
	configPathFlag     = kingpin.Flag("config", "Path to the config file").Short('c').Default("config.yml").File()
	jsonLog            = kingpin.Flag("json-log", "Log using json formatter").Bool()
)

var globalLogger *logrus.Entry

func init() {
	kingpin.Parse()
	gin.SetMode(gin.ReleaseMode)
}

func main() {
	if *generateConfigFlag {
		cfg := model.GetDefaultConfig()
		data, _ := yaml.Marshal(cfg)
		fmt.Println(string(data))
		return
	}

	data, err := ioutil.ReadAll(*configPathFlag)
	if err != nil {
		panic(err)
	}

	var cfg model.SimulatorConfig
	err = yaml.Unmarshal(data, &cfg)
	if err != nil {
		panic(err)
	}

	if *jsonLog {
		logrus.SetFormatter(&logrus.JSONFormatter{})
	}
	logrus.StandardLogger().SetLevel(logrus.InfoLevel)
	globalLogger = logrus.NewEntry(logrus.StandardLogger()).WithField("module", "global")

	simulator := &core.Simulator{
		HttpEndpoint: fmt.Sprintf(":%d", cfg.HTTPPort),
		MQTTHostname: cfg.MQTTEndpoint,
		MQTTPort:     cfg.MQTTPort,
		Sensors:      cfg.Sensors,
		Actuators:    cfg.Actuators,
	}
	err = simulator.Init(globalLogger.WithField("module", "simulator_root"))
	if err != nil {
		panic(err)
	}

	err = simulator.Run()
	if err != nil {
		panic(err)
	}
}
