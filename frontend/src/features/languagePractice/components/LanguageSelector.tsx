import { Button, Paper, Stack, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import DropdownSelect from "./DropdownSelect.tsx";
import type { Language } from "../types/types";

type LanguageSelectorProps = {
    languages?: Language[];
    isLoading: boolean;
    isSubmitting: boolean;
    selectedLanguage: string;
    onLanguageChange: (value: string) => void;
    onConfirm: () => Promise<void> | void;
};

export default function LanguageSelector({
    languages,
    isLoading,
    isSubmitting,
    selectedLanguage,
    onLanguageChange,
    onConfirm,
}: LanguageSelectorProps) {
    const { t } = useTranslation(["languagePractice", "common"]);

    if (isLoading) {
        return <div>Loading languages...</div>;
    }

    if (!languages || languages.length === 0) {
        return <div>No languages available.</div>;
    }

    return (
        <Paper>
            <Stack spacing={3} sx={{ p: 4, maxWidth: 600, margin: "0 auto" }}>
                <Typography variant="h4">{t("languagePractice:languageSelection.title")}</Typography>
                <Typography variant="body2" color="text.secondary">
                    {t("languagePractice:languageSelection.subtitle")}
                </Typography>
                <DropdownSelect
                    value={selectedLanguage}
                    onChange={onLanguageChange}
                    placeholder={t("languagePractice:languageSelection.selectLanguagePlaceholder")}
                    items={languages.map((language) => ({
                        value: language.languageCode,
                        label: language.languageName,
                    }))}
                />
                <Button
                    variant="contained"
                    color="primary"
                    disabled={!selectedLanguage || isSubmitting}
                    sx={{ alignSelf: "flex-start" }}
                    onClick={onConfirm}
                >
                    {t("common:actions.confirm")}
                </Button>
            </Stack>
        </Paper>
    );
}