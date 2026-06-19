
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { Payment, PaymentStatus } from '../models/payment/payment';



@Injectable({ providedIn: 'root' })
export class PaymentService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/Payment`;

    getByOrderId(orderId: number) {
        return this.http.get<Payment>(`${this.url}/order/${orderId}`);
    }
    updateStatus(id: number, status: PaymentStatus) {
        return this.http.put(`${this.url}/${id}/status`, { status });
    }
}