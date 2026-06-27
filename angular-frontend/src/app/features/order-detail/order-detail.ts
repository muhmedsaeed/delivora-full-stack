import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { OrderService } from '../../core/services/order';
import { ReviewService } from '../../core/services/review';
import { AuthService } from '../../core/services/auth';
import { Order } from '../../core/models/order/order';
import { OrderReview } from '../../core/models/order-review/order-review';
import { getApiErrorMessage } from '../../core/utils/api-error';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, DatePipe, CurrencyPipe],
  templateUrl: './order-detail.html'
})
export class OrderDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private orderService = inject(OrderService);
  private reviewService = inject(ReviewService);
  auth = inject(AuthService);
  private fb = inject(FormBuilder);

  order = signal<Order | null>(null);
  reviews = signal<OrderReview[]>([]);
  error = '';
  loading = signal<boolean>(true);

  reviewForm = this.fb.nonNullable.group({
    packagingRate: [5, [Validators.required, Validators.min(1), Validators.max(5)]],
    deliveryRate: [5, [Validators.required, Validators.min(1), Validators.max(5)]],
    comment: ['', Validators.maxLength(1000)]
  });

  get packagingRate(): number {
    return this.reviewForm.get('packagingRate')?.value ?? 5;
  }

  get deliveryRate(): number {
    return this.reviewForm.get('deliveryRate')?.value ?? 5;
  }


  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.orderService.getById(id).subscribe({
      next: (order) => {
        this.order.set(order);
        this.loading.set(false);
      },
      error: (err) => {
        this.error = getApiErrorMessage(err, 'Could not load order.');
        this.loading.set(false);
      }
    });
    this.reviewService.getOrderReviews(id).subscribe(r => this.reviews.set(r));
  }

  cancelOrder(): void {
    const current = this.order();
    if (!current || !confirm('Cancel this order?')) return;

    this.orderService.cancel(current.id).subscribe({
      next: () => this.order.update(o => o ? { ...o, status: 'Cancelled' } : o),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not cancel order.')
    });
  }

  submitReview(): void {
    const current = this.order();
    if (!current || this.reviewForm.invalid) return;

    this.reviewService.createOrderReview({
      orderId: current.id,
      ...this.reviewForm.getRawValue()
    }).subscribe({
      next: (review) => this.reviews.update(list => [review, ...list]),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not submit review.')
    });
  }

  canCancel(status: string): boolean {
    return status === 'Pending' && this.auth.isCustomer();
  }

  canReview(status: string): boolean {
    return status === 'Delivered' && this.auth.isCustomer();
  }
}
