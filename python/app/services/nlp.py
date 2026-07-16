from spacy.language import Language

def tokenize_text(nlp: Language, text: str) -> list[str]:
    """
    Tokenizes the input text using the provided spaCy NLP model.

    Args:
        nlp (Language): The spaCy NLP model.
        text (str): The input text to tokenize.

    Returns:
        list[str]: A list of token strings extracted from the input text.
    """
    cleaned_text = text.strip()
    if not cleaned_text:
        return []
    doc = nlp(cleaned_text)
    return [token.text for token in doc]