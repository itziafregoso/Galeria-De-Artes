namespace GaleriaDeArtes.PantallasJona
{
    /// <summary>
    /// Página activa del sistema, usada por MenuLateral para resaltar
    /// el botón correspondiente y evitar navegación redundante.
    /// </summary>
    public enum PaginaActiva { Artistas, Pinturas, Proveedor, Reportes, Exhibiciones }

    /// <summary>
    /// Menú lateral reutilizable. Se instancia pasando la página activa;
    /// el botón de esa página queda resaltado y deshabilitado para no
    /// navegar a sí mismo. Todos los demás delegan en Navegador.Ir().
    /// </summary>
    public class MenuLateral : UserControl
    {
        // ── Paleta del sistema ────────────────────────────────────────────────
        private static readonly Color Navy   = Color.FromArgb(21,  77,  113); // #154D71
        private static readonly Color Blue   = Color.FromArgb(28,  110, 164); // #1C6EA4
        private static readonly Color Sea    = Color.FromArgb(51,  161, 224); // #33A1E0
        private static readonly Color Blanco = Color.White;

        public MenuLateral(PaginaActiva activa)
        {
            Width     = 188;
            Dock      = DockStyle.Left;
            BackColor = Navy;

            // El orden de Add con Dock=Top sigue el patrón establecido en Reportes:
            // primer Control.Add → botón más arriba.
            Controls.Add(Boton("Artistas",     0,   PaginaActiva.Artistas,     activa));
            Controls.Add(Boton("Pinturas",     60,  PaginaActiva.Pinturas,     activa));
            Controls.Add(Boton("Proveedor",    120, PaginaActiva.Proveedor,    activa));
            Controls.Add(Boton("Reportes",     180, PaginaActiva.Reportes,     activa));
            Controls.Add(Boton("Exhibiciones", 240, PaginaActiva.Exhibiciones, activa));
        }

        private static Button Boton(string texto, int top, PaginaActiva pagina, PaginaActiva activa)
        {
            bool esActivo = pagina == activa;

            var btn = new Button
            {
                Text      = texto,
                Top       = top,
                Width     = 188,
                Height    = 60,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Blanco,
                BackColor = esActivo ? Blue : Navy,
                Font      = new Font("Segoe UI", 9.5f, esActivo ? FontStyle.Bold : FontStyle.Regular),
                Cursor    = Cursors.Hand,
                Dock      = DockStyle.Top
            };

            btn.FlatAppearance.BorderSize         = 0;
            btn.FlatAppearance.MouseOverBackColor = Blue;
            btn.FlatAppearance.MouseDownBackColor = Sea;

            // El botón activo no navega; los demás delegan en el servicio.
            if (!esActivo)
                btn.Click += (_, __) => Navegador.Ir(pagina);

            return btn;
        }
    }
}
