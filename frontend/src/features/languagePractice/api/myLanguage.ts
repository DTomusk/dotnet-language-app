import { api } from "../../../lib/api/api";
import type { SetLanguageRequest } from "../types/types";

export const getMyLanguage = async () => {
    return api.get<string>("/Me/Language");
}

export const setMyLanguage = async (input: SetLanguageRequest) => {
    return api.put("/Me/Language", JSON.stringify(input));
}