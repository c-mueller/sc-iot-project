package model

import "github.com/sirupsen/logrus"

type BrokerConfig struct {
	Hostname string
	Port     int
	Topic    string
}

type Worker interface {
	GetWorkerDeviceName() string
	GetState() WorkerState
	Init(entry *logrus.Entry, config BrokerConfig) error
	Start() error
	Stop() error
}

type SensorWorker interface {
	Worker
	GetCurrentState() SensorState
	SetValue(value float64)
}

type ActuatorWorker interface {
	Worker
	GetCurrentState() ActuatorState
}
