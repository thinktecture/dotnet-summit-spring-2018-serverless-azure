import {Component} from '@angular/core';
import {WindowRef} from "../../services/windowRef";
import {WebPushService} from "../../services/webPushService";

@Component({
  selector: 'app-menu',
  templateUrl: 'menu.html',
  styleUrls: ['menu.scss']
})
export class MenuComponent {
  private readonly _bodyCssClass = 'show-menu';

  public isClimateMenuOpen: boolean;
  public isPushMenuOpen: boolean;

  constructor(private _windowRef: WindowRef,
              private _webPush: WebPushService) {
  }

  public subscribeToPush() {
    this._webPush.subscribeToPush();
    this.closeMenu();
  }

  public unsubscribeFromPush() {
    this._webPush.unsubscribeFromPush();
    this.closeMenu();
  }

  public toggleClimateMenu() {
    if (this.isPushMenuOpen) {
      this.closeMenu();
    }

    this.isClimateMenuOpen = !this.isClimateMenuOpen;
    this._windowRef.nativeWindow.document.body.classList.toggle(this._bodyCssClass);
  }

  public togglePushMenu() {
    if (this.isClimateMenuOpen) {
      this.closeMenu();
    }

    this.isPushMenuOpen = !this.isPushMenuOpen;
    this._windowRef.nativeWindow.document.body.classList.toggle(this._bodyCssClass);
  }

  public closeMenu() {
    this.isClimateMenuOpen = false;
    this.isPushMenuOpen = false;
    this._windowRef.nativeWindow.document.body.classList.remove(this._bodyCssClass);
  }
}
