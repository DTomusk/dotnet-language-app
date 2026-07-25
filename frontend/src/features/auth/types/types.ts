export type RegistrationRequest = {
    displayName: string;
    password: string;
}

export type RegistrationResponse = {
    token: string;
}

export type LoginRequest = {
    displayName: string;
    password: string;
}

export type LoginResponse = {
    token: string;
}