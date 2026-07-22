import { useAvailableLanguages } from "../hooks/useAvailableLanguages";

export default function LanguageSelector() {
    const { data: languages, isLoading } = useAvailableLanguages();

    if (isLoading) {
        return <div>Loading languages...</div>;
    }

    if (!languages || languages.length === 0) {
        return <div>No languages available.</div>;
    }

    return (
        <select>
            {languages.map((language) => (
                <option key={language.id} value={language.id}>
                    {language.name}
                </option>
            ))}
        </select>
    );
}