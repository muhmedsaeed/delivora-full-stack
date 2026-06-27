import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FoodService } from '../../../core/services/food';
import { CategoryService } from '../../../core/services/category';
import { Food } from '../../../core/models/food/food';
import { Category } from '../../../core/models/category/category';
import { getApiErrorMessage } from '../../../core/utils/api-error';

@Component({
  selector: 'app-admin-foods',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './admin-foods.html'
})
export class AdminFoodsComponent implements OnInit {
  private foodService = inject(FoodService);
  private categoryService = inject(CategoryService);
  private fb = inject(FormBuilder);

  foods = signal<Food[]>([]);
  categories = signal<Category[]>([]);
  editingFood = signal<Food | null>(null);
  error = '';
  showForm = false;
  selectedImage?: File;
  imagePreview?: string;
  removeImage = false;

  form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    description: ['', Validators.required],
    price: [0, [Validators.required, Validators.min(1)]],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    isAvailable: [true]
  });

  ngOnInit(): void {
    this.load();
    this.categoryService.getAll().subscribe(c => this.categories.set(c));
  }

  load(): void {
    this.foodService.getAll().subscribe(f => this.foods.set(f));
  }

  save(): void {
    if (this.form.invalid) return;
    const v = this.form.getRawValue();
    const formData = new FormData();
    formData.append('name', v.name);
    formData.append('description', v.description);
    formData.append('price', String(v.price));
    formData.append('categoryId', String(v.categoryId));
    formData.append('isAvailable', String(v.isAvailable));
    if (this.selectedImage) {
      formData.append('image', this.selectedImage);
    }

    const editing = this.editingFood();
    if (editing) {
      formData.append('removeImage', String(this.removeImage));
    }

    const request = editing
      ? this.foodService.update(editing.id, formData)
      : this.foodService.create(formData);

    request.subscribe({
      next: () => {
        this.closeForm();
        this.load();
      },
      error: (err) => this.error = getApiErrorMessage(err, editing ? 'Could not update food.' : 'Could not create food.')
    });
  }

  startCreate(): void {
    this.error = '';
    this.showForm = true;
    this.editingFood.set(null);
    this.selectedImage = undefined;
    this.imagePreview = undefined;
    this.removeImage = false;
    this.form.reset({ name: '', description: '', price: 0, categoryId: 0, isAvailable: true });
  }

  startEdit(food: Food): void {
    this.error = '';
    this.showForm = true;
    this.editingFood.set(food);
    this.selectedImage = undefined;
    this.imagePreview = food.imageUrl;
    this.removeImage = false;
    this.form.reset({
      name: food.name,
      description: food.description,
      price: food.price,
      categoryId: food.categoryId,
      isAvailable: food.isAvailable
    });
  }

  closeForm(): void {
    this.showForm = false;
    this.editingFood.set(null);
    this.selectedImage = undefined;
    this.imagePreview = undefined;
    this.removeImage = false;
    this.form.reset({ name: '', description: '', price: 0, categoryId: 0, isAvailable: true });
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

  delete(id: number): void {
    if (!confirm('Delete this food item?')) return;
    this.foodService.delete(id).subscribe({
      next: () => this.load(),
      error: (err) => this.error = getApiErrorMessage(err, 'Could not delete food.')
    });
  }
}
