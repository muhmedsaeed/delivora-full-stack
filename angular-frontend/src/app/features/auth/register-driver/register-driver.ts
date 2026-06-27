import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { VehicleType } from '../../../core/models/auth/register-driver';
import { getApiErrorMessage } from '../../../core/utils/api-error';

@Component({
  selector: 'app-register-driver',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register-driver.html'
})
export class RegisterDriverComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);

  error = '';
  success = '';
  loading = false;
  showPassword = false;
  showConfirmPassword = false;

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  readonly vehicleTypes = Object.entries(VehicleType)
    .filter(([key]) => isNaN(Number(key)))
    .map(([label, value]) => ({ label, value: value as number }));

  form = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: [''],
    password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d).+$/)]],
    passwordConfirmed: ['', Validators.required],
    vehicleType: [VehicleType.Motorcycle, Validators.required],
    licenseNumber: ['', Validators.required]
  });

  onSubmit(): void {
    if (this.form.invalid) return;

    const { passwordConfirmed, ...payload } = this.form.getRawValue();
    if (payload.password !== passwordConfirmed) {
      this.error = 'Two passwords do not match';
      return;
    }

    this.loading = true;
    this.error = '';
    this.success = '';

    this.auth.registerDriver(payload).subscribe({
      next: (res) => {
        this.success = res.message;
        this.form.reset({
          username: '',
          fullName: '',
          email: '',
          phoneNumber: '',
          password: '',
          passwordConfirmed: '',
          vehicleType: VehicleType.Motorcycle,
          licenseNumber: ''
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = getApiErrorMessage(err, 'Driver registration failed. Please check your details.');
        this.loading = false;
      }
    });
  }
}
