namespace GaleriaDeArtes.CapaEntidad
{
    public class Pintura
    {
        public int IdPintura { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public string EstadoDisponibilidad { get; set; } = string.Empty;
        public int IdArtista { get; set; }
        public int IdTecnica { get; set; }
    }
}
