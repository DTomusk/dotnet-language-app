import { api } from "../../../lib/api/api";
import type { SetLanguageRequest, GetLanguageStatsResponse } from "../types/types";

export const getMyLanguage = async () => {
    return api.get<string>("/Me/Language");
}

export const setMyLanguage = async (input: SetLanguageRequest) => {
    return api.put("/Me/Language", JSON.stringify(input));
}

// TODO: consider moving to a different file
export const getMyLanguageStats = async () => {
    return api.get<GetLanguageStatsResponse>("/Me/Language/Stats");
}