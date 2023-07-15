namespace TurnosSalud.ModelsView
{
	public class Error
	{
		public string? RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

		public const string CampoRequerido = "{0} es obligatorio.";
		public const string CaracteresMaximos = "{0} no debe superar los {1} caracteres.";
	}
}
