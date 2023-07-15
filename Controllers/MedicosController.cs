using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TurnosSalud.Data;
using TurnosSalud.Models;
using TurnosSalud.Reglas;

namespace TurnosSalud.Controllers
{
	[Authorize]
	public class MedicosController : Controller
	{
		private readonly TurnosContext _context;

		public MedicosController(TurnosContext context)
		{
			_context = context;
		}

		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Medico.Include(m => m.Especialidad).ToListAsync());
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Medico == null)
			{
				return NotFound();
			}

			var medico = await _context.Medico.Include(m => m.Especialidad).FirstOrDefaultAsync(m => m.MedicoId == id);
			if (medico == null)
			{
				return NotFound();
			}

			return View(medico);
		}

		public IActionResult Create()
		{
			ViewData["EspecialidadId"] = new SelectList(_context.Especialidad, "EspecialidadId", "Nombre");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("MedicoId,Matricula,Nombre,EspecialidadId")] Medico medico)
		{
			if (_context.Medico.Any(m => m.Matricula == medico.Matricula))
				ModelState.AddModelError("Matricula", "Ya existe un medico con esa matricula");

			if (ModelState.IsValid)
			{
				_context.Add(medico);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["EspecialidadId"] = new SelectList(_context.Especialidad, "EspecialidadId", "Nombre", medico.EspecialidadId);
			return View(medico);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Medico == null)
			{
				return NotFound();
			}

			ViewData["EspecialidadId"] = new SelectList(_context.Especialidad, "EspecialidadId", "Nombre");
			var medico = await _context.Medico.FindAsync(id);
			if (medico == null)
			{
				return NotFound();
			}
			return View(medico);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("MedicoId,Matricula,Nombre,EspecialidadId")] Medico medico)
		{
			if (id != medico.MedicoId)
			{
				return NotFound();
			}

			if (_context.Medico.Any(m => m.Matricula == medico.Matricula))
				ModelState.AddModelError("Matricula", "Ya existe un medico con esa matricula");

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(medico);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!MedicoExists(medico.MedicoId))
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
			ViewData["EspecialidadId"] = new SelectList(_context.Especialidad, "EspecialidadId", "Nombre", medico.EspecialidadId);
			return View(medico);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Medico == null)
			{
				return NotFound();
			}

			var medico = await _context.Medico.Include(m => m.Especialidad)
				.FirstOrDefaultAsync(m => m.MedicoId == id);
			if (medico == null)
			{
				return NotFound();
			}

			return View(medico);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Medico == null)
			{
				return Problem("Entity set 'DbContext.Medico'  is null.");
			}
			var medico = await _context.Medico.FindAsync(id);
			if (medico != null)
			{
				_context.Medico.Remove(medico);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool MedicoExists(int id)
		{
			return _context.Medico.Any(e => e.MedicoId == id);
		}

		public async Task<IActionResult> GenerarTurno(int id)
		{
			ReglaTurnos regla = new(_context);
			DateTime fechaInicio = DateTime.Now;
			fechaInicio = fechaInicio.AddHours(-fechaInicio.Hour).AddHours(9);
			fechaInicio = fechaInicio.AddMinutes(-fechaInicio.Minute);
			fechaInicio = fechaInicio.AddSeconds(-fechaInicio.Second);
			await regla.GenerarTurnos(new ModelsView.GeneradorTurno()
			{
				CantidadTurnos = 18,
				FechaDesde = fechaInicio,
				FechaHasta = fechaInicio,
				MedicoId = id
			});
			return RedirectToAction(nameof(Index));
		}
	}
}
