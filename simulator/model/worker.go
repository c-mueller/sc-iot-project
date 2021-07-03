package model

type Worker interface {
	GetWorkerDeviceName() string
	GetState() WorkerState
	Init() error
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
