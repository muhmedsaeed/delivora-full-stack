
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { PaymentMethod } from '../models/payment-method/payment-method';

@Injectable({ providedIn: 'root' })
export class PaymentMethodService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/PaymentMethod`;

    getAll() { return this.http.get<PaymentMethod[]>(this.url); }
    create(dto: Partial<PaymentMethod>) {
        return this.http.post<PaymentMethod>(this.url, dto);
    }
}