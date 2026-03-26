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
            PnlPrincipal = new Panel();
            btnCotizaciones = new Button();
            btnAgregarPintura = new Button();
            txtBuscar = new TextBox();
            dgvPinturas = new DataGridView();
            PnlPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPinturas).BeginInit();
            SuspendLayout();
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = Color.FromArgb(21, 77, 113);
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Location = new Point(0, 0);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(188, 721);
            pnlMenu.TabIndex = 0;
            // 
            // PnlPrincipal
            // 
            PnlPrincipal.BackColor = Color.FromArgb(224, 224, 224);
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
            btnCotizaciones.Location = new Point(598, 55);
            btnCotizaciones.Name = "btnCotizaciones";
            btnCotizaciones.Size = new Size(177, 36);
            btnCotizaciones.TabIndex = 2;
            btnCotizaciones.Text = "Cotizaciones";
            btnCotizaciones.UseVisualStyleBackColor = true;
            // 
            // btnAgregarPintura
            // 
            btnAgregarPintura.Location = new Point(399, 55);
            btnAgregarPintura.Name = "btnAgregarPintura";
            btnAgregarPintura.Size = new Size(177, 36);
            btnAgregarPintura.TabIndex = 2;
            btnAgregarPintura.Text = "Agregar Pintura";
            btnAgregarPintura.UseVisualStyleBackColor = true;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(60, 60);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(279, 27);
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
            dgvPinturas.TabIndex = 0;
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
    }
}
