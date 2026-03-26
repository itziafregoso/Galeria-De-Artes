namespace GaleriaDeArtes.CapaEntidad
{
    // DTO que representa exactamente las columnas del SELECT del formulario Pinturas.
    // No mapea una sola tabla, sino el resultado combinado de la consulta con JOINs.
    public class PinturaVista
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tecnica { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Artista { get; set; } = string.Empty;
        public string Exhibiciones { get; set; } = string.Empty;
    }
}
