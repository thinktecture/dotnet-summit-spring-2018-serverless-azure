import {Component} from "@angular/core";
import {Location} from "@angular/common";
import {PlatformService} from "../../services/platformService";
import {Router} from "@angular/router";
import {OAuthService} from "angular-oauth2-oidc";

@Component({
  selector: "app-header",
  templateUrl: "header.html",
  styleUrls: ["header.scss"]
})
export class HeaderComponent {
  public isLoggedIn: boolean = false;

  public get isBackChevronVisible(): boolean {
    return this._location.path() !== "/home" && this._platform.isIOS;
  }

  constructor(private _location: Location, private _router: Router, private _platform: PlatformService, private _oauthService: OAuthService) {
    this.isLoggedIn = _oauthService.hasValidAccessToken();
    this._oauthService.events.filter(e => e.type === "user_profile_loaded").subscribe(e => {
      this.isLoggedIn = true;
    })
  }

  public logout(): void {
    this._oauthService.logOut(false);
    this.isLoggedIn = false;
    this._router.navigate(["/home"]);
  }

  public goBack() {
    this._location.back();
  }
}
