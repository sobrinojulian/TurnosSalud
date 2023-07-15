using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnosSalud.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Especialidad",
				columns: table => new
				{
					EspecialidadId = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Especialidad", x => x.EspecialidadId);
				});

			migrationBuilder.CreateTable(
				name: "Usuario",
				columns: table => new
				{
					UsuarioId = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
					Email = table.Column<string>(type: "TEXT", nullable: false),
					Password = table.Column<string>(type: "TEXT", nullable: false),
					Rol = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
				});

			migrationBuilder.CreateTable(
				name: "Medico",
				columns: table => new
				{
					MedicoId = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Matricula = table.Column<int>(type: "INTEGER", nullable: false),
					Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
					EspecialidadId = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Medico", x => x.MedicoId);
					table.ForeignKey(
						name: "FK_Medico_Especialidad_EspecialidadId",
						column: x => x.EspecialidadId,
						principalTable: "Especialidad",
						principalColumn: "EspecialidadId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Turno",
				columns: table => new
				{
					TurnoId = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
					UsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
					MedicoId = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Turno", x => x.TurnoId);
					table.ForeignKey(
						name: "FK_Turno_Medico_MedicoId",
						column: x => x.MedicoId,
						principalTable: "Medico",
						principalColumn: "MedicoId",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Turno_Usuario_UsuarioId",
						column: x => x.UsuarioId,
						principalTable: "Usuario",
						principalColumn: "UsuarioId");
				});

			migrationBuilder.CreateIndex(
				name: "IX_Medico_EspecialidadId",
				table: "Medico",
				column: "EspecialidadId");

			migrationBuilder.CreateIndex(
				name: "IX_Turno_MedicoId",
				table: "Turno",
				column: "MedicoId");

			migrationBuilder.CreateIndex(
				name: "IX_Turno_UsuarioId",
				table: "Turno",
				column: "UsuarioId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Turno");

			migrationBuilder.DropTable(
				name: "Medico");

			migrationBuilder.DropTable(
				name: "Usuario");

			migrationBuilder.DropTable(
				name: "Especialidad");
		}
	}
}
