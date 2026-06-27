import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { OrderService } from '../../core/services/order';
import { DriverService } from '../../core/services/driver';
import { Order, OrderStatus } from '../../core/models/order/order';
import { getApiErrorMessage } from '../../core/utils/api-error';

@Component({
  selector: 'app-driver-dashboard',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './driver-dashboard.html',
  styleUrl: './driver-dashboard.css'
})
export class DriverDashboardComponent implements OnInit {
  private orderService = inject(OrderService);
  private driverService = inject(DriverService);

  orders = signal<Order[]>([]);
  isAvailable = signal(true);
  error = '';

  readonly statuses = ['Confirmed', 'Preparing', 'PickedUp', 'Delivered'];

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderService.getAll().subscribe(o => this.orders.set(o));
  }

  toggleAvailability(): void {
    const next = !this.isAvailable();
    this.driverService.setAvailability(next).subscribe({
      next: () => this.isAvailable.set(next),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not update availability.')
    });
  }

  updateStatus(orderId: number, status: string): void {
    const statusValue = OrderStatus[status as keyof typeof OrderStatus];
    this.orderService.updateStatus(orderId, statusValue as OrderStatus).subscribe({
      next: () => this.loadOrders(),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not update order status.')
    });
  }
}
