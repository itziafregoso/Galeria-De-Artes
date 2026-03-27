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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblNacionalidad = new Label();
            txtNacionalidad = new TextBox();
            lblFecha = new Label();
            txtFecha = new TextBox();
            lblEstilo = new Label();
            txtEstilo = new TextBox();
            lblBiografia = new Label();
            txtBiografia = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(40, 40);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(135, 20);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre completo:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(220, 37);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(320, 27);
            txtNombre.TabIndex = 0;
            // 
            // lblNacionalidad
            // 
            lblNacionalidad.AutoSize = true;
            lblNacionalidad.Location = new Point(40, 90);
            lblNacionalidad.Name = "lblNacionalidad";
            lblNacionalidad.Size = new Size(101, 20);
            lblNacionalidad.TabIndex = 1;
            lblNacionalidad.Text = "Nacionalidad:";
            // 
            // txtNacionalidad
            // 
            txtNacionalidad.Location = new Point(220, 87);
            txtNacionalidad.Name = "txtNacionalidad";
            txtNacionalidad.Size = new Size(320, 27);
            txtNacionalidad.TabIndex = 1;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(40, 140);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(247, 20);
            lblFecha.TabIndex = 2;
            lblFecha.Text = "Fecha de nacimiento (dd/MM/yyyy):";
            // 
            // txtFecha
            // 
            txtFecha.Location = new Point(360, 137);
            txtFecha.Name = "txtFecha";
            txtFecha.PlaceholderText = "dd/MM/yyyy";
            txtFecha.Size = new Size(180, 27);
            txtFecha.TabIndex = 2;
            // 
            // lblEstilo
            // 
            lblEstilo.AutoSize = true;
            lblEstilo.Location = new Point(40, 190);
            lblEstilo.Name = "lblEstilo";
            lblEstilo.Size = new Size(146, 20);
            lblEstilo.TabIndex = 3;
            lblEstilo.Text = "Estilo predominante:";
            // 
            // txtEstilo
            // 
            txtEstilo.Location = new Point(220, 187);
            txtEstilo.Name = "txtEstilo";
            txtEstilo.Size = new Size(320, 27);
            txtEstilo.TabIndex = 3;
            // 
            // lblBiografia
            // 
            lblBiografia.AutoSize = true;
            lblBiografia.Location = new Point(40, 240);
            lblBiografia.Name = "lblBiografia";
            lblBiografia.Size = new Size(73, 20);
            lblBiografia.TabIndex = 4;
            lblBiografia.Text = "Biografía:";
            // 
            // txtBiografia
            // 
            txtBiografia.Location = new Point(220, 237);
            txtBiografia.Multiline = true;
            txtBiografia.Name = "txtBiografia";
            txtBiografia.ScrollBars = ScrollBars.Vertical;
            txtBiografia.Size = new Size(320, 100);
            txtBiografia.TabIndex = 4;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(28, 110, 164);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(220, 365);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 36);
            btnGuardar.TabIndex = 5;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(360, 365);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 36);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // AgregarEditarArtista
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(614, 430);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblNacionalidad);
            Controls.Add(txtNacionalidad);
            Controls.Add(lblFecha);
            Controls.Add(txtFecha);
            Controls.Add(lblEstilo);
            Controls.Add(txtEstilo);
            Controls.Add(lblBiografia);
            Controls.Add(txtBiografia);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AgregarEditarArtista";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Artista";
            Load += AgregarEditarArtista_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblNacionalidad;
        private TextBox txtNacionalidad;
        private Label lblFecha;
        private TextBox txtFecha;
        private Label lblEstilo;
        private TextBox txtEstilo;
        private Label lblBiografia;
        private TextBox txtBiografia;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
