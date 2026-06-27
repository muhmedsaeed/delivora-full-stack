export interface Profile {
    id: number;
    username: string;
    fullName: string;
    email: string;
    phoneNumber?: string;
    birthDate: string;
    status: string;
    profileImageUrl?: string;
    roles: string[];
    vehicleType?: string;
    licenseNumber?: string;
}

export interface AdminUser {
    id: number;
    username: string;
    fullName: string;
    email: string;
    phoneNumber?: string;
    status: string;
    profileImageUrl?: string;
    roles: string[];
    vehicleType?: string;
    licenseNumber?: string;
}
