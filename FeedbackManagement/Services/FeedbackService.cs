using FeedbackManagement.Models;

namespace FeedbackManagement.Services;

/// <summary>Simple thread-safe in-memory store (data resets when the app restarts).</summary>
public class FeedbackService : IFeedbackService
{
    private readonly List<Feedback> _items = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public FeedbackService()
    {
        Add(new Feedback
        {
            FullName = "Aarav Mehta", Email = "aarav.mehta@example.com", Phone = "9876543210",
            Category = "Product Quality", Rating = 5, VisitDate = DateTime.Today.AddDays(-9),
            Subject = "Excellent build quality",
            Message = "The product exceeded my expectations. Packaging was neat and everything works perfectly.",
            ContactMe = false, SubmittedOn = DateTime.Now.AddDays(-8), IsResolved = true
        });
        Add(new Feedback
        {
            FullName = "Priya Sharma", Email = "priya.sharma@example.com",
            Category = "Customer Service", Rating = 3, VisitDate = DateTime.Today.AddDays(-6),
            Subject = "Support took too long to reply",
            Message = "I raised a ticket about my order and it took three days to get a response from the team.",
            ContactMe = true, SubmittedOn = DateTime.Now.AddDays(-5)
        });
        Add(new Feedback
        {
            FullName = "Rohan Patel", Email = "rohan.patel@example.com", Phone = "9123456780",
            Category = "Delivery", Rating = 2, VisitDate = DateTime.Today.AddDays(-4),
            Subject = "Parcel arrived damaged",
            Message = "The outer box was torn and one item was broken. Please improve the packaging for fragile goods.",
            ContactMe = true, SubmittedOn = DateTime.Now.AddDays(-3)
        });
        Add(new Feedback
        {
            FullName = "Neha Joshi", Email = "neha.joshi@example.com",
            Category = "Website / App", Rating = 4, VisitDate = DateTime.Today.AddDays(-2),
            Subject = "Easy to use website",
            Message = "Checkout was quick and the search filters are helpful. A dark mode would be a nice addition.",
            ContactMe = false, SubmittedOn = DateTime.Now.AddDays(-1)
        });
        Add(new Feedback
        {
            FullName = "Karan Singh", Email = "karan.singh@example.com",
            Category = "Pricing", Rating = 3, VisitDate = DateTime.Today.AddDays(-1),
            Subject = "Prices slightly high",
            Message = "Quality is good but similar products are available for less. Some discounts would help.",
            ContactMe = false, SubmittedOn = DateTime.Now.AddHours(-6)
        });
    }

    public IReadOnlyList<Feedback> GetAll(string? search, string? category, int? rating, string? status)
    {
        lock (_lock)
        {
            IEnumerable<Feedback> query = _items;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var text = search.Trim();
                query = query.Where(f =>
                    f.FullName.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                    f.Subject.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                    f.Message.Contains(text, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(f => f.Category == category);
            }

            if (rating.HasValue)
            {
                query = query.Where(f => f.Rating == rating.Value);
            }

            if (status == "Resolved")
            {
                query = query.Where(f => f.IsResolved);
            }
            else if (status == "Pending")
            {
                query = query.Where(f => !f.IsResolved);
            }

            return query.OrderByDescending(f => f.SubmittedOn).ToList();
        }
    }

    public IReadOnlyList<Feedback> GetLatest(int count)
    {
        lock (_lock)
        {
            return _items.OrderByDescending(f => f.SubmittedOn).Take(count).ToList();
        }
    }

    public Feedback? GetById(int id)
    {
        lock (_lock)
        {
            return _items.FirstOrDefault(f => f.Id == id);
        }
    }

    public void Add(Feedback feedback)
    {
        lock (_lock)
        {
            feedback.Id = _nextId++;
            if (feedback.SubmittedOn == default)
            {
                feedback.SubmittedOn = DateTime.Now;
            }
            _items.Add(feedback);
        }
    }

    public bool Update(Feedback feedback)
    {
        lock (_lock)
        {
            var existing = _items.FirstOrDefault(f => f.Id == feedback.Id);
            if (existing == null)
            {
                return false;
            }

            existing.FullName = feedback.FullName;
            existing.Email = feedback.Email;
            existing.Phone = feedback.Phone;
            existing.Category = feedback.Category;
            existing.Rating = feedback.Rating;
            existing.VisitDate = feedback.VisitDate;
            existing.Subject = feedback.Subject;
            existing.Message = feedback.Message;
            existing.ContactMe = feedback.ContactMe;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var existing = _items.FirstOrDefault(f => f.Id == id);
            return existing != null && _items.Remove(existing);
        }
    }

    public bool ToggleResolved(int id)
    {
        lock (_lock)
        {
            var existing = _items.FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                return false;
            }

            existing.IsResolved = !existing.IsResolved;
            return true;
        }
    }

    public FeedbackStats GetStats()
    {
        lock (_lock)
        {
            return new FeedbackStats
            {
                Total = _items.Count,
                Resolved = _items.Count(f => f.IsResolved),
                Pending = _items.Count(f => !f.IsResolved),
                AverageRating = _items.Count == 0 ? 0 : Math.Round(_items.Average(f => f.Rating ?? 0), 1)
            };
        }
    }
}
