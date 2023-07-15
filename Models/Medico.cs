using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurnosSalud.ModelsView;

namespace TurnosSalud.Models
{
	[Table("Medico")]
	public class Medico
	{
		[Key]
		public int MedicoId { get; set; }

		[Required(ErrorMessage = Error.CampoRequerido)]
		public int Matricula { get; set; }

		[Required(ErrorMessage = Error.CampoRequerido)]
		[MaxLength(50, ErrorMessage = Error.CaracteresMaximos)]
		public string Nombre { get; set; } = "";

		[Display(Name = "Especialidad")]
		[Required(ErrorMessage = Error.CampoRequerido)]
		public int EspecialidadId { get; set; }
		public Especialidad? Especialidad { get; set; }

		public ICollection<Turno>? Turnos { get; set; }
	}
}
