import type { ApiError } from "../../../lib/api/error";
import { useMutation } from "@tanstack/react-query";
import { submission } from "../api/submission";
import type { SubmissionRequest } from "../types/types";

export function useSubmission() {
    return useMutation<string, ApiError, SubmissionRequest>({
        mutationFn: submission,
    });
}