import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { OrderService } from '../../../core/services/order';
import { DriverService } from '../../../core/services/driver';
import { Order, OrderStatus } from '../../../core/models/order/order';
import { Driver } from '../../../core/models/driver/driver';
import { getApiErrorMessage } from '../../../core/utils/api-error';

@Component({
  selector: 'app-admin-orders',
  standalone: true,
  imports: [FormsModule, RouterLink, DatePipe],
  templateUrl: './admin-orders.html',
  styleUrl: './admin-orders.css'
})
export class AdminOrdersComponent implements OnInit {
  private orderService = inject(OrderService);
  private driverService = inject(DriverService);

  orders = signal<Order[]>([]);
  drivers = signal<Driver[]>([]);
  error = '';
  selectedDriver: Record<number, number> = {};

  readonly statuses = Object.entries(OrderStatus)
    .filter(([key]) => isNaN(Number(key)))
    .map(([label, value]) => ({ label, value: value as number }));

  ngOnInit(): void {
    this.load();
    this.driverService.getAvailable().subscribe(d => this.drivers.set(d));
  }

  load(): void {
    this.orderService.getAll().subscribe({
      next: (orders) => {
        this.orders.set(orders);
        this.selectedDriver = {};
        for (const order of orders) {
          this.selectedDriver[order.id] = order.driverId ?? 0;
        }
      },
      error: (err) => this.error = getApiErrorMessage(err, 'Could not load orders.')
    });
  }

  updateStatus(orderId: number, status: string): void {
    const statusValue = OrderStatus[status as keyof typeof OrderStatus];
    this.orderService.updateStatus(orderId, statusValue as OrderStatus).subscribe({
      next: (order) => this.replaceOrder(order),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not update status.')
    });
  }

  assignDriver(orderId: number): void {
    const driverId = this.selectedDriver[orderId];
    if (!driverId) return;

    this.orderService.assignDriver(orderId, driverId).subscribe({
      next: (order) => this.replaceOrder(order),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not assign driver.')
    });
  }

  private replaceOrder(updated: Order): void {
    this.orders.update(orders => orders.map(order => order.id === updated.id ? updated : order));
    this.selectedDriver[updated.id] = updated.driverId ?? 0;
  }
}
