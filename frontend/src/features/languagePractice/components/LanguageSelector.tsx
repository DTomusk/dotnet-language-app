import { Button, Paper, Stack, Typography } from "@mui/material";
import { useAvailableLanguages } from "../hooks/useAvailableLanguages";
import { useState } from "react";
import { useTranslation } from "react-i18next";

export default function LanguageSelector() {
    const { data: languages, isLoading } = useAvailableLanguages();
    const [selectedLanguage, setSelectedLanguage] = useState<string | null>(null);
    const { t } = useTranslation(["languagePractice", "common"]);

    if (isLoading) {
        return <div>Loading languages...</div>;
    }

    if (!languages || languages.length === 0) {
        return <div>No languages available.</div>;
    }

    return (
        <Paper>
            <Stack spacing={2} sx={{ p: 4, maxWidth: 600, margin: "0 auto" }}>
                <Typography variant="h4">{t("languagePractice:languageSelection.title")}</Typography>
                <select
                    value={selectedLanguage ?? ""}
                    onChange={(e) => setSelectedLanguage(e.target.value)}
                >
                    <option value="">{t("common:selectLanguage")}</option>
                    {languages.map((language) => (
                        <option key={language.languageCode} value={language.languageCode}>
                            {language.languageName}
                        </option>
                    ))}
                </select>
                <Button variant="contained" color="primary" disabled={!selectedLanguage}>
                    {t("common:actions.confirm")}
                </Button>
            </Stack>
        </Paper>
    );
}