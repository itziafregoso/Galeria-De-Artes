using GaleriaDeArtes.CapaNegocio;
using System.Data;

namespace GaleriaDeArtes.PantallasJona
{
    /// <summary>
    /// Modal para ver y gestionar el estado físico de cada pieza de una obra.
    /// Se abre desde Inventario al presionar "Ver Piezas".
    /// </summary>
    public class DetallePiezas : Form
    {
        // ── Paleta del sistema ────────────────────────────────────────────────
        private static readonly Color Navy = Color.FromArgb(21,  77, 113);
        private static readonly Color Blue = Color.FromArgb(28, 110, 164);
        private static readonly Color Gris = Color.FromArgb(224, 224, 224);

        // ── Controles ─────────────────────────────────────────────────────────
        private DataGridView dgvPiezas = null!;
        private ComboBox     cboEstado = null!;

        // ── Estado ────────────────────────────────────────────────────────────
        private readonly InventarioBLL _bll;
        private readonly int           _idPintura;

        public DetallePiezas(InventarioBLL bll, int idPintura, string tituloPintura)
        {
            _bll       = bll;
            _idPintura = idPintura;

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            StartPosition   = FormStartPosition.CenterParent;
            ClientSize      = new Size(700, 530);
            Font            = new Font("Segoe UI", 10F);
            BackColor       = Color.White;
            Text            = $"Piezas — {tituloPintura}";

            Construir();
            Load += (_, __) => RefrescarGrid();
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONSTRUCCIÓN DE LA UI
        // ─────────────────────────────────────────────────────────────────────

        private void Construir()
        {
            // ── GroupBox: lista de piezas ─────────────────────────────────────
            var grpPiezas = new GroupBox
            {
                Text      = "Piezas registradas para esta obra",
                Location  = new Point(12, 10),
                Size      = new Size(672, 400),
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Navy
            };

            dgvPiezas          = CrearGrid();
            dgvPiezas.Location = new Point(8, 24);
            dgvPiezas.Size     = new Size(656, 365);
            grpPiezas.Controls.Add(dgvPiezas);

            // ── Panel inferior: cambio de estado ──────────────────────────────
            var pnlCambio = new Panel
            {
                Location  = new Point(12, 422),
                Size      = new Size(672, 70),
                BackColor = Gris
            };

            var lblEstado = new Label
            {
                Text      = "Nuevo estado:",
                Location  = new Point(12, 22),
                Size      = new Size(105, 23),
                Font      = new Font("Segoe UI", 9.5f),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cboEstado = new ComboBox
            {
                Location      = new Point(122, 19),
                Size          = new Size(210, 27),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 9.5f)
            };
            cboEstado.Items.AddRange(new object[]
            {
                "Disponible", "Vendida", "En Exhibición", "Dañada", "En Restauración"
            });
            cboEstado.SelectedIndex = 0;

            var btnActualizar = new Button
            {
                Text      = "Actualizar estado",
                Location  = new Point(344, 16),
                Size      = new Size(160, 34),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9F),
                ForeColor = Color.White,
                BackColor = Navy,
                Cursor    = Cursors.Hand
            };
            btnActualizar.FlatAppearance.BorderSize         = 0;
            btnActualizar.FlatAppearance.MouseOverBackColor = Blue;
            btnActualizar.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 161, 224);
            btnActualizar.Click += BtnActualizar_Click;

            pnlCambio.Controls.AddRange(new Control[] { lblEstado, cboEstado, btnActualizar });

            Controls.Add(grpPiezas);
            Controls.Add(pnlCambio);
        }

        // ─────────────────────────────────────────────────────────────────────
        // DATOS
        // ─────────────────────────────────────────────────────────────────────

        private void RefrescarGrid()
        {
            try
            {
                var tabla = _bll.ObtenerPiezasPorPintura(_idPintura);
                dgvPiezas.DataSource = tabla;

                if (dgvPiezas.Columns.Contains("id_pieza"))
                    dgvPiezas.Columns["id_pieza"].Visible = false;

                // Encabezados amigables
                if (dgvPiezas.Columns.Contains("Descripcion"))
                    dgvPiezas.Columns["Descripcion"].HeaderText = "Descripción";
                if (dgvPiezas.Columns.Contains("EstadoFisico"))
                    dgvPiezas.Columns["EstadoFisico"].HeaderText = "Estado Físico";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar piezas:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // EVENTOS
        // ─────────────────────────────────────────────────────────────────────

        private void BtnActualizar_Click(object? sender, EventArgs e)
        {
            if (dgvPiezas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una pieza para cambiar su estado.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int    idPieza     = Convert.ToInt32(dgvPiezas.CurrentRow.Cells["id_pieza"].Value);
            string nuevoEstado = cboEstado.SelectedItem?.ToString() ?? "Disponible";

            try
            {
                _bll.ActualizarEstadoPieza(idPieza, nuevoEstado);
                RefrescarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPER: DataGridView con estilo del sistema
        // ─────────────────────────────────────────────────────────────────────

        private static DataGridView CrearGrid()
        {
            var headerStyle = new DataGridViewCellStyle
            {
                Alignment          = DataGridViewContentAlignment.MiddleLeft,
                BackColor          = Color.FromArgb(28, 110, 164),
                Font               = new Font("Segoe UI", 9F),
                ForeColor          = Color.White,
                SelectionBackColor = Color.FromArgb(255, 249, 175),
                SelectionForeColor = Color.FromArgb(21, 77, 113),
                WrapMode           = DataGridViewTriState.True
            };

            return new DataGridView
            {
                AutoSizeColumnsMode           = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode              = DataGridViewAutoSizeRowsMode.AllCells,
                BackgroundColor               = Color.FromArgb(224, 224, 224),
                BorderStyle                   = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = headerStyle,
                ColumnHeadersHeightSizeMode   = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                GridColor                     = Color.FromArgb(224, 224, 224),
                ReadOnly                      = true,
                RowHeadersWidth               = 51,
                RowHeadersWidthSizeMode       = DataGridViewRowHeadersWidthSizeMode.DisableResizing,
                SelectionMode                 = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows            = false,
                AllowUserToDeleteRows         = false,
                MultiSelect                   = false
            };
        }
    }
}
