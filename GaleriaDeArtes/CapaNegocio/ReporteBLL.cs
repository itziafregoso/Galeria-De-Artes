using GaleriaDeArtes.CapaDatos;
using GaleriaDeArtes.CapaEntidad.Reportes;

namespace GaleriaDeArtes.CapaNegocio
{
    /// <summary>
    /// Capa de negocio del módulo de reportes.
    /// Centraliza las llamadas al DAL y puede agregar validaciones
    /// o transformaciones adicionales sin tocar la UI ni los datos.
    /// </summary>
    public class ReporteBLL
    {
        private readonly ReporteDAL _dal = new ReporteDAL();

        public List<FilaCatalogoPintura>  ObtenerCatalogoPinturas(FiltroPintura? filtro = null)
            => _dal.ObtenerCatalogoPinturas(filtro);

        public List<FilaResumenArtista>   ObtenerResumenPorArtista()
            => _dal.ObtenerResumenPorArtista();

        public List<FilaResumenTecnica>   ObtenerResumenPorTecnica()
            => _dal.ObtenerResumenPorTecnica();

        public List<FilaInventarioEstado> ObtenerInventarioPorEstado()
            => _dal.ObtenerInventarioPorEstado();

        public ResumenEjecutivo           ObtenerResumenEjecutivo()
            => _dal.ObtenerResumenEjecutivo();

        public List<FilaExhibicion>       ObtenerPinturasPorExhibicion()
            => _dal.ObtenerPinturasPorExhibicion();
    }
}
