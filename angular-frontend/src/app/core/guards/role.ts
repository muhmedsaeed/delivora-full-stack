import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';


export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
    return () => {
        const auth = inject(AuthService);
        const router = inject(Router);
        if (!auth.isLoggedIn()) {
            router.navigate(['/auth/login']);
            return false;
        }
        const hasRole = allowedRoles.some(r => auth.hasRole(r));
        if (!hasRole) {
            router.navigate(['/']);
            return false;
        }
        return true;
    };
};