package model

import (
	"bytes"
	"encoding/json"
)

type ActuatorType uint

const (
	Ventilation    ActuatorType = iota
	Heater                      = Ventilation + 1
	AirConditioner              = Heater + 1
	AirPurifier                 = AirConditioner + 1
)

var actuatorTypeToString = map[ActuatorType]string{
	Ventilation:    "Ventilation",
	AirPurifier:    "AirPurifier",
	Heater:         "Heater",
	AirConditioner: "AirConditioner",
}

var actuatorTypeFromString = map[string]ActuatorType{
	"Ventilation":    Ventilation,
	"AirPurifier":    AirPurifier,
	"Heater":         Heater,
	"AirConditioner": AirConditioner,
}

func (s ActuatorType) String() string {
	return actuatorTypeToString[s]
}

func (s ActuatorType) MarshalJSON() ([]byte, error) {
	buffer := bytes.NewBufferString(`"`)
	buffer.WriteString(actuatorTypeToString[s])
	buffer.WriteString(`"`)
	return buffer.Bytes(), nil
}

func (s *ActuatorType) UnmarshalJSON(b []byte) error {
	var j string
	err := json.Unmarshal(b, &j)
	if err != nil {
		return err
	}
	*s = actuatorTypeFromString[j]
	return nil
}

func (s ActuatorType) MarshalYAML() (interface{}, error) {
	return actuatorTypeToString[s], nil
}

func (s *ActuatorType) UnmarshalYAML(unmarshal func(interface{}) error) error {
	var typeString string
	err := unmarshal(&typeString)
	if err != nil {
		return err
	}
	*s = actuatorTypeFromString[typeString]
	return nil
}
