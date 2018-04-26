import {Routes} from '@angular/router';
import {HomeComponent} from './components/home/home';
import {LoginComponent} from './components/login/login';
import {IsAuthenticated} from './guards/isAuthenticated';
import {BlindsControlComponent} from "./components/blinds/blinds";
import {BuildingComponent} from "./components/building/building";
import {ClimateControlComponent} from "./components/climate/control/control";
import {ClimateDataComponent} from "./components/climate/data/data";
import { PhoneVerificationComponent } from './components/phoneVerification/phoneVerification';

export const ROUTES: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '/home'
  },
  {
    path: 'home',
    component: HomeComponent,
    data: { state: 'home' }
  },
  {
    path: 'building',
    component: BuildingComponent,
    canActivate: [IsAuthenticated],
    data: { state: 'building' }
  },
  {
    path: 'blinds',
    component: BlindsControlComponent,
    canActivate: [IsAuthenticated],
    data: { state: 'blinds' }
  },
  {
    path: 'climate',
    children: [
      {
        path: 'control',
        component: ClimateControlComponent,
        canActivate: [IsAuthenticated],
        data: { state: 'climate' }
      },
      {
        path: 'data',
        component: ClimateDataComponent,
        canActivate: [IsAuthenticated],
        data: { state: 'climate' }
      }
    ]
  },
  {
    path: 'phone',
    component: PhoneVerificationComponent,
    canActivate: [IsAuthenticated],
    data: { state: 'phone' }
  }
];
