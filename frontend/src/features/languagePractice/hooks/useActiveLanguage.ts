import { getMyLanguage, setMyLanguage } from "../api/myLanguage";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { SetLanguageRequest } from "../types/types";

const ACTIVE_LANGUAGE_QUERY_KEY = ["me", "language"] as const;

export function useGetActiveLanguage() {
    return useQuery<string>({
        queryKey: ACTIVE_LANGUAGE_QUERY_KEY,
        queryFn: async () => {
            const response = await getMyLanguage();
            return response;
        }
    });
}

export function useSetActiveLanguage() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (input: SetLanguageRequest) => setMyLanguage(input),
        onSuccess: () => {
            // Remove cached value so next read must fetch the latest language.
            queryClient.removeQueries({ queryKey: ACTIVE_LANGUAGE_QUERY_KEY, exact: true });
        },
    });
}