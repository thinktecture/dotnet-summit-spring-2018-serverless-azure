import {ErrorHandler, Injectable, Injector} from "@angular/core";
import {ToastrService} from "ngx-toastr";

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) { }

  public handleError(error) {
    const toastService = this.injector.get(ToastrService);
    const message = error.message ? error.message : error.toString();

    console.error("FROM ERROR HANDLER: " + message);

    toastService.error("Something went wrong.", 'Ooops!');
  }
}
