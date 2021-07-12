import {Component, OnInit} from '@angular/core';
import {ApiService} from "./util/svc/api.service";
import {Actuator, ActuatorMap, Sensor, SensorMap} from "./util/model";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  loading: boolean = true;

  sensors: any = {};
  actuators: Actuator[] = [];


  constructor(private api: ApiService) {
  }

  ngOnInit() {
    this.api.getSensorInfos().subscribe((e: SensorMap) => {
      this.sensors = {};
      for (let eKey in e) {
        const sensor = e[eKey];
        if (this.sensors[sensor.sensor.place] == null) {
          this.sensors[sensor.sensor.place] = [];
        }
        this.sensors[sensor.sensor.place].push(sensor);
      }
      this.loading = false;
    });
    this.api.getActuatorInfos().subscribe((e: ActuatorMap) => {
      for (let eKey in e) {
        const actuator = e[eKey];
        this.actuators.push(actuator);
      }
    })
  }

  getLocations(): string[] {
    const names = [];
    for (let sensorsKey in this.sensors) {
      names.push(sensorsKey);
    }
    return names;
  }

}
