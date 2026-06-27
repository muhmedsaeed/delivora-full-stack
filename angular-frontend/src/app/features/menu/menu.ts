import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FoodCardComponent } from '../../shared/components/food-card/food-card';
import { FoodService } from '../../core/services/food';
import { CategoryService } from '../../core/services/category';
import { Food } from '../../core/models/food/food';
import { Category } from '../../core/models/category/category';


@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [FoodCardComponent],
  templateUrl: './menu.html'
})
export class MenuComponent implements OnInit {
  private foodService = inject(FoodService);
  private categoryService = inject(CategoryService);
  private route = inject(ActivatedRoute);
  foods = signal<Food[]>([]);
  categories = signal<Category[]>([]);
  selectedCategory = signal<string | null>(null);

  ngOnInit(): void {
    this.categoryService.getAll().subscribe(c => this.categories.set(c));
    this.route.queryParams.subscribe(params => {
      const cat = params['category'] as string | undefined;
      this.selectedCategory.set(cat ?? null);
      this.loadFoods(cat);
    });
  }
  selectCategory(name: string | null): void {
    this.selectedCategory.set(name);
    this.loadFoods(name ?? undefined);
  }
  private loadFoods(categoryName?: string): void {
    if (categoryName) {
      this.foodService.getByCategoryName(categoryName)
        .subscribe(f => this.foods.set(f));
    } else {
      this.foodService.getAll().subscribe(f => this.foods.set(f));
    }
  }
}

