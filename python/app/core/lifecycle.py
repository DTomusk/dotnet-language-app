from contextlib import asynccontextmanager
import os

from fastapi import FastAPI
import spacy


DEFAULT_MODEL = os.getenv("NLP_MODEL", "it_core_news_sm")

LANGUAGE_CONFIG = {
    "it": {
        "model": "it_core_news_sm",
        "description": "Italian language model for spaCy",
    },
}

@asynccontextmanager
async def lifespan(app: FastAPI):
    try:
        app.state.nlp = spacy.load(DEFAULT_MODEL)
    except OSError as e:
        raise RuntimeError(f"Failed to load spaCy model '{DEFAULT_MODEL}': {e}")
    
    # Set the supported languages for the NLP model
    # TODO: inject
    app.state.language_map = LANGUAGE_CONFIG
    app.state.model_cache = {}
    
    # yield control back to the application
    yield

    # cleanup code
    app.state.nlp = None