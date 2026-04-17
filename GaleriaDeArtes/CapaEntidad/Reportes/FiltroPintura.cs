namespace GaleriaDeArtes.CapaEntidad.Reportes
{
    /// <summary>Filtro de rango de fechas usado en reportes de ventas.</summary>
    public class FiltroFechas
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public bool TieneFiltros => FechaDesde.HasValue || FechaHasta.HasValue;
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
