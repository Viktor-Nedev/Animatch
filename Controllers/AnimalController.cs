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
           
            if (string.IsNullOrEmpty(animal.Breed))
            {
                ModelState.Remove("Breed");
            }

            if (string.IsNullOrEmpty(animal.Town))
            {
                ModelState.Remove("Town");
            }

        
            var categoryExists = context.Categories.Any(c => c.Id == animal.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Невалидна категория");
            }

            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                try
                {
                 
                    animal.Category = null;

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
    }
}