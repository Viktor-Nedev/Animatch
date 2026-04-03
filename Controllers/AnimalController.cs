using Animatch.Data;
using Microsoft.AspNetCore.Mvc;
using Animatch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Animatch.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AnimalManagerDbContext context;
        private readonly IConfiguration configuration;

        public AnimalController(AnimalManagerDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Animal> animals = context
                .Animals
                .Include(animal => animal.Category)
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .ToList();

            ViewBag.Categories = context.Categories.ToList() ?? new List<Category>();
            ViewBag.Towns = animals
                .Select(a => a.Town)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t!)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            return View(animals);
        }


        [HttpGet]
        public IActionResult Map()
        {
            var animals = context.Animals
                .Where(a => a.Latitude.HasValue && a.Longitude.HasValue)
                .ToList();

            ViewBag.MapboxKey = configuration["MAPBOX_KEY"];
            return View(animals);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            var userId = GetUserId();
            var myAnimals = context.Animals
                .Where(a => a.OwnerId == userId)
                .Include(a => a.Category)
                .ToList();

            var viewModel = new Animatch.ViewModels.ProfileViewModel
            {
                Username = User.Identity?.Name ?? "Потребител",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? "Няма имейл",
                MyAnimals = myAnimals
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var categories = context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Animal animal)
        {
            var categoryExists = context.Categories.Any(c => c.Id == animal.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Невалидна категория");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    animal.OwnerId = GetUserId();
                    context.Animals.Add(animal);
                    int result = context.SaveChanges();

                    if (result > 0)
                    {
                        TempData["Success"] = "Животното е добавено успешно!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Грешка при запис: " + ex.Message);
                }
            }

            var categories = context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", animal.CategoryId);
            return View(animal);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = context.Animals
                .Include(a => a.Category)
                .FirstOrDefault(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            if (!IsOwner(animal))
            {
                return Forbid();
            }

            var categories = context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", animal.CategoryId);

            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            var categoryExists = context.Categories.Any(c => c.Id == animal.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Невалидна категория");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAnimal = context.Animals.Find(id);
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

                    context.Update(existingAnimal);
                    int result = context.SaveChanges();

                    if (result > 0)
                    {
                        TempData["Success"] = "Животното е редактирано успешно!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
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

            var categories = context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", animal.CategoryId);
            return View(animal);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = context.Animals
                .Include(a => a.Category)
                .FirstOrDefault(a => a.Id == id);

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
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var animal = context.Animals.Find(id);
                if (animal != null)
                {
                    if (!IsOwner(animal))
                    {
                        return Forbid();
                    }

                    context.Animals.Remove(animal);
                    int result = context.SaveChanges();

                    if (result > 0)
                    {
                        TempData["Success"] = "Животното е изтрито успешно!";
                    }
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
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = context.Animals
                .Include(a => a.Category)
                .FirstOrDefault(a => a.Id == id);

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
                CategoryName = animal.Category?.Name ?? "Няма категория",
                Latitude = animal.Latitude,
                Longitude = animal.Longitude
            };

            return View(viewModel);
        }

        private bool AnimalExists(int id)
        {
            return context.Animals.Any(e => e.Id == id);
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
