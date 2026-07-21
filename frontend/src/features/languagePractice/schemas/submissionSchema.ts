import { z } from "zod";

export const submissionSchema = z.object({
    text: z.string()
        .min(1, "Submission text is required")
        .max(500, "Submission text must be at most 500 characters"),
});

export type SubmissionSchema = z.infer<typeof submissionSchema>;