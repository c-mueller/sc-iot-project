package model

import "time"

type SensorMessage struct {
	SensorName string    `json:"sensor_name"`
	Unit       string    `json:"unit"`
	Value      float64   `json:"value"`
	MeasuredAt time.Time `json:"measured_at"`
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
	SensorName   string    `json:"sensor_name"`
	Sensor       Sensor    `json:"-"`
	CurrentValue float64   `json:"current_value"`
	LastMeasured time.Time `json:"last_measured"`
}

type Actuator struct {
	Type     ActuatorType `json:"type" yaml:"type"`
	Name     string       `json:"name" yaml:"name"`
	Topic    string       `json:"topic" yaml:"topic"`
	Location string       `json:"location" yaml:"location"`
}

type ActuatorState struct {
	ActuatorName string    `json:"actuator_name"`
	Actuator     Actuator  `json:"actuator"`
	Active       bool      `json:"active"`
	LastUpdated  time.Time `json:"last_updated"`
}
