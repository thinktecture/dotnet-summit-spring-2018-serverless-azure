import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from '../../../environments/environment';

@Injectable()
export class PhoneRegistrationService {
  private _sendSmsChallengeResponseUri: string;

  constructor(private _http: HttpClient) {
  }

  public registerPhoneNumber(phoneNumber: string) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    return this._http.post(environment.WebApiBaseUrl + "orchestrators/SmsPhoneVerification/",
      JSON.stringify(phoneNumber),
      { headers: headers })
      .map(data => {
        this._sendSmsChallengeResponseUri = (<any>data).sendEventPostUri;
        this._sendSmsChallengeResponseUri = this._sendSmsChallengeResponseUri.replace("{eventName}", "SmsChallengeResponse");
        return data;
      });
  }

  public sendChallengeCode(code: string) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    return this._http.post(this._sendSmsChallengeResponseUri, JSON.stringify(code), { headers: headers });
  }
}
