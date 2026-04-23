using GaleriaDeArtes.CapaNegocio;
using System.Data;

namespace GaleriaDeArtes.PantallasJona
{
    /// <summary>
    /// Módulo de Inventario.
    /// Muestra cada pintura con su stock calculado dinámicamente:
    /// COUNT de piezas donde estado_fisico = 'Disponible'.
    /// Botón "Ver Piezas" abre DetallePiezas para gestionar estado por pieza.
    /// </summary>
    public class Inventario : Form
    {
        // ── Paleta del sistema ────────────────────────────────────────────────
        private static readonly Color Navy = Color.FromArgb(21,  77, 113);
        private static readonly Color Blue = Color.FromArgb(28, 110, 164);
        private static readonly Color Gris = Color.FromArgb(224, 224, 224);

        // ── Controles ─────────────────────────────────────────────────────────
        private DataGridView dgvInventario = null!;
        private TextBox      txtBuscar     = null!;

        // ── Datos ─────────────────────────────────────────────────────────────
        private readonly InventarioBLL _bll = new();
        private DataTable _tabla = new();

        public Inventario()
        {
            ConfiguracionFormulario.Aplicar(this);
            Text = "Inventario — Galería de Artes";
            InicializarComponentes();
            Load += (_, __) => CargarInventario();
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONSTRUCCIÓN DE LA INTERFAZ
        // ─────────────────────────────────────────────────────────────────────

        private void InicializarComponentes()
        {
            var pnlPrincipal = new Panel { BackColor = Gris, Dock = DockStyle.Fill };

            // ── Buscador ──────────────────────────────────────────────────────
            txtBuscar = new TextBox
            {
                Location        = new Point(60, 60),
                Size            = new Size(280, 27),
                PlaceholderText = "Buscar por obra o técnica...",
                Font            = new Font("Segoe UI", 9.5f)
            };
            txtBuscar.TextChanged += (_, __) => AplicarFiltro();

            // ── Botón Ver Piezas ──────────────────────────────────────────────
            var btnVerPiezas = CrearBoton("Ver Piezas", new Point(360, 55));
            btnVerPiezas.Click += BtnVerPiezas_Click;

            // ── DataGridView ──────────────────────────────────────────────────
            var headerStyle = new DataGridViewCellStyle
            {
                Alignment          = DataGridViewContentAlignment.MiddleLeft,
                BackColor          = Blue,
                Font               = new Font("Segoe UI", 9F),
                ForeColor          = Color.White,
                SelectionBackColor = Color.FromArgb(255, 249, 175),
                SelectionForeColor = Navy,
                WrapMode           = DataGridViewTriState.True
            };

            dgvInventario = new DataGridView
            {
                Location                      = new Point(60, 121),
                Size                          = new Size(1025, 440),
                AutoSizeColumnsMode           = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode              = DataGridViewAutoSizeRowsMode.AllCells,
                BackgroundColor               = Gris,
                BorderStyle                   = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = headerStyle,
                ColumnHeadersHeightSizeMode   = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                GridColor                     = Gris,
                ReadOnly                      = true,
                RowHeadersWidth               = 51,
                RowHeadersWidthSizeMode       = DataGridViewRowHeadersWidthSizeMode.DisableResizing,
                SelectionMode                 = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows            = false,
                AllowUserToDeleteRows         = false,
                MultiSelect                   = false
            };

            pnlPrincipal.Controls.AddRange(new Control[]
            {
                txtBuscar, btnVerPiezas, dgvInventario
            });

            Controls.Add(pnlPrincipal);
            Controls.Add(new MenuLateral(PaginaActiva.Inventario));
        }

        // ─────────────────────────────────────────────────────────────────────
        // DATOS
        // ─────────────────────────────────────────────────────────────────────

        private void CargarInventario()
        {
            try
            {
                _tabla = _bll.ObtenerInventario();
                dgvInventario.DataSource = _tabla;

                if (dgvInventario.Columns.Contains("id_pintura"))
                    dgvInventario.Columns["id_pintura"].Visible = false;

                // Dar encabezados amigables
                if (dgvInventario.Columns.Contains("Tecnica"))
                    dgvInventario.Columns["Tecnica"].HeaderText = "Técnica";

                AjustarColumnas();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar inventario:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AjustarColumnas()
        {
            if (dgvInventario.Columns.Count == 0) return;

            void Peso(string nombre, int peso)
            {
                if (dgvInventario.Columns.Contains(nombre))
                    dgvInventario.Columns[nombre].FillWeight = peso;
            }

            Peso("Obra",    450);
            Peso("Tecnica",  250);
            Peso("Stock",     80);
        }

        // ─────────────────────────────────────────────────────────────────────
        // FILTRO (cliente, sin nueva consulta SQL)
        // ─────────────────────────────────────────────────────────────────────

        private void AplicarFiltro()
        {
            if (_tabla == null) return;

            string texto = txtBuscar.Text.Trim().Replace("'", "''");

            _tabla.DefaultView.RowFilter = string.IsNullOrEmpty(texto)
                ? string.Empty
                : $"CONVERT([Obra], System.String) LIKE '%{texto}%' OR " +
                  $"CONVERT([Tecnica], System.String) LIKE '%{texto}%'";
        }

        // ─────────────────────────────────────────────────────────────────────
        // EVENTOS
        // ─────────────────────────────────────────────────────────────────────

        private void BtnVerPiezas_Click(object? sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una obra para ver sus piezas.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int    idPintura = IdSeleccionado();
            string titulo    = dgvInventario.CurrentRow.Cells["Obra"].Value?.ToString() ?? "";

            new DetallePiezas(_bll, idPintura, titulo).ShowDialog();
            CargarInventario(); // Refresca el stock si cambió algún estado
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPERS
        // ─────────────────────────────────────────────────────────────────────

        private int IdSeleccionado()
            => Convert.ToInt32(dgvInventario.CurrentRow!.Cells["id_pintura"].Value);

        private Button CrearBoton(string texto, Point ubicacion)
        {
            var btn = new Button
            {
                Text      = texto,
                Location  = ubicacion,
                Size      = new Size(143, 36),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9F),
                ForeColor = Color.White,
                BackColor = Navy,
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize         = 0;
            btn.FlatAppearance.MouseOverBackColor = Blue;
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 161, 224);
            return btn;
        }
    }
}
