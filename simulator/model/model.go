package model

import "time"

type SensorMessage struct {
	Location   string    `json:"location"`
	SensorType string    `json:"sensortype"`
	Value      float64   `json:"value"`
	Timestamp  time.Time `json:"timestamp"`
}

type ActuatorMessage struct {
	Active bool `json:"active"`
}

type Sensor struct {
	Type         SensorType `json:"type" yaml:"type"`
	Place        PlaceType  `json:"place" yaml:"place"`
	Name         string     `json:"name" yaml:"name"`
	Unit         string     `json:"unit" yaml:"unit"`
	InitialValue float64    `json:"-" yaml:"initial_value"`
	Topic        string     `json:"-" yaml:"topic"`
	Location     string     `json:"location" yaml:"location"`
}

type SensorState struct {
	SensorName   string    `json:"-"`
	Sensor       Sensor    `json:"-"`
	CurrentValue float64   `json:"current_value"`
	LastMeasured time.Time `json:"last_measured"`
}

type Actuator struct {
	Type     ActuatorType `json:"type" yaml:"type"`
	Name     string       `json:"name" yaml:"name"`
	Topic    string       `json:"-" yaml:"topic"`
	Location string       `json:"location" yaml:"location"`
}

type ActuatorState struct {
	ActuatorName string    `json:"-"`
	Actuator     Actuator  `json:"-"`
	Active       bool      `json:"active"`
	LastUpdated  time.Time `json:"last_updated"`
}
