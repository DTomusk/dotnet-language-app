import pytest
from unittest.mock import Mock, MagicMock
from lingua import Language

from app.services.language_validation import (
    validate_language,
    UnsupportedLanguageForDetectionError,
    LINGUA_LANGUAGE_MAP,
)


class MockConfidence:
    """Mock object to represent a Lingua confidence result."""

    def __init__(self, language: Language, value: float):
        self.language = language
        self.value = value


@pytest.fixture
def mock_detector():
    """Create a mock LanguageDetector."""
    return Mock()


class TestValidateLanguage:
    """Unit tests for validate_language function."""

    def test_valid_language_high_confidence(self, mock_detector):
        """Test text that matches language with high confidence."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.ITALIAN, 0.95)
        ]

        result = validate_language(
            text="Ciao, come stai?",
            language="it",
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is True
        mock_detector.compute_language_confidence_values.assert_called_once_with(
            "Ciao, come stai?"
        )

    def test_valid_language_at_threshold(self, mock_detector):
        """Test text with confidence exactly at threshold."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.ITALIAN, 0.8)
        ]

        result = validate_language(
            text="Ciao",
            language="it",
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is True

    def test_valid_language_below_threshold(self, mock_detector):
        """Test text that matches language but confidence is below threshold."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.ITALIAN, 0.75)
        ]

        result = validate_language(
            text="a",
            language="it",
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is False

    def test_wrong_language_detected(self, mock_detector):
        """Test text detected as different language."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.GERMAN, 0.92)
        ]

        result = validate_language(
            text="Guten Tag",
            language="it",
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is False

    def test_empty_text(self, mock_detector):
        """Test with empty or whitespace-only text."""
        result = validate_language(
            text="   ",
            language="it",
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is False
        # Detector should never be called for empty text
        mock_detector.compute_language_confidence_values.assert_not_called()

    def test_empty_confidences(self, mock_detector):
        """Test when detector returns empty confidence list."""
        mock_detector.compute_language_confidence_values.return_value = []

        result = validate_language(
            text="Some text",
            language="it",
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is False

    def test_unsupported_language_code(self, mock_detector):
        """Test with unsupported language code."""
        with pytest.raises(UnsupportedLanguageForDetectionError) as exc_info:
            validate_language(
                text="Some text",
                language="xx",  # Invalid code
                detector=mock_detector,
                threshold=0.8,
            )

        assert "Unsupported language code" in str(exc_info.value)
        # Detector should not be called
        mock_detector.compute_language_confidence_values.assert_not_called()

    def test_language_code_case_insensitive(self, mock_detector):
        """Test that language codes are case-insensitive."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.ITALIAN, 0.9)
        ]

        result = validate_language(
            text="Ciao",
            language="IT",  # Uppercase
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is True

    def test_text_whitespace_trimmed(self, mock_detector):
        """Test that text whitespace is trimmed before detection."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.GERMAN, 0.9)
        ]

        validate_language(
            text="  Guten Tag  ",
            language="de",
            detector=mock_detector,
            threshold=0.8,
        )

        # Verify the detector was called with trimmed text
        mock_detector.compute_language_confidence_values.assert_called_once_with(
            "Guten Tag"
        )

    def test_default_threshold(self, mock_detector):
        """Test that default threshold is 0.8."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.ITALIAN, 0.8)
        ]

        # Call without specifying threshold
        result = validate_language(
            text="Ciao",
            language="it",
            detector=mock_detector,
        )

        assert result is True

    def test_custom_language_map(self, mock_detector):
        """Test with custom language map."""
        custom_map = {"fr": Language.FRENCH}

        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.FRENCH, 0.9)
        ]

        result = validate_language(
            text="Bonjour",
            language="fr",
            detector=mock_detector,
            threshold=0.8,
            language_map=custom_map,
        )

        assert result is True

    def test_multiple_confidence_scores_uses_top(self, mock_detector):
        """Test that only the top confidence score is used."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.ITALIAN, 0.85),  # Top
            MockConfidence(Language.GERMAN, 0.10),
            MockConfidence(Language.ENGLISH, 0.05),
        ]

        result = validate_language(
            text="Ciao",
            language="it",
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is True

    def test_top_is_correct_language_but_low_confidence(self, mock_detector):
        """Test when top language matches but confidence is too low."""
        mock_detector.compute_language_confidence_values.return_value = [
            MockConfidence(Language.ITALIAN, 0.7),  # Below threshold
            MockConfidence(Language.GERMAN, 0.65),
        ]

        result = validate_language(
            text="a",
            language="it",
            detector=mock_detector,
            threshold=0.8,
        )

        assert result is False
