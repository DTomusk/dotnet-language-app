from contextlib import asynccontextmanager
import os

from fastapi import FastAPI
import spacy


DEFAULT_MODEL = os.getenv("NLP_MODEL", "it_core_news_sm")

@asynccontextmanager
async def lifespan(app: FastAPI):
    try:
        app.state.nlp = spacy.load(DEFAULT_MODEL)
    except OSError as e:
        raise RuntimeError(f"Failed to load spaCy model '{DEFAULT_MODEL}': {e}")
    
    # yield control back to the application
    yield

    # cleanup code
    app.state.nlp = None