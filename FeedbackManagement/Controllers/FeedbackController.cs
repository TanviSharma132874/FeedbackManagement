using FeedbackManagement.Models;
using FeedbackManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackManagement.Controllers;

public class FeedbackController : Controller
{
    private readonly IFeedbackService _service;

    public FeedbackController(IFeedbackService service)
    {
        _service = service;
    }

    // GET: /Feedback
    public IActionResult Index(string? search, string? category, int? rating, string? status)
    {
        var model = new FeedbackListViewModel
        {
            Items = _service.GetAll(search, category, rating, status),
            Stats = _service.GetStats(),
            Search = search,
            Category = category,
            Rating = rating,
            Status = status
        };

        return View(model);
    }

    // GET: /Feedback/Latest  (returns only a partial view, loaded with fetch on the list page)
    public IActionResult Latest()
    {
        return PartialView("_LatestFeedback", _service.GetLatest(3));
    }

    // GET: /Feedback/Details/5
    public IActionResult Details(int id)
    {
        var item = _service.GetById(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // GET: /Feedback/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Feedback { VisitDate = DateTime.Today });
    }

    // POST: /Feedback/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Feedback model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _service.Add(model);
        TempData["Success"] = "Thank you! Your feedback has been submitted.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Feedback/Edit/5
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = _service.GetById(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // POST: /Feedback/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Feedback model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!_service.Update(model))
        {
            return NotFound();
        }

        TempData["Success"] = "Feedback updated successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    // GET: /Feedback/Delete/5
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var item = _service.GetById(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // POST: /Feedback/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!_service.Delete(id))
        {
            return NotFound();
        }

        TempData["Success"] = "Feedback deleted.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Feedback/ToggleResolved/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleResolved(int id)
    {
        if (!_service.ToggleResolved(id))
        {
            return NotFound();
        }

        TempData["Success"] = "Status updated.";
        return RedirectToAction(nameof(Index));
    }
}
