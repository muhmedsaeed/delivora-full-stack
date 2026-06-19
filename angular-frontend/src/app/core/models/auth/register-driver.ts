
export interface RegisterDriverRequest {
    username: string;
    email: string;
    password: string;
    fullName: string;
    phoneNumber?: string;
    vehicleType: VehicleType;
    licenseNumber: string;
}


export enum VehicleType {
    Car = 0,
    Motorcycle = 1,
    Bicycle = 2,
    Scooter = 3
}