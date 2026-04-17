namespace GaleriaDeArtes
{
    partial class Pinturas
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            pnlMenu = new Panel();
            btnPinturas = new Button();
            btnArtistas = new Button();
            PnlPrincipal = new Panel();
            button2 = new Button();
            button1 = new Button();
            btnCotizaciones = new Button();
            btnAgregarPintura = new Button();
            txtBuscar = new TextBox();
            dgvPinturas = new DataGridView();
            btnProveedor = new Button();
            btnReportes = new Button();
            pnlMenu.SuspendLayout();
            PnlPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPinturas).BeginInit();
            SuspendLayout();
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = Color.FromArgb(21, 77, 113);
            pnlMenu.Controls.Add(btnReportes);
            pnlMenu.Controls.Add(btnProveedor);
            pnlMenu.Controls.Add(btnPinturas);
            pnlMenu.Controls.Add(btnArtistas);
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Location = new Point(0, 0);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(188, 721);
            pnlMenu.TabIndex = 0;
            // 
            // btnPinturas
            // 
            btnPinturas.BackColor = Color.FromArgb(21, 77, 113);
            btnPinturas.Dock = DockStyle.Top;
            btnPinturas.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            btnPinturas.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 161, 224);
            btnPinturas.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, 110, 164);
            btnPinturas.FlatStyle = FlatStyle.Flat;
            btnPinturas.ForeColor = Color.White;
            btnPinturas.Location = new Point(0, 60);
            btnPinturas.Name = "btnPinturas";
            btnPinturas.Size = new Size(188, 60);
            btnPinturas.TabIndex = 4;
            btnPinturas.Text = "Pinturas";
            btnPinturas.UseVisualStyleBackColor = false;
            // 
            // btnArtistas
            // 
            btnArtistas.BackColor = Color.FromArgb(21, 77, 113);
            btnArtistas.BackgroundImageLayout = ImageLayout.None;
            btnArtistas.Dock = DockStyle.Top;
            btnArtistas.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            btnArtistas.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 161, 224);
            btnArtistas.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, 110, 164);
            btnArtistas.FlatStyle = FlatStyle.Flat;
            btnArtistas.ForeColor = Color.White;
            btnArtistas.Location = new Point(0, 0);
            btnArtistas.Name = "btnArtistas";
            btnArtistas.Size = new Size(188, 60);
            btnArtistas.TabIndex = 3;
            btnArtistas.Text = "Artistas";
            btnArtistas.UseVisualStyleBackColor = false;
            btnArtistas.Click += btnAgregarArtista_Click;
            // 
            // PnlPrincipal
            // 
            PnlPrincipal.BackColor = Color.FromArgb(224, 224, 224);
            PnlPrincipal.Controls.Add(button2);
            PnlPrincipal.Controls.Add(button1);
            PnlPrincipal.Controls.Add(btnCotizaciones);
            PnlPrincipal.Controls.Add(btnAgregarPintura);
            PnlPrincipal.Controls.Add(txtBuscar);
            PnlPrincipal.Controls.Add(dgvPinturas);
            PnlPrincipal.Dock = DockStyle.Fill;
            PnlPrincipal.Location = new Point(188, 0);
            PnlPrincipal.Name = "PnlPrincipal";
            PnlPrincipal.Size = new Size(1160, 721);
            PnlPrincipal.TabIndex = 1;
            // 
            // button2
            // 
            button2.Location = new Point(711, 55);
            button2.Name = "button2";
            button2.Size = new Size(177, 36);
            button2.TabIndex = 4;
            button2.Text = "Eliminar Pintura";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnEliminarPintura_Click;
            // 
            // button1
            // 
            button1.Location = new Point(513, 55);
            button1.Name = "button1";
            button1.Size = new Size(177, 36);
            button1.TabIndex = 3;
            button1.Text = "Editar Pintura";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnEditarPintura_Click;
            // 
            // btnCotizaciones
            // 
            btnCotizaciones.Location = new Point(908, 55);
            btnCotizaciones.Name = "btnCotizaciones";
            btnCotizaciones.Size = new Size(177, 36);
            btnCotizaciones.TabIndex = 2;
            btnCotizaciones.Text = "Cotizaciones";
            btnCotizaciones.UseVisualStyleBackColor = true;
            // 
            // btnAgregarPintura
            // 
            btnAgregarPintura.Location = new Point(319, 55);
            btnAgregarPintura.Name = "btnAgregarPintura";
            btnAgregarPintura.Size = new Size(177, 36);
            btnAgregarPintura.TabIndex = 2;
            btnAgregarPintura.Text = "Agregar Pintura";
            btnAgregarPintura.UseVisualStyleBackColor = true;
            btnAgregarPintura.Click += btnAgregarPintura_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(60, 60);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar pintura...";
            txtBuscar.Size = new Size(230, 27);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // dgvPinturas
            // 
            dgvPinturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPinturas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvPinturas.BackgroundColor = Color.FromArgb(224, 224, 224);
            dgvPinturas.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(28, 110, 164);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(255, 249, 175);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(21, 77, 113);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvPinturas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvPinturas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPinturas.GridColor = Color.FromArgb(224, 224, 224);
            dgvPinturas.Location = new Point(60, 121);
            dgvPinturas.Name = "dgvPinturas";
            dgvPinturas.RowHeadersWidth = 51;
            dgvPinturas.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvPinturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPinturas.Size = new Size(1025, 419);
            dgvPinturas.TabIndex = 0;
            // 
            // btnProveedor
            // 
            btnProveedor.BackColor = Color.FromArgb(21, 77, 113);
            btnProveedor.BackgroundImageLayout = ImageLayout.None;
            btnProveedor.Dock = DockStyle.Top;
            btnProveedor.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            btnProveedor.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 161, 224);
            btnProveedor.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, 110, 164);
            btnProveedor.FlatStyle = FlatStyle.Flat;
            btnProveedor.ForeColor = Color.White;
            btnProveedor.Location = new Point(0, 120);
            btnProveedor.Name = "btnProveedor";
            btnProveedor.Size = new Size(188, 60);
            btnProveedor.TabIndex = 5;
            btnProveedor.Text = "Proveedor";
            btnProveedor.UseVisualStyleBackColor = false;
            //
            // btnReportes
            //
            btnReportes.BackColor = Color.FromArgb(21, 77, 113);
            btnReportes.BackgroundImageLayout = ImageLayout.None;
            btnReportes.Dock = DockStyle.Top;
            btnReportes.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            btnReportes.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 161, 224);
            btnReportes.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, 110, 164);
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(0, 180);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(188, 60);
            btnReportes.TabIndex = 6;
            btnReportes.Text = "Reportes";
            btnReportes.UseVisualStyleBackColor = false;
            btnReportes.Click += new EventHandler(btnReportes_Click);
            //
            // Pinturas
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1348, 721);
            Controls.Add(PnlPrincipal);
            Controls.Add(pnlMenu);
            Name = "Pinturas";
            Text = "Pinturas";
            Load += Form1_Load;
            pnlMenu.ResumeLayout(false);
            PnlPrincipal.ResumeLayout(false);
            PnlPrincipal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPinturas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMenu;
        private Panel PnlPrincipal;
        private DataGridView dgvPinturas;
        private TextBox txtBuscar;
        private Button btnAgregarPintura;
        private Button btnCotizaciones;
        private Button btnAgregarArtista;
        private Button btnArtistas;
        private Button button2;
        private Button button1;
        private Button btnPinturas;
        private Button btnProveedor;
        private Button btnReportes;
    }
}
