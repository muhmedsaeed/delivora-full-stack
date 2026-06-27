import { CreateOrderItem, OrderItem } from "../order-item/order-item";


export interface Order {
    id: number;
    status: string;
    subTotal: number;
    deliveryFee: number;
    tax: number;
    total: number;
    createdAt: string;
    notes: string;
    customerName: string;
    driverId?: number;
    driverName: string;
    addressTitle: string;
    items: OrderItem[];
}


export interface CreateOrderRequest {
    addressId: number;
    driverId?: number;
    notes: string;
    items: CreateOrderItem[];
    paymentMethodId: number;
}


export enum OrderStatus {
    Pending = 0,
    Confirmed = 1,
    Preparing = 2,
    PickedUp = 3,
    Delivered = 4,
    Cancelled = 5
}
