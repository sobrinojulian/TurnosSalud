using TurnosSalud.Data;
using TurnosSalud.Models;
using TurnosSalud.ModelsView;

namespace TurnosSalud.Reglas
{
	public class ReglaTurnos
	{
		private readonly TurnosContext _context;

		public ReglaTurnos(TurnosContext context)
		{
			_context = context;
		}

		public async Task<string> GenerarTurnos(GeneradorTurno generador)
		{
			DateTime fechaInicio = generador.FechaDesde;
			fechaInicio = fechaInicio.AddMinutes(-fechaInicio.Minute);
			if (generador.FechaDesde.Minute >= 30)
				fechaInicio = fechaInicio.AddMinutes(30);
			var fechaInicioInicial = fechaInicio;
			bool estoyDentroDeLaFecha = fechaInicio <= generador.FechaHasta;
			while (estoyDentroDeLaFecha)
			{
				#region Generacion de Bloques dentro de un Dia
				for (int i = 0; i < generador.CantidadTurnos; i++)
				{
					bool noExiste = !_context.Turno.Any(o => o.MedicoId == generador.MedicoId && o.Fecha == fechaInicio);

					if (noExiste)
					{
						Turno turno = new()
						{
							Fecha = fechaInicio,
							MedicoId = generador.MedicoId
						};
						_context.Turno.Add(turno);
						await _context.SaveChangesAsync();
					}
					if (fechaInicio.Hour == 23 && fechaInicio.Minute == 30)
						break;

					fechaInicio = fechaInicio.AddMinutes(30);
				}
				#endregion

				fechaInicioInicial = fechaInicioInicial.AddDays(1);
				fechaInicio = fechaInicioInicial;
				estoyDentroDeLaFecha = fechaInicio <= generador.FechaHasta;
			}
			return "";
		}
	}
}
