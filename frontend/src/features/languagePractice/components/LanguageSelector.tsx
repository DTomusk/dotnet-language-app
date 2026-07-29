import { Alert, Button, Paper, Stack, Typography } from "@mui/material";
import Spinner from "../../../components/Spinner";
import { useTranslation } from "react-i18next";
import DropdownSelect from "../../../components/DropdownSelect.tsx";
import type { DropdownSelectItem } from "../../../components/DropdownSelect";

type LanguageSelectorProps = {
    items: DropdownSelectItem[];
    isLoading: boolean;
    isSubmitting: boolean;
    selectedLanguage: string;
    onLanguageChange: (value: string) => void;
    onConfirm: () => Promise<void> | void;
};

export default function LanguageSelector({
    items,
    isLoading,
    isSubmitting,
    selectedLanguage,
    onLanguageChange,
    onConfirm,
}: LanguageSelectorProps) {
    const { t } = useTranslation(["languagePractice", "common"]);

    if (isLoading) {
        return <Spinner aria-label={t("languagePractice:languageSelection.loadingLanguages")} />;
    }

    if (items.length === 0) {
        return <Alert severity="error">{t("languagePractice:languageSelection.noLanguagesAvailable")}</Alert>;
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
                    items={items}
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