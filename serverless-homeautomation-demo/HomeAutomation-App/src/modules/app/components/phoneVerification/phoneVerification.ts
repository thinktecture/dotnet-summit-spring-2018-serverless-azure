import {Component} from '@angular/core';
import { PhoneRegistrationService } from '../../services/phoneRegistrationService';

@Component({
  selector: 'app-phone-verification',
  templateUrl: 'phoneVerification.html',
  styleUrls: ['phoneVerification.scss']
})
export class PhoneVerificationComponent {
  public phoneNumber: string = "+491752914416";
  public smsChallengeCode: string;
  public numberSent: boolean = false;
  public result: boolean = false;

  constructor(private _phoneRegistrationService: PhoneRegistrationService) {
  }

  public startPhoneNumberVerification() {
    this._phoneRegistrationService.registerPhoneNumber(this.phoneNumber)
      .subscribe(result => {
        this.numberSent = true
      });
  }

  public sendCode() {
    this._phoneRegistrationService.sendChallengeCode(this.smsChallengeCode)
      .subscribe(result => {
        this.result = true;
      });
  }
}
