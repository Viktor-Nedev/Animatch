using System.Diagnostics;
using Animatch.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Animatch.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		[Route("Home/Error/{statusCode:int?}")]
		public IActionResult Error(int? statusCode = 500)
		{
			var resolvedStatusCode = statusCode ?? 500;
			Response.StatusCode = resolvedStatusCode;

			if (resolvedStatusCode == 404)
			{
				return View("~/Views/Shared/404.cshtml");
			}

			if (resolvedStatusCode == 500)
			{
				return View("~/Views/Shared/500.cshtml");
			}

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
