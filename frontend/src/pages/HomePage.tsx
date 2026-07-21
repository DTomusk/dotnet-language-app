import SubmissionForm from "../features/languagePractice/components/SubmissionForm";
import { useSubmission } from "../features/languagePractice/hooks/useSubmission";
import type { SubmissionSchema } from "../features/languagePractice/schemas/submissionSchema";

export default function HomePage() {
    const mutation = useSubmission();

    const onSubmit = async (formData: SubmissionSchema) => {
        const response = await mutation.mutateAsync(formData);
        console.log("Submission response:", response);
    }

    return (
        <SubmissionForm onSubmit={onSubmit} />
    );
}