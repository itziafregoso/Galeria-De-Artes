namespace GaleriaDeArtes
{
    partial class AgregarPintura
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
            lblTitulo      = new Label();
            txtTitulo      = new TextBox();
            lblPrecio      = new Label();
            txtPrecio      = new TextBox();
            lblEstado      = new Label();
            cmbEstado      = new ComboBox();
            lblArtista     = new Label();
            cmbArtista     = new ComboBox();
            lblTecnica     = new Label();
            cmbTecnica     = new ComboBox();
            lblAnio        = new Label();
            txtAnio        = new TextBox();
            lblDimensiones = new Label();
            txtDimensiones = new TextBox();
            btnGuardar     = new Button();
            btnCancelar    = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(40, 40);
            lblTitulo.Name     = "lblTitulo";
            lblTitulo.Text     = "Título:";
            //
            // txtTitulo
            //
            txtTitulo.Location = new Point(220, 37);
            txtTitulo.Name     = "txtTitulo";
            txtTitulo.Size     = new Size(320, 27);
            txtTitulo.TabIndex = 0;
            //
            // lblPrecio
            //
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(40, 90);
            lblPrecio.Name     = "lblPrecio";
            lblPrecio.Text     = "Precio base:";
            //
            // txtPrecio
            //
            txtPrecio.Location        = new Point(220, 87);
            txtPrecio.Name            = "txtPrecio";
            txtPrecio.Size            = new Size(180, 27);
            txtPrecio.TabIndex        = 1;
            txtPrecio.PlaceholderText = "0.00";
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(40, 140);
            lblEstado.Name     = "lblEstado";
            lblEstado.Text     = "Estado de disponibilidad:";
            //
            // cmbEstado
            //
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstado.Items.AddRange(new object[] { "Disponible", "Vendida", "En exhibición" });
            cmbEstado.Location  = new Point(220, 137);
            cmbEstado.Name      = "cmbEstado";
            cmbEstado.Size      = new Size(220, 28);
            cmbEstado.TabIndex  = 2;
            //
            // lblArtista
            //
            lblArtista.AutoSize = true;
            lblArtista.Location = new Point(40, 190);
            lblArtista.Name     = "lblArtista";
            lblArtista.Text     = "Artista:";
            //
            // cmbArtista
            //
            cmbArtista.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArtista.Location  = new Point(220, 187);
            cmbArtista.Name      = "cmbArtista";
            cmbArtista.Size      = new Size(320, 28);
            cmbArtista.TabIndex  = 3;
            //
            // lblTecnica
            //
            lblTecnica.AutoSize = true;
            lblTecnica.Location = new Point(40, 240);
            lblTecnica.Name     = "lblTecnica";
            lblTecnica.Text     = "Técnica:";
            //
            // cmbTecnica
            //
            cmbTecnica.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTecnica.Location  = new Point(220, 237);
            cmbTecnica.Name      = "cmbTecnica";
            cmbTecnica.Size      = new Size(320, 28);
            cmbTecnica.TabIndex  = 4;
            //
            // lblAnio
            //
            lblAnio.AutoSize = true;
            lblAnio.Location = new Point(40, 290);
            lblAnio.Name     = "lblAnio";
            lblAnio.Text     = "Año de creación:";
            //
            // txtAnio
            //
            txtAnio.Location        = new Point(220, 287);
            txtAnio.Name            = "txtAnio";
            txtAnio.Size            = new Size(120, 27);
            txtAnio.TabIndex        = 5;
            txtAnio.PlaceholderText = "Ej: 1503";
            //
            // lblDimensiones
            //
            lblDimensiones.AutoSize = true;
            lblDimensiones.Location = new Point(40, 340);
            lblDimensiones.Name     = "lblDimensiones";
            lblDimensiones.Text     = "Dimensiones:";
            //
            // txtDimensiones
            //
            txtDimensiones.Location        = new Point(220, 337);
            txtDimensiones.Name            = "txtDimensiones";
            txtDimensiones.Size            = new Size(320, 27);
            txtDimensiones.TabIndex        = 6;
            txtDimensiones.PlaceholderText = "Ej: 77 x 53 cm";
            //
            // btnGuardar
            //
            btnGuardar.BackColor               = Color.FromArgb(28, 110, 164);
            btnGuardar.FlatStyle               = FlatStyle.Flat;
            btnGuardar.ForeColor               = Color.White;
            btnGuardar.Location                = new Point(220, 395);
            btnGuardar.Name                    = "btnGuardar";
            btnGuardar.Size                    = new Size(120, 36);
            btnGuardar.TabIndex                = 7;
            btnGuardar.Text                    = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click                  += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.Location                = new Point(360, 395);
            btnCancelar.Name                    = "btnCancelar";
            btnCancelar.Size                    = new Size(120, 36);
            btnCancelar.TabIndex                = 8;
            btnCancelar.Text                    = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click                  += btnCancelar_Click;
            //
            // AgregarPintura
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode       = AutoScaleMode.Font;
            ClientSize          = new Size(614, 460);
            Controls.Add(lblTitulo);
            Controls.Add(txtTitulo);
            Controls.Add(lblPrecio);
            Controls.Add(txtPrecio);
            Controls.Add(lblEstado);
            Controls.Add(cmbEstado);
            Controls.Add(lblArtista);
            Controls.Add(cmbArtista);
            Controls.Add(lblTecnica);
            Controls.Add(cmbTecnica);
            Controls.Add(lblAnio);
            Controls.Add(txtAnio);
            Controls.Add(lblDimensiones);
            Controls.Add(txtDimensiones);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            Name            = "AgregarPintura";
            StartPosition   = FormStartPosition.CenterParent;
            Text            = "Pintura";
            Load           += AgregarPintura_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label    lblTitulo;
        private TextBox  txtTitulo;
        private Label    lblPrecio;
        private TextBox  txtPrecio;
        private Label    lblEstado;
        private ComboBox cmbEstado;
        private Label    lblArtista;
        private ComboBox cmbArtista;
        private Label    lblTecnica;
        private ComboBox cmbTecnica;
        private Label    lblAnio;
        private TextBox  txtAnio;
        private Label    lblDimensiones;
        private TextBox  txtDimensiones;
        private Button   btnGuardar;
        private Button   btnCancelar;
    }
}
