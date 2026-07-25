import { api } from "../../../lib/api/api"

import type { LoginRequest, LoginResponse } from "../types/types";

export const login = async (input: LoginRequest) => {
    return api.post<LoginResponse>("/auth/login", JSON.stringify(input));
}