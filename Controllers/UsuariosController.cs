using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnosSalud.Data;
using TurnosSalud.Models;

namespace TurnosSalud.Controllers
{
	public class UsuariosController : Controller
	{
		private readonly TurnosContext _context;

		public UsuariosController(TurnosContext context)
		{
			_context = context;
		}

		[Authorize]
		public IActionResult Index()
		{
			return RedirectToAction("Index", "Turnos");
		}

		[AllowAnonymous]
		public IActionResult Login()
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Usuarios");
			}
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Login(Usuario _usuario)
		{
			var usuario = _context.Usuario.FirstOrDefault(u => u.Email == _usuario.Email && u.Password == _usuario.Password);

			if (usuario != null)
			{
				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
				identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
				identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
				identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol.ToString()));
				identity.AddClaim(new Claim("Password", usuario.Password));
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()));

				var principal = new ClaimsPrincipal(identity);

				HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
				return RedirectToAction("Index", "Usuarios");
			}
			ViewBag.Mensaje = "Usuario incorrecto...";
			return View();
		}

		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Usuarios");
		}

		[AllowAnonymous]
		public IActionResult Registrar()
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Usuarios");
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public async Task<IActionResult> Registrar([Bind("UsuarioId,Nombre,Email,Password,Rol")] Usuario usuario)
		{
			if (_context.Usuario.Any(u => u.Email == usuario.Email))
				ModelState.AddModelError("Email", "Ya existe un usuario registrado con ese Email");

			if (ModelState.IsValid)
			{
				_context.Add(usuario);
				await _context.SaveChangesAsync();
				return RedirectToAction("Login", "Usuarios");
			}
			return View(usuario);
		}
	}
}
