package sensorworker

import (
	"github.com/c-mueller/sc-iot-project/simulator/model"
	"github.com/sirupsen/logrus"
	"time"
)

type Worker struct {
	Sensor model.Sensor
	UpdateInterval time.Duration

	currentValue float64
	lastEmitted time.Time

	logger *logrus.Entry
}