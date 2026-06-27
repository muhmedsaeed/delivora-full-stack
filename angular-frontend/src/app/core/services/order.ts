
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { CreateOrderRequest, Order, OrderStatus } from '../models/order/order';


@Injectable({ providedIn: 'root' })
export class OrderService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/Order`;

    getAll() { return this.http.get<Order[]>(this.url); }
    getById(id: number) { return this.http.get<Order>(`${this.url}/${id}`); }
    create(dto: CreateOrderRequest) {
        return this.http.post<Order>(this.url, dto);
    }
    updateStatus(id: number, status: OrderStatus) {
        return this.http.put<Order>(`${this.url}/${id}/status`, { status });
    }
    assignDriver(id: number, driverId: number) {
        return this.http.put<Order>(`${this.url}/${id}/assign-driver`, { driverId });
    }
    cancel(id: number) {
        return this.http.delete(`${this.url}/${id}`);
    }
}
