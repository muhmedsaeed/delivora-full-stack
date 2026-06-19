

export interface Driver {
    userId: number;
    fullName: string;
    email: string;
    phone?: string;
    vehicleType: string;
    licenseNumber: string;
    isAvailable: boolean;
    totalEarnings: number;
}