package model

import (
	"bytes"
	"encoding/json"
)

type SensorType uint

const (
	Temperature       SensorType = iota
	Humidity                     = Temperature + 1
	ParticulateMatter            = Humidity + 1
	CO2                          = ParticulateMatter + 1
)

var sensorTypeToString = map[SensorType]string{
	Temperature:       "Temperature",
	Humidity:          "Humidity",
	ParticulateMatter: "ParticulateMatter",
	CO2:               "CO2",
}

var sensorTypeFromString = map[string]SensorType{
	"Temperature":       Temperature,
	"Humidity":          Humidity,
	"ParticulateMatter": ParticulateMatter,
	"CO2":               CO2,
}

func (s SensorType) String() string {
	return sensorTypeToString[s]
}

func (s SensorType) MarshalJSON() ([]byte, error) {
	buffer := bytes.NewBufferString(`"`)
	buffer.WriteString(sensorTypeToString[s])
	buffer.WriteString(`"`)
	return buffer.Bytes(), nil
}

func (s *SensorType) UnmarshalJSON(b []byte) error {
	var j string
	err := json.Unmarshal(b, &j)
	if err != nil {
		return err
	}
	*s = sensorTypeFromString[j]
	return nil
}

func (s SensorType) MarshalYAML() (interface{}, error) {
	return sensorTypeToString[s], nil
}

func (s *SensorType) UnmarshalYAML(unmarshal func(interface{}) error) error {
	var typeString string
	err := unmarshal(&typeString)
	if err != nil {
		return err
	}
	*s = sensorTypeFromString[typeString]
	return nil
}
