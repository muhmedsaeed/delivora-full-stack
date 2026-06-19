import { Injectable, signal, computed } from '@angular/core';
import { CartItem } from '../models/cart/cart-item';


const CART_KEY = 'delivora_cart';
@Injectable({ providedIn: 'root' })
export class CartService {
    private items = signal<CartItem[]>(this.loadCart());
    readonly cartItems = this.items.asReadonly();
    readonly totalItems = computed(() =>
        this.items().reduce((sum, i) => sum + i.quantity, 0)
    );
    readonly subTotal = computed(() =>
        this.items().reduce((sum, i) => sum + i.price * i.quantity, 0)
    );
    readonly deliveryFee = 25;
    readonly taxRate = 0.14;
    readonly tax = computed(() =>
        Math.round(this.subTotal() * this.taxRate * 100) / 100
    );
    readonly total = computed(() =>
        this.subTotal() + this.deliveryFee + this.tax()
    );
    addItem(item: CartItem): void {
        const current = [...this.items()];
        const existing = current.find(i => i.foodId === item.foodId);
        if (existing) {
            existing.quantity += item.quantity;
        } else {
            current.push({ ...item });
        }
        this.items.set(current);
        this.saveCart();
    }
    updateQuantity(foodId: number, quantity: number): void {
        if (quantity <= 0) { this.removeItem(foodId); return; }
        const current = this.items().map(i =>
            i.foodId === foodId ? { ...i, quantity } : i
        );
        this.items.set(current);
        this.saveCart();
    }
    removeItem(foodId: number): void {
        this.items.set(this.items().filter(i => i.foodId !== foodId));
        this.saveCart();
    }
    clear(): void {
        this.items.set([]);
        localStorage.removeItem(CART_KEY);
    }
    private saveCart(): void {
        localStorage.setItem(CART_KEY, JSON.stringify(this.items()));
    }
    private loadCart(): CartItem[] {
        const raw = localStorage.getItem(CART_KEY);
        if (!raw) return [];
        try { return JSON.parse(raw); } catch { return []; }
    }
}
