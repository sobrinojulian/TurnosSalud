using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurnosSalud.ModelsView;

namespace TurnosSalud.Models
{
	[Table("Especialidad")]
	public class Especialidad
	{
		[Key]
		public int EspecialidadId { get; set; }

		[Required(ErrorMessage = Error.CampoRequerido)]
		[MaxLength(50, ErrorMessage = Error.CaracteresMaximos)]
		public string Nombre { get; set; } = "";

		public ICollection<Medico>? Medicos { get; set; }
	}
}
