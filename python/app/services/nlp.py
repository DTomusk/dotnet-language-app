from spacy.language import Language
import spacy
from typing import Mapping

class UnsupportedLanguageError(ValueError):
    pass

def get_or_load_model(
    language_code: str,
    language_model_map: Mapping[str, dict[str, str]],
    model_cache: dict[str, Language],
) -> Language:
    """
    Retrieves a spaCy model for the given language code. If the model is not already loaded,
    it will be loaded and cached for future use.

    Args:
        language_code (str): The language code for which to retrieve the model.
        language_model_map (Mapping[str, dict[str, str]]): A mapping of language codes to their corresponding
            spaCy model names and descriptions.
        model_cache (dict[str, Language]): A cache of already loaded spaCy models.

    Returns:
        Language: The spaCy model for the given language code.
    """
    model_name = language_model_map.get(language_code, {}).get("model")
    if model_name is None:
        raise ValueError(f"No model found for language code: {language_code}")
    
    cached_model = model_cache.get(model_name)
    if cached_model is not None:
        return cached_model
    
    try: 
        nlp = spacy.load(model_name)
        model_cache[model_name] = nlp
        return nlp
    except OSError as e:
        raise RuntimeError(f"Failed to load spaCy model '{model_name}': {e}")

def analyze_text(nlp: Language, text: str) -> list[str]:
    """
    Analyzes the input text using the provided spaCy NLP model.

    Args:
        nlp (Language): The spaCy NLP model.
        text (str): The input text to analyze.

    Returns:
        list[str]: A list of lemma strings extracted from the input text.
    """
    cleaned_text = text.strip()
    if not cleaned_text:
        return []
    doc = nlp(cleaned_text)

    lemmas = [
        token.lemma_.lower()
        for token in doc 
        if token.is_alpha
        and not token.is_punct
        and not token.like_num
        and not token.is_space
    ]

    return lemmas