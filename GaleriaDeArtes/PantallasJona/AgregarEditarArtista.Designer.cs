namespace GaleriaDeArtes
{
    partial class AgregarEditarArtista
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader       = new Panel();
            lblTitulo       = new Label();
            lblSeccion1     = new Label();
            lblNombre       = new Label();
            txtNombre       = new TextBox();
            lblApellidoP    = new Label();
            txtApellidoP    = new TextBox();
            lblApellidoM    = new Label();
            txtApellidoM    = new TextBox();
            lblCorreo       = new Label();
            txtCorreo       = new TextBox();
            lblTelefono     = new Label();
            txtTelefono     = new TextBox();
            lblFecha        = new Label();
            dtpFecha        = new DateTimePicker();
            lblEdad         = new Label();
            txtEdad         = new TextBox();
            lblSeccion2     = new Label();
            lblCalle        = new Label();
            txtCalle        = new TextBox();
            lblColonia      = new Label();
            txtColonia      = new TextBox();
            lblCiudad       = new Label();
            txtCiudad       = new TextBox();
            lblCodigoPostal = new Label();
            txtCodigoPostal = new TextBox();
            btnGuardar      = new Button();
            btnCancelar     = new Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ──────────────────────────────────────────────
            pnlHeader.BackColor = Color.FromArgb(28, 110, 164);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Dock      = DockStyle.Top;
            pnlHeader.Height    = 65;
            pnlHeader.Name      = "pnlHeader";
            pnlHeader.TabIndex  = 0;

            // ── lblTitulo ──────────────────────────────────────────────
            lblTitulo.AutoSize  = false;
            lblTitulo.Dock      = DockStyle.Fill;
            lblTitulo.Font      = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Name      = "lblTitulo";
            lblTitulo.TabIndex  = 0;
            lblTitulo.Text      = "Artista";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            // ── lblSeccion1 ────────────────────────────────────────────
            lblSeccion1.AutoSize  = false;
            lblSeccion1.BackColor = Color.FromArgb(224, 237, 249);
            lblSeccion1.Font      = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSeccion1.ForeColor = Color.FromArgb(21, 77, 113);
            lblSeccion1.Location  = new Point(30, 82);
            lblSeccion1.Name      = "lblSeccion1";
            lblSeccion1.Size      = new Size(555, 22);
            lblSeccion1.TabIndex  = 1;
            lblSeccion1.Text      = "  INFORMACIÓN PERSONAL";
            lblSeccion1.TextAlign = ContentAlignment.MiddleLeft;

            // ── Fila 1: Nombre ─────────────────────────────────────────
            lblNombre.AutoSize  = false;
            lblNombre.Location  = new Point(30, 117);
            lblNombre.Name      = "lblNombre";
            lblNombre.Size      = new Size(170, 27);
            lblNombre.TabIndex  = 2;
            lblNombre.Text      = "Nombre: *";
            lblNombre.TextAlign = ContentAlignment.MiddleRight;

            txtNombre.Location = new Point(210, 114);
            txtNombre.Name     = "txtNombre";
            txtNombre.Size     = new Size(375, 27);
            txtNombre.TabIndex = 0;

            // ── Fila 2: Apellido Paterno ───────────────────────────────
            lblApellidoP.AutoSize  = false;
            lblApellidoP.Location  = new Point(30, 162);
            lblApellidoP.Name      = "lblApellidoP";
            lblApellidoP.Size      = new Size(170, 27);
            lblApellidoP.TabIndex  = 3;
            lblApellidoP.Text      = "Apellido Paterno: *";
            lblApellidoP.TextAlign = ContentAlignment.MiddleRight;

            txtApellidoP.Location = new Point(210, 159);
            txtApellidoP.Name     = "txtApellidoP";
            txtApellidoP.Size     = new Size(375, 27);
            txtApellidoP.TabIndex = 1;

            // ── Fila 3: Apellido Materno ───────────────────────────────
            lblApellidoM.AutoSize  = false;
            lblApellidoM.Location  = new Point(30, 207);
            lblApellidoM.Name      = "lblApellidoM";
            lblApellidoM.Size      = new Size(170, 27);
            lblApellidoM.TabIndex  = 4;
            lblApellidoM.Text      = "Apellido Materno: *";
            lblApellidoM.TextAlign = ContentAlignment.MiddleRight;

            txtApellidoM.Location = new Point(210, 204);
            txtApellidoM.Name     = "txtApellidoM";
            txtApellidoM.Size     = new Size(375, 27);
            txtApellidoM.TabIndex = 2;

            // ── Fila 4: Correo ─────────────────────────────────────────
            lblCorreo.AutoSize  = false;
            lblCorreo.Location  = new Point(30, 252);
            lblCorreo.Name      = "lblCorreo";
            lblCorreo.Size      = new Size(170, 27);
            lblCorreo.TabIndex  = 5;
            lblCorreo.Text      = "Correo electrónico: *";
            lblCorreo.TextAlign = ContentAlignment.MiddleRight;

            txtCorreo.Location = new Point(210, 249);
            txtCorreo.Name     = "txtCorreo";
            txtCorreo.Size     = new Size(375, 27);
            txtCorreo.TabIndex = 3;

            // ── Fila 5: Teléfono ───────────────────────────────────────
            lblTelefono.AutoSize  = false;
            lblTelefono.Location  = new Point(30, 297);
            lblTelefono.Name      = "lblTelefono";
            lblTelefono.Size      = new Size(170, 27);
            lblTelefono.TabIndex  = 6;
            lblTelefono.Text      = "Teléfono: *";
            lblTelefono.TextAlign = ContentAlignment.MiddleRight;

            txtTelefono.Location = new Point(210, 294);
            txtTelefono.Name     = "txtTelefono";
            txtTelefono.Size     = new Size(200, 27);
            txtTelefono.TabIndex = 4;
            txtTelefono.MaxLength = 15;
            txtTelefono.KeyPress += txtTelefono_KeyPress;

            // ── Fila 6: Fecha de nacimiento ────────────────────────────
            lblFecha.AutoSize  = false;
            lblFecha.Location  = new Point(30, 342);
            lblFecha.Name      = "lblFecha";
            lblFecha.Size      = new Size(170, 27);
            lblFecha.TabIndex  = 7;
            lblFecha.Text      = "Fecha de nacimiento: *";
            lblFecha.TextAlign = ContentAlignment.MiddleRight;

            dtpFecha.Checked      = false;
            dtpFecha.CustomFormat = "dd/MM/yyyy";
            dtpFecha.Format       = DateTimePickerFormat.Custom;
            dtpFecha.Location     = new Point(210, 339);
            dtpFecha.Name         = "dtpFecha";
            dtpFecha.ShowCheckBox = true;
            dtpFecha.Size         = new Size(200, 27);
            dtpFecha.TabIndex     = 5;
            dtpFecha.ValueChanged += dtpFecha_ValueChanged;

            // ── Fila 7: Edad ───────────────────────────────────────────
            lblEdad.AutoSize  = false;
            lblEdad.Location  = new Point(30, 387);
            lblEdad.Name      = "lblEdad";
            lblEdad.Size      = new Size(170, 27);
            lblEdad.TabIndex  = 8;
            lblEdad.Text      = "Edad (años):";
            lblEdad.TextAlign = ContentAlignment.MiddleRight;

            txtEdad.BackColor  = Color.FromArgb(235, 235, 235);
            txtEdad.Location   = new Point(210, 384);
            txtEdad.Name       = "txtEdad";
            txtEdad.ReadOnly   = true;
            txtEdad.Size       = new Size(80, 27);
            txtEdad.TabIndex   = 6;
            txtEdad.TabStop    = false;

            // ── lblSeccion2 ────────────────────────────────────────────
            lblSeccion2.AutoSize  = false;
            lblSeccion2.BackColor = Color.FromArgb(224, 237, 249);
            lblSeccion2.Font      = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSeccion2.ForeColor = Color.FromArgb(21, 77, 113);
            lblSeccion2.Location  = new Point(30, 430);
            lblSeccion2.Name      = "lblSeccion2";
            lblSeccion2.Size      = new Size(555, 22);
            lblSeccion2.TabIndex  = 9;
            lblSeccion2.Text      = "  DIRECCIÓN";
            lblSeccion2.TextAlign = ContentAlignment.MiddleLeft;

            // ── Fila 8: Calle ──────────────────────────────────────────
            lblCalle.AutoSize  = false;
            lblCalle.Location  = new Point(30, 465);
            lblCalle.Name      = "lblCalle";
            lblCalle.Size      = new Size(170, 27);
            lblCalle.TabIndex  = 10;
            lblCalle.Text      = "Calle: *";
            lblCalle.TextAlign = ContentAlignment.MiddleRight;

            txtCalle.Location = new Point(210, 462);
            txtCalle.Name     = "txtCalle";
            txtCalle.Size     = new Size(375, 27);
            txtCalle.TabIndex = 7;

            // ── Fila 9: Colonia ────────────────────────────────────────
            lblColonia.AutoSize  = false;
            lblColonia.Location  = new Point(30, 510);
            lblColonia.Name      = "lblColonia";
            lblColonia.Size      = new Size(170, 27);
            lblColonia.TabIndex  = 11;
            lblColonia.Text      = "Colonia: *";
            lblColonia.TextAlign = ContentAlignment.MiddleRight;

            txtColonia.Location = new Point(210, 507);
            txtColonia.Name     = "txtColonia";
            txtColonia.Size     = new Size(375, 27);
            txtColonia.TabIndex = 8;

            // ── Fila 10: Ciudad ────────────────────────────────────────
            lblCiudad.AutoSize  = false;
            lblCiudad.Location  = new Point(30, 555);
            lblCiudad.Name      = "lblCiudad";
            lblCiudad.Size      = new Size(170, 27);
            lblCiudad.TabIndex  = 12;
            lblCiudad.Text      = "Ciudad: *";
            lblCiudad.TextAlign = ContentAlignment.MiddleRight;

            txtCiudad.Location = new Point(210, 552);
            txtCiudad.Name     = "txtCiudad";
            txtCiudad.Size     = new Size(375, 27);
            txtCiudad.TabIndex = 9;

            // ── Fila 11: Código Postal ─────────────────────────────────
            lblCodigoPostal.AutoSize  = false;
            lblCodigoPostal.Location  = new Point(30, 600);
            lblCodigoPostal.Name      = "lblCodigoPostal";
            lblCodigoPostal.Size      = new Size(170, 27);
            lblCodigoPostal.TabIndex  = 13;
            lblCodigoPostal.Text      = "Código Postal: *";
            lblCodigoPostal.TextAlign = ContentAlignment.MiddleRight;

            txtCodigoPostal.Location  = new Point(210, 597);
            txtCodigoPostal.MaxLength  = 10;
            txtCodigoPostal.Name       = "txtCodigoPostal";
            txtCodigoPostal.Size       = new Size(120, 27);
            txtCodigoPostal.TabIndex   = 10;

            // ── Botones ────────────────────────────────────────────────
            btnGuardar.BackColor = Color.FromArgb(28, 110, 164);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(21, 77, 113);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.Location  = new Point(210, 648);
            btnGuardar.Name      = "btnGuardar";
            btnGuardar.Size      = new Size(140, 38);
            btnGuardar.TabIndex  = 11;
            btnGuardar.Text      = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;

            btnCancelar.Location = new Point(365, 648);
            btnCancelar.Name     = "btnCancelar";
            btnCancelar.Size     = new Size(140, 38);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text     = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;

            // ── Form ───────────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode       = AutoScaleMode.Font;
            BackColor           = Color.White;
            ClientSize          = new Size(630, 708);
            FormBorderStyle     = FormBorderStyle.FixedDialog;
            MaximizeBox         = false;
            MinimizeBox         = false;
            Name                = "AgregarEditarArtista";
            StartPosition       = FormStartPosition.CenterParent;
            Text                = "Artista";
            Load               += AgregarEditarArtista_Load;

            Controls.Add(pnlHeader);
            Controls.Add(lblSeccion1);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblApellidoP);
            Controls.Add(txtApellidoP);
            Controls.Add(lblApellidoM);
            Controls.Add(txtApellidoM);
            Controls.Add(lblCorreo);
            Controls.Add(txtCorreo);
            Controls.Add(lblTelefono);
            Controls.Add(txtTelefono);
            Controls.Add(lblFecha);
            Controls.Add(dtpFecha);
            Controls.Add(lblEdad);
            Controls.Add(txtEdad);
            Controls.Add(lblSeccion2);
            Controls.Add(lblCalle);
            Controls.Add(txtCalle);
            Controls.Add(lblColonia);
            Controls.Add(txtColonia);
            Controls.Add(lblCiudad);
            Controls.Add(txtCiudad);
            Controls.Add(lblCodigoPostal);
            Controls.Add(txtCodigoPostal);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);

            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitulo;
        private Label lblSeccion1;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellidoP;
        private TextBox txtApellidoP;
        private Label lblApellidoM;
        private TextBox txtApellidoM;
        private Label lblCorreo;
        private TextBox txtCorreo;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private Label lblEdad;
        private TextBox txtEdad;
        private Label lblSeccion2;
        private Label lblCalle;
        private TextBox txtCalle;
        private Label lblColonia;
        private TextBox txtColonia;
        private Label lblCiudad;
        private TextBox txtCiudad;
        private Label lblCodigoPostal;
        private TextBox txtCodigoPostal;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
