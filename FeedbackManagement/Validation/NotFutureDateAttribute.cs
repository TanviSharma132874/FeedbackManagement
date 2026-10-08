using System.ComponentModel.DataAnnotations;

namespace FeedbackManagement.Validation;

/// <summary>Custom validation: the date must not be later than today.</summary>
public class NotFutureDateAttribute : ValidationAttribute
{
    public NotFutureDateAttribute() : base("Date of experience cannot be in the future.")
    {
    }

    public override bool IsValid(object? value)
    {
        // Null is handled by [Required]
        return value is not DateTime date || date.Date <= DateTime.Today;
    }
}
