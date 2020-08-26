import { Injectable } from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';
import {baseURL} from '../shared/baseURL';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorsInterceptorService implements HttpInterceptor {

  constructor() { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          let errorMsg = '';

          if (error.error instanceof ErrorEvent){
            errorMsg = `Error: ${error.error.message}`;
          } else {
            if (`${baseURL}/register` && error.status === 500){
              errorMsg = `Too weak password`;
            } else {
              errorMsg = `Error code: ${error.status} Message: ${error.message}`;
            }
          }

          console.log(errorMsg);
          return throwError(errorMsg);
        })
      );
  }
}
