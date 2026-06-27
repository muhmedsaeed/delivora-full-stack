
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';


export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const router = inject(Router);
    const isAuthRequest = /\/Auth\/(login|register)/i.test(req.url);

    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401 && !isAuthRequest) {
                localStorage.removeItem('delivora_token');
                localStorage.removeItem('delivora_user');
                router.navigate(['/auth/login']);
            }
            if (error.status === 403) {
                console.error('Access denied');
            }
            if (error.status === 423) {
                const message = error.error?.message || 'Your account is locked. You can view pages, but actions are disabled.';
                alert(message);
            }
            return throwError(() => error);
        })
    );
};
