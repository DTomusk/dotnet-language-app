import { myLanguage } from "../api/myLanguage";
import { useQuery } from "@tanstack/react-query";

export function useActiveLanguage() {
    return useQuery<string>({
        queryKey: ["me", "language"],
        queryFn: async () => {
            const response = await myLanguage();
            return response;
        }
    });
}