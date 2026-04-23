using GaleriaDeArtes.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GaleriaDeArtes.CapaDatos
{
    /// <summary>
    /// Acceso a datos del módulo de Inventario.
    ///
    /// ESQUEMA REQUERIDO (dbo.PIEZA):
    ///   id_pieza      INT IDENTITY PK
    ///   id_pintura    INT FK → PINTURA
    ///   descripcion   NVARCHAR(200) NULL
    ///   estado_fisico NVARCHAR(50)  NOT NULL  -- 'Disponible','Vendida','En Exhibición','Dañada','En Restauración'
    /// </summary>
    public class InventarioDAL
    {
        // ─── 1. INVENTARIO (stock dinámico por piezas disponibles) ────────────
        // Stock = COUNT de piezas con estado_fisico = 'Disponible'.
        // LEFT JOIN en PIEZA para que pinturas sin piezas registradas muestren Stock = 0.

        public DataTable ObtenerInventario()
        {
            const string sql = @"
                SELECT
                    p.id_pintura,
                    p.titulo                                                         AS Obra,
                    ISNULL(t.nombre_tecnica, '—')                                   AS Tecnica,
                    COUNT(CASE WHEN pz.estado_fisico = 'Disponible' THEN 1 END)     AS Stock
                FROM dbo.PINTURA p
                LEFT JOIN dbo.TECNICA t  ON p.id_tecnica  = t.id_tecnica
                LEFT JOIN dbo.PIEZA   pz ON pz.id_pintura = p.id_pintura
                GROUP BY p.id_pintura, p.titulo, t.nombre_tecnica
                ORDER BY p.titulo";

            using SqlConnection  conn = Conexion.ObtenerConexion();
            using SqlDataAdapter da   = new(sql, conn);
            var tabla = new DataTable();
            da.Fill(tabla);
            return tabla;
        }

        // ─── 2. PIEZAS DE UNA PINTURA ─────────────────────────────────────────

        public DataTable ObtenerPiezasPorPintura(int idPintura)
        {
            const string sql = @"
                SELECT
                    id_pieza,
                    ISNULL(descripcion, '—') AS Descripcion,
                    estado_fisico            AS EstadoFisico
                FROM dbo.PIEZA
                WHERE id_pintura = @idPintura
                ORDER BY id_pieza";

            using SqlConnection  conn = Conexion.ObtenerConexion();
            using SqlCommand     cmd  = new(sql, conn);
            cmd.Parameters.AddWithValue("@idPintura", idPintura);
            using SqlDataAdapter da = new(cmd);
            var tabla = new DataTable();
            da.Fill(tabla);
            return tabla;
        }

        // ─── 3. ACTUALIZAR ESTADO DE UNA PIEZA ───────────────────────────────

        public void ActualizarEstadoPieza(int idPieza, string nuevoEstado)
        {
            const string sql = "UPDATE dbo.PIEZA SET estado_fisico = @estado WHERE id_pieza = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand    cmd  = new(sql, conn);
            cmd.Parameters.AddWithValue("@estado", nuevoEstado);
            cmd.Parameters.AddWithValue("@id",     idPieza);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
