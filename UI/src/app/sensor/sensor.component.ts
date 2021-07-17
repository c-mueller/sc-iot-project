import {Component, Input, OnInit} from '@angular/core';
import {SensorMeasure, SensorType} from "../util/model";

@Component({
  selector: 'app-sensor',
  templateUrl: './sensor.component.html',
  styleUrls: ['./sensor.component.css']
})
export class SensorComponent implements OnInit {

  @Input()
  sensorMeasure: SensorMeasure | undefined;

  types = SensorType;

  constructor() {
  }

  ngOnInit(): void {}

  getDisplayNameForType(type: SensorType) {
    switch (type) {
      case SensorType.Temperature: {
        return 'Temperature';
      }
      case SensorType.Humidity: {
        return 'Humidity';
      }
      case SensorType.ParticulateMatter: {
        return 'Particulate matter';
      }
      case SensorType.CO2: {
        return 'CO2';
      }
      default: {
        return '';
      }
    }
  }

  getUnitForType(type: SensorType): string {
    switch (type) {
      case SensorType.Temperature: {
        return '°C';
      }
      case SensorType.Humidity: {
        return '%';
      }
      case SensorType.ParticulateMatter: {
        return 'ug/m3';
      }
      case SensorType.CO2: {
        return 'ppm';
      }
      default: {
        return '';
      }
    }
  }
}
