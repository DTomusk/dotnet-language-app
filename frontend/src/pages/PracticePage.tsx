import LanguageSelector from "../features/languagePractice/components/LanguageSelector";
import Spinner from "../components/Spinner";
import SubmissionForm from "../features/languagePractice/components/SubmissionForm";
import { useGetActiveLanguage, useSetActiveLanguage } from "../features/languagePractice/hooks/useActiveLanguage";
import { useAvailableLanguages } from "../features/languagePractice/hooks/useAvailableLanguages";
import { useSubmission } from "../features/languagePractice/hooks/useSubmission";
import type { SubmissionSchema } from "../features/languagePractice/schemas/submissionSchema";
import { useState } from "react";

import { useTranslation } from "react-i18next";
import Alert from "@mui/material/Alert";
import Stack from "@mui/material/Stack";

export default function PracticePage() {
    const { t } = useTranslation(["common"]);
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

    // If submit succeeds, redirect to submission page (which will show state of analysis and link to analysis once done)
    // If it fails, show error and disable submit button until user changes the input
    // 
    const onSubmit = async (formData: SubmissionSchema) => {
        const response = await submissionMutation.mutateAsync(formData);
        console.log("Submission response:", response);
    }

    if (isLoading) {
        return <Spinner aria-label={t("common:loading")} />;
    }

    if (error) {
        return <Alert severity="error">{t("common:error")}: {error.message}</Alert>;
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
        <Stack spacing={2} sx={{ maxWidth: 600, margin: "0 auto", p: 4 }}>
            {submissionMutation.isError && (
                <Alert severity="error">
                    {t("common:error")}: {submissionMutation.error?.message}
                </Alert>
            )}
            {submissionMutation.isSuccess && (
                <Alert severity="success">
                    {t("common:success")}
                </Alert>
            )}
            <SubmissionForm onSubmit={onSubmit} 
                            isSubmitting={submissionMutation.isPending}
                            isSuccess={submissionMutation.isSuccess}
            />
        </Stack>
    );
}