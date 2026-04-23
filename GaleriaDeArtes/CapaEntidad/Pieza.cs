namespace GaleriaDeArtes.CapaEntidad
{
    public class Pieza
    {
        public int    IdPieza      { get; set; }
        public int    IdPintura    { get; set; }
        public string Descripcion  { get; set; } = string.Empty;
        public string EstadoFisico { get; set; } = "Disponible";
    }
}
