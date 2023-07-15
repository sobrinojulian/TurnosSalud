using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TurnosSalud.Data;
using TurnosSalud.Models;
using TurnosSalud.ModelsView;
using TurnosSalud.Reglas;

namespace TurnosSalud.Controllers
{
	[Authorize]
	public class TurnosController : Controller
	{
		private readonly TurnosContext _context;

		public TurnosController(TurnosContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			ViewData["EspecialidadId"] = new SelectList(_context.Especialidad, "EspecialidadId", "Nombre");
			return View(await _context.Turno.Include(m => m.Medico).ToListAsync());
		}

		[HttpPost]
		public async Task<IActionResult> Index(int especialidades, string disponibilidad)
		{
			var turnos = await _context.Turno.Include(t => t.Medico).ThenInclude(m => m.Especialidad).ToListAsync();

			if (especialidades != 0)
			{
				turnos = turnos.Where(t => t.Medico.EspecialidadId == especialidades).ToList();
			}

			if (disponibilidad == "Libres")
			{
				turnos = turnos.Where(t => t.estaLibre()).ToList();
			}
			else if (disponibilidad == "Reservadas")
			{
				turnos = turnos.Where(t => !t.estaLibre()).ToList();
			}

			ViewData["EspecialidadId"] = new SelectList(_context.Especialidad, "EspecialidadId", "Nombre", especialidades);

			return View(nameof(Index), turnos);
		}



		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Turno == null)
			{
				return NotFound();
			}

			var turno = await _context.Turno.Include(m => m.Medico).Include(m => m.Usuario).FirstOrDefaultAsync(m => m.TurnoId == id);
			if (turno == null)
			{
				return NotFound();
			}

			return View(turno);
		}

		[Authorize(Roles = "Administrador")]
		public IActionResult Create()
		{
			ViewData["MedicoId"] = new SelectList(_context.Medico, "MedicoId", "Nombre");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("TurnoId,Fecha,MedicoId")] Turno turno)
		{
			if (ModelState.IsValid)
			{
				turno.Fecha = RoundToNearestHourOrHalfHour(turno.Fecha);
				_context.Add(turno);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(turno);
		}

		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> Edit(int? id)
		{
			ViewData["MedicoId"] = new SelectList(_context.Medico, "MedicoId", "Nombre");
			if (id == null || _context.Turno == null)
			{
				return NotFound();
			}

			var turno = await _context.Turno.FindAsync(id);
			if (turno == null)
			{
				return NotFound();
			}
			return View(turno);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("TurnoId,Fecha,Usuario")] Turno turno)
		{
			if (id != turno.TurnoId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(turno);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TurnoExists(turno.TurnoId))
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
			return View(turno);
		}

		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Turno == null)
			{
				return NotFound();
			}

			var turno = await _context.Turno.FirstOrDefaultAsync(m => m.TurnoId == id);
			if (turno == null)
			{
				return NotFound();
			}

			return View(turno);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Turno == null)
			{
				return Problem("Entity set 'DbContext.Turno'  is null.");
			}
			var turno = await _context.Turno.FindAsync(id);
			if (turno != null)
			{
				_context.Turno.Remove(turno);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public IActionResult DeleteAll()
		{
			try
			{
				var allRecords = _context.Turno.ToList();

				_context.Turno.RemoveRange(allRecords);
				_context.SaveChanges();

				string referer = Request.Headers["Referer"].ToString();
				return Redirect(referer);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while deleting all records: {ex.Message}");
			}
		}

		private bool TurnoExists(int id)
		{
			return _context.Turno.Any(e => e.TurnoId == id);
		}

		public IActionResult Generacion()
		{
			ViewData["MedicoId"] = new SelectList(_context.Medico, "MedicoId", "Nombre");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Generacion(GeneradorTurno generador)
		{
			var regla = new ReglaTurnos(_context);
			await regla.GenerarTurnos(generador);
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Reservar(int? id)
		{
			if (id == null || _context.Turno == null)
			{
				return NotFound();
			}

			// TODO: Sin el ThenInclude?
			var turno = await _context.Turno.Include(m => m.Medico).ThenInclude(m => m.Especialidad).FirstOrDefaultAsync(m => m.TurnoId == id);
			if (turno == null)
			{
				return NotFound();
			}
			return View(turno);
		}
		[HttpPost]
		public async Task<IActionResult> Reservar(int id)
		{
			int usuarioId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
			var turno = await _context.Turno.FindAsync(id);

			if (id != turno?.TurnoId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					turno.Usuario = usuario;
					_context.Update(turno);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TurnoExists(turno.TurnoId))
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
			return View(turno);
		}

		public async Task<IActionResult> Reservados()
		{
			int usuarioId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			// TODO: Sin el ThenInclude?
			var turnos = await _context.Turno.Include(m => m.Medico).ThenInclude(m => m.Especialidad).ToListAsync();
			turnos = turnos.Where(t => t.UsuarioId == usuarioId).ToList();

			return View(turnos);
		}

		private static DateTime RoundToNearestHourOrHalfHour(DateTime dateTime)
		{
			int minute = dateTime.Minute;
			if (minute < 15)
			{
				// Round down to the nearest hour
				return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
			}
			else if (minute >= 15 && minute < 45)
			{
				// Round to the nearest half an hour
				return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 30, 0);
			}
			else
			{
				// Round up to the next hour
				return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour + 1, 0, 0);
			}
		}
	}
}
