import { Component, OnInit, inject, signal } from '@angular/core';
import { AdminUsersService } from '../../../core/services/admin-users';
import { AdminUser } from '../../../core/models/profile/profile';
import { getApiErrorMessage } from '../../../core/utils/api-error';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  templateUrl: './admin-users.html'
})
export class AdminUsersComponent implements OnInit {
  private adminUsersService = inject(AdminUsersService);

  users = signal<AdminUser[]>([]);
  selectedUser = signal<AdminUser | null>(null);
  roleFilter = '';
  error = '';

  ngOnInit(): void {
    this.load();
  }

  load(role = this.roleFilter): void {
    this.roleFilter = role;
    this.adminUsersService.getAll(role || undefined).subscribe({
      next: (users) => this.users.set(users),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not load users.')
    });
  }

  showDetails(user: AdminUser): void {
    this.adminUsersService.getById(user.id).subscribe({
      next: (details) => this.selectedUser.set(details),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not load user details.')
    });
  }

  lock(user: AdminUser): void {
    if (!confirm(`Lock ${user.fullName}?`)) return;
    this.adminUsersService.lock(user.id).subscribe({
      next: () => this.load(),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not lock user.')
    });
  }

  unlock(user: AdminUser): void {
    this.adminUsersService.unlock(user.id).subscribe({
      next: () => this.load(),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not unlock user.')
    });
  }
}
