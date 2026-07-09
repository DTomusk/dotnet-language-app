using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.LanguagePractice.ValueObjects;

public sealed record LanguageCode
{
    public string Value { get; }

    private LanguageCode(string value) => Value = value;
    
    public static LanguageCode Italian => new("it");

    public static LanguageCode From(string code)
    {
        return code.ToLowerInvariant() switch
        {
            "it" => Italian,
            _ => throw new ArgumentException($"Invalid language code: {code}")
        };
    }

    public override string ToString() => Value;
}
