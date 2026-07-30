import pytest
from unittest.mock import Mock, patch, MagicMock
from fastapi.testclient import TestClient
from lingua import Language

from app.main import app
from app.schemas.validate import ValidateRequest, ValidateResponse


@pytest.fixture
def client():
    """Create a FastAPI test client."""
    return TestClient(app)


@pytest.fixture
def mock_app_state(client):
    """Mock app state for testing."""
    client.app.state.language_detector = Mock()
    client.app.state.language_detector_threshold = 0.8
    return client.app.state


class MockConfidence:
    """Mock object to represent a Lingua confidence result."""

    def __init__(self, language: Language, value: float):
        self.language = language
        self.value = value


class TestValidateRoute:
    """Integration tests for the /validate endpoint."""

    def test_validate_endpoint_valid_language(self, mock_app_state):
        """Test /validate endpoint with text in correct language and high confidence."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            mock_validate.return_value = True

            client = TestClient(app)
            response = client.post(
                "/validate",
                json={"text": "Ciao, come stai?", "languageCode": "it"},
            )

            assert response.status_code == 200
            data = response.json()
            assert data["valid"] is True

    def test_validate_endpoint_invalid_language(self, mock_app_state):
        """Test /validate endpoint with text in different language."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            mock_validate.return_value = False

            client = TestClient(app)
            response = client.post(
                "/validate",
                json={"text": "Guten Tag", "languageCode": "it"},
            )

            assert response.status_code == 200
            data = response.json()
            assert data["valid"] is False

    def test_validate_endpoint_low_confidence(self, mock_app_state):
        """Test /validate endpoint when confidence is too low."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            mock_validate.return_value = False

            client = TestClient(app)
            response = client.post(
                "/validate", json={"text": "a", "languageCode": "it"}
            )

            assert response.status_code == 200
            data = response.json()
            assert data["valid"] is False

    def test_validate_endpoint_unsupported_language(self, mock_app_state):
        """Test /validate endpoint with unsupported language code."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            from app.services.language_validation import (
                UnsupportedLanguageForDetectionError,
            )

            mock_validate.side_effect = UnsupportedLanguageForDetectionError(
                "Unsupported language code: xx"
            )

            client = TestClient(app)
            response = client.post(
                "/validate", json={"text": "Some text", "languageCode": "xx"}
            )

            assert response.status_code == 200
            data = response.json()
            assert data["valid"] is False

    def test_validate_endpoint_missing_text(self, mock_app_state):
        """Test /validate endpoint with missing text field."""
        client = TestClient(app)
        response = client.post(
            "/validate", json={"languageCode": "it"}
        )

        assert response.status_code == 422  # Validation error

    def test_validate_endpoint_missing_language_code(self, mock_app_state):
        """Test /validate endpoint with missing languageCode field."""
        client = TestClient(app)
        response = client.post(
            "/validate", json={"text": "Ciao"}
        )

        assert response.status_code == 422  # Validation error

    def test_validate_endpoint_empty_text(self, mock_app_state):
        """Test /validate endpoint with empty text."""
        client = TestClient(app)
        response = client.post(
            "/validate",
            json={"text": "", "languageCode": "it"},
        )

        assert response.status_code == 422  # Schema validation should reject

    def test_validate_endpoint_empty_language_code(self, mock_app_state):
        """Test /validate endpoint with empty language code."""
        client = TestClient(app)
        response = client.post(
            "/validate",
            json={"text": "Ciao", "languageCode": ""},
        )

        assert response.status_code == 422  # Schema validation should reject

    def test_validate_endpoint_whitespace_only_text(self, mock_app_state):
        """Test /validate endpoint with whitespace-only text."""
        client = TestClient(app)
        response = client.post(
            "/validate",
            json={"text": "   ", "languageCode": "it"},
        )

        assert response.status_code == 422  # Schema validation should reject

    def test_validate_endpoint_language_code_normalized(self, mock_app_state):
        """Test that language code is normalized to lowercase."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            mock_validate.return_value = True

            client = TestClient(app)
            response = client.post(
                "/validate",
                json={"text": "Ciao", "languageCode": "IT"},
            )

            assert response.status_code == 200
            # Verify that validate_language was called with lowercase
            called_language = mock_validate.call_args.kwargs["language"]
            assert called_language == "it"

    def test_validate_endpoint_text_normalized(self, mock_app_state):
        """Test that text is normalized (whitespace trimmed)."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            mock_validate.return_value = True

            client = TestClient(app)
            response = client.post(
                "/validate",
                json={"text": "  Ciao  ", "languageCode": "it"},
            )

            assert response.status_code == 200
            # Verify that validate_language was called with trimmed text
            called_text = mock_validate.call_args.kwargs["text"]
            assert called_text == "Ciao"

    def test_validate_endpoint_passes_detector_from_app_state(self, mock_app_state):
        """Test that detector from app state is passed to validator."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            mock_validate.return_value = True

            client = TestClient(app)
            response = client.post(
                "/validate", json={"text": "Ciao", "languageCode": "it"}
            )

            assert response.status_code == 200
            # Verify detector was passed
            mock_validate.assert_called_once()
            assert "detector" in mock_validate.call_args.kwargs

    def test_validate_endpoint_passes_threshold_from_app_state(self, mock_app_state):
        """Test that threshold from app state is passed to validator."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            mock_validate.return_value = True

            client = TestClient(app)
            response = client.post(
                "/validate", json={"text": "Ciao", "languageCode": "it"}
            )

            assert response.status_code == 200
            # Verify threshold was passed
            mock_validate.assert_called_once()
            assert "threshold" in mock_validate.call_args.kwargs
            # Default threshold should be 0.8
            assert mock_validate.call_args.kwargs["threshold"] == 0.8

    def test_validate_response_schema(self, mock_app_state):
        """Test that response conforms to ValidateResponse schema."""
        with patch(
            "app.services.language_validation.validate_language"
        ) as mock_validate:
            mock_validate.return_value = True

            client = TestClient(app)
            response = client.post(
                "/validate", json={"text": "Ciao", "languageCode": "it"}
            )

            assert response.status_code == 200
            # Verify response can be parsed as ValidateResponse
            validate_response = ValidateResponse(**response.json())
            assert validate_response.valid is True
