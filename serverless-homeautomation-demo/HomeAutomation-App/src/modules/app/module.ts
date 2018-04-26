import {BrowserModule} from "@angular/platform-browser";
import {ErrorHandler, NgModule} from "@angular/core";
import {FormsModule} from "@angular/forms";
import {RouterModule} from "@angular/router";

import {RootComponent} from "./components/root/root";
import {ROUTES} from "./routes";
import {HomeComponent} from "./components/home/home";
import {HeaderComponent} from "./components/header/header";
import {MenuComponent} from "./components/menu/menu";
import {WindowRef} from "./services/windowRef";
import {PlatformService} from "./services/platformService";
import {NgxElectronModule} from "ngx-electron";
import {DesktopIntegrationService} from "./services/desktopIntegrationService";
import {LoginComponent} from "./components/login/login";
import {IsAuthenticated} from "./guards/isAuthenticated";
import {BlindsControlComponent} from "./components/blinds/blinds";
import {BlindsControlService} from "./services/blindsControlService";
import {OAuthModule, OAuthService} from "angular-oauth2-oidc";
import {DeviceStateService} from "./services/deviceStateService";
import {BuildingInfoService} from "./services/buildingInfoService";
import {BuildingComponent} from "./components/building/building";
import {PushNotificationService} from "./services/pushNotificationService";
import {ToastrModule} from "ngx-toastr";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {GlobalErrorHandler} from "./services/globalErrorHandler";
import {ClimateDataComponent} from "./components/climate/data/data";
import {ClimateDataService} from "./services/climateDataService";
import {ClimateControlService} from "./services/climateControlService";
import {ClimateControlComponent} from "./components/climate/control/control";
import {ServiceWorkerModule} from "@angular/service-worker";
import {environment} from "../../environments/environment.prod";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {NgProgressModule} from "@ngx-progressbar/core";
import {AuthInterceptor} from "./services/authInterceptor";
import {NgProgressHttpClientModule} from "@ngx-progressbar/http-client";
import {HttpModule} from "@angular/http";
import {WebPushService} from "./services/webPushService";
import { PhoneVerificationComponent } from "./components/phoneVerification/phoneVerification";
import { PhoneRegistrationService } from "./services/phoneRegistrationService";

@NgModule({
  declarations: [
    RootComponent,
    LoginComponent,
    HomeComponent,
    HeaderComponent,
    MenuComponent,
    BlindsControlComponent,
    BuildingComponent,
    ClimateDataComponent,
    ClimateControlComponent,
    PhoneVerificationComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    RouterModule.forRoot(ROUTES, {useHash: true}),
    NgProgressModule.forRoot(),
    NgProgressHttpClientModule,
    NgxElectronModule,
    OAuthModule.forRoot(),
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: "toast-top-full-width",
      closeButton: true
    }),
    ServiceWorkerModule.register("/ngsw-worker.js", {enabled: environment.production})
  ],
  bootstrap: [RootComponent],
  providers: [
    OAuthService,
    WindowRef,
    BlindsControlService,
    DeviceStateService,
    BuildingInfoService,
    ClimateDataService,
    ClimateControlService,
    PlatformService,
    DesktopIntegrationService,
    IsAuthenticated,
    PhoneRegistrationService,
    PushNotificationService,
    WebPushService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    }
  ]
})
export class AppModule {
}
