using GaleriaDeArtes.CapaNegocio;
using GaleriaDeArtes.Data;
using GaleriaDeArtes.PantallasJona;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;


namespace GaleriaDeArtes
{
    public partial class Pinturas : Form
    {

        private readonly PinturaBLL _bll = new PinturaBLL();
        private DataTable _tablaPinturas;

        public Pinturas()
        {
            InitializeComponent();
            ConfiguracionFormulario.Aplicar(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarPinturas();
        }

        private void CargarPinturas()
        {
            string query = @"
                SELECT
                    p.id_pintura                                   AS id_pintura,
                    p.titulo                                       AS [Título],
                    a.nombre_completo                              AS [Artista],
                    t.nombre_tecnica                               AS [Técnica],
                    p.anio_creacion                                AS [Año de Creación],
                    p.dimensiones                                  AS [Dimensiones],
                    p.precio_base                                  AS [Precio],
                    p.estado_disponibilidad                        AS [Estado],
                    STRING_AGG(e.nombre_exhibicion, ', ')          AS [Exhibición]
                FROM dbo.PINTURA p
                LEFT JOIN dbo.ARTISTA            a  ON p.id_artista    = a.id_artista
                LEFT JOIN dbo.TECNICA            t  ON p.id_tecnica    = t.id_tecnica
                LEFT JOIN dbo.DETALLE_EXHIBICION de ON p.id_pintura   = de.id_pintura
                LEFT JOIN dbo.EXHIBICION         e  ON de.id_exhibicion = e.id_exhibicion
                GROUP BY
                    p.id_pintura,
                    p.titulo,
                    a.nombre_completo,
                    t.nombre_tecnica,
                    p.anio_creacion,
                    p.dimensiones,
                    p.precio_base,
                    p.estado_disponibilidad";

            try
            {
                using (SqlConnection conn = Conexion.ObtenerConexion())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    _tablaPinturas = new DataTable();
                    adapter.Fill(_tablaPinturas);
                    dgvPinturas.DataSource = _tablaPinturas;

                    if (dgvPinturas.Columns.Contains("id_pintura"))
                        dgvPinturas.Columns["id_pintura"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pinturas:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAgregarPintura_Click(object sender, EventArgs e)
        {
            var frm = new AgregarPintura(_bll);
            if (frm.ShowDialog() == DialogResult.OK)
                CargarPinturas();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (_tablaPinturas == null) return;

            string filtro = txtBuscar.Text.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(filtro))
            {
                _tablaPinturas.DefaultView.RowFilter = string.Empty;
                return;
            }

            var condiciones = new System.Collections.Generic.List<string>();
            foreach (DataColumn col in _tablaPinturas.Columns)
                condiciones.Add($"CONVERT([{col.ColumnName}], System.String) LIKE '%{filtro}%'");

            _tablaPinturas.DefaultView.RowFilter = string.Join(" OR ", condiciones);
        }

        private void btnEditarPintura_Click(object sender, EventArgs e)
        {
            if (dgvPinturas.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvPinturas.CurrentRow.Cells["id_pintura"].Value);
            var pintura = _bll.ObtenerPorId(id);
            if (pintura == null) return;

            var frm = new AgregarPintura(_bll, pintura);
            if (frm.ShowDialog() == DialogResult.OK)
                CargarPinturas();
        }

        private void btnEliminarPintura_Click(object sender, EventArgs e)
        {
            if (dgvPinturas.CurrentRow == null) return;

            string titulo = dgvPinturas.CurrentRow.Cells["Título"].Value?.ToString() ?? "";
            int id = Convert.ToInt32(dgvPinturas.CurrentRow.Cells["id_pintura"].Value);

            var confirm = MessageBox.Show(
                $"¿Deseas eliminar la pintura \"{titulo}\"?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _bll.Eliminar(id);
                CargarPinturas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar pintura:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarArtista_Click(object sender, EventArgs e)
        {
            var frmArtistas = new Artistas();
            frmArtistas.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            frmArtistas.Show();
        }
    }
}
