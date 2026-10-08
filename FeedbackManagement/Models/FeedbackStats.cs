namespace FeedbackManagement.Models;

public class FeedbackStats
{
    public int Total { get; set; }
    public int Pending { get; set; }
    public int Resolved { get; set; }
    public double AverageRating { get; set; }
}
