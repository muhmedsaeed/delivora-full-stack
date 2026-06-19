


export interface Payment {
    id: number;
    amount: number;
    status: string;
    paymentDate: string;
    orderId: number;
    methodName: string;
}



export enum PaymentStatus {
    Pending = 0,
    Paid = 1,
    Failed = 2,
    Refunded = 3
}