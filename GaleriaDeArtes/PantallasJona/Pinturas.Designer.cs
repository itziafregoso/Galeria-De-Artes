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
            btnArtistas = new Button();
            PnlPrincipal = new Panel();
            btnCotizaciones = new Button();
            btnAgregarPintura = new Button();
            txtBuscar = new TextBox();
            dgvPinturas = new DataGridView();
            button1 = new Button();
            button2 = new Button();
            pnlMenu.SuspendLayout();
            PnlPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPinturas).BeginInit();
            SuspendLayout();
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = Color.FromArgb(21, 77, 113);
            pnlMenu.Controls.Add(btnArtistas);
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Location = new Point(0, 0);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(188, 721);
            pnlMenu.TabIndex = 0;
            // 
            // btnArtistas
            // 
            btnArtistas.BackColor = Color.FromArgb(21, 77, 113);
            btnArtistas.BackgroundImageLayout = ImageLayout.None;
            btnArtistas.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            btnArtistas.FlatStyle = FlatStyle.Flat;
            btnArtistas.ForeColor = Color.White;
            btnArtistas.Location = new Point(-6, 0);
            btnArtistas.Name = "btnArtistas";
            btnArtistas.Size = new Size(206, 55);
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
            dgvPinturas.Size = new Size(1025, 419);
            dgvPinturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPinturas.TabIndex = 0;
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
    }
}
