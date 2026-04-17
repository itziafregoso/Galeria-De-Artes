using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GaleriaDeArtes.CapaDatos
{
    /// <summary>
    /// Acceso a datos para dbo.EXHIBICION y dbo.DETALLE_EXHIBICION.
    ///
    /// Esquema real confirmado:
    ///   EXHIBICION        : id_exhibicion PK, nombre_exhibicion, fecha_inicio,
    ///                       fecha_fin, tematica (null), costo_entrada (null, def 0)
    ///   DETALLE_EXHIBICION: id_det_exhibicion PK, id_exhibicion (null FK), id_pintura (null FK)
    ///   FK on delete      : NO_ACTION  → se deben borrar detalles antes que la cabecera
    /// </summary>
    public class ExhibicionDAL
    {
        // ─── 1. LISTADO COMPLETO ──────────────────────────────────────────────
        // Columnas calculadas: Total Días, Total Obras, Estado (no existen en BD).

        public DataTable ObtenerTodos()
        {
            const string sql = @"
                SELECT
                    e.id_exhibicion,
                    e.nombre_exhibicion                                      AS [Nombre],
                    CONVERT(NVARCHAR(10), e.fecha_inicio, 103)               AS [Fecha Inicio],
                    CONVERT(NVARCHAR(10), e.fecha_fin,    103)               AS [Fecha Fin],
                    ISNULL(e.tematica, '—')                                  AS [Temática],
                    e.costo_entrada                                          AS [Costo Entrada],
                    DATEDIFF(day, e.fecha_inicio, e.fecha_fin) + 1           AS [Total Días],
                    ISNULL(de.total_obras, 0)                                AS [Total Obras],
                    CASE
                        WHEN CAST(GETDATE() AS DATE) < e.fecha_inicio THEN 'Próxima'
                        WHEN CAST(GETDATE() AS DATE) > e.fecha_fin    THEN 'Finalizada'
                        ELSE 'En curso'
                    END                                                       AS [Estado]
                FROM dbo.EXHIBICION e
                LEFT JOIN (
                    SELECT id_exhibicion, COUNT(*) AS total_obras
                    FROM   dbo.DETALLE_EXHIBICION
                    WHERE  id_exhibicion IS NOT NULL
                    GROUP BY id_exhibicion
                ) de ON e.id_exhibicion = de.id_exhibicion
                ORDER BY e.fecha_inicio DESC";

            using SqlConnection    conn    = Conexion.ObtenerConexion();
            using SqlCommand       cmd     = new(sql, conn);
            using SqlDataAdapter   adapter = new(cmd);
            var tabla = new DataTable();
            adapter.Fill(tabla);
            return tabla;
        }

        // ─── 2. OBTENER POR ID ────────────────────────────────────────────────

        public Exhibicion? ObtenerPorId(int id)
        {
            const string sql =
                "SELECT * FROM dbo.EXHIBICION WHERE id_exhibicion = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();

            using SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Exhibicion
            {
                IdExhibicion     = r.GetInt32(r.GetOrdinal("id_exhibicion")),
                NombreExhibicion = r["nombre_exhibicion"]?.ToString() ?? "",
                FechaInicio      = r.GetDateTime(r.GetOrdinal("fecha_inicio")),
                FechaFin         = r.GetDateTime(r.GetOrdinal("fecha_fin")),
                Tematica         = r["tematica"] == DBNull.Value
                                   ? "" : r["tematica"].ToString()!,
                CostoEntrada     = r["costo_entrada"] == DBNull.Value
                                   ? 0m : Convert.ToDecimal(r["costo_entrada"])
            };
        }

        // ─── 3. INSERTAR ──────────────────────────────────────────────────────

        public void Insertar(Exhibicion e)
        {
            const string sql = @"
                INSERT INTO dbo.EXHIBICION
                    (nombre_exhibicion, fecha_inicio, fecha_fin, tematica, costo_entrada)
                VALUES
                    (@nombre, @fechaInicio, @fechaFin, @tematica, @costoEntrada)";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            AgregarParametros(cmd, e);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ─── 4. ACTUALIZAR ────────────────────────────────────────────────────

        public void Actualizar(Exhibicion e)
        {
            const string sql = @"
                UPDATE dbo.EXHIBICION SET
                    nombre_exhibicion = @nombre,
                    fecha_inicio      = @fechaInicio,
                    fecha_fin         = @fechaFin,
                    tematica          = @tematica,
                    costo_entrada     = @costoEntrada
                WHERE id_exhibicion = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            AgregarParametros(cmd, e);
            cmd.Parameters.AddWithValue("@id", e.IdExhibicion);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ─── 5. ELIMINAR ──────────────────────────────────────────────────────
        // FK es NO_ACTION → primero se eliminan los detalles, luego la cabecera.

        public void Eliminar(int id)
        {
            using SqlConnection  conn = Conexion.ObtenerConexion();
            conn.Open();
            using SqlTransaction tx   = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand(
                    "DELETE FROM dbo.DETALLE_EXHIBICION WHERE id_exhibicion = @id",
                    conn, tx))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SqlCommand(
                    "DELETE FROM dbo.EXHIBICION WHERE id_exhibicion = @id",
                    conn, tx))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // ─── 6. OBRAS DE UNA EXHIBICIÓN ───────────────────────────────────────
        // Incluye id_det_exhibicion como PK para operaciones de borrado.

        public DataTable ObtenerObrasEnExhibicion(int idExhibicion)
        {
            const string sql = @"
                SELECT
                    de.id_det_exhibicion,
                    p.id_pintura,
                    p.titulo                            AS [Título],
                    ISNULL(a.nombre_completo, '—')      AS [Artista],
                    p.precio_base                       AS [Precio Base],
                    ISNULL(p.estado_disponibilidad,'—') AS [Estado]
                FROM dbo.DETALLE_EXHIBICION de
                JOIN  dbo.PINTURA  p ON de.id_pintura = p.id_pintura
                LEFT JOIN dbo.ARTISTA a ON p.id_artista = a.id_artista
                WHERE de.id_exhibicion = @id
                ORDER BY p.titulo";

            using SqlConnection    conn    = Conexion.ObtenerConexion();
            using SqlCommand       cmd     = new(sql, conn);
            using SqlDataAdapter   adapter = new(cmd);
            cmd.Parameters.AddWithValue("@id", idExhibicion);
            var tabla = new DataTable();
            adapter.Fill(tabla);
            return tabla;
        }

        // ─── 7. PINTURAS DISPONIBLES PARA AGREGAR ────────────────────────────
        // Excluye las que ya tienen entrada en DETALLE_EXHIBICION para esta exhibición.

        public DataTable ObtenerPinturasParaAgregar(int idExhibicion)
        {
            const string sql = @"
                SELECT
                    p.id_pintura,
                    p.titulo                             AS [Título],
                    ISNULL(a.nombre_completo, '—')       AS [Artista],
                    p.precio_base                        AS [Precio Base],
                    ISNULL(p.estado_disponibilidad,'—')  AS [Estado]
                FROM dbo.PINTURA p
                LEFT JOIN dbo.ARTISTA a ON p.id_artista = a.id_artista
                WHERE p.id_pintura NOT IN (
                    SELECT id_pintura
                    FROM   dbo.DETALLE_EXHIBICION
                    WHERE  id_exhibicion = @id
                      AND  id_pintura IS NOT NULL
                )
                ORDER BY p.titulo";

            using SqlConnection    conn    = Conexion.ObtenerConexion();
            using SqlCommand       cmd     = new(sql, conn);
            using SqlDataAdapter   adapter = new(cmd);
            cmd.Parameters.AddWithValue("@id", idExhibicion);
            var tabla = new DataTable();
            adapter.Fill(tabla);
            return tabla;
        }

        // ─── 8. AGREGAR OBRA ──────────────────────────────────────────────────

        public void AgregarObra(int idExhibicion, int idPintura)
        {
            const string sql = @"
                INSERT INTO dbo.DETALLE_EXHIBICION (id_exhibicion, id_pintura)
                VALUES (@idExhibicion, @idPintura)";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            cmd.Parameters.AddWithValue("@idExhibicion", idExhibicion);
            cmd.Parameters.AddWithValue("@idPintura",    idPintura);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ─── 9. QUITAR OBRA ───────────────────────────────────────────────────
        // Usa id_det_exhibicion (PK de DETALLE_EXHIBICION) para borrado exacto.

        public void QuitarObra(int idDetExhibicion)
        {
            const string sql =
                "DELETE FROM dbo.DETALLE_EXHIBICION WHERE id_det_exhibicion = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            cmd.Parameters.AddWithValue("@id", idDetExhibicion);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ─── 10. CONTEO DE OBRAS ─────────────────────────────────────────────

        public int ContarObras(int idExhibicion)
        {
            const string sql = @"
                SELECT COUNT(*) FROM dbo.DETALLE_EXHIBICION
                WHERE id_exhibicion = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            cmd.Parameters.AddWithValue("@id", idExhibicion);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        // ─── HELPER PRIVADO ───────────────────────────────────────────────────

        private static void AgregarParametros(SqlCommand cmd, Exhibicion e)
        {
            cmd.Parameters.AddWithValue("@nombre", e.NombreExhibicion);
            cmd.Parameters.Add("@fechaInicio",  SqlDbType.Date).Value = e.FechaInicio.Date;
            cmd.Parameters.Add("@fechaFin",     SqlDbType.Date).Value = e.FechaFin.Date;
            cmd.Parameters.AddWithValue("@tematica",
                string.IsNullOrWhiteSpace(e.Tematica) ? DBNull.Value : (object)e.Tematica);
            cmd.Parameters.AddWithValue("@costoEntrada", e.CostoEntrada);
        }
    }
}
