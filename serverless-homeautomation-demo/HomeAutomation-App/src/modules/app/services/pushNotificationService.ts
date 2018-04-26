import { Injectable } from "@angular/core";
import { PlatformService } from "./platformService";
import { environment } from "../../../environments/environment";

declare var window: any;

@Injectable()
export class PushNotificationService {
  private _pushNotification: any;

  constructor(private _platformService: PlatformService) {
  }

  public register() {
    if (this._platformService.isMobileDevice) {
      this._pushNotification = window.PushNotification.init({
        notificationHubPath: environment.NotificationHubPath,
        connectionString: environment.NotificationHubConnectionString,
        android: {
          sound: true
        },
        ios: {
          alert: true,
          badge: true,
          sound: false
        }
      });

      this._pushNotification.on("registration", function (data) {
        //console.log(data.registrationId);
        //console.log(data.azureRegId);

        //alert(JSON.stringify(data));
      });

      this._pushNotification.on("notification", function (data) {
        /*console.log(data.message);
        console.log(data.title);
        console.log(data.count);
        console.log(data.sound);
        console.log(data.image);
        console.log(data.additionalData);*/

        alert(data.message);
      });
    }
  }
}
