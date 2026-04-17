using GaleriaDeArtes.CapaNegocio;
using System.Data;

namespace GaleriaDeArtes.PantallasJona
{
    /// <summary>
    /// Modal para gestionar pinturas de una exhibición.
    /// Izquierda: obras actualmente en DETALLE_EXHIBICION para esta exhibición.
    /// Derecha  : pinturas no asociadas aún (disponibles para agregar).
    /// Usa id_det_exhibicion (PK de DETALLE_EXHIBICION) para quitar obras.
    /// </summary>
    public class VerObrasExhibicion : Form
    {
        // ── Paleta del sistema ────────────────────────────────────────────────
        private static readonly Color Navy = Color.FromArgb(21,  77, 113);
        private static readonly Color Blue = Color.FromArgb(28, 110, 164);
        private static readonly Color Gris = Color.FromArgb(224, 224, 224);

        // ── Controles ─────────────────────────────────────────────────────────
        private DataGridView dgvActuales     = null!;
        private DataGridView dgvDisponibles  = null!;

        // ── Estado ────────────────────────────────────────────────────────────
        private readonly ExhibicionBLL _bll;
        private readonly int           _idExhibicion;

        public VerObrasExhibicion(ExhibicionBLL bll, int idExhibicion, string nombreExhibicion)
        {
            _bll          = bll;
            _idExhibicion = idExhibicion;

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            StartPosition   = FormStartPosition.CenterParent;
            ClientSize      = new Size(1000, 580);
            Font            = new Font("Segoe UI", 10F);
            BackColor       = Color.White;
            Text            = $"Obras de la exhibición — {nombreExhibicion}";

            Construir();
            Load += (_, __) => RefrescarGrids();
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONSTRUCCIÓN DE LA UI
        // ─────────────────────────────────────────────────────────────────────

        private void Construir()
        {
            // ── Panel izquierdo: obras actuales ───────────────────────────────
            var grpActuales = new GroupBox
            {
                Text      = "Obras en esta exhibición",
                Location  = new Point(12, 10),
                Size      = new Size(472, 530),
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Navy
            };

            dgvActuales = CrearGrid();
            dgvActuales.Location = new Point(8, 24);
            dgvActuales.Size     = new Size(456, 455);
            grpActuales.Controls.Add(dgvActuales);

            var btnQuitar = new Button
            {
                Text      = "⬅  Quitar obra seleccionada",
                Location  = new Point(8, 486),
                Size      = new Size(456, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(193, 50, 50),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnQuitar.FlatAppearance.BorderSize = 0;
            btnQuitar.Click += BtnQuitar_Click;
            grpActuales.Controls.Add(btnQuitar);

            // ── Panel derecho: pinturas disponibles ───────────────────────────
            var grpDisponibles = new GroupBox
            {
                Text      = "Pinturas disponibles para agregar",
                Location  = new Point(496, 10),
                Size      = new Size(492, 530),
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Navy
            };

            dgvDisponibles = CrearGrid();
            dgvDisponibles.Location = new Point(8, 24);
            dgvDisponibles.Size     = new Size(476, 455);
            grpDisponibles.Controls.Add(dgvDisponibles);

            var btnAgregar = new Button
            {
                Text      = "Agregar a la exhibición  ➡",
                Location  = new Point(8, 486),
                Size      = new Size(476, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Blue,
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregar_Click;
            grpDisponibles.Controls.Add(btnAgregar);

            Controls.Add(grpActuales);
            Controls.Add(grpDisponibles);
        }

        // ─────────────────────────────────────────────────────────────────────
        // DATOS
        // ─────────────────────────────────────────────────────────────────────

        private void RefrescarGrids()
        {
            try
            {
                // Obras actuales — incluye id_det_exhibicion (PK) e id_pintura, ambos ocultos
                var tablaActuales = _bll.ObtenerObrasEnExhibicion(_idExhibicion);
                dgvActuales.DataSource = tablaActuales;
                Ocultar(dgvActuales, "id_det_exhibicion");
                Ocultar(dgvActuales, "id_pintura");

                // Pinturas disponibles — solo id_pintura oculto
                var tablaDisponibles = _bll.ObtenerPinturasParaAgregar(_idExhibicion);
                dgvDisponibles.DataSource = tablaDisponibles;
                Ocultar(dgvDisponibles, "id_pintura");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Ocultar(DataGridView dgv, string columna)
        {
            if (dgv.Columns.Contains(columna))
                dgv.Columns[columna].Visible = false;
        }

        // ─────────────────────────────────────────────────────────────────────
        // EVENTOS
        // ─────────────────────────────────────────────────────────────────────

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (dgvDisponibles.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una pintura para agregar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idPintura = Convert.ToInt32(
                dgvDisponibles.CurrentRow.Cells["id_pintura"].Value);

            try
            {
                _bll.AgregarObra(_idExhibicion, idPintura);
                RefrescarGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar obra:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnQuitar_Click(object? sender, EventArgs e)
        {
            if (dgvActuales.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una obra para quitar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string titulo           = dgvActuales.CurrentRow.Cells["Título"].Value?.ToString() ?? "";
            int    idDetExhibicion  = Convert.ToInt32(
                dgvActuales.CurrentRow.Cells["id_det_exhibicion"].Value);

            if (MessageBox.Show(
                    $"¿Quitar \"{titulo}\" de esta exhibición?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            try
            {
                _bll.QuitarObra(idDetExhibicion);
                RefrescarGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al quitar obra:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPER: crea DataGridView con el estilo del sistema
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
