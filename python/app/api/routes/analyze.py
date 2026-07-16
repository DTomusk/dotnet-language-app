from fastapi import APIRouter, HTTPException, Request
from app.schemas.analyze import AnalyzeRequest, AnalyzeResponse
from app.services.nlp import tokenize_text

router = APIRouter(tags=["analyze"])

@router.post("/analyze", response_model=AnalyzeResponse)
def analyze_text(payload: AnalyzeRequest, request: Request) -> AnalyzeResponse:
    tokens = tokenize_text(request.app.state.nlp, payload.text)
    if not tokens:
        raise HTTPException(status_code=400, detail="Text cannot be empty.")

    return AnalyzeResponse(tokens=tokens)
    