using GaleriaDeArtes.CapaEntidad.Reportes;
using GaleriaDeArtes.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace GaleriaDeArtes.CapaDatos
{
    /// <summary>
    /// Acceso a datos exclusivo del módulo de reportes.
    /// Cada método ejecuta una consulta SQL optimizada y retorna
    /// DTOs listos para ser usados por el generador de PDF.
    /// </summary>
    public class ReporteDAL
    {
        // ─── 1. CATÁLOGO GENERAL DE PINTURAS ─────────────────────────────────

        /// <summary>
        /// Retorna el catálogo completo de pinturas con sus relaciones.
        /// Aplica filtros opcionales mediante parámetros SQL (sin concatenación).
        /// </summary>
        public List<FilaCatalogoPintura> ObtenerCatalogoPinturas(FiltroPintura? filtro = null)
        {
            var lista      = new List<FilaCatalogoPintura>();
            var parametros = new List<(string Nombre, object Valor)>();

            var sql = new StringBuilder(@"
                SELECT
                    p.titulo,
                    ISNULL(a.nombre_completo, '—') AS artista,
                    ISNULL(t.nombre_tecnica,  '—') AS tecnica,
                    p.anio_creacion,
                    ISNULL(p.dimensiones, '—')     AS dimensiones,
                    p.precio_base,
                    p.estado_disponibilidad        AS estado,
                    ISNULL(STRING_AGG(e.nombre_exhibicion, ', '), '—') AS exhibiciones
                FROM dbo.PINTURA p
                LEFT JOIN dbo.ARTISTA            a  ON p.id_artista    = a.id_artista
                LEFT JOIN dbo.TECNICA            t  ON p.id_tecnica    = t.id_tecnica
                LEFT JOIN dbo.DETALLE_EXHIBICION de ON p.id_pintura    = de.id_pintura
                LEFT JOIN dbo.EXHIBICION         e  ON de.id_exhibicion = e.id_exhibicion
                WHERE 1=1");

            // Filtros dinámicos (todos con parámetros para evitar SQL injection)
            if (filtro != null)
            {
                if (filtro.IdArtista.HasValue)
                {
                    sql.Append(" AND p.id_artista = @idArtista");
                    parametros.Add(("@idArtista", filtro.IdArtista.Value));
                }
                if (!string.IsNullOrEmpty(filtro.EstadoDisponibilidad))
                {
                    sql.Append(" AND p.estado_disponibilidad = @estado");
                    parametros.Add(("@estado", filtro.EstadoDisponibilidad));
                }
                if (filtro.AnioDesde.HasValue)
                {
                    sql.Append(" AND p.anio_creacion >= @anioDesde");
                    parametros.Add(("@anioDesde", filtro.AnioDesde.Value));
                }
                if (filtro.AnioHasta.HasValue)
                {
                    sql.Append(" AND p.anio_creacion <= @anioHasta");
                    parametros.Add(("@anioHasta", filtro.AnioHasta.Value));
                }
                if (filtro.PrecioMinimo.HasValue)
                {
                    sql.Append(" AND p.precio_base >= @precioMin");
                    parametros.Add(("@precioMin", filtro.PrecioMinimo.Value));
                }
                if (filtro.PrecioMaximo.HasValue)
                {
                    sql.Append(" AND p.precio_base <= @precioMax");
                    parametros.Add(("@precioMax", filtro.PrecioMaximo.Value));
                }
            }

            sql.Append(@"
                GROUP BY
                    p.id_pintura, p.titulo, a.nombre_completo,
                    t.nombre_tecnica,  p.anio_creacion,
                    p.dimensiones,     p.precio_base,
                    p.estado_disponibilidad
                ORDER BY a.nombre_completo, p.titulo");

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new SqlCommand(sql.ToString(), conn);
            foreach (var (nombre, valor) in parametros)
                cmd.Parameters.AddWithValue(nombre, valor);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaCatalogoPintura
                {
                    Titulo       = reader["titulo"]?.ToString()       ?? "",
                    Artista      = reader["artista"]?.ToString()      ?? "—",
                    Tecnica      = reader["tecnica"]?.ToString()      ?? "—",
                    AnioCreacion = reader["anio_creacion"] == DBNull.Value
                                    ? null
                                    : Convert.ToInt32(reader["anio_creacion"]),
                    Dimensiones  = reader["dimensiones"]?.ToString()  ?? "—",
                    PrecioBase   = Convert.ToDecimal(reader["precio_base"]),
                    Estado       = reader["estado"]?.ToString()       ?? "",
                    Exhibiciones = reader["exhibiciones"]?.ToString() ?? "—"
                });
            }
            return lista;
        }

        // ─── 2. RESUMEN POR ARTISTA ───────────────────────────────────────────

        public List<FilaResumenArtista> ObtenerResumenPorArtista()
        {
            var lista = new List<FilaResumenArtista>();
            const string sql = @"
                SELECT
                    a.nombre_completo                       AS artista,
                    ISNULL(a.nacionalidad, '—')             AS nacionalidad,
                    ISNULL(a.estilo_predominante, '—')      AS estilo,
                    COUNT(p.id_pintura)                     AS total_pinturas,
                    ISNULL(SUM(p.precio_base),  0)          AS valor_total,
                    ISNULL(AVG(p.precio_base),  0)          AS precio_promedio
                FROM dbo.ARTISTA a
                LEFT JOIN dbo.PINTURA p ON a.id_artista = p.id_artista
                GROUP BY
                    a.id_artista, a.nombre_completo,
                    a.nacionalidad, a.estilo_predominante
                ORDER BY total_pinturas DESC, valor_total DESC";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new SqlCommand(sql, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaResumenArtista
                {
                    NombreArtista      = reader["artista"]?.ToString()      ?? "",
                    Nacionalidad       = reader["nacionalidad"]?.ToString() ?? "—",
                    EstiloPredominante = reader["estilo"]?.ToString()       ?? "—",
                    TotalPinturas      = Convert.ToInt32(reader["total_pinturas"]),
                    ValorTotal         = Convert.ToDecimal(reader["valor_total"]),
                    PrecioPromedio     = Convert.ToDecimal(reader["precio_promedio"])
                });
            }
            return lista;
        }

        // ─── 3. RESUMEN POR TÉCNICA ───────────────────────────────────────────

        public List<FilaResumenTecnica> ObtenerResumenPorTecnica()
        {
            var lista = new List<FilaResumenTecnica>();
            const string sql = @"
                SELECT
                    t.nombre_tecnica,
                    COUNT(p.id_pintura)            AS total_pinturas,
                    ISNULL(SUM(p.precio_base), 0)  AS valor_total,
                    ISNULL(AVG(p.precio_base), 0)  AS precio_promedio
                FROM dbo.TECNICA t
                LEFT JOIN dbo.PINTURA p ON t.id_tecnica = p.id_tecnica
                GROUP BY t.id_tecnica, t.nombre_tecnica
                ORDER BY total_pinturas DESC, valor_total DESC";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new SqlCommand(sql, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaResumenTecnica
                {
                    NombreTecnica  = reader["nombre_tecnica"]?.ToString() ?? "",
                    TotalPinturas  = Convert.ToInt32(reader["total_pinturas"]),
                    ValorTotal     = Convert.ToDecimal(reader["valor_total"]),
                    PrecioPromedio = Convert.ToDecimal(reader["precio_promedio"])
                });
            }
            return lista;
        }

        // ─── 4. INVENTARIO POR ESTADO ─────────────────────────────────────────

        public List<FilaInventarioEstado> ObtenerInventarioPorEstado()
        {
            var lista = new List<FilaInventarioEstado>();
            // La ventana OVER() calcula el porcentaje relativo sin subconsulta extra
            const string sql = @"
                SELECT
                    p.estado_disponibilidad                             AS estado,
                    COUNT(*)                                            AS cantidad,
                    SUM(p.precio_base)                                  AS valor_total,
                    AVG(p.precio_base)                                  AS precio_promedio,
                    COUNT(*) * 100.0 / SUM(COUNT(*)) OVER ()           AS porcentaje
                FROM dbo.PINTURA p
                GROUP BY p.estado_disponibilidad
                ORDER BY cantidad DESC";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new SqlCommand(sql, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaInventarioEstado
                {
                    Estado         = reader["estado"]?.ToString()         ?? "",
                    Cantidad       = Convert.ToInt32(reader["cantidad"]),
                    ValorTotal     = Convert.ToDecimal(reader["valor_total"]),
                    PrecioPromedio = Convert.ToDecimal(reader["precio_promedio"]),
                    Porcentaje     = Convert.ToDouble(reader["porcentaje"])
                });
            }
            return lista;
        }

        // ─── 5. RESUMEN EJECUTIVO ─────────────────────────────────────────────

        public ResumenEjecutivo ObtenerResumenEjecutivo()
        {
            // Una sola consulta con subconsultas escalares para minimizar round-trips
            const string sql = @"
                SELECT
                    (SELECT COUNT(*) FROM dbo.ARTISTA)                          AS total_artistas,
                    (SELECT COUNT(*) FROM dbo.PINTURA)                          AS total_pinturas,
                    (SELECT ISNULL(SUM(precio_base), 0) FROM dbo.PINTURA)       AS valor_total,
                    (SELECT ISNULL(AVG(precio_base), 0) FROM dbo.PINTURA)       AS precio_promedio,
                    (SELECT TOP 1 t.nombre_tecnica
                     FROM dbo.TECNICA t
                     JOIN dbo.PINTURA p ON t.id_tecnica = p.id_tecnica
                     GROUP BY t.nombre_tecnica
                     ORDER BY COUNT(*) DESC)                                    AS tecnica_top,
                    (SELECT TOP 1 a.nombre_completo
                     FROM dbo.ARTISTA a
                     JOIN dbo.PINTURA p ON a.id_artista = p.id_artista
                     GROUP BY a.nombre_completo
                     ORDER BY COUNT(*) DESC)                                    AS artista_top,
                    (SELECT COUNT(*) FROM dbo.PINTURA
                     WHERE estado_disponibilidad = 'Disponible')                AS disponibles,
                    (SELECT COUNT(*) FROM dbo.PINTURA
                     WHERE estado_disponibilidad = 'Vendida')                   AS vendidas,
                    (SELECT COUNT(*) FROM dbo.PINTURA
                     WHERE estado_disponibilidad = 'Reservada')                 AS reservadas,
                    (SELECT COUNT(*) FROM dbo.EXHIBICION)                       AS total_exhibiciones";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new SqlCommand(sql, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read()) return new ResumenEjecutivo();

            return new ResumenEjecutivo
            {
                TotalArtistas        = Convert.ToInt32(reader["total_artistas"]),
                TotalPinturas        = Convert.ToInt32(reader["total_pinturas"]),
                ValorTotalInventario = Convert.ToDecimal(reader["valor_total"]),
                PrecioPromedio       = Convert.ToDecimal(reader["precio_promedio"]),
                TecnicaMasUsada      = reader["tecnica_top"]?.ToString()  ?? "—",
                ArtistaConMasObras   = reader["artista_top"]?.ToString()  ?? "—",
                PinturasDisponibles  = Convert.ToInt32(reader["disponibles"]),
                PinturasVendidas     = Convert.ToInt32(reader["vendidas"]),
                PinturasReservadas   = Convert.ToInt32(reader["reservadas"]),
                TotalExhibiciones    = Convert.ToInt32(reader["total_exhibiciones"])
            };
        }

        // ─── 6. PINTURAS POR EXHIBICIÓN ───────────────────────────────────────

        public List<FilaExhibicion> ObtenerPinturasPorExhibicion()
        {
            var lista = new List<FilaExhibicion>();
            const string sql = @"
                SELECT
                    e.id_exhibicion,
                    e.nombre_exhibicion,
                    p.titulo,
                    ISNULL(a.nombre_completo, '—') AS artista,
                    ISNULL(t.nombre_tecnica,  '—') AS tecnica,
                    p.precio_base,
                    p.estado_disponibilidad        AS estado,
                    p.anio_creacion,
                    ISNULL(p.dimensiones, '—')     AS dimensiones
                FROM dbo.EXHIBICION e
                LEFT JOIN dbo.DETALLE_EXHIBICION de ON e.id_exhibicion  = de.id_exhibicion
                LEFT JOIN dbo.PINTURA            p  ON de.id_pintura    = p.id_pintura
                LEFT JOIN dbo.ARTISTA            a  ON p.id_artista     = a.id_artista
                LEFT JOIN dbo.TECNICA            t  ON p.id_tecnica     = t.id_tecnica
                ORDER BY e.id_exhibicion, a.nombre_completo, p.titulo";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new SqlCommand(sql, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            FilaExhibicion? actual = null;
            while (reader.Read())
            {
                string nombreExhib = reader["nombre_exhibicion"]?.ToString() ?? "";

                // Nuevo grupo cuando cambia la exhibición
                if (actual == null || actual.NombreExhibicion != nombreExhib)
                {
                    actual = new FilaExhibicion { NombreExhibicion = nombreExhib };
                    lista.Add(actual);
                }

                // Solo agrega pintura si la fila tiene datos (LEFT JOIN puede traer NULLs)
                if (reader["titulo"] != DBNull.Value)
                {
                    actual.Pinturas.Add(new FilaCatalogoPintura
                    {
                        Titulo       = reader["titulo"]?.ToString()    ?? "",
                        Artista      = reader["artista"]?.ToString()   ?? "—",
                        Tecnica      = reader["tecnica"]?.ToString()   ?? "—",
                        AnioCreacion = reader["anio_creacion"] == DBNull.Value
                                        ? null
                                        : Convert.ToInt32(reader["anio_creacion"]),
                        Dimensiones  = reader["dimensiones"]?.ToString() ?? "—",
                        PrecioBase   = reader["precio_base"] == DBNull.Value
                                        ? 0m
                                        : Convert.ToDecimal(reader["precio_base"]),
                        Estado       = reader["estado"]?.ToString()    ?? ""
                    });
                }
            }
            return lista;
        }
    }
}
