import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Food } from '../../../core/models/food/food';
import { CartService } from '../../../core/services/cart';
import { AuthService } from '../../../core/services/auth';



@Component({
  selector: 'app-food-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './food-card.html',
  styleUrl: './food-card.css'
})
export class FoodCardComponent {
  @Input({ required: true }) food!: Food;
  cart = inject(CartService);
  auth = inject(AuthService);

  addToCart(): void {
    this.cart.addItem({
      foodId: this.food.id,
      name: this.food.name,
      price: this.food.price,
      quantity: 1,
      imageUrl: this.food.imageUrl
    });
  }
}
