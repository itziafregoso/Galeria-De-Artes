namespace GaleriaDeArtes.CapaEntidad.Reportes
{
    /// <summary>Filtro de rango de fechas y búsqueda de texto para reportes.</summary>
    public class FiltroFechas
    {
        public DateTime? FechaDesde    { get; set; }
        public DateTime? FechaHasta    { get; set; }
        /// <summary>Nombre parcial de cliente o proveedor para filtrar (NULL = sin filtro).</summary>
        public string?   TextoBusqueda { get; set; }

        public bool TieneFiltros =>
            FechaDesde.HasValue || FechaHasta.HasValue ||
            !string.IsNullOrWhiteSpace(TextoBusqueda);

        public string DescripcionPeriodo()
        {
            if (FechaDesde.HasValue && FechaHasta.HasValue)
                return $"{FechaDesde:dd/MM/yyyy} — {FechaHasta:dd/MM/yyyy}";
            if (FechaDesde.HasValue)
                return $"Desde {FechaDesde:dd/MM/yyyy}";
            if (FechaHasta.HasValue)
                return $"Hasta {FechaHasta:dd/MM/yyyy}";
            return "Todas las fechas";
        }
    }

    /// <summary>Filtro de mes y año para el reporte Ventas por Mes.</summary>
    public class FiltroMes
    {
        public int Mes  { get; set; }
        public int Anio { get; set; }

        public string NombreMes =>
            new System.Globalization.CultureInfo("es-MX")
                .DateTimeFormat.GetMonthName(Mes);
    }
}
