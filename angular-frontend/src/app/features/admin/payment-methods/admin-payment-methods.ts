import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PaymentMethodService } from '../../../core/services/payment-method';
import { PaymentMethod } from '../../../core/models/payment-method/payment-method';
import { getApiErrorMessage } from '../../../core/utils/api-error';

@Component({
  selector: 'app-admin-payment-methods',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './admin-payment-methods.html'
})
export class AdminPaymentMethodsComponent implements OnInit {
  private paymentMethodService = inject(PaymentMethodService);
  private fb = inject(FormBuilder);

  methods = signal<PaymentMethod[]>([]);
  error = '';
  showForm = false;

  form = this.fb.nonNullable.group({
    name: ['CashOnDelivery', Validators.required],
    description: ['', Validators.required],
    isActive: [true]
  });

  readonly methodNames = [
    'COD', 'Card', 'Wallet', 'Stripe', 'PayPal',
    'ApplePay', 'GooglePay', 'BankTransfer', 'CashOnDelivery'
  ];

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.paymentMethodService.getAll().subscribe(m => this.methods.set(m));
  }

  save(): void {
    if (this.form.invalid) return;
    this.paymentMethodService.create(this.form.getRawValue()).subscribe({
      next: () => {
        this.form.reset({ name: 'CashOnDelivery', isActive: true });
        this.showForm = false;
        this.load();
      },
      error: (err) => this.error = getApiErrorMessage(err, 'Could not create payment method.')
    });
  }
}
