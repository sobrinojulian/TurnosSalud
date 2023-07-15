using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurnosSalud.ModelsView;

namespace TurnosSalud.Models
{
	public enum RolEnum
	{
		Administrador,
		Paciente,
	}

	[Table("Usuario")]
	public class Usuario
	{
		[Key]
		public int UsuarioId { get; set; }

		[Required(ErrorMessage = Error.CampoRequerido)]
		[MaxLength(50, ErrorMessage = Error.CaracteresMaximos)]
		public string Nombre { get; set; } = "";

		[Required(ErrorMessage = Error.CampoRequerido)]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = "";

		[Required(ErrorMessage = Error.CampoRequerido)]
		[DataType(DataType.Password)]
		public string Password { get; set; } = "";

		[Required(ErrorMessage = Error.CampoRequerido)]
		public RolEnum Rol { get; set; }

		public ICollection<Turno>? Turnos { get; set; }
	}
}
