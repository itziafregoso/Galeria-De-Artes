namespace GaleriaDeArtes.CapaEntidad
{
    /// <summary>
    /// Refleja exactamente la tabla dbo.EXHIBICION de la base de datos.
    /// Columnas: id_exhibicion, nombre_exhibicion, fecha_inicio, fecha_fin,
    ///           hora_inicio (TIME), hora_fin (TIME)
    /// </summary>
    public class Exhibicion
    {
        public int      IdExhibicion     { get; set; }
        public string   NombreExhibicion { get; set; } = "";
        public DateTime FechaInicio      { get; set; }
        public DateTime FechaFin         { get; set; }
        public TimeSpan HoraInicio       { get; set; }
        public TimeSpan HoraFin          { get; set; }
    }
}
