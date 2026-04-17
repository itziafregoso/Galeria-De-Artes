namespace GaleriaDeArtes
{
    partial class Artistas
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            pnlMenu = new Panel();
            btnArtistas = new Button();
            btnPinturas = new Button();
            btnReportes = new Button();
            PnlPrincipal = new Panel();
            btnEliminarArtista = new Button();
            btnEditarArtista = new Button();
            btnAgregarArtista = new Button();
            txtBuscar = new TextBox();
            dgvArtistas = new DataGridView();
            btnProveedor = new Button();
            pnlMenu.SuspendLayout();
            PnlPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArtistas).BeginInit();
            SuspendLayout();
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = Color.FromArgb(21, 77, 113);
            pnlMenu.Controls.Add(btnPinturas);
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
            btnPinturas.FlatStyle = FlatStyle.Flat;
            btnPinturas.ForeColor = Color.White;
            btnPinturas.Location = new Point(0, 0);
            btnPinturas.Name = "btnPinturas";
            btnPinturas.Size = new Size(188, 60);
            btnPinturas.TabIndex = 3;
            btnPinturas.Text = "Pinturas";
            btnPinturas.UseVisualStyleBackColor = false;
            btnPinturas.Click += btnPinturas_Click;
            // 
            // PnlPrincipal
            // 
            PnlPrincipal.BackColor = Color.FromArgb(224, 224, 224);
            PnlPrincipal.Controls.Add(btnEliminarArtista);
            PnlPrincipal.Controls.Add(btnEditarArtista);
            PnlPrincipal.Controls.Add(btnAgregarArtista);
            PnlPrincipal.Controls.Add(txtBuscar);
            PnlPrincipal.Controls.Add(dgvArtistas);
            PnlPrincipal.Dock = DockStyle.Fill;
            PnlPrincipal.Location = new Point(188, 0);
            PnlPrincipal.Name = "PnlPrincipal";
            PnlPrincipal.Size = new Size(1160, 721);
            PnlPrincipal.TabIndex = 1;
            // 
            // btnEliminarArtista
            // 
            btnEliminarArtista.Location = new Point(797, 55);
            btnEliminarArtista.Name = "btnEliminarArtista";
            btnEliminarArtista.Size = new Size(177, 36);
            btnEliminarArtista.TabIndex = 4;
            btnEliminarArtista.Text = "Eliminar Artista";
            btnEliminarArtista.UseVisualStyleBackColor = true;
            btnEliminarArtista.Click += btnEliminarArtista_Click;
            // 
            // btnEditarArtista
            // 
            btnEditarArtista.Location = new Point(598, 55);
            btnEditarArtista.Name = "btnEditarArtista";
            btnEditarArtista.Size = new Size(177, 36);
            btnEditarArtista.TabIndex = 3;
            btnEditarArtista.Text = "Editar Artista";
            btnEditarArtista.UseVisualStyleBackColor = true;
            btnEditarArtista.Click += btnEditarArtista_Click;
            // 
            // btnAgregarArtista
            // 
            btnAgregarArtista.Location = new Point(399, 55);
            btnAgregarArtista.Name = "btnAgregarArtista";
            btnAgregarArtista.Size = new Size(177, 36);
            btnAgregarArtista.TabIndex = 2;
            btnAgregarArtista.Text = "Agregar Artista";
            btnAgregarArtista.UseVisualStyleBackColor = true;
            btnAgregarArtista.Click += btnAgregarArtista_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(60, 60);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar artista...";
            txtBuscar.Size = new Size(279, 27);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // dgvArtistas
            // 
            dgvArtistas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArtistas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvArtistas.BackgroundColor = Color.FromArgb(224, 224, 224);
            dgvArtistas.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(28, 110, 164);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(255, 249, 175);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(21, 77, 113);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvArtistas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvArtistas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArtistas.GridColor = Color.FromArgb(224, 224, 224);
            dgvArtistas.Location = new Point(60, 121);
            dgvArtistas.Name = "dgvArtistas";
            dgvArtistas.RowHeadersWidth = 51;
            dgvArtistas.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvArtistas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArtistas.Size = new Size(1025, 419);
            dgvArtistas.TabIndex = 0;
            // 
            // Artistas
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1348, 721);
            Controls.Add(PnlPrincipal);
            Controls.Add(pnlMenu);
            Name = "Artistas";
            Text = "Artistas";
            Load += Artistas_Load;
            pnlMenu.ResumeLayout(false);
            PnlPrincipal.ResumeLayout(false);
            PnlPrincipal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArtistas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMenu;
        private Panel PnlPrincipal;
        private DataGridView dgvArtistas;
        private TextBox txtBuscar;
        private Button btnPinturas;
        private Button btnAgregarArtista;
        private Button btnEditarArtista;
        private Button btnEliminarArtista;
    }
}
