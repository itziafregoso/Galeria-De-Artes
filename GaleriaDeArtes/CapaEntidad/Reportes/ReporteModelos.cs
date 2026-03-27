namespace GaleriaDeArtes.CapaEntidad.Reportes
{
    // ─────────────────────────────────────────────────────────────────────────
    // DTOs para los distintos reportes. No mapean tablas directamente,
    // sino el resultado de las consultas SQL de cada reporte.
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Fila del catálogo general de pinturas (con JOINs).</summary>
    public class FilaCatalogoPintura
    {
        public string  Titulo        { get; set; } = "";
        public string  Artista       { get; set; } = "";
        public string  Tecnica       { get; set; } = "";
        public int?    AnioCreacion  { get; set; }
        public string  Dimensiones   { get; set; } = "";
        public decimal PrecioBase    { get; set; }
        public string  Estado        { get; set; } = "";
        public string  Exhibiciones  { get; set; } = "—";
    }

    /// <summary>Fila del reporte de producción por artista.</summary>
    public class FilaResumenArtista
    {
        public string  NombreArtista      { get; set; } = "";
        public string  Nacionalidad       { get; set; } = "";
        public string  EstiloPredominante { get; set; } = "";
        public int     TotalPinturas      { get; set; }
        public decimal ValorTotal         { get; set; }
        public decimal PrecioPromedio     { get; set; }
    }

    /// <summary>Fila del reporte de pinturas agrupadas por técnica.</summary>
    public class FilaResumenTecnica
    {
        public string  NombreTecnica  { get; set; } = "";
        public int     TotalPinturas  { get; set; }
        public decimal ValorTotal     { get; set; }
        public decimal PrecioPromedio { get; set; }
    }

    /// <summary>Fila del reporte de inventario por estado de disponibilidad.</summary>
    public class FilaInventarioEstado
    {
        public string  Estado         { get; set; } = "";
        public int     Cantidad       { get; set; }
        public decimal ValorTotal     { get; set; }
        public decimal PrecioPromedio { get; set; }
        public double  Porcentaje     { get; set; }
    }

    /// <summary>Datos consolidados para el Resumen Ejecutivo (una sola página).</summary>
    public class ResumenEjecutivo
    {
        public int     TotalArtistas        { get; set; }
        public int     TotalPinturas        { get; set; }
        public decimal ValorTotalInventario { get; set; }
        public decimal PrecioPromedio       { get; set; }
        public string  TecnicaMasUsada      { get; set; } = "—";
        public string  ArtistaConMasObras   { get; set; } = "—";
        public int     PinturasDisponibles  { get; set; }
        public int     PinturasVendidas     { get; set; }
        public int     PinturasReservadas   { get; set; }
        public int     TotalExhibiciones    { get; set; }
    }

    /// <summary>Grupo de pinturas que pertenecen a una misma exhibición.</summary>
    public class FilaExhibicion
    {
        public string                  NombreExhibicion { get; set; } = "";
        public List<FilaCatalogoPintura> Pinturas        { get; set; } = new();
    }
}
