import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FoodCardComponent } from '../../shared/components/food-card/food-card';
import { CategoryService } from '../../core/services/category';
import { FoodService } from '../../core/services/food';
import { Category } from '../../core/models/category/category';
import { Food } from '../../core/models/food/food';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, FoodCardComponent],
  templateUrl: './home.html'
})

export class HomeComponent implements OnInit {
  private categoryService = inject(CategoryService);
  private foodService = inject(FoodService);
  categories = signal<Category[]>([]);
  featuredFoods = signal<Food[]>([]);
  ngOnInit(): void {
    this.categoryService.getAll().subscribe(c => this.categories.set(c));
    this.foodService.getAll().subscribe(f => this.featuredFoods.set(f.slice(0, 6)));
  }
}
