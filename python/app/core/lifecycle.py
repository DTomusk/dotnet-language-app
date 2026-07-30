from contextlib import asynccontextmanager

from fastapi import FastAPI
from lingua import Language, LanguageDetector, LanguageDetectorBuilder

LANGUAGE_CONFIG = {
    "it": {
        "model": "it_core_news_sm",
        "description": "Italian language model for spaCy",
    },
    "de": {
        "model": "de_core_news_sm",
        "description": "German language model for spaCy",
    }
}

LINGUA_LANGUAGE_MAP = {
    "it": Language.ITALIAN,
    "de": Language.GERMAN,
}

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Set the supported languages for the NLP model
    # TODO: inject
    app.state.language_map = LANGUAGE_CONFIG
    app.state.model_cache = {}

    supported_detection_languages = [
        LINGUA_LANGUAGE_MAP[lang_code] for lang_code in LANGUAGE_CONFIG.keys() if lang_code in LINGUA_LANGUAGE_MAP
    ]
    # Build singleton once at startup
    app.state.language_detector = (
        LanguageDetectorBuilder
        .from_languages(*supported_detection_languages)
        .build()
    )
    app.state.language_detector_threshold = 0.8
    
    # yield control back to the application
    yield

    # cleanup code
    app.state.model_cache.clear()
    app.state.language_detector = None
