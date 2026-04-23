using GaleriaDeArtes.CapaDatos;
using GaleriaDeArtes.CapaEntidad.Reportes;

namespace GaleriaDeArtes.CapaNegocio
{
    public class ReporteBLL
    {
        private readonly ReporteDAL _dal = new();

        public List<FilaVentaPeriodo>       ObtenerVentasPorPeriodo(FiltroFechas? filtro = null)    => _dal.ObtenerVentasPorPeriodo(filtro);
        public List<FilaTopPintura>         ObtenerTopPinturas(FiltroFechas? filtro = null)          => _dal.ObtenerTopPinturas(filtro);
        public List<FilaInventario>         ObtenerInventarioActual()                                => _dal.ObtenerInventarioActual();
        public List<FilaCompraPorProveedor> ObtenerComprasPorProveedor(FiltroFechas? filtro = null)  => _dal.ObtenerComprasPorProveedor(filtro);
        public List<FilaVentaPorCliente>    ObtenerVentasPorCliente(FiltroFechas? filtro = null)     => _dal.ObtenerVentasPorCliente(filtro);
        public List<FilaVentaPorMes>        ObtenerVentasPorMes(FiltroMes filtro)                    => _dal.ObtenerVentasPorMes(filtro);
    }
}
