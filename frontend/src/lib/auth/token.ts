const JWT_TOKEN_KEY = "jwt";

function getToken(): string | null {
    return localStorage.getItem(JWT_TOKEN_KEY);
}

function setToken(token: string): void {
    localStorage.setItem(JWT_TOKEN_KEY, token);
}

function clearToken(): void {
    localStorage.removeItem(JWT_TOKEN_KEY);
}

export { getToken, setToken, clearToken };