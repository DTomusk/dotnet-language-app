import { api } from "../../../lib/api/api"

import type { LoginRequest, LoginResponse } from "../types/types";

export const register = async (input: LoginRequest) => {
    return api.post<LoginResponse>("/auth/register", JSON.stringify(input));
}