import { Component, inject, OnInit, signal } from '@angular/core';
import { DriverService } from '../../../core/services/driver';
import { Driver } from '../../../core/models/driver/driver';
import { getApiErrorMessage } from '../../../core/utils/api-error';

@Component({
  selector: 'app-admin-drivers',
  standalone: true,
  templateUrl: './admin-drivers.html'
})
export class AdminDriversComponent implements OnInit {
  private driverService = inject(DriverService);

  drivers = signal<Driver[]>([]);
  error = '';

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.driverService.getAll().subscribe({
      next: (drivers) => this.drivers.set(drivers),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not load drivers.')
    });
  }

  approve(driverId: number): void {
    this.driverService.approve(driverId).subscribe({
      next: () => this.load(),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not approve driver.')
    });
  }
}
