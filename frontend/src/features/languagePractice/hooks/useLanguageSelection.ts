import { useEffect, useMemo, useState } from "react";
import { useAvailableLanguages } from "./useAvailableLanguages";
import { useGetActiveLanguage, useSetActiveLanguage } from "./useActiveLanguage";

export function useLanguageSelection() {
    const { data: activeLanguage, isLoading: isLoadingActiveLanguage, error } = useGetActiveLanguage();
    const { data: languages, isLoading: isLoadingLanguages } = useAvailableLanguages();
    const setActiveLanguageMutation = useSetActiveLanguage();
    const [selectedLanguage, setSelectedLanguage] = useState("");

    useEffect(() => {
        if (activeLanguage) {
            setSelectedLanguage(activeLanguage);
        }
    }, [activeLanguage]);

    const languageItems = useMemo(() => {
        return (languages ?? []).map((language) => ({
            value: language.languageCode,
            label: language.languageName,
        }));
    }, [languages]);

    const confirmLanguage = async (languageCode?: string) => {
        const nextLanguageCode = languageCode ?? selectedLanguage;

        if (!nextLanguageCode) {
            return;
        }

        await setActiveLanguageMutation.mutateAsync({ languageCode: nextLanguageCode });
    };

    return {
        activeLanguage,
        error,
        languages,
        languageItems,
        selectedLanguage,
        setSelectedLanguage,
        confirmLanguage,
        isLoading: isLoadingActiveLanguage || isLoadingLanguages,
        isLoadingLanguages,
        isSubmitting: setActiveLanguageMutation.isPending,
    };
}