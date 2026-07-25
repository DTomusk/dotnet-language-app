import type { GetLanguageStatsResponse } from "../types/types";
import { useQuery } from "@tanstack/react-query";
import { getMyLanguageStats } from "../api/myLanguage";

export function useLanguageStats() {
    return useQuery<GetLanguageStatsResponse>({
        queryKey: ["me", "language", "stats"],
        queryFn: async () => {
            const response = await getMyLanguageStats();
            return response;
        },
    });
}