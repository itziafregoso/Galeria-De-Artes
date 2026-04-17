using GaleriaDeArtes.CapaDatos;
using GaleriaDeArtes.CapaEntidad;
using System.Data;

namespace GaleriaDeArtes.CapaNegocio
{
    public class ExhibicionBLL
    {
        private readonly ExhibicionDAL _dal = new();

        public DataTable   ObtenerTodos()                              => _dal.ObtenerTodos();
        public Exhibicion? ObtenerPorId(int id)                        => _dal.ObtenerPorId(id);
        public int         ContarObras(int idExhibicion)               => _dal.ContarObras(idExhibicion);
        public DataTable   ObtenerObrasEnExhibicion(int id)            => _dal.ObtenerObrasEnExhibicion(id);
        public DataTable   ObtenerPinturasParaAgregar(int id)          => _dal.ObtenerPinturasParaAgregar(id);
        public void        AgregarObra(int idExhib, int idPintura)     => _dal.AgregarObra(idExhib, idPintura);
        public void        QuitarObra(int idDetExhibicion)             => _dal.QuitarObra(idDetExhibicion);
        public void        Eliminar(int id)                            => _dal.Eliminar(id);

        public (bool Ok, string Mensaje) Insertar(Exhibicion e)
        {
            var v = Validar(e);
            if (!v.Ok) return v;
            _dal.Insertar(e);
            return (true, "Exhibición registrada correctamente.");
        }

        public (bool Ok, string Mensaje) Actualizar(Exhibicion e)
        {
            var v = Validar(e);
            if (!v.Ok) return v;
            _dal.Actualizar(e);
            return (true, "Exhibición actualizada correctamente.");
        }

        // ─── Validaciones de negocio ──────────────────────────────────────────

        private static (bool Ok, string Mensaje) Validar(Exhibicion e)
        {
            if (string.IsNullOrWhiteSpace(e.NombreExhibicion))
                return (false, "El nombre de la exhibición es obligatorio.");

            if (e.NombreExhibicion.Length > 150)
                return (false, "El nombre no puede superar 150 caracteres.");

            if (e.FechaFin < e.FechaInicio)
                return (false, "La fecha de fin no puede ser anterior a la fecha de inicio.");

            if (e.CostoEntrada < 0)
                return (false, "El costo de entrada no puede ser negativo.");

            return (true, "");
        }
    }
}
