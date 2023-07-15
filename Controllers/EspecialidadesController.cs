using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurnosSalud.Data;
using TurnosSalud.Models;

namespace TurnosSalud.Controllers
{
	[Authorize]
	public class EspecialidadesController : Controller
	{
		private readonly TurnosContext _context;

		public EspecialidadesController(TurnosContext context)
		{
			_context = context;
		}

		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Especialidad.ToListAsync());
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Especialidad == null)
			{
				return NotFound();
			}

			var especialidad = await _context.Especialidad
				.FirstOrDefaultAsync(m => m.EspecialidadId == id);
			if (especialidad == null)
			{
				return NotFound();
			}

			return View(especialidad);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("EspecialidadId,Nombre")] Especialidad especialidad)
		{
			if (_context.Especialidad.Any(e => e.Nombre == especialidad.Nombre))
				ModelState.AddModelError("Nombre", "Ya existe una especialidad con ese nombre");

			if (ModelState.IsValid)
			{
				_context.Add(especialidad);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(especialidad);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Especialidad == null)
			{
				return NotFound();
			}

			var especialidad = await _context.Especialidad.FindAsync(id);
			if (especialidad == null)
			{
				return NotFound();
			}
			return View(especialidad);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("EspecialidadId,Nombre")] Especialidad especialidad)
		{
			if (_context.Especialidad.Any(e => e.Nombre == especialidad.Nombre))
				ModelState.AddModelError("Nombre", "Ya existe una especialidad con ese nombre");

			if (id != especialidad.EspecialidadId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(especialidad);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EspecialidadExists(especialidad.EspecialidadId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(especialidad);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Especialidad == null)
			{
				return NotFound();
			}

			var especialidad = await _context.Especialidad
				.FirstOrDefaultAsync(m => m.EspecialidadId == id);
			if (especialidad == null)
			{
				return NotFound();
			}

			return View(especialidad);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Especialidad == null)
			{
				return Problem("Entity set 'DbContext.Especialidad'  is null.");
			}
			var especialidad = await _context.Especialidad.FindAsync(id);
			if (especialidad != null)
			{
				_context.Especialidad.Remove(especialidad);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> Seed()
		{
			var existingEspecialidades = await _context.Especialidad.ToListAsync();
			var newEspecialidades = new List<Especialidad>
			{
				new Especialidad { Nombre = "Pediatria" },
				new Especialidad { Nombre = "Cardiologia" },
				new Especialidad { Nombre = "Dermatologia" },
				new Especialidad { Nombre = "Gastroenterologia" },
				new Especialidad { Nombre = "Oftalmologia" },
				new Especialidad { Nombre = "Neurologia" },
				new Especialidad { Nombre = "Ortopedia" },
				new Especialidad { Nombre = "Endocrinologia" },
				new Especialidad { Nombre = "Urologia" },
				new Especialidad { Nombre = "Oncologia" },
				new Especialidad { Nombre = "Psiquiatria" },
				new Especialidad { Nombre = "Nefrologia" },
			}
			.Where(e => !existingEspecialidades.Any(ex => ex.Nombre == e.Nombre))
			.ToList();

			if (existingEspecialidades.Any())
			{
				existingEspecialidades.AddRange(newEspecialidades);
				await _context.Especialidad.AddRangeAsync(newEspecialidades);
				await _context.SaveChangesAsync();
			}
			else
			{
				existingEspecialidades = newEspecialidades;
				await _context.Especialidad.AddRangeAsync(existingEspecialidades);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}


		private bool EspecialidadExists(int id)
		{
			return _context.Especialidad.Any(e => e.EspecialidadId == id);
		}

		[HttpPost]
		public IActionResult DeleteAll()
		{
			try
			{
				var allRecords = _context.Especialidad.ToList();

				_context.Especialidad.RemoveRange(allRecords);
				_context.SaveChanges();

				string referer = Request.Headers["Referer"].ToString();
				return Redirect(referer);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while deleting all records: {ex.Message}");
			}
		}
	}
}
