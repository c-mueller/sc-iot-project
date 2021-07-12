import {Component, Input, OnInit} from '@angular/core';
import {ApiService} from "../util/svc/api.service";
import {Sensor, SensorRanges, SensorType} from "../util/model";

@Component({
  selector: 'app-sensor',
  templateUrl: './sensor.component.html',
  styleUrls: ['./sensor.component.css']
})
export class SensorComponent implements OnInit {

  @Input()
  sensorName: string = "";

  sensor: Sensor | null = null;
  sensorConfig = SensorRanges[SensorType.Temperature];

  loaded: boolean = false;

  nValSet = false;
  newValue = 0

  constructor(private api: ApiService) {
  }

  ngOnInit(): void {
    setInterval(() => {
      this.api.getSensor(this.sensorName).subscribe((e) => {
        this.sensor = e;
        this.sensorConfig = SensorRanges[e.sensor.type];
        this.loaded = true;
        if (!this.nValSet) {
          this.newValue = this.sensor.current_state.current_value;
          this.nValSet = true;
        }
      })
    }, 500);
  }

  sendValue($event: MouseEvent) {
    this.api.updateSensor(this.sensorName, this.newValue).subscribe(e => {
    });
  }
}
