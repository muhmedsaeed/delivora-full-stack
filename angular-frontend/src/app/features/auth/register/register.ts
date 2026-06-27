import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { getApiErrorMessage } from '../../../core/utils/api-error';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  error = '';
  loading = false;
  showPassword = false;
  showConfirmPassword = false;

  form = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d).+$/)]],
    passwordConfirmed: ['', Validators.required],
    phoneNumber: ['', Validators.required],
    birthDate: ['', Validators.required],
    street: [''],
    city: [''],
    country: ['Egypt']
  });
  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    const v = this.form.getRawValue();
    if (v.password !== v.passwordConfirmed) {
      this.error = 'Two passwords do not match';
      return;
    }
    this.loading = true;
    this.auth.register(v).subscribe({
      next: () => this.router.navigate(['/menu']),
      error: (err) => {
        this.error = getApiErrorMessage(err, 'Registration failed. Please check your details.');
        this.loading = false;
      }
    });
  }
}
