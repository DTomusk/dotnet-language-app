from contextlib import asynccontextmanager

from pydantic import BaseModel
import spacy
from fastapi import FastAPI, HTTPException, Request

DEFAULT_MODEL = "it_core_news_sm"

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

app = FastAPI(lifespan=lifespan)

class AnalyzeRequest(BaseModel):
    text: str

class AnalyzeResponse(BaseModel):
    tokens: list[str]

@app.get("/")
def root():
    return {"message": "Hello, FastAPI!"}

@app.post("/analyze")
def analyze_text(payload: AnalyzeRequest, request: Request):
    text = payload.text.strip()
    if not text:
        raise HTTPException(status_code=400, detail="Text cannot be empty.")
    
    doc = request.app.state.nlp(text)
    return AnalyzeResponse(tokens=[token.text for token in doc])