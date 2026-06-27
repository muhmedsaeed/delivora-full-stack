import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CartService } from '../../core/services/cart';
import { AddressService } from '../../core/services/address';
import { PaymentMethodService } from '../../core/services/payment-method';
import { OrderService } from '../../core/services/order';
import { Address } from '../../core/models/address/address';
import { PaymentMethod } from '../../core/models/payment-method/payment-method';
import { getApiErrorMessage } from '../../core/utils/api-error';


@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './checkout.html'
})
export class CheckoutComponent implements OnInit {
  cart = inject(CartService);
  private addressService = inject(AddressService);
  private paymentMethodService = inject(PaymentMethodService);
  private orderService = inject(OrderService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  addresses = signal<Address[]>([]);
  paymentMethods = signal<PaymentMethod[]>([]);
  error = '';
  loading = false;
  form = this.fb.nonNullable.group({
    addressId: [0, [Validators.required, Validators.min(1)]],
    paymentMethodId: [0, [Validators.required, Validators.min(1)]],
    notes: ['']
  });
  ngOnInit(): void {
    this.addressService.getAll().subscribe(a => this.addresses.set(a));
    this.paymentMethodService.getAll().subscribe(m => this.paymentMethods.set(m));
  }
  placeOrder(): void {
    if (this.form.invalid || this.cart.cartItems().length === 0) return;
    this.loading = true;
    const v = this.form.getRawValue();
    this.orderService.create({
      addressId: v.addressId,
      paymentMethodId: v.paymentMethodId,
      notes: v.notes,
      items: this.cart.cartItems().map(i => ({
        foodId: i.foodId, quantity: i.quantity
      }))
    }).subscribe({
      next: (order) => {
        this.cart.clear();
        this.router.navigate(['/orders', order.id]);
      },
      error: (err) => {
        this.error = getApiErrorMessage(err, 'Order creation failed.');
        this.loading = false;
      }
    });
  }
}