using System.ComponentModel.DataAnnotations;

namespace FeedbackManagement.Validation;

/// <summary>Custom validation: rejects text containing blocked words.</summary>
public class NoOffensiveWordsAttribute : ValidationAttribute
{
    private static readonly string[] Blocked = { "stupid", "idiot", "dumb", "moron" };

    public NoOffensiveWordsAttribute() : base("Please avoid offensive language in your feedback.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is not string text || string.IsNullOrWhiteSpace(text))
        {
            return true;
        }

        return !Blocked.Any(word => text.Contains(word, StringComparison.OrdinalIgnoreCase));
    }
}
