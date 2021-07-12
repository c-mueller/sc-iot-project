import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {HttpClientModule} from "@angular/common/http";
import { SensorComponent } from './sensor/sensor.component';
import {FormsModule} from "@angular/forms";
import { ActuatorComponent } from './actuator/actuator.component';
import { FormatDatePipe } from './util/format-date.pipe';

@NgModule({
  declarations: [
    AppComponent,
    SensorComponent,
    ActuatorComponent,
    FormatDatePipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
