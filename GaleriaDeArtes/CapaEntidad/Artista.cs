namespace GaleriaDeArtes.CapaEntidad
{
    public class Artista
    {
        public int IdArtista { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string EstiloPredominante { get; set; } = string.Empty;
        public string Biografia { get; set; } = string.Empty;
    }
}
