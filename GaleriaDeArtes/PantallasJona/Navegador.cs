namespace GaleriaDeArtes.PantallasJona
{
    /// <summary>
    /// Servicio estático de navegación entre formularios principales.
    /// Garantiza que solo un formulario esté visible a la vez y que
    /// cerrar cualquier formulario principal termine la aplicación.
    /// </summary>
    public static class Navegador
    {
        private static Form? _actual;

        // Handler de salida registrado una sola vez en el formulario activo.
        private static readonly FormClosedEventHandler _salir =
            (_, __) => Application.Exit();

        /// <summary>
        /// Llama en Program.cs con el formulario inicial para arrancar el sistema.
        /// </summary>
        public static void Inicializar(Form inicio)
        {
            _actual = inicio;
            inicio.FormClosed += _salir;
        }

        /// <summary>
        /// Navega al formulario indicado: oculta el actual, crea el destino y lo muestra.
        /// Si ya estamos en esa página, no hace nada.
        /// </summary>
        public static void Ir(PaginaActiva destino)
        {
            Form nuevo = destino switch
            {
                PaginaActiva.Artistas      => new global::GaleriaDeArtes.Artistas(),
                PaginaActiva.Pinturas      => new global::GaleriaDeArtes.Pinturas(),
                PaginaActiva.Reportes      => new Reportes(),
                PaginaActiva.Exhibiciones  => new Exhibiciones(),
                PaginaActiva.Proveedor     => null!, // aún no implementado
                _ => null!
            };

            if (nuevo == null) return;

            // Transferir el handler de salida al nuevo formulario
            _actual?.FormClosed -= _salir;
            var anterior = _actual;
            _actual = nuevo;
            _actual.FormClosed += _salir;

            anterior?.Hide();
            _actual.Show();
        }
    }
}
