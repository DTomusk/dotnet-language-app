import { useMutation } from "@tanstack/react-query";
import { login } from "../api/login";
import type { ApiError } from "../../../lib/api/error";
import type { LoginRequest, LoginResponse } from "../types/types";

export function useLogin() {
    return useMutation<
        LoginResponse,
        ApiError,
        LoginRequest
    >({ 
        mutationFn: login
    })

}