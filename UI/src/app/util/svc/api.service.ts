import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Observable} from "rxjs";
import {ActuatorInfo, SensorContext} from "../model";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private client: HttpClient) {
  }

  getSensorInfos(): Observable<SensorContext> {
    return this.client.get<SensorContext>(environment.apiEndpoint + "/api/sensors");
  }

  getActuatorInfos(): Observable<ActuatorInfo[]> {
    return this.client.get<ActuatorInfo[]>(environment.apiEndpoint + "/api/actuators");
  }
}
