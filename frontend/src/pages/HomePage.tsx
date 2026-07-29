import { Button, Stack, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useLanguageStats } from "../features/languagePractice/hooks/useLanguageStats";
import { useGetActiveLanguage, useSetActiveLanguage } from "../features/languagePractice/hooks/useActiveLanguage";
import { useAvailableLanguages } from "../features/languagePractice/hooks/useAvailableLanguages";
import Spinner from "../components/Spinner";
import Alert from "@mui/material/Alert";
import LanguageSelector from "../features/languagePractice/components/LanguageSelector";
import { useState } from "react";
import { useTranslation } from "react-i18next";

export default function HomePage() {
    const navigate = useNavigate();
    const { t } = useTranslation(["common"]);
    
    const { data: languageStats } = useLanguageStats();

    const { data, isLoading, error, refetch } = useGetActiveLanguage();
    const { data: languages, isLoading: isLoadingLanguages } = useAvailableLanguages();
    const setLanguageMutation = useSetActiveLanguage();
    const [selectedLanguage, setSelectedLanguage] = useState("");

    const onConfirmLanguage = async () => {
        if (!selectedLanguage) {
            return;
        }

        await setLanguageMutation.mutateAsync({ languageCode: selectedLanguage });
        await refetch();
    };

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
        <Stack spacing={5} 
            sx={{
                maxWidth: 600,
                width: "100%",
                textAlign: "center",
            }}>
            {/* TODO: these strings should be internationalized */}
            <Typography variant="h3" component="h1">
                Welcome back {languageStats?.displayName}
            </Typography>
            <Typography variant="body1" color="text.secondary">
                You have practised {languageStats?.uniqueLemmas} unique words in the last {languageStats?.daysPractised} {languageStats?.daysPractised === 1 ? "day" : "days"}.
            </Typography>
            <Button
                variant="contained"
                size="large"
                color="primary"
                sx={{ alignSelf: "center" }}
                onClick={() => navigate("/practice")}
            >
                Start Practicing
            </Button>
        </Stack>
    )
}