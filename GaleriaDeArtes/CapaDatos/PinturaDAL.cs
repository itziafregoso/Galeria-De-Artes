using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace GaleriaDeArtes.CapaDatos
{
    public class PinturaDAL
    {
        public List<Artista> ObtenerArtistas()
        {
            var lista = new List<Artista>();
            string query = "SELECT id_artista, nombre_completo FROM dbo.ARTISTA ORDER BY nombre_completo";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(new Artista
                {
                    IdArtista      = reader.GetInt32(0),
                    NombreCompleto = reader.GetString(1)
                });

            return lista;
        }

        public List<Tecnica> ObtenerTecnicas()
        {
            var lista = new List<Tecnica>();
            string query = "SELECT id_tecnica, nombre_tecnica FROM dbo.TECNICA ORDER BY nombre_tecnica";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(new Tecnica
                {
                    IdTecnica     = reader.GetInt32(0),
                    NombreTecnica = reader.GetString(1)
                });

            return lista;
        }

        public Pintura? ObtenerPorId(int id)
        {
            string query = "SELECT * FROM dbo.PINTURA WHERE id_pintura = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new Pintura
            {
                IdPintura            = reader.GetInt32(reader.GetOrdinal("id_pintura")),
                Titulo               = reader["titulo"]?.ToString() ?? "",
                PrecioBase           = reader.GetDecimal(reader.GetOrdinal("precio_base")),
                EstadoDisponibilidad = reader["estado_disponibilidad"]?.ToString() ?? "",
                IdArtista            = reader.GetInt32(reader.GetOrdinal("id_artista")),
                IdTecnica            = reader.GetInt32(reader.GetOrdinal("id_tecnica")),
                AnioCreacion         = reader["anio_creacion"] == DBNull.Value ? null : reader.GetInt32(reader.GetOrdinal("anio_creacion")),
                Dimensiones          = reader["dimensiones"] == DBNull.Value ? "" : reader["dimensiones"].ToString()!
            };
        }

        public void Insertar(Pintura p)
        {
            string query = @"
                INSERT INTO dbo.PINTURA (titulo, precio_base, estado_disponibilidad, id_artista, id_tecnica, anio_creacion, dimensiones)
                VALUES (@titulo, @precio, @estado, @artista, @tecnica, @anio, @dimensiones)";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@titulo",      p.Titulo);
            cmd.Parameters.AddWithValue("@precio",      p.PrecioBase);
            cmd.Parameters.AddWithValue("@estado",      p.EstadoDisponibilidad);
            cmd.Parameters.AddWithValue("@artista",     p.IdArtista);
            cmd.Parameters.AddWithValue("@tecnica",     p.IdTecnica);
            cmd.Parameters.AddWithValue("@anio",        p.AnioCreacion.HasValue ? p.AnioCreacion.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@dimensiones", string.IsNullOrEmpty(p.Dimensiones) ? DBNull.Value : p.Dimensiones);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(Pintura p)
        {
            string query = @"
                UPDATE dbo.PINTURA SET
                    titulo                = @titulo,
                    precio_base           = @precio,
                    estado_disponibilidad = @estado,
                    id_artista            = @artista,
                    id_tecnica            = @tecnica,
                    anio_creacion         = @anio,
                    dimensiones           = @dimensiones
                WHERE id_pintura = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@titulo",      p.Titulo);
            cmd.Parameters.AddWithValue("@precio",      p.PrecioBase);
            cmd.Parameters.AddWithValue("@estado",      p.EstadoDisponibilidad);
            cmd.Parameters.AddWithValue("@artista",     p.IdArtista);
            cmd.Parameters.AddWithValue("@tecnica",     p.IdTecnica);
            cmd.Parameters.AddWithValue("@anio",        p.AnioCreacion.HasValue ? p.AnioCreacion.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@dimensiones", string.IsNullOrEmpty(p.Dimensiones) ? DBNull.Value : p.Dimensiones);
            cmd.Parameters.AddWithValue("@id",          p.IdPintura);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            string query = "DELETE FROM dbo.PINTURA WHERE id_pintura = @id";

            using SqlConnection conn = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
