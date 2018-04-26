import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient } from "@angular/common/http";
import { environment } from '../../../environments/environment';

@Injectable()
export class ClimateDataService {
  constructor(private _http: HttpClient) {
  }

  public getData() {
    return this._http.get<any>(environment.WebApiBaseUrl + "climate/data");
  }
}
