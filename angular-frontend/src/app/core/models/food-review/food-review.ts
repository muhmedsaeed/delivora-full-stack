

export interface FoodReview {
    id: number;
    rating: number;
    comment: string;
    lastUpdate: string;
    customerName: string;
    foodName: string;
    foodId: number;
}


export interface CreateFoodReviewRequest {
    foodId: number;
    rating: number;
    comment: string;
}