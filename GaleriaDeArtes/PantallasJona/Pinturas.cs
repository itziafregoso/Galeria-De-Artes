using GaleriaDeArtes.Data;
using GaleriaDeArtes.PantallasJona;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace GaleriaDeArtes
{
    public partial class Pinturas : Form
    {
     
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
                    p.titulo                                        AS [Título],
                    t.nombre_tecnica                               AS [Técnica],
                    p.precio_base                                  AS [Precio],
                    p.estado_disponibilidad                        AS [Estado],
                    a.nombre_completo                              AS [Artista],
                    STRING_AGG(e.nombre_exhibicion, ', ')          AS [Exhibición]
                FROM dbo.PINTURA p
                LEFT JOIN dbo.ARTISTA           a  ON p.id_artista   = a.id_artista
                LEFT JOIN dbo.TECNICA           t  ON p.id_tecnica   = t.id_tecnica
                LEFT JOIN dbo.DETALLE_EXHIBICION de ON p.id_pintura  = de.id_pintura
                LEFT JOIN dbo.EXHIBICION        e  ON de.id_exhibicion = e.id_exhibicion
                GROUP BY
                    p.titulo,
                    t.nombre_tecnica,
                    p.precio_base,
                    p.estado_disponibilidad,
                    a.nombre_completo";

            try
            {
                using (SqlConnection conn = Conexion.ObtenerConexion()) 
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    _tablaPinturas = new DataTable();
                    adapter.Fill(_tablaPinturas);
                    dgvPinturas.DataSource = _tablaPinturas;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pinturas:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}
