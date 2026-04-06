using Animatch.Models;
using Animatch.Services;
using Animatch.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Animatch.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly IWalkRequestService walkRequestService;
        private readonly IAnimalService animalService;

        public RequestsController(IWalkRequestService walkRequestService, IAnimalService animalService)
        {
            this.walkRequestService = walkRequestService;
            this.animalService = animalService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int animalId)
        {
            var animal = await animalService.GetByIdWithCategoryAsync(animalId);
            if (animal == null)
            {
                return NotFound();
            }

            var model = new WalkRequestCreateViewModel
            {
                AnimalId = animal.Id,
                AnimalName = animal.Name,
                AnimalTown = animal.Town ?? "Не е посочено",
                AnimalCategory = animal.Category?.Name ?? "Без категория"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WalkRequestCreateViewModel model)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var animal = await animalService.GetByIdWithCategoryAsync(model.AnimalId);
            if (animal == null)
            {
                return NotFound();
            }

            model.AnimalName = animal.Name;
            model.AnimalTown = animal.Town ?? "Не е посочено";
            model.AnimalCategory = animal.Category?.Name ?? "Без категория";

            if (animal.OwnerId == userId)
            {
                ModelState.AddModelError(string.Empty, "Не можете да изпратите заявка за собственото си животно.");
            }

            var hasPending = await walkRequestService.HasPendingAsync(animal.Id, userId);
            if (hasPending)
            {
                ModelState.AddModelError(string.Empty, "Вече имате чакаща заявка за това животно.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new WalkRequest
            {
                AnimalId = animal.Id,
                RequesterId = userId,
                Message = model.Message,
                RequestedOn = DateTime.UtcNow,
                Status = RequestStatus.Pending
            };

            await walkRequestService.AddAsync(request);

            TempData["Success"] = "Заявката е изпратена успешно.";
            return RedirectToAction(nameof(MyRequests));
        }

        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var isAdmin = User.IsInRole("Administrator");
            var created = await walkRequestService.GetByRequesterAsync(userId);
            var incoming = isAdmin
                ? await walkRequestService.GetAllAsync()
                : await walkRequestService.GetIncomingForOwnerAsync(userId);

            var model = new MyWalkRequestsViewModel
            {
                CreatedRequests = created,
                IncomingRequests = incoming,
                IsAdmin = isAdmin
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var request = await walkRequestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            if (!CanView(request))
            {
                return Forbid();
            }

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string? returnAction = null)
        {
            var request = await walkRequestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            if (!CanManage(request))
            {
                return Forbid();
            }

            request.Status = RequestStatus.Approved;
            await walkRequestService.UpdateAsync(request);
            TempData["Success"] = "Заявката е одобрена.";

            if (returnAction == nameof(MyRequests))
            {
                return RedirectToAction(nameof(MyRequests));
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string? returnAction = null)
        {
            var request = await walkRequestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            if (!CanManage(request))
            {
                return Forbid();
            }

            request.Status = RequestStatus.Rejected;
            await walkRequestService.UpdateAsync(request);
            TempData["Success"] = "Заявката е отхвърлена.";

            if (returnAction == nameof(MyRequests))
            {
                return RedirectToAction(nameof(MyRequests));
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private bool CanView(WalkRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            if (User.IsInRole("Administrator"))
            {
                return true;
            }

            return request.RequesterId == userId || request.Animal.OwnerId == userId;
        }

        private bool CanManage(WalkRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            if (User.IsInRole("Administrator"))
            {
                return true;
            }

            return request.Animal.OwnerId == userId;
        }
    }
}
