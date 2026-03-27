using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace GaleriaDeArtes.CapaDatos
{
    public class ArtistaDAL
    {
        public DataTable ObtenerTodos()
        {
            string query = @"
                SELECT
                    id_artista,
                    nombre_completo     AS [Nombre Completo],
                    nacionalidad        AS [Nacionalidad],
                    fecha_nacimiento    AS [Fecha de Nacimiento],
                    estilo_predominante AS [Estilo Predominante],
                    biografia           AS [Biografía]
                FROM dbo.ARTISTA";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            var tabla = new DataTable();
            adapter.Fill(tabla);
            return tabla;
        }

        public Artista? ObtenerPorId(int id)
        {
            string query = "SELECT * FROM dbo.ARTISTA WHERE id_artista = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new Artista
            {
                IdArtista          = reader.GetInt32(reader.GetOrdinal("id_artista")),
                NombreCompleto     = reader["nombre_completo"]?.ToString() ?? "",
                Nacionalidad       = reader["nacionalidad"] == DBNull.Value ? "" : reader["nacionalidad"].ToString()!,
                FechaNacimiento    = reader["fecha_nacimiento"] == DBNull.Value
                                        ? null
                                        : reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento")),
                EstiloPredominante = reader["estilo_predominante"] == DBNull.Value ? "" : reader["estilo_predominante"].ToString()!,
                Biografia          = reader["biografia"] == DBNull.Value ? "" : reader["biografia"].ToString()!
            };
        }

        public void Insertar(Artista a)
        {
            string query = @"
                INSERT INTO dbo.ARTISTA
                    (nombre_completo, nacionalidad, fecha_nacimiento, estilo_predominante, biografia)
                VALUES
                    (@nombre, @nac, @fecha, @estilo, @bio)";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nombre", a.NombreCompleto);
            cmd.Parameters.AddWithValue("@nac",    string.IsNullOrEmpty(a.Nacionalidad)       ? DBNull.Value : a.Nacionalidad);
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime2).Value = a.FechaNacimiento.HasValue ? a.FechaNacimiento.Value : DBNull.Value;
            cmd.Parameters.AddWithValue("@estilo", string.IsNullOrEmpty(a.EstiloPredominante) ? DBNull.Value : a.EstiloPredominante);
            cmd.Parameters.AddWithValue("@bio",    string.IsNullOrEmpty(a.Biografia)           ? DBNull.Value : a.Biografia);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(Artista a)
        {
            string query = @"
                UPDATE dbo.ARTISTA SET
                    nombre_completo     = @nombre,
                    nacionalidad        = @nac,
                    fecha_nacimiento    = @fecha,
                    estilo_predominante = @estilo,
                    biografia           = @bio
                WHERE id_artista = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nombre", a.NombreCompleto);
            cmd.Parameters.AddWithValue("@nac",    string.IsNullOrEmpty(a.Nacionalidad)       ? DBNull.Value : a.Nacionalidad);
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime2).Value = a.FechaNacimiento.HasValue ? a.FechaNacimiento.Value : DBNull.Value;
            cmd.Parameters.AddWithValue("@estilo", string.IsNullOrEmpty(a.EstiloPredominante) ? DBNull.Value : a.EstiloPredominante);
            cmd.Parameters.AddWithValue("@bio",    string.IsNullOrEmpty(a.Biografia)           ? DBNull.Value : a.Biografia);
            cmd.Parameters.AddWithValue("@id",     a.IdArtista);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public int ContarPinturas(int idArtista)
        {
            string query = "SELECT COUNT(*) FROM dbo.PINTURA WHERE id_artista = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", idArtista);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void Eliminar(int id, bool eliminarPinturas = false)
        {
            using SqlConnection conn = Conexion.ObtenerConexion();
            conn.Open();
            using SqlTransaction tx = conn.BeginTransaction();
            try
            {
                if (eliminarPinturas)
                {
                    using SqlCommand cmdPinturas = new SqlCommand(
                        "DELETE FROM dbo.PINTURA WHERE id_artista = @id", conn, tx);
                    cmdPinturas.Parameters.AddWithValue("@id", id);
                    cmdPinturas.ExecuteNonQuery();
                }

                using SqlCommand cmdArtista = new SqlCommand(
                    "DELETE FROM dbo.ARTISTA WHERE id_artista = @id", conn, tx);
                cmdArtista.Parameters.AddWithValue("@id", id);
                cmdArtista.ExecuteNonQuery();

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}
