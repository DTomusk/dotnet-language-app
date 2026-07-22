import LanguageSelector from "../features/languagePractice/components/LanguageSelector";
import SubmissionForm from "../features/languagePractice/components/SubmissionForm";
import { useActiveLanguage } from "../features/languagePractice/hooks/useActiveLanguage";
import { useSubmission } from "../features/languagePractice/hooks/useSubmission";
import type { SubmissionSchema } from "../features/languagePractice/schemas/submissionSchema";

export default function HomePage() {
    const { data, isLoading, error } = useActiveLanguage();
    const mutation = useSubmission();

    const onSubmit = async (formData: SubmissionSchema) => {
        const response = await mutation.mutateAsync(formData);
        console.log("Submission response:", response);
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error.message}</div>;
    }

    if (!data) {
        return <LanguageSelector />;
    }

    return (
        <SubmissionForm onSubmit={onSubmit} />
    );
}