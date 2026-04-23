using GaleriaDeArtes.CapaDatos;
using System.Data;

namespace GaleriaDeArtes.CapaNegocio
{
    public class InventarioBLL
    {
        private readonly InventarioDAL _dal = new();

        public DataTable ObtenerInventario()
            => _dal.ObtenerInventario();

        public DataTable ObtenerPiezasPorPintura(int idPintura)
            => _dal.ObtenerPiezasPorPintura(idPintura);

        public void ActualizarEstadoPieza(int idPieza, string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEstado))
                throw new ArgumentException("El estado no puede estar vacío.");

            _dal.ActualizarEstadoPieza(idPieza, nuevoEstado);
        }
    }
}
