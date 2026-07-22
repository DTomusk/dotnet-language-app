import { Button, Paper, Stack, Typography } from "@mui/material";
import { useAvailableLanguages } from "../hooks/useAvailableLanguages";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import DropdownSelect from "./DropdownSelect.tsx";

export default function LanguageSelector() {
    const { data: languages, isLoading } = useAvailableLanguages();
    const [selectedLanguage, setSelectedLanguage] = useState("");
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
                    onChange={setSelectedLanguage}
                    placeholder={t("languagePractice:languageSelection.selectLanguagePlaceholder")}
                    items={languages.map((language) => ({
                        value: language.languageCode,
                        label: language.languageName,
                    }))}
                />
                <Button
                    variant="contained"
                    color="primary"
                    disabled={!selectedLanguage}
                    sx={{ alignSelf: "flex-start" }}
                >
                    {t("common:actions.confirm")}
                </Button>
            </Stack>
        </Paper>
    );
}