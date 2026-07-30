from pydantic import BaseModel, field_validator


class ValidateRequest(BaseModel):
    text: str
    languageCode: str

    @field_validator("languageCode")
    @classmethod
    def normalize_language_code(cls, v: str) -> str:
        normalized = v.strip().lower()
        if not normalized:
            raise ValueError("languageCode is required.")
        return normalized
    
    @field_validator("text")
    @classmethod
    def normalize_text(cls, v: str) -> str:
        normalized = v.strip()
        if not normalized:
            raise ValueError("text is required.")
        return normalized

class ValidateResponse(BaseModel):
    valid: bool