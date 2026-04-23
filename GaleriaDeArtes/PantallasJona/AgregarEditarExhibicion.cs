using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.CapaNegocio;

namespace GaleriaDeArtes.PantallasJona
{
    /// <summary>
    /// Modal de alta y edición de exhibiciones.
    /// Campos: nombre_exhibicion, fecha_inicio, fecha_fin, hora_inicio, hora_fin.
    /// Patrón idéntico a AgregarEditarArtista: constructor dual + DialogResult.OK.
    /// </summary>
    public class AgregarEditarExhibicion : Form
    {
        // ── Paleta del sistema ────────────────────────────────────────────────
        private static readonly Color Blue = Color.FromArgb(28, 110, 164);

        // ── Controles ─────────────────────────────────────────────────────────
        private TextBox        txtNombre      = null!;
        private DateTimePicker dtpFechaInicio = null!;
        private DateTimePicker dtpFechaFin    = null!;
        private DateTimePicker dtpHoraInicio  = null!;
        private DateTimePicker dtpHoraFin     = null!;

        // ── Estado ────────────────────────────────────────────────────────────
        private readonly ExhibicionBLL _bll;
        private readonly Exhibicion?   _exhibicion;
        private readonly bool          _esEdicion;

        // Constructor para agregar
        public AgregarEditarExhibicion(ExhibicionBLL bll)
        {
            _bll       = bll;
            _esEdicion = false;
            Construir();
            Text = "Agregar Exhibición";
        }

        // Constructor para editar
        public AgregarEditarExhibicion(ExhibicionBLL bll, Exhibicion exhibicion)
        {
            _bll        = bll;
            _exhibicion = exhibicion;
            _esEdicion  = true;
            Construir();
            Text = "Editar Exhibición";
            CargarDatos();
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONSTRUCCIÓN DE LA UI
        // Medidas y posiciones siguen AgregarEditarArtista.Designer.cs.
        // ─────────────────────────────────────────────────────────────────────

        private void Construir()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            StartPosition   = FormStartPosition.CenterParent;
            ClientSize      = new Size(620, 400);
            Font            = new Font("Segoe UI", 10F);
            BackColor       = Color.White;

            Label Lbl(string t, int y) => new Label
            {
                Text     = t,
                Location = new Point(40, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            // ── Nombre exhibición (nombre_exhibicion, varchar 150) ─────────────
            Controls.Add(Lbl("Nombre de la exhibición:*", 43));
            txtNombre = new TextBox
            {
                Location  = new Point(240, 40),
                Size      = new Size(335, 27),
                MaxLength = 150
            };
            Controls.Add(txtNombre);

            // ── Fecha inicio (fecha_inicio, DATE) ─────────────────────────────
            Controls.Add(Lbl("Fecha de inicio:*", 93));
            dtpFechaInicio = new DateTimePicker
            {
                Location = new Point(240, 90),
                Size     = new Size(160, 27),
                Format   = DateTimePickerFormat.Short,
                Value    = DateTime.Today
            };
            Controls.Add(dtpFechaInicio);

            // ── Fecha fin (fecha_fin, DATE) ───────────────────────────────────
            Controls.Add(Lbl("Fecha de fin:*", 143));
            dtpFechaFin = new DateTimePicker
            {
                Location = new Point(240, 140),
                Size     = new Size(160, 27),
                Format   = DateTimePickerFormat.Short,
                Value    = DateTime.Today.AddDays(7)
            };
            Controls.Add(dtpFechaFin);

            // ── Hora inicio (hora_inicio, TIME) ──────────────────────────────
            Controls.Add(Lbl("Hora de inicio:*", 193));
            dtpHoraInicio = new DateTimePicker
            {
                Location   = new Point(240, 190),
                Size       = new Size(160, 27),
                Format     = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Value      = DateTime.Today.AddHours(10)
            };
            Controls.Add(dtpHoraInicio);

            // ── Hora fin (hora_fin, TIME) ─────────────────────────────────────
            Controls.Add(Lbl("Hora de fin:*", 243));
            dtpHoraFin = new DateTimePicker
            {
                Location   = new Point(240, 240),
                Size       = new Size(160, 27),
                Format     = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Value      = DateTime.Today.AddHours(18)
            };
            Controls.Add(dtpHoraFin);

            // ── Botones ───────────────────────────────────────────────────────
            var btnGuardar = new Button
            {
                Text      = "Guardar",
                Location  = new Point(240, 320),
                Size      = new Size(120, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Blue,
                ForeColor = Color.White,
                Cursor    = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;
            Controls.Add(btnGuardar);

            var btnCancelar = new Button
            {
                Text     = "Cancelar",
                Location = new Point(380, 320),
                Size     = new Size(120, 36),
                Cursor   = Cursors.Hand
            };
            btnCancelar.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancelar);

            Controls.Add(new Label
            {
                Text      = "* Campos obligatorios",
                Location  = new Point(40, 330),
                AutoSize  = true,
                ForeColor = Color.FromArgb(130, 130, 130),
                Font      = new Font("Segoe UI", 8.5f)
            });
        }

        // ─────────────────────────────────────────────────────────────────────
        // CARGA DE DATOS (modo edición)
        // ─────────────────────────────────────────────────────────────────────

        private void CargarDatos()
        {
            if (_exhibicion == null) return;

            txtNombre.Text       = _exhibicion.NombreExhibicion;
            dtpFechaInicio.Value = _exhibicion.FechaInicio;
            dtpFechaFin.Value    = _exhibicion.FechaFin;
            dtpHoraInicio.Value  = DateTime.Today.Add(_exhibicion.HoraInicio);
            dtpHoraFin.Value     = DateTime.Today.Add(_exhibicion.HoraFin);
        }

        // ─────────────────────────────────────────────────────────────────────
        // GUARDAR
        // ─────────────────────────────────────────────────────────────────────

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            var exhibicion = new Exhibicion
            {
                IdExhibicion     = _esEdicion && _exhibicion != null ? _exhibicion.IdExhibicion : 0,
                NombreExhibicion = txtNombre.Text.Trim(),
                FechaInicio      = dtpFechaInicio.Value.Date,
                FechaFin         = dtpFechaFin.Value.Date,
                HoraInicio       = dtpHoraInicio.Value.TimeOfDay,
                HoraFin          = dtpHoraFin.Value.TimeOfDay
            };

            var (ok, mensaje) = _esEdicion
                ? _bll.Actualizar(exhibicion)
                : _bll.Insertar(exhibicion);

            if (!ok)
            {
                MessageBox.Show(mensaje, "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(mensaje, "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
