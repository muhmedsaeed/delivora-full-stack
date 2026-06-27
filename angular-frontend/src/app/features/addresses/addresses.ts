import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AddressService } from '../../core/services/address';
import { Address } from '../../core/models/address/address';
import { getApiErrorMessage } from '../../core/utils/api-error';

@Component({
  selector: 'app-addresses',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './addresses.html'
})
export class AddressesComponent implements OnInit {
  private addressService = inject(AddressService);
  private fb = inject(FormBuilder);
  addresses = signal<Address[]>([]);
  showForm = false;
  editingId: number | null = null;
  error = '';

  form = this.fb.nonNullable.group({
    title: ['Home', Validators.required],
    street: ['', Validators.required],
    city: ['', Validators.required],
    country: ['Egypt']
  });

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.addressService.getAll().subscribe(a => this.addresses.set(a));
  }

  startCreate(): void {
    this.editingId = null;
    this.form.reset({ title: 'Home', country: 'Egypt' });
    this.showForm = true;
  }

  startEdit(address: Address): void {
    this.editingId = address.id;
    this.form.patchValue(address);
    this.showForm = true;
  }

  cancelForm(): void {
    this.form.reset({ title: 'Home', country: 'Egypt' });
    this.showForm = false;
    this.editingId = null;
    this.error = '';
  }

  save(): void {
    if (this.form.invalid) return;
    const payload = this.form.getRawValue();
    const request = this.editingId
      ? this.addressService.update(this.editingId, payload)
      : this.addressService.create(payload);

    request.subscribe({
      next: () => {
        this.form.reset({ title: 'Home', country: 'Egypt' });
        this.showForm = false;
        this.editingId = null;
        this.load();
      },
      error: (err) => this.error = getApiErrorMessage(err, 'Could not save address.')
    });
  }

  delete(id: number): void {
    if (confirm('Delete this address?')) {
      this.addressService.delete(id).subscribe({
        next: () => this.load(),
        error: (err) => this.error = getApiErrorMessage(err, 'Could not delete address.')
      });
    }
  }
}
