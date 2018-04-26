import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/operator/take";
import "rxjs/add/observable/throw";
import { SwPush } from "@angular/service-worker";
import { ToastrService } from "ngx-toastr";
import { environment } from "../../../environments/environment";

@Injectable()
export class WebPushService {
  private _webApiBaseUrl: string

  constructor(private _http: HttpClient,
              private _swPush: SwPush,
              private _toast: ToastrService) {
    this._webApiBaseUrl = environment.WebApiBaseUrl;
  }

  public subscribeToPush() {
    this._swPush.requestSubscription({
      serverPublicKey: environment.VapIdPublicKey
    })
      .then(pushSubscription => {
        this.addSubscriber(pushSubscription)
          .subscribe(
            res => {
              console.log("[App] Add subscriber request answer", res);
              this._toast.success("Subscribed", "Push");

              this._swPush.messages
                .subscribe((notification: any) => {
                  console.log("[App] Push message received", notification);

                  this._toast.info(notification.notification.body.toString(), "Notification");
                });
            },
            err => {
              console.log("[App] Add subscriber request failed", err);
            }
          )
      })
      .catch(err => {
        console.error(err);
      });
  }

  public unsubscribeFromPush() {
    this._swPush.subscription
      .take(1)
      .subscribe(pushSubscription => {
        console.log("[App] pushSubscription", pushSubscription);

        this.deleteSubscriber(pushSubscription)
          .subscribe(
            res => {
              console.log("[App] Delete subscriber request answer", res);

              pushSubscription.unsubscribe()
                .then(success => {
                  console.log("[App] Unsubscription successful", success);
                  this._toast.success("Unsubscribed", "Push");
                })
                .catch(err => {
                  console.log("[App] Unsubscription failed", err);
                  this._toast.error("Unsubscribe...", "Push");
                })
            },
            err => {
              console.log("[App] Delete subscription request failed", err);
            }
          )
      });
  }

  public addSubscriber(subscription) {
    const url = `${this._webApiBaseUrl}webpush`;
    console.log("[Push Service] Adding subscriber");

    let body = {
      action: "subscribe",
      subscription: subscription
    };

    return this._http
      .post(url, body)
      .catch(this.handleError);
  }

  public deleteSubscriber(subscription) {
    const url = `${this._webApiBaseUrl}webpush`;
    console.log("[Push Service] Deleting subscriber");

    let body = {
      action: "unsubscribe",
      subscription: subscription
    };

    return this._http
      .post(url, body)
      .catch(this.handleError);
  }

  private handleError(error: Response | any) {
    let message: string;

    if (error instanceof Response) {
      message = `${error.statusText || "Network error"}`;
    } else {
      message = error.message ? error.message : error.toString();
    }

    return Observable.throw(message);
  }
}
