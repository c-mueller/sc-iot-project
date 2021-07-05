package core

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/gin-gonic/gin"
	"strconv"
)

type sensorInfo struct {
	State        model.WorkerState `json:"state"`
	CurrentState model.SensorState `json:"current_state"`
	Sensor       model.Sensor      `json:"sensor"`
}

func (s *Simulator) SetSensorValue(context *gin.Context) {
	worker := s.sensorWorkers[context.Param("name")]
	if worker == nil {
		context.String(404, "Sensor not Found")
		return
	}

	value, success := context.GetPostForm("value")
	if !success {
		context.String(400, "Invalid input")
		return
	}
	parsedValue, err := strconv.ParseFloat(value, 64)
	if err != nil {
		context.String(400, "Invalid input. Input must be a floating point number")
		return
	}
	worker.SetValue(parsedValue)
	context.Status(200)
}

func (s *Simulator) GetSensor(context *gin.Context) {
	worker := s.sensorWorkers[context.Param("name")]
	if worker == nil {
		context.String(404, "Sensor not Found")
		return
	}

	workerState := worker.GetCurrentState()
	workerSensorInfo := sensorInfo{
		State:        worker.GetState(),
		CurrentState: workerState,
		Sensor:       workerState.Sensor,
	}
	context.JSON(200, workerSensorInfo)
}

func (s *Simulator) ListSensors(context *gin.Context) {
	workerSensorInfo := make(map[string]sensorInfo, 0)
	for _, worker := range s.sensorWorkers {
		workerState := worker.GetCurrentState()
		workerSensorInfo[worker.GetWorkerDeviceName()] = sensorInfo{
			State:        worker.GetState(),
			CurrentState: workerState,
			Sensor:       workerState.Sensor,
		}
	}

	context.JSON(200, workerSensorInfo)
}

func (s *Simulator) ListActuators(context *gin.Context) {
	context.JSON(200, s.Actuators)
}
