using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurnosSalud.ModelsView;

namespace TurnosSalud.Models
{
	[Table("Turno")]
	public class Turno
	{
		[Key]
		public int TurnoId { get; set; }

		[Required(ErrorMessage = Error.CampoRequerido)]
		[DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
		public DateTime Fecha { get; set; }

		public int? UsuarioId { get; set; }
		public Usuario? Usuario { get; set; }

		public int MedicoId { get; set; }
		public Medico? Medico { get; set; }

		public bool estaLibre()
		{
			return UsuarioId == null;
		}
	}
}
