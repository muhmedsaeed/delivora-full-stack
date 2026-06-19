
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { CreateFoodReviewRequest, FoodReview } from '../models/food-review/food-review';
import { CreateOrderReviewRequest, OrderReview } from '../models/order-review/order-review';


@Injectable({ providedIn: 'root' })
export class ReviewService {
    private http = inject(HttpClient);
    getFoodReviews(foodId: number) {
        return this.http.get<FoodReview[]>(
            `${environment.apiUrl}/FoodReview/food/${foodId}`
        );
    }
    createFoodReview(dto: CreateFoodReviewRequest) {
        return this.http.post<FoodReview>(
            `${environment.apiUrl}/FoodReview`, dto
        );
    }
    getOrderReviews(orderId: number) {
        return this.http.get<OrderReview[]>(
            `${environment.apiUrl}/OrderReview/order/${orderId}`
        );
    }
    createOrderReview(dto: CreateOrderReviewRequest) {
        return this.http.post<OrderReview>(
            `${environment.apiUrl}/OrderReview`, dto
        );
    }
}
