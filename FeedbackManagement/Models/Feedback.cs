using System.ComponentModel.DataAnnotations;
using FeedbackManagement.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FeedbackManagement.Models;

public class Feedback
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 60 characters.")]
    [RegularExpression(@"^[A-Za-z][A-Za-z\s.'-]*$", ErrorMessage = "Name can contain only letters, spaces, dots, apostrophes and hyphens.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
    [Display(Name = "Mobile Number (optional)")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a rating.")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    [Display(Name = "Rating")]
    public int? Rating { get; set; }

    [Required(ErrorMessage = "Date of experience is required.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
    [NotFutureDate]
    [Display(Name = "Date of Experience")]
    public DateTime? VisitDate { get; set; }

    [Required(ErrorMessage = "Subject is required.")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Subject must be between 5 and 100 characters.")]
    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please write your feedback.")]
    [StringLength(1000, MinimumLength = 20, ErrorMessage = "Feedback must be between 20 and 1000 characters.")]
    [NoOffensiveWords]
    [Display(Name = "Your Feedback")]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "Contact me about this feedback")]
    public bool ContactMe { get; set; }

    // Set by the application, never by the posted form.
    [BindNever]
    [Display(Name = "Submitted On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy, hh:mm tt}")]
    public DateTime SubmittedOn { get; set; }

    [BindNever]
    [Display(Name = "Resolved")]
    public bool IsResolved { get; set; }
}
