using Animatch.Data;
using Microsoft.AspNetCore.Mvc;
using Animatch.Models;
using Microsoft.EntityFrameworkCore;
using Animatch.ViewModels;

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


			return View(animals);
		}
	}
}
