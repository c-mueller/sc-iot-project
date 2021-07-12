import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Observable} from "rxjs";
import {Actuator, ActuatorMap, Sensor, SensorInfo, SensorMap} from "../model";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private client: HttpClient) {
  }

  getSensorInfos(): Observable<SensorMap> {
    return this.client.get<SensorMap>(environment.apiEndpoint + "/api/sensors");
  }

  getActuatorInfos(): Observable<ActuatorMap> {
    return this.client.get<ActuatorMap>(environment.apiEndpoint + "/api/actuators");
  }

  getActuator(name: string): Observable<Actuator> {
    return this.client.get<Actuator>(environment.apiEndpoint + "/api/actuators/" + name);
  }

  getSensor(name: string): Observable<Sensor> {
    return this.client.get<Sensor>(environment.apiEndpoint + "/api/sensors/" + name);
  }

  updateSensor(name: string, newValue: number): Observable<any> {
    const formData = new FormData();
    formData.append("value", newValue.toString())
    return this.client.post(environment.apiEndpoint + "/api/sensors/" + name, formData)
  }
}
