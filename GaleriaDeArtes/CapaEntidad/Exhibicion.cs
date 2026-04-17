namespace GaleriaDeArtes.CapaEntidad
{
    /// <summary>
    /// Refleja exactamente la tabla dbo.EXHIBICION de la base de datos.
    /// Columnas: id_exhibicion, nombre_exhibicion, fecha_inicio, fecha_fin,
    ///           tematica (nullable), costo_entrada (nullable, default 0.00)
    /// </summary>
    public class Exhibicion
    {
        public int      IdExhibicion     { get; set; }
        public string   NombreExhibicion { get; set; } = "";
        public DateTime FechaInicio      { get; set; }
        public DateTime FechaFin         { get; set; }
        public string   Tematica         { get; set; } = "";
        public decimal  CostoEntrada     { get; set; }
    }
}
