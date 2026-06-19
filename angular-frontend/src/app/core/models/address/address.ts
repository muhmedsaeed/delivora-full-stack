

export interface Address {
    id: number;
    title: string;
    street: string;
    city: string;
    country: string;
}


export interface CreateAddressRequest {
    title: string;
    street: string;
    city: string;
    country: string;
}