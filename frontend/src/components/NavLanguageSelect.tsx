import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import DropdownSelect from "./DropdownSelect";
import { useLanguageSelection } from "../features/languagePractice/hooks/useLanguageSelection";

export default function NavLanguageSelect() {
    const { t } = useTranslation(["languagePractice", "common"]);
    const { languageItems, selectedLanguage, setSelectedLanguage, confirmLanguage, isLoading } = useLanguageSelection();

    const onChange = async (next: string) => {
        const previous = selectedLanguage;
        setSelectedLanguage(next);

        try {
            await confirmLanguage(next);
        } catch {
            setSelectedLanguage(previous);
        }
    };

    if (isLoading || languageItems.length === 0) {
        return null;
    }

    return (
        <Box sx={{ width: 150 }}>
            <DropdownSelect
                value={selectedLanguage}
                onChange={onChange}
                placeholder={t("languagePractice:languageSelection.selectLanguagePlaceholder")}
                items={languageItems}
            />
        </Box>
    );
}