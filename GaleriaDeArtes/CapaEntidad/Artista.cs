using System;

namespace GaleriaDeArtes.CapaEntidad
{
    public class Artista
    {
        public int IdArtista { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;

        // Settable para compatibilidad con PinturaDAL.ObtenerArtistas(); se recalcula al guardar.
        public string NombreCompleto { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public int Edad => FechaNacimiento.HasValue ? CalcularEdad(FechaNacimiento.Value) : 0;

        // Dirección
        public string Calle { get; set; } = string.Empty;
        public string Colonia { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;

        // Campos heredados — se conservan para pinturas ya registradas
        public string Nacionalidad { get; set; } = string.Empty;
        public string EstiloPredominante { get; set; } = string.Empty;
        public string Biografia { get; set; } = string.Empty;

        private static int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            int edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }
    }
}
