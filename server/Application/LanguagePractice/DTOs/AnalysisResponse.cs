using Domain.LanguagePractice.Entities;

namespace Application.LanguagePractice.DTOs;

public record AnalysisResponse(IEnumerable<string> Lemmas);