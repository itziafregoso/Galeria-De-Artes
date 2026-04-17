using GaleriaDeArtes.CapaEntidad.Reportes;
using GaleriaDeArtes.Data;
using Microsoft.Data.SqlClient;

namespace GaleriaDeArtes.CapaDatos
{
    /// <summary>
    /// Acceso a datos del módulo de reportes.
    /// Cada método ejecuta una consulta optimizada y retorna DTOs
    /// listos para el generador de PDF.
    ///
    /// ESQUEMA REAL (GaleriaArte):
    ///   VENTA      : id_venta, id_cliente, id_usuario, fecha_venta, total_venta, estado_venta
    ///   DETALLE_VENTA : (id_venta, id_pintura) PK compuesta, precio_final_acordado
    ///   COMPRA     : id_compra, id_proveedor, id_usuario, fecha_compra, total_compra, estado_compra
    ///   DETALLE_COMPRA: (id_compra, id_pintura) PK compuesta, precio_adquisicion
    ///   PROVEEDOR  : id_proveedor, nombre_empresa, ...
    ///   CLIENTE    : id_cliente, nombre, apellido_p, apellido_m, ...
    /// </summary>
    public class ReporteDAL
    {
        // ─── 1. VENTAS POR PERIODO ────────────────────────────────────────────
        // Detalle por pintura: VENTA → DETALLE_VENTA → PINTURA → CLIENTE
        // Solo ventas con estado 'Completada' para evitar ruido.

        public List<FilaVentaPeriodo> ObtenerVentasPorPeriodo(FiltroFechas? filtro = null)
        {
            var lista = new List<FilaVentaPeriodo>();
            const string sql = @"
                SELECT
                    p.titulo,
                    ISNULL(c.nombre + ' ' + c.apellido_p, '(Sin cliente)') AS cliente,
                    v.fecha_venta,
                    dv.precio_final_acordado                               AS precio_venta
                FROM dbo.VENTA v
                JOIN  dbo.DETALLE_VENTA dv ON v.id_venta    = dv.id_venta
                JOIN  dbo.PINTURA       p  ON dv.id_pintura = p.id_pintura
                LEFT JOIN dbo.CLIENTE   c  ON v.id_cliente  = c.id_cliente
                WHERE v.estado_venta = 'Completada'
                  AND (@fechaDesde IS NULL OR v.fecha_venta >= @fechaDesde)
                  AND (@fechaHasta IS NULL OR v.fecha_venta <= @fechaHasta)
                ORDER BY v.fecha_venta DESC";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);

            cmd.Parameters.AddWithValue("@fechaDesde",
                filtro?.FechaDesde.HasValue == true ? (object)filtro.FechaDesde.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@fechaHasta",
                filtro?.FechaHasta.HasValue == true
                    ? (object)filtro.FechaHasta.Value.Date.AddDays(1).AddTicks(-1)
                    : DBNull.Value);

            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new FilaVentaPeriodo
                {
                    Titulo      = r["titulo"]?.ToString()  ?? "",
                    Cliente     = r["cliente"]?.ToString() ?? "",
                    FechaVenta  = Convert.ToDateTime(r["fecha_venta"]),
                    PrecioVenta = Convert.ToDecimal(r["precio_venta"])
                });
            return lista;
        }

        // ─── 2. TOP 10 PINTURAS MÁS VENDIDAS ─────────────────────────────────
        // Path correcto: PINTURA → DETALLE_VENTA → VENTA (filtra Completadas)
        // ROW_NUMBER en la subconsulta evita conflicto con TOP y ORDER BY.

        public List<FilaTopPintura> ObtenerTopPinturas()
        {
            var lista = new List<FilaTopPintura>();
            const string sql = @"
                SELECT TOP 10
                    ROW_NUMBER() OVER (ORDER BY COUNT(dv.id_venta) DESC) AS posicion,
                    p.titulo,
                    ISNULL(a.nombre_completo, '—')        AS artista,
                    COUNT(dv.id_venta)                     AS veces_vendida,
                    ISNULL(SUM(dv.precio_final_acordado), 0) AS ingreso_total
                FROM dbo.PINTURA       p
                JOIN  dbo.DETALLE_VENTA dv ON p.id_pintura  = dv.id_pintura
                JOIN  dbo.VENTA         v  ON dv.id_venta   = v.id_venta
                LEFT JOIN dbo.ARTISTA   a  ON p.id_artista  = a.id_artista
                WHERE v.estado_venta = 'Completada'
                GROUP BY p.id_pintura, p.titulo, a.nombre_completo
                ORDER BY COUNT(dv.id_venta) DESC";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new FilaTopPintura
                {
                    Posicion     = Convert.ToInt32(r["posicion"]),
                    Titulo       = r["titulo"]?.ToString()  ?? "",
                    Artista      = r["artista"]?.ToString() ?? "—",
                    VecesVendida = Convert.ToInt32(r["veces_vendida"]),
                    IngresoTotal = Convert.ToDecimal(r["ingreso_total"])
                });
            return lista;
        }

        // ─── 3. INVENTARIO ACTUAL ─────────────────────────────────────────────
        // Sin cambios de lógica. La tabla PINTURA tiene todos los estados.

        public List<FilaInventario> ObtenerInventarioActual()
        {
            var lista = new List<FilaInventario>();
            const string sql = @"
                SELECT
                    p.titulo,
                    ISNULL(a.nombre_completo, '—') AS artista,
                    ISNULL(t.nombre_tecnica,  '—') AS tecnica,
                    ISNULL(p.dimensiones,     '—') AS dimensiones,
                    p.precio_base,
                    p.estado_disponibilidad        AS estado
                FROM dbo.PINTURA p
                LEFT JOIN dbo.ARTISTA a ON p.id_artista = a.id_artista
                LEFT JOIN dbo.TECNICA t ON p.id_tecnica = t.id_tecnica
                ORDER BY p.estado_disponibilidad, a.nombre_completo, p.titulo";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new FilaInventario
                {
                    Titulo      = r["titulo"]?.ToString()      ?? "",
                    Artista     = r["artista"]?.ToString()     ?? "—",
                    Tecnica     = r["tecnica"]?.ToString()     ?? "—",
                    Dimensiones = r["dimensiones"]?.ToString() ?? "—",
                    PrecioBase  = Convert.ToDecimal(r["precio_base"]),
                    Estado      = r["estado"]?.ToString()      ?? ""
                });
            return lista;
        }

        // ─── 4. COMPRAS POR PROVEEDOR ─────────────────────────────────────────
        // Columna correcta: PROVEEDOR.nombre_empresa (no nombre_proveedor).
        // Solo compras Completadas para el monto; el conteo incluye todas.
        // Se usa total_compra de COMPRA (suma de cabecera, validada contra detalles).

        public List<FilaCompraPorProveedor> ObtenerComprasPorProveedor()
        {
            var lista = new List<FilaCompraPorProveedor>();
            const string sql = @"
                SELECT
                    pr.nombre_empresa                          AS proveedor,
                    COUNT(c.id_compra)                         AS total_compras,
                    ISNULL(SUM(
                        CASE WHEN c.estado_compra = 'Completada'
                             THEN c.total_compra ELSE 0 END), 0) AS monto_total,
                    MAX(c.fecha_compra)                        AS ultima_compra
                FROM dbo.PROVEEDOR pr
                LEFT JOIN dbo.COMPRA c ON pr.id_proveedor = c.id_proveedor
                GROUP BY pr.id_proveedor, pr.nombre_empresa
                ORDER BY monto_total DESC";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new FilaCompraPorProveedor
                {
                    Proveedor    = r["proveedor"]?.ToString()    ?? "",
                    TotalCompras = Convert.ToInt32(r["total_compras"]),
                    MontoTotal   = Convert.ToDecimal(r["monto_total"]),
                    UltimaCompra = r["ultima_compra"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(r["ultima_compra"])
                });
            return lista;
        }

        // ─── 5. VENTAS POR CLIENTE ────────────────────────────────────────────
        // Columnas correctas: CLIENTE.nombre + apellido_p, VENTA.total_venta.
        // Agrupación por id_cliente y nombre para evitar colisiones de apellidos.

        public List<FilaVentaPorCliente> ObtenerVentasPorCliente()
        {
            var lista = new List<FilaVentaPorCliente>();
            const string sql = @"
                SELECT
                    ISNULL(c.nombre + ' ' + c.apellido_p, '(Sin cliente)') AS cliente,
                    COUNT(v.id_venta)              AS total_ventas,
                    ISNULL(SUM(v.total_venta), 0)  AS monto_total
                FROM dbo.VENTA v
                LEFT JOIN dbo.CLIENTE c ON v.id_cliente = c.id_cliente
                WHERE v.estado_venta = 'Completada'
                GROUP BY c.id_cliente, c.nombre, c.apellido_p
                ORDER BY monto_total DESC";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new FilaVentaPorCliente
                {
                    Cliente     = r["cliente"]?.ToString()     ?? "",
                    TotalVentas = Convert.ToInt32(r["total_ventas"]),
                    MontoTotal  = Convert.ToDecimal(r["monto_total"])
                });
            return lista;
        }

        // ─── 6. VENTAS POR MES ────────────────────────────────────────────────
        // Filtra por mes y año exactos usando parámetros para evitar SQL Injection.

        public List<FilaVentaPorMes> ObtenerVentasPorMes(FiltroMes filtro)
        {
            var lista = new List<FilaVentaPorMes>();
            const string sql = @"
                SELECT
                    FORMAT(v.fecha_venta, 'yyyy-MM') AS mes,
                    COUNT(v.id_venta)                 AS total_ventas,
                    ISNULL(SUM(v.total_venta), 0)    AS monto_total
                FROM dbo.VENTA v
                WHERE v.estado_venta = 'Completada'
                  AND MONTH(v.fecha_venta) = @Mes
                  AND YEAR(v.fecha_venta)  = @Anio
                GROUP BY FORMAT(v.fecha_venta, 'yyyy-MM')
                ORDER BY mes ASC";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            cmd.Parameters.AddWithValue("@Mes",  filtro.Mes);
            cmd.Parameters.AddWithValue("@Anio", filtro.Anio);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new FilaVentaPorMes
                {
                    Mes         = r["mes"]?.ToString()       ?? "",
                    TotalVentas = Convert.ToInt32(r["total_ventas"]),
                    MontoTotal  = Convert.ToDecimal(r["monto_total"])
                });
            return lista;
        }
    }
}
