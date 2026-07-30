from lingua import Language, LanguageDetector
from typing import Mapping

LINGUA_LANGUAGE_MAP: Mapping[str, Language] = {
    "it": Language.ITALIAN,
    "de": Language.GERMAN,
}

class UnsupportedLanguageForDetectionError(ValueError):
    pass

def validate_language(
    text: str, 
    language: str,
    detector: LanguageDetector,
    threshold: float = 0.8,
    language_map: Mapping[str, Language] = LINGUA_LANGUAGE_MAP
) -> bool:
    """
    Validates if the given text is in the specified language.

    Args:
        text (str): The text to validate.
        language (str): The language code to validate against.
        detector (LanguageDetector): An instance of the LanguageDetector class.
        threshold (float): The confidence threshold for language detection. Default is 0.8.
        language_map (Mapping[str, Language]): A mapping of language codes to Lingua Language enums.

    Returns:
        bool: True if the text is in the specified language, False otherwise.
    """
    cleaned = text.strip()
    if not cleaned:
        return False

    expected = language_map.get(language.lower())
    if expected is None:
        raise UnsupportedLanguageForDetectionError(
            f"Unsupported language code for detection: {language}"
        )

    confidences = detector.compute_language_confidence_values(cleaned)
    if not confidences:
        return False

    top = confidences[0]
    return top.language == expected and top.value >= threshold
