using Microsoft.AspNetCore.Mvc;
using Animatch.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Animatch.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Animatch.Controllers
{
    public class AnimalController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAnimalService animalService;
        private readonly ICategoryService categoryService;
        private readonly IEventService eventService;

        public AnimalController(IConfiguration configuration, UserManager<IdentityUser> userManager, IAnimalService animalService, ICategoryService categoryService, IEventService eventService)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.animalService = animalService;
            this.categoryService = categoryService;
            this.eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, string? town, int page = 1, int pageSize = 9)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 3 or > 24 ? 9 : pageSize;

            var (animals, totalCount) = await animalService.GetPagedFilteredAsync(searchTerm, categoryId, town, page, pageSize);
            var categories = (await categoryService.GetAllAsync()).OrderBy(c => c.Name).ToList();
            var towns = await animalService.GetDistinctTownsAsync();

            var model = new Animatch.ViewModels.AnimalIndexQueryViewModel
            {
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                Town = town,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Animals = animals,
                Categories = categories,
                Towns = towns
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Map()
        {
            var animals = await animalService.GetWithCoordinatesAsync();

            ViewBag.MapboxKey = configuration["MAPBOX_KEY"];
            return View(animals);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = GetUserId();
            var myAnimals = string.IsNullOrEmpty(userId)
                ? Enumerable.Empty<Animal>()
                : await animalService.GetByOwnerAsync(userId);

            var currentUser = await userManager.GetUserAsync(User);
            var roles = currentUser != null
                ? await userManager.GetRolesAsync(currentUser)
                : new List<string>();
            var roleName = roles.FirstOrDefault() ?? "User";
            var canCreateEvents = roles.Contains("Organizer") || roles.Contains("Administrator");
            var myEvents = (string.IsNullOrEmpty(userId) || !canCreateEvents)
                ? Enumerable.Empty<Event>()
                : await eventService.GetByCreatorAsync(userId);

            var viewModel = new Animatch.ViewModels.ProfileViewModel
            {
                Username = User.Identity?.Name ?? "Потребител",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? "Няма имейл",
                Role = roleName,
                MyAnimals = myAnimals,
                CanCreateEvents = canCreateEvents,
                MyEvents = myEvents
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var categories = await categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Animal animal)
        {
            NormalizeCoordinates(animal);

            var category = await categoryService.GetByIdAsync(animal.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Невалидна категория");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    animal.OwnerId = GetUserId();
                    await animalService.AddAsync(animal);

                    TempData["Success"] = "Животното е добавено успешно!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Грешка при запис: " + ex.Message);
                }
            }

            var categories = await categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", animal.CategoryId);
            return View(animal);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await animalService.GetByIdWithCategoryAsync(id.Value);

            if (animal == null)
            {
                return NotFound();
            }

            if (!IsOwner(animal))
            {
                return Forbid();
            }

            var categories = await categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", animal.CategoryId);

            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            NormalizeCoordinates(animal);

            var category = await categoryService.GetByIdAsync(animal.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Невалидна категория");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAnimal = await animalService.GetByIdWithCategoryAsync(id);
                    if (existingAnimal == null)
                    {
                        return NotFound();
                    }

                    if (!IsOwner(existingAnimal))
                    {
                        return Forbid();
                    }

                    existingAnimal.Name = animal.Name;
                    existingAnimal.Species = animal.Species;
                    existingAnimal.Breed = animal.Breed;
                    existingAnimal.Town = animal.Town;
                    existingAnimal.Description = animal.Description;
                    existingAnimal.CategoryId = animal.CategoryId;
                    existingAnimal.PhoneNumber = animal.PhoneNumber;
                    existingAnimal.ImageUrl = animal.ImageUrl;
                    existingAnimal.Latitude = animal.Latitude;
                    existingAnimal.Longitude = animal.Longitude;

                    await animalService.UpdateAsync(existingAnimal);

                    TempData["Success"] = "Животното е редактирано успешно!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AnimalExists(animal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Грешка при запис: " + ex.Message);
                }
            }

            var categories = await categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", animal.CategoryId);
            return View(animal);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await animalService.GetByIdWithCategoryAsync(id.Value);

            if (animal == null)
            {
                return NotFound();
            }

            if (!IsOwner(animal))
            {
                return Forbid();
            }

            return View(animal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var animal = await animalService.GetByIdWithCategoryAsync(id);
                if (animal != null)
                {
                    if (!IsOwner(animal))
                    {
                        return Forbid();
                    }

                    await animalService.DeleteAsync(id);
                    TempData["Success"] = "Животното е изтрито успешно!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Грешка при изтриване: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await animalService.GetByIdWithCategoryAsync(id.Value);

            if (animal == null)
            {
                return NotFound();
            }

            var viewModel = new Animatch.ViewModels.AnimalDetailsViewModel
            {
                Id = animal.Id,
                Name = animal.Name,
                Species = animal.Species,
                Breed = animal.Breed,
                Town = animal.Town,
                Description = animal.Description,
                PhoneNumber = animal.PhoneNumber,
                ImageUrl = animal.ImageUrl,
                OwnerId = animal.OwnerId,
                CategoryName = animal.Category?.Name ?? "Няма категория",
                Latitude = animal.Latitude,
                Longitude = animal.Longitude
            };

            return View(viewModel);
        }

        private async Task<bool> AnimalExists(int id)
        {
            return await animalService.ExistsAsync(id);
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private bool IsOwner(Animal animal)
        {
            if (User.IsInRole("Administrator"))
            {
                return true;
            }

            var userId = GetUserId();
            return !string.IsNullOrEmpty(userId) && animal.OwnerId == userId;
        }

        private void NormalizeCoordinates(Animal animal)
        {
            var latitudeRaw = Request.Form["Latitude"].ToString();
            var longitudeRaw = Request.Form["Longitude"].ToString();
            ModelState.Remove(nameof(Animal.Latitude));
            ModelState.Remove(nameof(Animal.Longitude));

            if (TryParsePair(latitudeRaw, out var pairLat, out var pairLng) && string.IsNullOrWhiteSpace(longitudeRaw))
            {
                animal.Latitude = pairLat;
                animal.Longitude = pairLng;
                ValidateCoordinateRange(animal.Latitude, animal.Longitude);
                return;
            }

            if (!TryParseSingle(latitudeRaw, out var latitude))
            {
                ModelState.AddModelError(nameof(Animal.Latitude), "Невалидна географска ширина.");
            }

            if (!TryParseSingle(longitudeRaw, out var longitude))
            {
                ModelState.AddModelError(nameof(Animal.Longitude), "Невалидна географска дължина.");
            }

            animal.Latitude = latitude;
            animal.Longitude = longitude;
            ValidateCoordinateRange(animal.Latitude, animal.Longitude);
        }

        private static bool TryParsePair(string? input, out double? latitude, out double? longitude)
        {
            latitude = null;
            longitude = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var compact = input.Trim();
            if (!compact.Contains(", ") && !compact.Contains(";"))
            {
                return false;
            }

            var parts = compact.Split(new[] { ",", ";" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return false;
            }

            if (!TryParseSingle(parts[0], out latitude) || !TryParseSingle(parts[1], out longitude))
            {
                return false;
            }

            return latitude.HasValue && longitude.HasValue;
        }

        private static bool TryParseSingle(string? input, out double? value)
        {
            value = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                return true;
            }

            var normalized = input.Trim().Replace(',', '.');

            if (double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
            {
                value = parsed;
                return true;
            }

            if (double.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out parsed))
            {
                value = parsed;
                return true;
            }

            return false;
        }

        private void ValidateCoordinateRange(double? latitude, double? longitude)
        {
            if (latitude.HasValue && (latitude < -90 || latitude > 90))
            {
                ModelState.AddModelError(nameof(Animal.Latitude), "Географската ширина трябва да е между -90 и 90.");
            }

            if (longitude.HasValue && (longitude < -180 || longitude > 180))
            {
                ModelState.AddModelError(nameof(Animal.Longitude), "Географската дължина трябва да е между -180 и 180.");
            }
        }
    }
}
