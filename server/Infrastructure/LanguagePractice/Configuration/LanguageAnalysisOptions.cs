namespace Infrastructure.LanguagePractice.Configuration;

public class LanguageAnalysisApiOptions
{
    public const string SectionName = "ExternalApis:LanguageAnalysis";

    public string BaseUrl { get; set; } = string.Empty;
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
}
