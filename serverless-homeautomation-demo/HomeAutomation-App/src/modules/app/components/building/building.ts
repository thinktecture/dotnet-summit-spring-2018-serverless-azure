import {Component, OnInit} from "@angular/core";
import {BuildingInfoService} from "../../services/buildingInfoService";
import { environment } from "../../../../environments/environment";

@Component({
  selector: "app-building",
  templateUrl: "building.html",
  styleUrls: ["building.scss"]
})
export class BuildingComponent implements OnInit {
  public hubDeviceId: string = environment.DefaultHubDevice;
  public buildingInfo: any;

  constructor(private _buildingInfoService: BuildingInfoService) {
  }

  public ngOnInit(): void {
    this.getInfo();
  }

  public getInfo() {
    this._buildingInfoService.getInfo(this.hubDeviceId)
      .subscribe(data => {
        const nodes = data.rooms.map(room => {
          return {
            name: room.name,
            devices: room.devices
          };
        });

        this.buildingInfo = nodes;
      });
  }
}
