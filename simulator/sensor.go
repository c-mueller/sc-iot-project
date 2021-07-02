package main

type SensorType uint
type ActuatorType uint
type PlaceType uint

const (
	Temperature SensorType = iota
	Humidity               = Temperature + 1
	AirParticle            = Humidity + 1
	CO2                    = AirParticle + 1

	Ventilation ActuatorType = iota
	Heater                   = Ventilation + 1

	Indoors  PlaceType = iota
	Outdoors           = Indoors + 1
)

type Sensor struct {
	Type  SensorType `json:"type"`
	Place PlaceType  `json:"place"`
	Name  string     `json:"name"`
	Unit  string     `json:"unit"`

	CurrentValue float64 `json:"current_value"`
}

type Actuator struct {
	Type  ActuatorType `json:"type"`
	Place PlaceType    `json:"place"`
	Name  string       `json:"name"`
}
