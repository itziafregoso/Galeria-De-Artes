using GaleriaDeArtes.PantallasJona;
using QuestPDF.Infrastructure;

namespace GaleriaDeArtes
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            ApplicationConfiguration.Initialize();

            var inicio = new Pinturas();
            Navegador.Inicializar(inicio);
            Application.Run(inicio);
        }
    }
}
