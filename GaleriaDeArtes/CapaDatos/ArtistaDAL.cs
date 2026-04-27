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
                    ISNULL(nombre, '')           AS [Nombre],
                    ISNULL(apellido_paterno, '') AS [Apellido Paterno],
                    ISNULL(apellido_materno, '') AS [Apellido Materno],
                    ISNULL(correo, '')           AS [Correo],
                    ISNULL(telefono, '')         AS [Teléfono],
                    fecha_nacimiento             AS [Fecha de Nacimiento]
                FROM dbo.ARTISTA
                ORDER BY apellido_paterno, nombre";

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
                Nombre             = reader["nombre"] == DBNull.Value ? "" : reader["nombre"].ToString()!,
                ApellidoPaterno    = reader["apellido_paterno"] == DBNull.Value ? "" : reader["apellido_paterno"].ToString()!,
                ApellidoMaterno    = reader["apellido_materno"] == DBNull.Value ? "" : reader["apellido_materno"].ToString()!,
                Correo             = reader["correo"] == DBNull.Value ? "" : reader["correo"].ToString()!,
                Telefono           = reader["telefono"] == DBNull.Value ? "" : reader["telefono"].ToString()!,
                FechaNacimiento    = reader["fecha_nacimiento"] == DBNull.Value
                                        ? null
                                        : reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento")),
                Calle              = reader["calle"] == DBNull.Value ? "" : reader["calle"].ToString()!,
                Colonia            = reader["colonia"] == DBNull.Value ? "" : reader["colonia"].ToString()!,
                Ciudad             = reader["ciudad"] == DBNull.Value ? "" : reader["ciudad"].ToString()!,
                CodigoPostal       = reader["codigo_postal"] == DBNull.Value ? "" : reader["codigo_postal"].ToString()!,
                Nacionalidad       = reader["nacionalidad"] == DBNull.Value ? "" : reader["nacionalidad"].ToString()!,
                EstiloPredominante = reader["estilo_predominante"] == DBNull.Value ? "" : reader["estilo_predominante"].ToString()!,
                Biografia          = reader["biografia"] == DBNull.Value ? "" : reader["biografia"].ToString()!
            };
        }

        public void Insertar(Artista a)
        {
            string nombreCompleto = $"{a.Nombre} {a.ApellidoPaterno} {a.ApellidoMaterno}".Trim();

            string query = @"
                INSERT INTO dbo.ARTISTA
                    (nombre_completo, nombre, apellido_paterno, apellido_materno,
                     correo, telefono, fecha_nacimiento,
                     calle, colonia, ciudad, codigo_postal,
                     nacionalidad, estilo_predominante, biografia)
                VALUES
                    (@nombreCompleto, @nombre, @apellidoP, @apellidoM,
                     @correo, @telefono, @fecha,
                     @calle, @colonia, @ciudad, @cp,
                     @nac, @estilo, @bio)";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            AgregarParametros(cmd, a, nombreCompleto);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(Artista a)
        {
            string nombreCompleto = $"{a.Nombre} {a.ApellidoPaterno} {a.ApellidoMaterno}".Trim();

            string query = @"
                UPDATE dbo.ARTISTA SET
                    nombre_completo    = @nombreCompleto,
                    nombre             = @nombre,
                    apellido_paterno   = @apellidoP,
                    apellido_materno   = @apellidoM,
                    correo             = @correo,
                    telefono           = @telefono,
                    fecha_nacimiento   = @fecha,
                    calle              = @calle,
                    colonia            = @colonia,
                    ciudad             = @ciudad,
                    codigo_postal      = @cp,
                    nacionalidad       = @nac,
                    estilo_predominante = @estilo,
                    biografia          = @bio
                WHERE id_artista = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            AgregarParametros(cmd, a, nombreCompleto);
            cmd.Parameters.AddWithValue("@id", a.IdArtista);
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

        private static void AgregarParametros(SqlCommand cmd, Artista a, string nombreCompleto)
        {
            cmd.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);
            cmd.Parameters.AddWithValue("@nombre",    a.Nombre);
            cmd.Parameters.AddWithValue("@apellidoP", a.ApellidoPaterno);
            cmd.Parameters.AddWithValue("@apellidoM", a.ApellidoMaterno);
            cmd.Parameters.AddWithValue("@correo",    string.IsNullOrEmpty(a.Correo)        ? DBNull.Value : (object)a.Correo);
            cmd.Parameters.AddWithValue("@telefono",  string.IsNullOrEmpty(a.Telefono)      ? DBNull.Value : (object)a.Telefono);
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime2).Value =
                a.FechaNacimiento.HasValue ? a.FechaNacimiento.Value : DBNull.Value;
            cmd.Parameters.AddWithValue("@calle",     string.IsNullOrEmpty(a.Calle)         ? DBNull.Value : (object)a.Calle);
            cmd.Parameters.AddWithValue("@colonia",   string.IsNullOrEmpty(a.Colonia)       ? DBNull.Value : (object)a.Colonia);
            cmd.Parameters.AddWithValue("@ciudad",    string.IsNullOrEmpty(a.Ciudad)        ? DBNull.Value : (object)a.Ciudad);
            cmd.Parameters.AddWithValue("@cp",        string.IsNullOrEmpty(a.CodigoPostal)  ? DBNull.Value : (object)a.CodigoPostal);
            cmd.Parameters.AddWithValue("@nac",       string.IsNullOrEmpty(a.Nacionalidad)  ? DBNull.Value : (object)a.Nacionalidad);
            cmd.Parameters.AddWithValue("@estilo",    string.IsNullOrEmpty(a.EstiloPredominante) ? DBNull.Value : (object)a.EstiloPredominante);
            cmd.Parameters.AddWithValue("@bio",       string.IsNullOrEmpty(a.Biografia)     ? DBNull.Value : (object)a.Biografia);
        }
    }
}
