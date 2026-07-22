import LanguageSelector from "../features/languagePractice/components/LanguageSelector";
import SubmissionForm from "../features/languagePractice/components/SubmissionForm";
import { useGetActiveLanguage, useSetActiveLanguage } from "../features/languagePractice/hooks/useActiveLanguage";
import { useAvailableLanguages } from "../features/languagePractice/hooks/useAvailableLanguages";
import { useSubmission } from "../features/languagePractice/hooks/useSubmission";
import type { SubmissionSchema } from "../features/languagePractice/schemas/submissionSchema";
import { useState } from "react";

export default function HomePage() {
    const { data, isLoading, error, refetch } = useGetActiveLanguage();
    const { data: languages, isLoading: isLoadingLanguages } = useAvailableLanguages();
    const setLanguageMutation = useSetActiveLanguage();
    const submissionMutation = useSubmission();
    const [selectedLanguage, setSelectedLanguage] = useState("");

    const onConfirmLanguage = async () => {
        if (!selectedLanguage) {
            return;
        }

        await setLanguageMutation.mutateAsync({ languageCode: selectedLanguage });
        await refetch();
    };

    const onSubmit = async (formData: SubmissionSchema) => {
        const response = await submissionMutation.mutateAsync(formData);
        console.log("Submission response:", response);
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error.message}</div>;
    }

    if (!data) {
        return (
            <LanguageSelector
                languages={languages}
                isLoading={isLoadingLanguages}
                isSubmitting={setLanguageMutation.isPending}
                selectedLanguage={selectedLanguage}
                onLanguageChange={setSelectedLanguage}
                onConfirm={onConfirmLanguage}
            />
        );
    }

    return (
        <SubmissionForm onSubmit={onSubmit} />
    );
}