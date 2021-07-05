package model

import "time"

var sensors = []Sensor{
	{
		Type:         Temperature,
		Place:        Indoors,
		Topic:        "room001/input/temperature",
		Location:     "room001",
		Name:         "temperature-indoors",
		Unit:         "Celsius",
		InitialValue: 20,
	},
	{
		Type:         AirParticle,
		Place:        Indoors,
		Topic:        "room001/input/particulate_matter",
		Location:     "room001",
		Name:         "air-particles-indoors",
		Unit:         "ppm",
		InitialValue: 50,
	},
	{
		Type:         Humidity,
		Place:        Indoors,
		Topic:        "room001/input/humidity",
		Location:     "room001",
		Name:         "humidity-outdoors",
		Unit:         "Percent",
		InitialValue: 50,
	},
	{
		Type:         CO2,
		Place:        Indoors,
		Name:         "co2-indoors",
		Unit:         "ppm",
		InitialValue: 200,
		Topic:        "room001/input/humidity",
		Location:     "room001",
	},
	{
		Type:         Temperature,
		Place:        Outdoors,
		Topic:        "outdoors/particulate_matter",
		Location:     "outdoors",
		Name:         "temperature-outdoors",
		Unit:         "Celsius",
		InitialValue: 20,
	},
	{
		Type:         AirParticle,
		Place:        Outdoors,
		Topic:        "outdoors/particulate_matter",
		Location:     "outdoors",
		Name:         "air-particles-outdoors",
		Unit:         "ppm",
		InitialValue: 50,
	},
}

var actuators = []Actuator{
	{
		Type:     Ventilation,
		Name:     "ventilation",
		Topic:    "room001/output/ventilation",
		Location: "room001",
	},
	{
		Type:     Heater,
		Name:     "heater",
		Topic:    "room001/output/heater",
		Location: "room001",
	},
	{
		Type:     AirPurifier,
		Name:     "air-purifier",
		Topic:    "room001/output/air_purifier",
		Location: "room001",
	},
	{
		Type:     AirConditioner,
		Name:     "air-conditioner",
		Topic:    "room001/output/air_conditioning",
		Location: "room001",
	},
}

type SimulatorConfig struct {
	HTTPPort             int           `yaml:"http_port"`
	MQTTEndpoint         string        `yaml:"mqtt_endpoint"`
	MQTTPort             int           `yaml:"mqtt_port"`
	SensorUpdateInterval time.Duration `yaml:"sensor_update_interval"`
	Sensors              []Sensor      `yaml:"sensors"`
	Actuators            []Actuator    `yaml:"actuators"`
}

func GetDefaultConfig() SimulatorConfig {
	return SimulatorConfig{
		HTTPPort:             8080,
		MQTTEndpoint:         "127.0.0.1",
		MQTTPort:             1883,
		SensorUpdateInterval: time.Second * 15,
		Sensors:              sensors,
		Actuators:            actuators,
	}
}
