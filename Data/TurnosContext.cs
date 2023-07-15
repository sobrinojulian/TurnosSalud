using Microsoft.EntityFrameworkCore;

namespace TurnosSalud.Data
{
	public class TurnosContext : DbContext
	{
		public TurnosContext(DbContextOptions<TurnosContext> options)
			: base(options)
		{
			Medico = Set<TurnosSalud.Models.Medico>();
			Turno = Set<TurnosSalud.Models.Turno>();
			Usuario = Set<TurnosSalud.Models.Usuario>();
		}

		public DbSet<TurnosSalud.Models.Especialidad> Especialidad { get; set; } = default!;

		public DbSet<TurnosSalud.Models.Medico> Medico { get; set; }

		public DbSet<TurnosSalud.Models.Turno> Turno { get; set; }

		public DbSet<TurnosSalud.Models.Usuario> Usuario { get; set; }
	}
}
