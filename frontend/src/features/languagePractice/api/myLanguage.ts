import { api } from "../../../lib/api/api";

export const myLanguage = async () => {
    return api.get<string>("/Me/Language");
}