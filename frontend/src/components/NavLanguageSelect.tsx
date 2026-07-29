import { useTranslation } from "react-i18next";
import { useState, useEffect } from "react";
import { useSetActiveLanguage, useGetActiveLanguage } from "../features/languagePractice/hooks/useActiveLanguage";
import Box from "@mui/material/Box";
import DropdownSelect from "./DropdownSelect";
import { useAvailableLanguages } from "../features/languagePractice/hooks/useAvailableLanguages";

export default function NavLanguageSelect() {
  const { t } = useTranslation(["languagePractice", "common"]);
  const { data: activeLanguage, isLoading: isLoadingActive, refetch } = useGetActiveLanguage();
  const { data: languages, isLoading: isLoadingLanguages } = useAvailableLanguages();
  const setLanguage = useSetActiveLanguage();
  const [selectedLanguage, setSelectedLanguage] = useState("");

  useEffect(() => {
    if (activeLanguage) setSelectedLanguage(activeLanguage);
  }, [activeLanguage]);

  const onChange = async (next: string) => {
    const previous = selectedLanguage;
    setSelectedLanguage(next);
    try {
      await setLanguage.mutateAsync({ languageCode: next });
      await refetch();
    } catch {
      setSelectedLanguage(previous);
    }
  };

  if (isLoadingActive || isLoadingLanguages || !languages?.length) return null;

  return (
    <Box sx={{ width: 150 }}>
      <DropdownSelect
        value={selectedLanguage}
        onChange={onChange}
        placeholder={t("languagePractice:languageSelection.selectLanguagePlaceholder")}
        items={languages.map((language) => ({
            value: language.languageCode,
            label: language.languageName,
        }))}
      />
    </Box>
  );
}