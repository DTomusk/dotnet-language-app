# Language Validation Test Suite

This directory contains comprehensive unit and integration tests for the language validation flow in the NLP API.

## Test Coverage

### Unit Tests (`test_language_validation.py`)
Tests for the `validate_language()` function in isolation using mocked detectors:

- **High confidence match**: Text correctly identified with confidence above threshold
- **Threshold boundary**: Confidence exactly at threshold (edge case)
- **Low confidence mismatch**: Correct language but confidence below threshold
- **Wrong language**: Text detected as different language
- **Empty/whitespace text**: Rejected before detector call
- **Empty confidence results**: Detector returns no results
- **Unsupported language code**: Raises appropriate error
- **Case insensitivity**: Language codes normalized to lowercase
- **Text trimming**: Whitespace stripped before detection
- **Default threshold**: Uses 0.8 when not specified
- **Custom language map**: Supports alternate language mappings
- **Multiple confidences**: Only top-ranked result considered
- **Low confidence on correct language**: Returns false when top matches but score too low

### Integration Tests (`test_validate_route.py`)
Tests for the `/validate` endpoint:

- **Valid language detected**: Returns `valid: true`
- **Invalid language detected**: Returns `valid: false`
- **Low confidence rejection**: Returns `valid: false`
- **Unsupported language handling**: Gracefully returns `valid: false`
- **Missing fields**: Pydantic schema validation (422 errors)
- **Empty fields**: Schema validation rejects empty values
- **Whitespace fields**: Schema validation rejects whitespace-only values
- **Language code normalization**: Uppercase codes converted to lowercase
- **Text normalization**: Whitespace trimmed before validation
- **Detector from app state**: Confirms detector passed from FastAPI app state
- **Threshold from app state**: Confirms threshold passed from FastAPI app state
- **Response schema**: Validates response matches `ValidateResponse` model

## Running Tests

### Install test dependencies
```powershell
pip install pytest pytest-asyncio
```

### Run all tests
```powershell
pytest tests/ -v
```

### Run specific test file
```powershell
pytest tests/test_language_validation.py -v
pytest tests/test_validate_route.py -v
```

### Run specific test
```powershell
pytest tests/test_language_validation.py::TestValidateLanguage::test_valid_language_high_confidence -v
```

### Run with coverage
```powershell
pip install pytest-cov
pytest tests/ --cov=app --cov-report=html
```

### Run with short output
```powershell
pytest tests/
```

## Test Structure

- **MockConfidence**: Helper class simulating Lingua's confidence result objects
- **Mock fixtures**: Use `unittest.mock.Mock()` for detector instances
- **mock_app_state**: Fixture that initializes FastAPI app state for endpoint tests
- **TestClient**: FastAPI's test client used for endpoint route testing

## Key Testing Patterns

### Unit Test Pattern
```python
def test_something(self, mock_detector):
    mock_detector.compute_language_confidence_values.return_value = [
        MockConfidence(Language.ITALIAN, 0.95)
    ]
    
    result = validate_language(
        text="Ciao",
        language="it",
        detector=mock_detector,
        threshold=0.8,
    )
    
    assert result is True
```

### Integration Test Pattern
```python
def test_endpoint_something(self, mock_app_state):
    with patch("app.services.language_validation.validate_language") as mock:
        mock.return_value = True
        
        client = TestClient(app)
        response = client.post(
            "/validate",
            json={"text": "Ciao", "languageCode": "it"},
        )
        
        assert response.status_code == 200
        assert response.json()["valid"] is True
```

## Notes

- Tests use mocking for the Lingua detector to avoid runtime dependencies on language models
- The `/validate` endpoint tests patch the `validate_language` function to focus on endpoint logic
- TestClient requires `mock_app_state` fixture to initialize app state (detector + threshold)
- All 27 tests should pass in ~3-4 seconds

## Future Enhancements

- Add end-to-end tests with real Lingua detector (currently mocked)
- Add performance benchmarks for detector initialization
- Add tests for error handling edge cases (detector failures, timeouts)
- Add parametrized tests for multiple language combinations
