package model

import (
	"bytes"
	"encoding/json"
)

type PlaceType uint

const (
	Indoors  PlaceType = iota
	Outdoors           = Indoors + 1
)

var placeTypeToString = map[PlaceType]string{
	Indoors:  "Indoors",
	Outdoors: "Outdoors",
}

var placeTypeFromString = map[string]PlaceType{
	"Indoors":  Indoors,
	"Outdoors": Outdoors,
}

func (s PlaceType) String() string {
	return placeTypeToString[s]
}

func (s PlaceType) MarshalJSON() ([]byte, error) {
	buffer := bytes.NewBufferString(`"`)
	buffer.WriteString(placeTypeToString[s])
	buffer.WriteString(`"`)
	return buffer.Bytes(), nil
}

func (s *PlaceType) UnmarshalJSON(b []byte) error {
	var j string
	err := json.Unmarshal(b, &j)
	if err != nil {
		return err
	}
	*s = placeTypeFromString[j]
	return nil
}

