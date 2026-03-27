using GaleriaDeArtes.CapaDatos;
using GaleriaDeArtes.CapaEntidad;
using System.Collections.Generic;

namespace GaleriaDeArtes.CapaNegocio
{
    public class PinturaBLL
    {
        private readonly PinturaDAL _dal = new PinturaDAL();

        public List<Artista>  ObtenerArtistas()  => _dal.ObtenerArtistas();
        public List<Tecnica>  ObtenerTecnicas()  => _dal.ObtenerTecnicas();
        public Pintura?       ObtenerPorId(int id) => _dal.ObtenerPorId(id);

        public (bool Ok, string Mensaje) Insertar(Pintura p)
        {
            if (string.IsNullOrWhiteSpace(p.Titulo))
                return (false, "El título es obligatorio.");
            if (p.PrecioBase <= 0)
                return (false, "El precio debe ser mayor a 0.");
            if (p.IdArtista == 0)
                return (false, "Debe seleccionar un artista.");
            if (p.IdTecnica == 0)
                return (false, "Debe seleccionar una técnica.");

            _dal.Insertar(p);
            return (true, "Pintura agregada correctamente.");
        }

        public (bool Ok, string Mensaje) Actualizar(Pintura p)
        {
            if (string.IsNullOrWhiteSpace(p.Titulo))
                return (false, "El título es obligatorio.");
            if (p.PrecioBase <= 0)
                return (false, "El precio debe ser mayor a 0.");
            if (p.IdArtista == 0)
                return (false, "Debe seleccionar un artista.");
            if (p.IdTecnica == 0)
                return (false, "Debe seleccionar una técnica.");

            _dal.Actualizar(p);
            return (true, "Pintura actualizada correctamente.");
        }

        public void Eliminar(int id) => _dal.Eliminar(id);
    }
}
