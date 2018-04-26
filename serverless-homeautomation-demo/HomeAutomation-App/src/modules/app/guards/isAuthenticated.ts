import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from "@angular/router";
import {Observable} from "rxjs/Rx";
import {Injectable} from "@angular/core";
import {OAuthService} from "angular-oauth2-oidc";
import { environment } from "../../../environments/environment";

@Injectable()
export class IsAuthenticated implements CanActivate {
  constructor(private _oauthService: OAuthService, private _router: Router) {
    if (!environment.LoginRoute) {
      throw new Error("Login route has not been configured.");
    }
  }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    var result = this._oauthService.hasValidAccessToken();

    if (!result) {
      this._router.navigate([environment.LoginRoute], {
        queryParams: {
          redirectTo: state.url
        }
      });

      return false;
    }

    return true;
  }
}
