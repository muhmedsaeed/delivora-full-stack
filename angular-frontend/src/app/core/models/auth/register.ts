

export interface RegisterRequest {
    username: string;
    fullName: string;
    email: string;
    password: string;
    passwordConfirmed: string;
    phoneNumber: string;
    birthDate: string;
    addressTitle?: string;
    street?: string;
    city?: string;
    country?: string;
}