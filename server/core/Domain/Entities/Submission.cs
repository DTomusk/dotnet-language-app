namespace Core.Domain.Entities;

public class Submission
{
    public Guid ID { get; set; }
    public Guid UserID { get; set; } 
    public required string LanguageCode { get; set; }
    public required string Text { get; set; }
}
