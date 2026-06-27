import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProfileService } from '../../core/services/profile';
import { AuthService } from '../../core/services/auth';
import { Profile } from '../../core/models/profile/profile';
import { VehicleType } from '../../core/models/auth/register-driver';
import { getApiErrorMessage } from '../../core/utils/api-error';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './profile.html'
})
export class ProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private profileService = inject(ProfileService);
  private auth = inject(AuthService);
  private router = inject(Router);

  profile = signal<Profile | null>(null);
  error = '';
  success = '';
  loading = false;
  selectedImage?: File;
  imagePreview?: string;
  removeImage = false;

  readonly vehicleTypes = Object.entries(VehicleType)
    .filter(([key]) => isNaN(Number(key)))
    .map(([label, value]) => ({ label, value: value as number }));

  form = this.fb.nonNullable.group({
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: [''],
    birthDate: [''],
    vehicleType: [VehicleType.Motorcycle],
    licenseNumber: ['']
  });

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.profileService.get().subscribe({
      next: (profile) => {
        this.profile.set(profile);
        this.imagePreview = profile.profileImageUrl;
        this.removeImage = false;
        this.form.reset({
          fullName: profile.fullName,
          email: profile.email,
          phoneNumber: profile.phoneNumber ?? '',
          birthDate: this.toDateInput(profile.birthDate),
          vehicleType: this.toVehicleTypeValue(profile.vehicleType),
          licenseNumber: profile.licenseNumber ?? ''
        });
      },
      error: (err) => this.error = getApiErrorMessage(err, 'Could not load profile.')
    });
  }

  save(): void {
    if (this.form.invalid) return;
    const current = this.profile();
    if (!current) return;

    const v = this.form.getRawValue();
    const formData = new FormData();
    formData.append('fullName', v.fullName);
    formData.append('email', v.email);
    formData.append('phoneNumber', v.phoneNumber ?? '');
    if (v.birthDate) formData.append('birthDate', v.birthDate);
    if (current.roles.includes('Driver')) {
      formData.append('vehicleType', String(v.vehicleType));
      formData.append('licenseNumber', v.licenseNumber ?? '');
    }
    if (this.selectedImage) formData.append('profileImage', this.selectedImage);
    formData.append('removeImage', String(this.removeImage));

    this.loading = true;
    this.error = '';
    this.success = '';

    this.profileService.update(formData).subscribe({
      next: (profile) => {
        this.profile.set(profile);
        this.selectedImage = undefined;
        this.imagePreview = profile.profileImageUrl;
        this.removeImage = false;
        this.auth.updateStoredUser({
          fullName: profile.fullName,
          email: profile.email,
          status: profile.status,
          profileImageUrl: profile.profileImageUrl,
          roles: profile.roles
        });
        this.success = 'Profile updated successfully.';
        this.loading = false;
      },
      error: (err) => {
        this.error = getApiErrorMessage(err, 'Could not update profile.');
        this.loading = false;
      }
    });
  }

  onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    this.selectedImage = file;
    this.removeImage = false;
    this.imagePreview = URL.createObjectURL(file);
  }

  markImageForRemoval(): void {
    this.selectedImage = undefined;
    this.imagePreview = undefined;
    this.removeImage = true;
  }

  deleteImage(): void {
    this.profileService.deleteImage().subscribe({
      next: () => {
        this.selectedImage = undefined;
        this.imagePreview = undefined;
        this.removeImage = false;
        this.load();
      },
      error: (err) => this.error = getApiErrorMessage(err, 'Could not delete profile image.')
    });
  }

  deactivate(): void {
    if (!confirm('Deactivate your account?')) return;

    this.profileService.deactivate().subscribe({
      next: () => {
        this.auth.logout();
        this.router.navigate(['/auth/login']);
      },
      error: (err) => this.error = getApiErrorMessage(err, 'Could not deactivate account.')
    });
  }

  private toDateInput(value: string): string {
    if (!value) return '';
    return value.slice(0, 10);
  }

  private toVehicleTypeValue(value?: string): VehicleType {
    if (!value) return VehicleType.Motorcycle;
    return VehicleType[value as keyof typeof VehicleType] ?? VehicleType.Motorcycle;
  }
}
