using GaleriaDeArtes.CapaDatos;
using GaleriaDeArtes.CapaEntidad;
using System.Data;
using System.Text.RegularExpressions;

namespace GaleriaDeArtes.CapaNegocio
{
    public class ArtistaBLL
    {
        private readonly ArtistaDAL _dal = new ArtistaDAL();

        public DataTable ObtenerTodos() => _dal.ObtenerTodos();

        public Artista? ObtenerPorId(int id) => _dal.ObtenerPorId(id);

        public (bool Ok, string Mensaje) Insertar(Artista a)
        {
            var (ok, msg) = Validar(a);
            if (!ok) return (false, msg);
            _dal.Insertar(a);
            return (true, "Artista agregado correctamente.");
        }

        public (bool Ok, string Mensaje) Actualizar(Artista a)
        {
            var (ok, msg) = Validar(a);
            if (!ok) return (false, msg);
            _dal.Actualizar(a);
            return (true, "Artista actualizado correctamente.");
        }

        public int ContarPinturas(int id) => _dal.ContarPinturas(id);

        public void Eliminar(int id, bool eliminarPinturas = false) => _dal.Eliminar(id, eliminarPinturas);

        private static (bool Ok, string Mensaje) Validar(Artista a)
        {
            if (string.IsNullOrWhiteSpace(a.Nombre))
                return (false, "El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(a.ApellidoPaterno))
                return (false, "El apellido paterno es obligatorio.");

            if (string.IsNullOrWhiteSpace(a.ApellidoMaterno))
                return (false, "El apellido materno es obligatorio.");

            if (string.IsNullOrWhiteSpace(a.Correo))
                return (false, "El correo electrónico es obligatorio.");

            if (!Regex.IsMatch(a.Correo.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "El formato del correo electrónico no es válido.");

            if (string.IsNullOrWhiteSpace(a.Telefono))
                return (false, "El teléfono es obligatorio.");

            if (!Regex.IsMatch(a.Telefono.Trim(), @"^\d+$"))
                return (false, "El teléfono solo debe contener números.");

            if (!a.FechaNacimiento.HasValue)
                return (false, "La fecha de nacimiento es obligatoria.");

            if (string.IsNullOrWhiteSpace(a.Calle))
                return (false, "La calle es obligatoria.");

            if (string.IsNullOrWhiteSpace(a.Colonia))
                return (false, "La colonia es obligatoria.");

            if (string.IsNullOrWhiteSpace(a.Ciudad))
                return (false, "La ciudad es obligatoria.");

            if (string.IsNullOrWhiteSpace(a.CodigoPostal))
                return (false, "El código postal es obligatorio.");

            return (true, string.Empty);
        }
    }
}
