import {Component} from '@angular/core';
import 'rxjs/add/operator/retrywhen';
import {DesktopIntegrationService} from '../../services/desktopIntegrationService';
import {OAuthService} from 'angular-oauth2-oidc';
import {resourceOwnerConfig} from '../../auth.config';
import {PushNotificationService} from '../../services/pushNotificationService';
import {routeTransition} from '../../routeAnimations';
import {WebPushService} from '../../services/webPushService';

@Component({
  selector: 'app-root',
  animations: [routeTransition],
  templateUrl: 'root.html',
  styleUrls: ['root.scss']
})
export class RootComponent {
  constructor(private _oauthService: OAuthService,
              private _desktopIntegration: DesktopIntegrationService,
              private _pushNotificationService: PushNotificationService) {
    this._desktopIntegration.register();
    this._pushNotificationService.register();

    this.initOAuth();
  }

  public getState(outlet) {
    return outlet.activatedRouteData.state;
  }

  private initOAuth() {
    this._oauthService.setStorage(localStorage);
    this._oauthService.configure(resourceOwnerConfig);
    this._oauthService.loadDiscoveryDocument();
  }
}
