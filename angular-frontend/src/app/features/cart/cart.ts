import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartService } from '../../core/services/cart';

@Component({
selector: 'app-cart',
standalone: true,
imports: [RouterLink],
templateUrl: './cart.html'
})
export class CartComponent {
cart = inject(CartService);
}