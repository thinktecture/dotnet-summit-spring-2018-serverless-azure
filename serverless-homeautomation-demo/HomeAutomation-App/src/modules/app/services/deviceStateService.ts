import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient } from "@angular/common/http";
import { environment } from '../../../environments/environment';

@Injectable()
export class DeviceStateService {
  constructor(private _http: HttpClient) {
  }

  public getState(hubDeviceId: string, targetDevice: string) {
    return this._http.get(environment.WebApiBaseUrl + "devicestate/" + encodeURIComponent(hubDeviceId) + "/" + encodeURIComponent(targetDevice), { observe: 'response' });
  }
}
