import {Component, OnInit} from '@angular/core';
import {ApiService} from "./util/svc/api.service";
import {ActuatorInfo, SensorContext} from "./util/model";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  sensors: SensorContext | undefined;
  actuators: ActuatorInfo[] = [];

  constructor(private api: ApiService) {
  }

  ngOnInit() {
    setInterval(() => {
      this.api.getSensorInfos().subscribe((e: SensorContext) => {
        this.sensors = e;
      });
    }, 500);

    setInterval(() => {
      this.api.getActuatorInfos().subscribe((e: ActuatorInfo[]) => {
        this.actuators = e;
      })
    }, 500);
  }
}
