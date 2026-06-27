

export interface AuthResponse {
    token: string;
    expiresAt: string;
    username: string;
    email: string;
    fullName: string;
    status: string;
    profileImageUrl?: string;
    roles: string[];
}
