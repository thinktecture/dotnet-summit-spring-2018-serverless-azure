import {Component} from "@angular/core";
import {ClimateControlService} from "../../../services/climateControlService";

@Component({
  selector: 'app-climate-control',
  templateUrl: 'control.html',
  styleUrls: ['control.scss']
})
export class ClimateControlComponent {
  // TODO: get this from a server API
  public deviceIDs = [
    "hub-weyer-1",
    "hub-mcu-weyer-1"
  ]

  public deviceId: string = this.deviceIDs[0];

  constructor(private _climateControlService: ClimateControlService) {
  }

  public start() {
    this._climateControlService.startDataCollection(this.deviceId).subscribe();
  }

  public stop() {
    this._climateControlService.stopDataCollection(this.deviceId).subscribe();
  }
}
