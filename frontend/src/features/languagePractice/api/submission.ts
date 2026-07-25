import { api } from "../../../lib/api/api";
import type { SubmissionRequest } from "../types/types";

export const submission = async (input: SubmissionRequest) => {
    return api.post<string>("/submission", JSON.stringify(input));
};