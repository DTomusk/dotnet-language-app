import { useQuery } from "@tanstack/react-query";
import type { Language } from "../types/types";
import { availableLanguages } from "../api/availableLanguages";

export function useAvailableLanguages() {
    return useQuery<Language[]>({
        queryKey: ["languages"],
        queryFn: async () => availableLanguages()
    });
}