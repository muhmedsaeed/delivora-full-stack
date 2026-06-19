

export interface Food {
    id: number;
    name: string;
    description: string;
    price: number;
    imageUrl?: string;
    isAvailable: boolean;
    categoryId: number;
    categoryName: string;
}


export interface CreateFoodRequest {
    name: string;
    description: string;
    price: number;
    isAvailable: boolean;
    categoryId: number;
    image?: File;
}