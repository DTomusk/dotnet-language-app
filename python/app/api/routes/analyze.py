from fastapi import APIRouter, HTTPException, Request
from app.schemas.analyze import AnalyzeRequest, AnalyzeResponse
from app.services.nlp import analyze_text, get_or_load_model, UnsupportedLanguageError

router = APIRouter(tags=["analyze"])


@router.post("/analyze", response_model=AnalyzeResponse)
def analyze(
    payload: AnalyzeRequest, 
    request: Request,
) -> AnalyzeResponse:
    try:
        nlp = get_or_load_model(
            language_code=payload.languageCode,
            language_model_map=request.app.state.language_map,
            model_cache=request.app.state.model_cache,
        )
    except UnsupportedLanguageError as e:
        raise HTTPException(status_code=422, detail=str(e))

    lemmas = analyze_text(nlp, payload.text)
    return AnalyzeResponse(lemmas=lemmas)
    