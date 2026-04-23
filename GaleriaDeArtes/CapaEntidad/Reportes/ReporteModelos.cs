namespace GaleriaDeArtes.CapaEntidad.Reportes
{
    // ─────────────────────────────────────────────────────────────────────────
    // DTOs del módulo de reportes. Representan el resultado de cada consulta,
    // no el mapeo directo de una tabla.
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Fila del reporte Ventas por Periodo.</summary>
    public class FilaVentaPeriodo
    {
        public string   Titulo      { get; set; } = "";
        public string   Cliente     { get; set; } = "";
        public DateTime FechaVenta  { get; set; }
        public decimal  PrecioVenta { get; set; }
    }

    /// <summary>Fila del Top 10 pinturas más vendidas.</summary>
    public class FilaTopPintura
    {
        public int     Posicion     { get; set; }
        public string  Titulo       { get; set; } = "";
        public string  Artista      { get; set; } = "";
        public int     VecesVendida { get; set; }
        public decimal IngresoTotal { get; set; }
    }

    /// <summary>Fila del reporte de Inventario Actual.</summary>
    public class FilaInventario
    {
        public string  Titulo      { get; set; } = "";
        public string  Artista     { get; set; } = "";
        public string  Tecnica     { get; set; } = "";
        public string  Dimensiones { get; set; } = "";
        public decimal PrecioBase  { get; set; }
        public string  Estado      { get; set; } = "";
        /// <summary>Piezas con estado_fisico = 'Disponible' en la tabla PIEZA.</summary>
        public int     Stock       { get; set; }
    }

    /// <summary>Fila del reporte Compras agrupadas por Proveedor.</summary>
    public class FilaCompraPorProveedor
    {
        public string    Proveedor    { get; set; } = "";
        public int       TotalCompras { get; set; }
        public decimal   MontoTotal   { get; set; }
        public DateTime? UltimaCompra { get; set; }
    }

    /// <summary>Fila del reporte Ventas agrupadas por Cliente.</summary>
    public class FilaVentaPorCliente
    {
        public string  Cliente     { get; set; } = "";
        public int     TotalVentas { get; set; }
        public decimal MontoTotal  { get; set; }
    }

    /// <summary>Fila del reporte Ventas agrupadas por Mes (formato yyyy-MM).</summary>
    public class FilaVentaPorMes
    {
        public string  Mes         { get; set; } = "";
        public int     TotalVentas { get; set; }
        public decimal MontoTotal  { get; set; }
    }
}
