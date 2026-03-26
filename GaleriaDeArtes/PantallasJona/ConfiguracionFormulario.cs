using System.Drawing;
using System.Windows.Forms;

namespace GaleriaDeArtes.PantallasJona
{
    public static class ConfiguracionFormulario
    {
        public static void Aplicar(Form formulario)
        {
            // Tamaño base de diseño
            formulario.Size = new Size(1366, 768);
            formulario.MinimumSize = new Size(1200, 700);

            // Posición y estado
            formulario.StartPosition = FormStartPosition.CenterScreen;
         

            // Apariencia general
            formulario.BackColor = Color.White;
            formulario.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // Borde
            formulario.FormBorderStyle = FormBorderStyle.Sizable;

            // Opcional
            formulario.MaximizeBox = true;
            formulario.MinimizeBox = true;
        }
    }
}