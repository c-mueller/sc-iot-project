import {Component, Input, OnInit} from '@angular/core';
import {ActuatorInfo} from "../util/model";

@Component({
  selector: 'app-actuator',
  templateUrl: './actuator.component.html',
  styleUrls: ['./actuator.component.css']
})
export class ActuatorComponent implements OnInit {

  @Input()
  actuatorInfo: ActuatorInfo | undefined;

  constructor() {}

  ngOnInit(): void {
  }

}
