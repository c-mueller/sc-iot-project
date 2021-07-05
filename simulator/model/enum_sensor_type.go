package model

import (
	"bytes"
	"encoding/json"
)

type SensorType uint

const (
	Temperature SensorType = iota
	Humidity               = Temperature + 1
	AirParticle            = Humidity + 1
	CO2                    = AirParticle + 1
)

var sensorTypeToString = map[SensorType]string{
	Temperature: "Temperature",
	Humidity:    "Humidity",
	AirParticle: "AirParticle",
	CO2:         "CO2",
}

var sensorTypeFromString = map[string]SensorType{
	"Temperature": Temperature,
	"Humidity":    Humidity,
	"AirParticle": AirParticle,
	"CO2":         CO2,
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
