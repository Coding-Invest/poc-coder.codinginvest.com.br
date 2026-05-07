import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private http: HttpClient) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('accessToken');
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          const refreshToken = localStorage.getItem('refreshToken');
          if (refreshToken) {
            return this.http.post<any>('/api/auth/refresh', { refreshToken }).pipe(
              switchMap((response) => {
                localStorage.setItem('accessToken', response.accessToken);
                localStorage.setItem('expiry', response.expiry);
                const newReq = req.clone({
                  setHeaders: {
                    Authorization: `Bearer ${response.accessToken}`
                  }
                });
                return next.handle(newReq);
              }),
              catchError(() => {
                localStorage.clear();
                window.location.href = '/login';
                return throwError(error);
              })
            );
          } else {
            localStorage.clear();
            window.location.href = '/login';
          }
        }
        return throwError(error);
      })
    );
  }
}