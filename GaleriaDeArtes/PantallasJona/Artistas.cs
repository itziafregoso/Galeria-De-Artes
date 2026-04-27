using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.CapaNegocio;
using GaleriaDeArtes.PantallasJona;
using System;
using System.Data;
using System.Windows.Forms;

namespace GaleriaDeArtes
{
    public partial class Artistas : Form
    {
        private readonly ArtistaBLL _bll = new ArtistaBLL();
        private DataTable _tablaArtistas = new DataTable();

        public Artistas()
        {
            InitializeComponent();

            // Reemplazar el menú del Designer por el componente reutilizable
            Controls.Remove(pnlMenu);
            Controls.Add(new MenuLateral(PaginaActiva.Artistas));

            ConfiguracionFormulario.Aplicar(this);
        }

        private void Artistas_Load(object sender, EventArgs e)
        {
            CargarArtistas();
        }

        private void CargarArtistas()
        {
            try
            {
                _tablaArtistas = _bll.ObtenerTodos();
                dgvArtistas.DataSource = _tablaArtistas;

                if (dgvArtistas.Columns.Contains("id_artista"))
                    dgvArtistas.Columns["id_artista"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar artistas:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarArtista_Click(object sender, EventArgs e)
        {
            var frm = new AgregarEditarArtista(_bll);
            if (frm.ShowDialog() == DialogResult.OK)
                CargarArtistas();
        }

        private void btnEditarArtista_Click(object sender, EventArgs e)
        {
            if (dgvArtistas.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvArtistas.CurrentRow.Cells["id_artista"].Value);
            Artista? artista = _bll.ObtenerPorId(id);
            if (artista == null) return;

            var frm = new AgregarEditarArtista(_bll, artista);
            if (frm.ShowDialog() == DialogResult.OK)
                CargarArtistas();
        }

        private void btnEliminarArtista_Click(object sender, EventArgs e)
        {
            if (dgvArtistas.CurrentRow == null) return;

            string nombre    = $"{dgvArtistas.CurrentRow.Cells["Nombre"].Value} {dgvArtistas.CurrentRow.Cells["Apellido Paterno"].Value}".Trim();
            int id = Convert.ToInt32(dgvArtistas.CurrentRow.Cells["id_artista"].Value);

            bool eliminarPinturas = false;
            int totalPinturas = _bll.ContarPinturas(id);

            if (totalPinturas > 0)
            {
                var respuesta = MessageBox.Show(
                    $"El artista \"{nombre}\" tiene {totalPinturas} pintura(s) registrada(s).\n\n" +
                    $"¿Deseas eliminar al artista junto con todas sus pinturas?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (respuesta != DialogResult.Yes) return;
                eliminarPinturas = true;
            }
            else
            {
                var confirm = MessageBox.Show(
                    $"¿Deseas eliminar al artista \"{nombre}\"?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm != DialogResult.Yes) return;
            }

            try
            {
                _bll.Eliminar(id, eliminarPinturas);
                CargarArtistas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar artista:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (_tablaArtistas == null) return;

            string filtro = txtBuscar.Text.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(filtro))
            {
                _tablaArtistas.DefaultView.RowFilter = string.Empty;
                return;
            }

            var condiciones = new System.Collections.Generic.List<string>();
            foreach (DataColumn col in _tablaArtistas.Columns)
            {
                if (col.ColumnName != "id_artista")
                    condiciones.Add($"CONVERT([{col.ColumnName}], System.String) LIKE '%{filtro}%'");
            }

            _tablaArtistas.DefaultView.RowFilter = string.Join(" OR ", condiciones);
        }

        // Estos métodos son referenciados por el Designer original (btnPinturas, btnReportes).
        // pnlMenu es eliminado del layout, pero los handlers deben compilar.
        private void btnPinturas_Click(object sender, EventArgs e)
            => Navegador.Ir(PaginaActiva.Pinturas);

        private void btnReportes_Click(object sender, EventArgs e)
            => Navegador.Ir(PaginaActiva.Reportes);
    }
}
