import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient } from "@angular/common/http";
import { environment } from '../../../environments/environment';

@Injectable()
export class ClimateControlService {
  constructor(private _http: HttpClient) {
  }

  public startDataCollection(deviceId: string) {
    return this._http.get<any>(environment.WebApiBaseUrl + "climate/" + deviceId + "/start");
  }

  public stopDataCollection(deviceId: string) {
    return this._http.get<any>(environment.WebApiBaseUrl + "climate/" + deviceId + "/stop");
  }
}
