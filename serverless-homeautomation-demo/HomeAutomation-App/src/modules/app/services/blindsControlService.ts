import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import {HttpClient} from "@angular/common/http";
import { environment } from '../../../environments/environment';

@Injectable()
export class BlindsControlService {
  constructor(private _http: HttpClient) {
  }

  public moveUp(hubDeviceId: string, targetDevice: string) {
    return this._http.get<any>(environment.WebApiBaseUrl + "blinds/" + encodeURIComponent(hubDeviceId) + "/" + encodeURIComponent(targetDevice) + "/up");
  }

  public moveDown(hubDeviceId: string, targetDevice: string) {
    return this._http.get<any>(environment.WebApiBaseUrl + "blinds/" + encodeURIComponent(hubDeviceId) + "/" + encodeURIComponent(targetDevice) + "/down");
  }

  public stop(hubDeviceId: string, targetDevice: string) {
    return this._http.get<any>(environment.WebApiBaseUrl + "blinds/" + encodeURIComponent(hubDeviceId) + "/" + encodeURIComponent(targetDevice) + "/stop");
  }
}
