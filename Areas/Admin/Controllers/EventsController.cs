using Animatch.Models;
using Animatch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Animatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class EventsController : Controller
    {
        private readonly IEventService eventService;

        public EventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? location, int page = 1, int pageSize = 6)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 3 or > 24 ? 6 : pageSize;

            var (events, totalCount) = await eventService.GetPagedAsync(searchTerm, location, page, pageSize);

            var model = new Animatch.ViewModels.EventIndexQueryViewModel
            {
                SearchTerm = searchTerm,
                Location = location,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Events = events
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ev = await eventService.GetByIdAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            return View(ev);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Event { Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event ev)
        {
            if (!ModelState.IsValid)
            {
                return View(ev);
            }

            ev.CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await eventService.AddAsync(ev);

            TempData["Success"] = "Събитието е създадено успешно.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await eventService.GetByIdAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event ev)
        {
            if (id != ev.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(ev);
            }

            var existing = await eventService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = ev.Title;
            existing.Description = ev.Description;
            existing.Date = ev.Date;
            existing.Location = ev.Location;

            await eventService.UpdateAsync(existing);
            TempData["Success"] = "Събитието е редактирано успешно.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await eventService.GetByIdAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            return View(ev);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await eventService.GetByIdAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            await eventService.DeleteAsync(id);
            TempData["Success"] = "Събитието е изтрито успешно.";
            return RedirectToAction(nameof(Index));
        }
    }
}
