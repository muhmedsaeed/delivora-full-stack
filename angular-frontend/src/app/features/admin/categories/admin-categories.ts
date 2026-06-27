import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryService } from '../../../core/services/category';
import { Category } from '../../../core/models/category/category';
import { getApiErrorMessage } from '../../../core/utils/api-error';

@Component({
  selector: 'app-admin-categories',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './admin-categories.html'
})
export class AdminCategoriesComponent implements OnInit {
  private categoryService = inject(CategoryService);
  private fb = inject(FormBuilder);

  categories = signal<Category[]>([]);
  error = '';
  showForm = false;

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    description: ['']
  });

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.categoryService.getAll().subscribe(c => this.categories.set(c));
  }

  save(): void {
    if (this.form.invalid) return;
    const formData = new FormData();
    formData.append('name', this.form.value.name!);
    formData.append('description', this.form.value.description || '');

    this.categoryService.create(formData).subscribe({
      next: () => {
        this.form.reset();
        this.showForm = false;
        this.load();
      },
      error: (err) => this.error = getApiErrorMessage(err, 'Could not create category.')
    });
  }

  // edit(id: number): void {
  //   this.categoryService.update(id, ).subscribe({
  //     next: () => this.load(),
  //     error: (err) => this.error = getApiErrorMessage(err, 'Could not update category.')
  //   });
  // }

  delete(id: number): void {
    if (!confirm('Delete this category?')) return;
    this.categoryService.delete(id).subscribe({
      next: () => this.load(),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not delete category.')
    });
  }
}
