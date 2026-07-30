from fastapi import APIRouter, Request
from app.schemas.validate import ValidateRequest, ValidateResponse

router = APIRouter(tags=["validate"])

@router.post("/validate", response_model=ValidateResponse)
def validate(
    payload: ValidateRequest,
    request: Request,
) -> ValidateResponse:
    """
    Validates if the given text is in the specified language.

    Args:
        payload (ValidateRequest): The request payload containing the text and language code.
        request (Request): The FastAPI request object.

    Returns:
        ValidateResponse: The response indicating whether the text is valid for the specified language.
    """
    from app.services.language_validation import validate_language, UnsupportedLanguageForDetectionError

    try:
        is_valid = validate_language(
            text=payload.text,
            language=payload.languageCode,
            detector=request.app.state.language_detector,
            threshold=request.app.state.language_detector_threshold,
        )
    except UnsupportedLanguageForDetectionError as e:
        return ValidateResponse(valid=False)

    return ValidateResponse(valid=is_valid)