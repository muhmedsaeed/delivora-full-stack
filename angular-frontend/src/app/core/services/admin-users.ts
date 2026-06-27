import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { AdminUser } from '../models/profile/profile';

@Injectable({ providedIn: 'root' })
export class AdminUsersService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/admin/users`;

    getAll(role?: string) {
        const options = role ? { params: { role } } : undefined;
        return this.http.get<AdminUser[]>(this.url, options);
    }

    getById(id: number) {
        return this.http.get<AdminUser>(`${this.url}/${id}`);
    }

    lock(id: number) {
        return this.http.put(`${this.url}/${id}/lock`, {});
    }

    unlock(id: number) {
        return this.http.put(`${this.url}/${id}/unlock`, {});
    }
}
