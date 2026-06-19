

export interface OrderReview {
    id: number;
    packagingRate: number;
    deliveryRate: number;
    comment: string;
    createdAt: string;
    customerName: string;
    orderId: number;
}


export interface CreateOrderReviewRequest {
    orderId: number;
    packagingRate: number;
    deliveryRate: number;
    comment: string;
}