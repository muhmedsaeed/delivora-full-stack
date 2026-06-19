
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { Driver } from '../models/driver/driver';


@Injectable({ providedIn: 'root' })
export class DriverService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/Driver`;

    getAll() { return this.http.get<Driver[]>(this.url); }
    getAvailable() { return this.http.get<Driver[]>(`${this.url}/available`); }
    setAvailability(isAvailable: boolean) {
        return this.http.put(`${this.url}/availability`, isAvailable);
    }
}

