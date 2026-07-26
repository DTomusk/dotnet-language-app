from contextlib import asynccontextmanager

from fastapi import FastAPI

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

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Set the supported languages for the NLP model
    # TODO: inject
    app.state.language_map = LANGUAGE_CONFIG
    app.state.model_cache = {}
    
    # yield control back to the application
    yield

    # cleanup code
    app.state.nlp = None