import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {HttpClientModule} from "@angular/common/http";
import {SensorComponent} from './sensor/sensor.component';
import {FormsModule} from "@angular/forms";
import {ActuatorComponent} from './actuator/actuator.component';
import {FormatDatePipe} from './util/format-date.pipe';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatCardModule} from "@angular/material/card";

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
    FormsModule,
    BrowserAnimationsModule,
    MatCardModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
