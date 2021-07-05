package model

import (
	"bytes"
	"encoding/json"
)

type WorkerState uint

const (
	Active   WorkerState = iota
	Inactive             = Active + 1
)

var workerStateToString = map[WorkerState]string{
	Active:  "Active",
	Inactive: "Inactive",
}

var workerStateFromString = map[string]WorkerState{
	"Active":  Active,
	"Inactive": Inactive,
}

func (s WorkerState) String() string {
	return workerStateToString[s]
}

func (s WorkerState) MarshalJSON() ([]byte, error) {
	buffer := bytes.NewBufferString(`"`)
	buffer.WriteString(workerStateToString[s])
	buffer.WriteString(`"`)
	return buffer.Bytes(), nil
}

func (s *WorkerState) UnmarshalJSON(b []byte) error {
	var j string
	err := json.Unmarshal(b, &j)
	if err != nil {
		return err
	}
	*s = workerStateFromString[j]
	return nil
}