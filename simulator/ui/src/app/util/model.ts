export enum SensorType {
  Temperature = "Temperature",
  Humidity = "Humidity",
  ParticulateMatter = "ParticulateMatter",
  CO2 = "CO2"
}

export enum ActuatorType {
  Heater = "Heater",
  Ventilation = "Ventilation",
  AirConditioner = "AirConditioner",
  AirPurifier = "AirPurifier"
}

export const SensorRanges: any = {
  [SensorType.Temperature]: {
    min: -20,
    max: 50,
    step: 0.25,
    unit: '°C'
  },
  [SensorType.Humidity]: {
    min: 0,
    max: 100,
    step: 0.5,
    unit: '%'
  },
  [SensorType.CO2]: {
    min: 0,
    max: 2500,
    step: 10,
    unit: 'ppm'
  },
  [SensorType.ParticulateMatter]: {
    min: 0,
    max: 200,
    step: 1,
    unit: 'ug/m³'
  }
}

export interface ActuatorMap {
  [name: string]: Actuator
}

export interface Actuator {
  state: string
  current_state: ActuatorState
  actuator: ActuatorInfo
}

export interface ActuatorState {
  active: boolean
  last_updated: string
}

export interface ActuatorInfo {
  type: string
  name: string
  location: string
}

export interface SensorMap {
  [name: string]: Sensor
}

export interface Sensor {
  current_state: SensorState
  sensor: SensorInfo
  state: string
}

export interface SensorInfo {
  location: string
  name: string
  place: string
  type: string
  unit: string
}

export interface SensorState {
  last_measured: string
  current_value: number
}
