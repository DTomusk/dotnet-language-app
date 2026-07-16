from pydantic import BaseModel


class AnalyzeRequest(BaseModel):
    text: str

class AnalyzeResponse(BaseModel):
    tokens: list[str]