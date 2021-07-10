import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../environments/environment";
import {Observable} from "rxjs";
import {Sensor, SensorInfo, SensorMap} from "../util/model";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private client: HttpClient) {
  }

  getSensorInfos(): Observable<SensorMap> {
    return this.client.get<SensorMap>(environment.apiEndpoint + "/api/sensors");
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
