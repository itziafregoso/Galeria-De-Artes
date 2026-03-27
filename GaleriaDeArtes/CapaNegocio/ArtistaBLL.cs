using GaleriaDeArtes.CapaDatos;
using GaleriaDeArtes.CapaEntidad;
using System.Data;

namespace GaleriaDeArtes.CapaNegocio
{
    public class ArtistaBLL
    {
        private readonly ArtistaDAL _dal = new ArtistaDAL();

        public DataTable ObtenerTodos() => _dal.ObtenerTodos();

        public Artista? ObtenerPorId(int id) => _dal.ObtenerPorId(id);

        public (bool Ok, string Mensaje) Insertar(Artista a)
        {
            if (string.IsNullOrWhiteSpace(a.NombreCompleto))
                return (false, "El nombre completo es obligatorio.");

            _dal.Insertar(a);
            return (true, "Artista agregado correctamente.");
        }

        public (bool Ok, string Mensaje) Actualizar(Artista a)
        {
            if (string.IsNullOrWhiteSpace(a.NombreCompleto))
                return (false, "El nombre completo es obligatorio.");

            _dal.Actualizar(a);
            return (true, "Artista actualizado correctamente.");
        }

        public int ContarPinturas(int id) => _dal.ContarPinturas(id);

        public void Eliminar(int id, bool eliminarPinturas = false) => _dal.Eliminar(id, eliminarPinturas);
    }
}
