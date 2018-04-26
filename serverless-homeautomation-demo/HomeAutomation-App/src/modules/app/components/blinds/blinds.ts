import {Component} from "@angular/core";
import {BlindsControlService} from "../../services/blindsControlService";
import {DeviceStateService} from "../../services/deviceStateService";
import {Observable} from "rxjs/Observable";
import 'rxjs/add/observable/timer';
import 'rxjs/add/operator/switchMap';
import { environment } from "../../../../environments/environment";

@Component({
  selector: "app-blinds",
  templateUrl: "blinds.html",
  styleUrls: ["blinds.scss"]
})
export class BlindsControlComponent {
  public currentHubDeviceId: string = environment.DefaultHubDevice;
  public currentTargetDevice: string = "livingroom-blinds";
  public currentStatus: string = "OK";
  public isChecking: boolean = false;

  constructor(private _controlService: BlindsControlService,
              private _statusService: DeviceStateService) {
  }

  public up() {
    this._controlService.moveUp(this.currentHubDeviceId, this.currentTargetDevice)
      .subscribe(() => {
        this.checkForStatusComplete();
      });
  }

  public down() {
    this._controlService.moveDown(this.currentHubDeviceId, this.currentTargetDevice)
      .subscribe(() => {
        this.checkForStatusComplete();
      });
  }

  public stop() {
    this._controlService.stop(this.currentHubDeviceId, this.currentTargetDevice)
      .subscribe(() => {
        this.checkForStatusComplete();
      });
  }

  private checkForStatusComplete() {
    this.isChecking = true;
    this.currentStatus = "...";

    Observable.timer(0, 4000)
      .switchMap(() => this._statusService.getState(this.currentHubDeviceId, this.currentTargetDevice))
      .first((response: any) => {
        if (response.status === 202) {
          this.currentStatus = response.body.position + "% ...";
          return false;
        }
        return true;
      })
      .subscribe(response => {
        this.currentStatus = response.body.position + "%";
        this.isChecking = false;
      });
  }
}
