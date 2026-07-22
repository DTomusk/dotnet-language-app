import { submission } from "../api/submission";
import type { SubmissionRequest } from "../types/types";

export function useSubmission() {
    return {
        mutateAsync: async (input: SubmissionRequest) => {
            return submission(input);
        },
    };
}