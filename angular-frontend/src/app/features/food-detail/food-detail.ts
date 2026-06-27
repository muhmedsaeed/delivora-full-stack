import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FoodService } from '../../core/services/food';
import { ReviewService } from '../../core/services/review';
import { CartService } from '../../core/services/cart';
import { AuthService } from '../../core/services/auth';
import { Food } from '../../core/models/food/food';
import { FoodReview } from '../../core/models/food-review/food-review';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-food-detail',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, CurrencyPipe],
  templateUrl: './food-detail.html'
})
export class FoodDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private foodService = inject(FoodService);
  private reviewService = inject(ReviewService);
  cart = inject(CartService);
  auth = inject(AuthService);
  private fb = inject(FormBuilder);
  food = signal<Food | null>(null);
  reviews = signal<FoodReview[]>([]);
  reviewForm = this.fb.nonNullable.group({
    rating: [5, [Validators.required, Validators.min(1), Validators.max(5)]],
    comment: ['', Validators.maxLength(1000)]
  });
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.foodService.getById(id).subscribe(f => this.food.set(f));
    this.reviewService.getFoodReviews(id).subscribe(r => this.reviews.set(r));
  }
  addToCart(): void {
    const f = this.food();
    if (!f) return;
    this.cart.addItem({ foodId: f.id, name: f.name, price: f.price, quantity: 1, imageUrl: f.imageUrl });
  }
  submitReview(): void {
    const f = this.food();
    if (!f || this.reviewForm.invalid) return;
    this.reviewService.createFoodReview({
      foodId: f.id, ...this.reviewForm.getRawValue()
    }).subscribe(r => this.reviews.update(list => [r, ...list]));
  }
}
