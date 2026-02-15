using Animatch.Data;
using Microsoft.AspNetCore.Mvc;
using Animatch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Animatch.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AnimalManagerDbContext context;

        public AnimalController(AnimalManagerDbContext context)
        {
            this.context = context;
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
            ViewBag.Towns = animals.Select(a => a.Town).Where(t => !string.IsNullOrEmpty(t)).Distinct().OrderBy(t => t).ToList() ?? new List<string>();

            return View(animals);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            var categories = context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", animal.CategoryId);

            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                    existingAnimal.Name = animal.Name;
                    existingAnimal.Species = animal.Species;
                    existingAnimal.Breed = animal.Breed;
                    existingAnimal.Town = animal.Town;
                    existingAnimal.Description = animal.Description;
                    existingAnimal.CategoryId = animal.CategoryId;

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

            return View(animal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var animal = context.Animals.Find(id);
                if (animal != null)
                {
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

            var viewModel = new Animatch.ViewModels.Animal.AnimalDetailsViewModel
            {
                Id = animal.Id,
                Name = animal.Name,
                Species = animal.Species,
                Breed = animal.Breed,
                Town = animal.Town,
                Description = animal.Description,
                CategoryName = animal.Category?.Name ?? "Няма категория"
            };

            return View(viewModel);
        }

        private bool AnimalExists(int id)
        {
            return context.Animals.Any(e => e.Id == id);
        }
    }
}