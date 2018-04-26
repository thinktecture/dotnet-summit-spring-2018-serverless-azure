import {Injectable, Injector} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {OAuthService} from "angular-oauth2-oidc";
import {Observable} from "rxjs/Observable";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private securityService: OAuthService;

  constructor(private injector: Injector) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let requestToForward = req;

    if (this.securityService === undefined) {
      this.securityService = this.injector.get(OAuthService);
    }

    if (this.securityService !== undefined) {
      let token = this.securityService.getAccessToken();

      if (token !== "") {
        let tokenValue = "Bearer " + token;
        requestToForward = req.clone({setHeaders: {"Authorization": tokenValue}});
      }
    } else {
      console.debug("OidcSecurityService undefined: NO auth header!");
    }

    return next.handle(requestToForward);
  }
}
