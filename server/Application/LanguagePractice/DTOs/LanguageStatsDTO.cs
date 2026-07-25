using System;
using System.Collections.Generic;
using System.Text;

namespace Application.LanguagePractice.DTOs;

public record LanguageStatsDTO(string DisplayName, int UniqueLemmas, DateTime StartedAt);
