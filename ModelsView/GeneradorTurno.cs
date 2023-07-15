using System.ComponentModel.DataAnnotations;

namespace TurnosSalud.ModelsView
{
	public class GeneradorTurno
	{
		[Display(Name = "Médico")]
		public int MedicoId { get; set; }

		[Display(Name = "Fecha Desde")]
		public DateTime FechaDesde { get; set; }

		[Display(Name = "Cantidad de Turnos por Dia")]
		public int CantidadTurnos { get; set; }

		[Display(Name = "Fecha Hasta")]
		[DataType(DataType.Date)]
		public DateTime FechaHasta { get; set; }
	}
}
