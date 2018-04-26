import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient } from "@angular/common/http";
import { environment } from '../../../environments/environment';

@Injectable()
export class BuildingInfoService {
  constructor(private _http: HttpClient) {
  }

  public getInfo(hubDeviceId: string) {
    return this._http.get<any>(environment.WebApiBaseUrl + "buildinginfo/" + encodeURIComponent(hubDeviceId));
  }
}
