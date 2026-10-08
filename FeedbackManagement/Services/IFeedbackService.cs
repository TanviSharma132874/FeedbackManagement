using FeedbackManagement.Models;

namespace FeedbackManagement.Services;

public interface IFeedbackService
{
    IReadOnlyList<Feedback> GetAll(string? search, string? category, int? rating, string? status);
    IReadOnlyList<Feedback> GetLatest(int count);
    Feedback? GetById(int id);
    void Add(Feedback feedback);
    bool Update(Feedback feedback);
    bool Delete(int id);
    bool ToggleResolved(int id);
    FeedbackStats GetStats();
}
