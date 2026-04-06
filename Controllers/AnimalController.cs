using Microsoft.AspNetCore.Mvc;
using Animatch.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Animatch.Services;
using Microsoft.EntityFrameworkCore;

namespace Animatch.Controllers
{
    public class AnimalController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAnimalService animalService;
        private readonly ICategoryService categoryService;

        public AnimalController(IConfiguration configuration, UserManager<IdentityUser> userManager, IAnimalService animalService, ICategoryService categoryService)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.animalService = animalService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var animals = (await animalService.GetAllWithCategoryAsync()).OrderBy(a => a.Name).ToList();

            ViewBag.Categories = await categoryService.GetAllAsync();
            ViewBag.Towns = await animalService.GetDistinctTownsAsync();

            return View(animals);
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

            var viewModel = new Animatch.ViewModels.ProfileViewModel
            {
                Username = User.Identity?.Name ?? "Потребител",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? "Няма имейл",
                Role = roleName,
                MyAnimals = myAnimals
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
            var userId = GetUserId();
            return !string.IsNullOrEmpty(userId) && animal.OwnerId == userId;
        }
    }
}
