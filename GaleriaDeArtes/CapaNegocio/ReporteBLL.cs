using GaleriaDeArtes.CapaDatos;
using GaleriaDeArtes.CapaEntidad.Reportes;

namespace GaleriaDeArtes.CapaNegocio
{
    /// <summary>
    /// Capa de negocio del módulo de reportes.
    /// Centraliza las llamadas al DAL y puede agregar validaciones
    /// o transformaciones sin tocar la UI ni la capa de datos.
    /// </summary>
    public class ReporteBLL
    {
        private readonly ReporteDAL _dal = new();

        public List<FilaVentaPeriodo>       ObtenerVentasPorPeriodo(FiltroFechas? filtro = null) => _dal.ObtenerVentasPorPeriodo(filtro);
        public List<FilaTopPintura>         ObtenerTopPinturas()                                  => _dal.ObtenerTopPinturas();
        public List<FilaInventario>         ObtenerInventarioActual()                             => _dal.ObtenerInventarioActual();
        public List<FilaCompraPorProveedor> ObtenerComprasPorProveedor()                         => _dal.ObtenerComprasPorProveedor();
        public List<FilaVentaPorCliente>    ObtenerVentasPorCliente()                            => _dal.ObtenerVentasPorCliente();
        public List<FilaVentaPorMes>        ObtenerVentasPorMes(FiltroMes filtro)                 => _dal.ObtenerVentasPorMes(filtro);
    }
}
