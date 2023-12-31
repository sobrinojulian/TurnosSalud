using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TurnosSalud.ModelsView;

namespace TurnosSalud.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			if (User.Identity?.IsAuthenticated == false)
			{
				return RedirectToAction("Login", "Usuarios");
			}
			return RedirectToAction("Index", "Usuarios");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new Error { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
