namespace GaleriaDeArtes.CapaEntidad.Reportes
{
    /// <summary>
    /// Parámetros opcionales de filtrado para el reporte de catálogo general.
    /// Todos los campos son nullables; solo se aplican los que tengan valor.
    /// </summary>
    public class FiltroPintura
    {
        public int?     IdArtista             { get; set; }
        public string?  EstadoDisponibilidad  { get; set; }
        public int?     AnioDesde             { get; set; }
        public int?     AnioHasta             { get; set; }
        public decimal? PrecioMinimo          { get; set; }
        public decimal? PrecioMaximo          { get; set; }

        /// <summary>Devuelve true si hay al menos un filtro activo.</summary>
        public bool TieneFiltros =>
            IdArtista.HasValue            ||
            !string.IsNullOrEmpty(EstadoDisponibilidad) ||
            AnioDesde.HasValue            ||
            AnioHasta.HasValue            ||
            PrecioMinimo.HasValue         ||
            PrecioMaximo.HasValue;
    }
}
