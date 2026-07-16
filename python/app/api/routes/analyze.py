from fastapi import APIRouter, HTTPException, Request
from app.schemas.analyze import AnalyzeRequest, AnalyzeResponse
from app.services.nlp import analyze_text

router = APIRouter(tags=["analyze"])

@router.post("/analyze", response_model=AnalyzeResponse)
def analyze(payload: AnalyzeRequest, request: Request) -> AnalyzeResponse:
    lemmas = analyze_text(request.app.state.nlp, payload.text)
    if not lemmas:
        raise HTTPException(status_code=400, detail="Text cannot be empty.")

    return AnalyzeResponse(lemmas=lemmas)
    