
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';


export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const router = inject(Router);
    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401) {
                localStorage.removeItem('delivora_token');
                localStorage.removeItem('delivora_user');
                router.navigate(['/auth/login']);
            }
            if (error.status === 403) {
                console.error('Access denied');
            }
            if (error.status === 423) {
                alert('The account has been temporarily locked due to incorrect attempts.');
            }
            return throwError(() => error);
        })
    );
};