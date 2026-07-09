using Domain.LanguagePractice.ValueObjects;

namespace Domain.LanguagePractice.Entities;

public class Submission
{
    public Guid ID { get; set; }
    public Guid UserID { get; set; } 
    public required LanguageCode LanguageCode { get; set; }
    public required string Text { get; set; }
}
