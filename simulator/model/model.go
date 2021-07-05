package model

import "time"

type SensorMessage struct {
	SensorName string    `json:"sensor_name"`
	Unit       string    `json:"unit"`
	Value      float64   `json:"value"`
	MeasuredAt time.Time `json:"measured_at"`
}

type Sensor struct {
	Type         SensorType `json:"type"`
	Place        PlaceType  `json:"place"`
	Name         string     `json:"name"`
	Unit         string     `json:"unit"`
	InitialValue float64    `json:"-"`
}

type SensorState struct {
	SensorName   string    `json:"sensor_name"`
	Sensor       Sensor    `json:"sensor"`
	CurrentValue float64   `json:"current_value"`
	LastMeasured time.Time `json:"last_measured"`
}

type Actuator struct {
	Type ActuatorType `json:"type"`
	Name string       `json:"name"`
}

type ActuatorState struct {
	ActuatorName string    `json:"actuator_name"`
	Actuator     Actuator  `json:"actuator"`
	Active       bool      `json:"active"`
	LastUpdated  time.Time `json:"last_updated"`
}
