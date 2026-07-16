from spacy.language import Language

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