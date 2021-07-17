import {Component, Input, OnInit} from '@angular/core';
import {ApiService} from "../util/svc/api.service";
import {SensorMeasure, SensorType} from "../util/model";

@Component({
  selector: 'app-sensor',
  templateUrl: './sensor.component.html',
  styleUrls: ['./sensor.component.css']
})
export class SensorComponent implements OnInit {

  @Input()
  sensorMeasure: SensorMeasure | undefined;

  nValSet = false;
  newValue = 0

  constructor(private api: ApiService) {
  }

  ngOnInit(): void {

  }
}
