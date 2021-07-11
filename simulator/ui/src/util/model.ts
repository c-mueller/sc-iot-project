export enum SensorType {
  Temperature = "Temperature",
  Humidity = "Humidity",
  AirParticle = "AirParticle",
  CO2 = "CO2"
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
    max: 10000,
    step: 1,
    unit: 'ppm'
  },
  [SensorType.AirParticle]: {
    min: 0,
    max: 10000,
    step: 1,
    unit: 'ug/m³'
  }
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
