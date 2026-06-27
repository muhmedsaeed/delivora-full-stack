import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { getApiErrorMessage } from '../../../core/utils/api-error';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html'
})

export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  error = '';
  loading = false;
  showPassword = false;

  form = this.fb.nonNullable.group({
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d).+$/)]]
  });

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading = true;
    this.error = '';
    this.auth.login(this.form.getRawValue()).subscribe({
      next: () => {
        const roles = this.auth.roles();
        if (roles.includes('Admin')) this.router.navigate(['/admin']);
        else if (roles.includes('Driver')) this.router.navigate(['/driver']);
        else this.router.navigate(['/menu']);
      },
      error: (err) => {
        this.error = getApiErrorMessage(err, 'Login failed. Check your username and password.');
        this.loading = false;
      }
    });
  }
}
