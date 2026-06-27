import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { OrderService } from '../../core/services/order';
import { Order } from '../../core/models/order/order';
import { DatePipe, CurrencyPipe } from '@angular/common';


@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [RouterLink, DatePipe, CurrencyPipe],
  templateUrl: './orders.html'
})
export class OrdersComponent implements OnInit {
  private orderService = inject(OrderService);
  orders = signal<Order[]>([]);
  ngOnInit(): void {
    this.orderService.getAll().subscribe(o => this.orders.set(o));
  }
}
