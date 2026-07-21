import { submission } from "../api/submission";

export function useSubmission() {
    return {
        mutateAsync: async (input: { text: string }) => {
            return submission(input);
        },
    };
}