from fastapi import FastAPI
from app.core.lifecycle import lifespan
from app.api.routes.health import router as health_router
from app.api.routes.analyze import router as analyze_router

app = FastAPI(
    title="NLP API",
    version="1.0.0",
    lifespan=lifespan,
)

app.include_router(health_router)
app.include_router(analyze_router)
