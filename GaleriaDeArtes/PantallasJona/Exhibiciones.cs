using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.CapaNegocio;
using System.Data;

namespace GaleriaDeArtes.PantallasJona
{
    /// <summary>
    /// Pantalla principal del módulo de Exhibiciones.
    /// Layout idéntico a Artistas: panel gris + búsqueda + filtro + DGV.
    /// </summary>
    public class Exhibiciones : Form
    {
        // ── Paleta del sistema ────────────────────────────────────────────────
        private static readonly Color Navy = Color.FromArgb(21,  77, 113);
        private static readonly Color Blue = Color.FromArgb(28, 110, 164);
        private static readonly Color Gris = Color.FromArgb(224, 224, 224);

        // ── Controles ─────────────────────────────────────────────────────────
        private DataGridView dgvExhibiciones  = null!;
        private TextBox      txtBuscar        = null!;
        private ComboBox     cboFiltroEstado  = null!;

        // ── Datos ─────────────────────────────────────────────────────────────
        private readonly ExhibicionBLL _bll = new();
        private DataTable _tabla = new();

        public Exhibiciones()
        {
            ConfiguracionFormulario.Aplicar(this);
            Text = "Exhibiciones — Galería de Artes";
            InicializarComponentes();
            Load += (_, __) => CargarExhibiciones();
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONSTRUCCIÓN DE LA INTERFAZ
        // Posiciones tomadas directamente de Artistas.Designer.cs como referencia.
        // ─────────────────────────────────────────────────────────────────────

        private void InicializarComponentes()
        {
            var pnlPrincipal = new Panel { BackColor = Gris, Dock = DockStyle.Fill };

            // ── Buscador ──────────────────────────────────────────────────────
            txtBuscar = new TextBox
            {
                Location        = new Point(60, 60),
                Size            = new Size(240, 27),
                PlaceholderText = "Buscar exhibición...",
                Font            = new Font("Segoe UI", 9.5f)
            };
            txtBuscar.TextChanged += (_, __) => AplicarFiltros();

            // ── Filtro estado ─────────────────────────────────────────────────
            cboFiltroEstado = new ComboBox
            {
                Location      = new Point(312, 60),
                Size          = new Size(145, 27),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 9.5f)
            };
            cboFiltroEstado.Items.AddRange(
                new object[] { "Todas", "Próxima", "En curso", "Finalizada" });
            cboFiltroEstado.SelectedIndex = 0;
            cboFiltroEstado.SelectedIndexChanged += (_, __) => AplicarFiltros();

            // ── Botones de acción ─────────────────────────────────────────────
            var btnAgregar  = CrearBoton("Agregar Exhibición",  new Point(487, 55));
            var btnEditar   = CrearBoton("Editar Exhibición",   new Point(637, 55));
            var btnEliminar = CrearBoton("Eliminar Exhibición", new Point(787, 55));
            var btnVerObras = CrearBoton("Ver Obras",           new Point(937, 55));

            btnAgregar.Click  += BtnAgregar_Click;
            btnEditar.Click   += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVerObras.Click += BtnVerObras_Click;

            // ── DataGridView (estilo idéntico a dgvArtistas) ──────────────────
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

            dgvExhibiciones = new DataGridView
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
                txtBuscar, cboFiltroEstado,
                btnAgregar, btnEditar, btnEliminar, btnVerObras,
                dgvExhibiciones
            });

            Controls.Add(pnlPrincipal);
            Controls.Add(new MenuLateral(PaginaActiva.Exhibiciones));
        }

        // ─────────────────────────────────────────────────────────────────────
        // DATOS
        // ─────────────────────────────────────────────────────────────────────

        private void CargarExhibiciones()
        {
            try
            {
                _tabla = _bll.ObtenerTodos();
                dgvExhibiciones.DataSource = _tabla;

                // Ocultar PK; no tiene sentido mostrarla en el grid
                if (dgvExhibiciones.Columns.Contains("id_exhibicion"))
                    dgvExhibiciones.Columns["id_exhibicion"].Visible = false;

                AjustarPesoColumnas();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar exhibiciones:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AjustarPesoColumnas()
        {
            if (dgvExhibiciones.Columns.Count == 0) return;

            void Peso(string nombre, int peso)
            {
                if (dgvExhibiciones.Columns.Contains(nombre))
                    dgvExhibiciones.Columns[nombre].FillWeight = peso;
            }

            Peso("Nombre",       280);
            Peso("Fecha Inicio",  90);
            Peso("Fecha Fin",     90);
            Peso("Hora Inicio",   80);
            Peso("Hora Fin",      80);
            Peso("Total Obras",   80);
            Peso("Estado",        90);
        }

        // ─────────────────────────────────────────────────────────────────────
        // FILTROS CLIENTE (sobre DataTable, sin nueva consulta SQL)
        // ─────────────────────────────────────────────────────────────────────

        private void AplicarFiltros()
        {
            if (_tabla == null) return;

            string texto  = txtBuscar.Text.Trim().Replace("'", "''");
            string estado = cboFiltroEstado.SelectedItem?.ToString() ?? "Todas";

            var partes = new List<string>();

            if (!string.IsNullOrEmpty(texto))
                partes.Add($"CONVERT([Nombre], System.String) LIKE '%{texto}%'");

            if (estado != "Todas")
                partes.Add($"[Estado] = '{estado}'");

            _tabla.DefaultView.RowFilter = partes.Count > 0
                ? string.Join(" AND ", partes)
                : string.Empty;
        }

        // ─────────────────────────────────────────────────────────────────────
        // EVENTOS BOTONES
        // ─────────────────────────────────────────────────────────────────────

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            var frm = new AgregarEditarExhibicion(_bll);
            if (frm.ShowDialog() == DialogResult.OK)
                CargarExhibiciones();
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (dgvExhibiciones.CurrentRow == null) return;

            int         id    = IdSeleccionado();
            Exhibicion? exhib = _bll.ObtenerPorId(id);
            if (exhib == null) return;

            var frm = new AgregarEditarExhibicion(_bll, exhib);
            if (frm.ShowDialog() == DialogResult.OK)
                CargarExhibiciones();
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvExhibiciones.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una exhibición para eliminar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nombre = dgvExhibiciones.CurrentRow.Cells["Nombre"].Value?.ToString() ?? "";
            int    id     = IdSeleccionado();
            int    obras  = _bll.ContarObras(id);

            string msj = obras > 0
                ? $"La exhibición \"{nombre}\" tiene {obras} obra(s) asociada(s).\n\n" +
                  "Se eliminarán también todos los registros de DETALLE_EXHIBICION.\n¿Continuar?"
                : $"¿Eliminar la exhibición \"{nombre}\"?";

            if (MessageBox.Show(msj, "Confirmar eliminación",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                _bll.Eliminar(id);
                CargarExhibiciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVerObras_Click(object? sender, EventArgs e)
        {
            if (dgvExhibiciones.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una exhibición para gestionar sus obras.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int    id     = IdSeleccionado();
            string nombre = dgvExhibiciones.CurrentRow.Cells["Nombre"].Value?.ToString() ?? "";

            new VerObrasExhibicion(_bll, id, nombre).ShowDialog();
            CargarExhibiciones(); // Actualiza Total Obras si cambió
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPERS
        // ─────────────────────────────────────────────────────────────────────

        private int IdSeleccionado()
            => Convert.ToInt32(dgvExhibiciones.CurrentRow!.Cells["id_exhibicion"].Value);

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
