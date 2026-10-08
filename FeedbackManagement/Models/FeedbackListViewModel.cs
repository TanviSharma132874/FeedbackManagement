namespace FeedbackManagement.Models;

public class FeedbackListViewModel
{
    public IReadOnlyList<Feedback> Items { get; set; } = new List<Feedback>();
    public FeedbackStats Stats { get; set; } = new FeedbackStats();

    // Filter values
    public string? Search { get; set; }
    public string? Category { get; set; }
    public int? Rating { get; set; }
    public string? Status { get; set; }
}
