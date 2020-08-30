import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  //Intercepts HttpRequest and handles them by adding the api url.
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const apiUrl = "http://localhost:59858/api/admin";
    const apiReq = req.clone({ url: `${apiUrl}${req.url}` });
    return next.handle(apiReq);
  }
}
