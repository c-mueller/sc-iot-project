import {Component, Input, OnInit} from '@angular/core';
import {ApiService} from "../util/svc/api.service";
import {Actuator, SensorRanges} from "../util/model"

@Component({
  selector: 'app-actuator',
  templateUrl: './actuator.component.html',
  styleUrls: ['./actuator.component.css']
})
export class ActuatorComponent implements OnInit {

  @Input()
  actuatorName: string = "";

  loaded: boolean = false;
  actuator: Actuator | null = null;

  constructor(public api: ApiService) {
  }

  ngOnInit(): void {
    setInterval(() => {
      this.api.getActuator(this.actuatorName).subscribe((e) => {
        this.actuator = e;
        this.loaded = true;
      })
    }, 500);
  }

}
