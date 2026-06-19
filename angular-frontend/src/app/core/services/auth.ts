import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { environment } from '../../../enviroments/enviroment';
import { AuthResponse } from '../models/auth/auth-response';
import { LoginRequest } from '../models/auth/login';
import { RegisterRequest } from '../models/auth/register';
import { RegisterDriverRequest } from '../models/auth/register-driver';

const TOKEN_KEY = 'delivora_token';
const USER_KEY = 'delivora_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private http = inject(HttpClient);
    private router = inject(Router);
    private apiUrl = `${environment.apiUrl}/Auth`;
    private currentUser = signal<AuthResponse | null>(this.loadUser());
    readonly user = this.currentUser.asReadonly();
    readonly isLoggedIn = computed(() => !!this.currentUser());
    readonly roles = computed(() => this.currentUser()?.roles ?? []);
    readonly isAdmin = computed(() => this.roles().includes('Admin'));
    readonly isCustomer = computed(() => this.roles().includes('Customer'));
    readonly isDriver = computed(() => this.roles().includes('Driver'));
    login(dto: LoginRequest) {
        return this.http.post<AuthResponse>(`${this.apiUrl}/login`, dto).pipe(
            tap(res => this.setSession(res))
        );
    }
    register(dto: RegisterRequest) {
        return this.http.post<AuthResponse>(`${this.apiUrl}/register`, dto).pipe(
            tap(res => this.setSession(res))
        );
    }
    registerDriver(dto: RegisterDriverRequest) {
        return this.http.post<AuthResponse>(`${this.apiUrl}/register-driver`, dto).pipe(
            tap(res => this.setSession(res))
        );
    }
    logout(): void {
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(USER_KEY);
        this.currentUser.set(null);
        this.router.navigate(['/auth/login']);
    }
    getToken(): string | null {
        return localStorage.getItem(TOKEN_KEY);
    }
    hasRole(role: string): boolean {
        return this.roles().includes(role);
    }
    private setSession(res: AuthResponse): void {
        localStorage.setItem(TOKEN_KEY, res.token);
        localStorage.setItem(USER_KEY, JSON.stringify(res));
        this.currentUser.set(res);
    }
    private loadUser(): AuthResponse | null {
        const raw = localStorage.getItem(USER_KEY);
        if (!raw) return null;
        try { return JSON.parse(raw); } catch { return null; }
    }
}
