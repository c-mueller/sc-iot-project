export enum SensorType
{
  Temperature,
  Humidity,
  ParticulateMatter,
  CO2,
}

export enum Location
{
  Inside,
  Outside,
}

export interface SensorContext {
  locations: SensorLocation[];
}

export interface SensorLocation {
  locationId: string;
  location: Location;
  measures: SensorMeasure[];
}

export interface SensorMeasure {
  name: string;
  type: SensorType;
  value: number;
  measuredAt: string;
}

export interface ActuatorInfo {
  name: string;
  active: boolean;
}
