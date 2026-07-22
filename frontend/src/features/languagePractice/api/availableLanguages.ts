import { api } from "../../../lib/api/api";
import type { Language } from "../types/types";

export const availableLanguages = async () => {
    return api.get<Language[]>("/AvailableLanguages");
}