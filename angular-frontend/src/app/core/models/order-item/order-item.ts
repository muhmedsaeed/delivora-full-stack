

export interface OrderItem {
    id: number;
    foodId: number;
    foodName: string;
    quantity: number;
    unitPrice: number;
    totalPrice: number;
}


export interface CreateOrderItem {
    foodId: number;
    quantity: number;
}