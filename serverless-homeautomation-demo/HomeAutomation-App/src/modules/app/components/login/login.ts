import {Component} from "@angular/core";
import "rxjs/add/operator/delay";
import {ActivatedRoute, Router} from "@angular/router";
import {OAuthService} from "angular-oauth2-oidc";

@Component({
  selector: "app-login",
  templateUrl: "login.html",
  styleUrls: ["login.scss"]
})
export class LoginComponent {
  public username: string;
  public password: string;
  public error: string;

  constructor(private _oauthService: OAuthService, private _activatedRoute: ActivatedRoute, private _router: Router) {
  }

  public login() {
    this
      ._oauthService
      .fetchTokenUsingPasswordFlowAndLoadUserProfile(this.username, this.password)
      .then(() => {
        console.debug("Successfully logged in.");
        this._router.navigate([this._activatedRoute.snapshot.queryParams["redirectTo"]]);
      })
      .catch((error) => {
        console.error("Error logging in: ", error);
        this.error = error
      });
  }
}
