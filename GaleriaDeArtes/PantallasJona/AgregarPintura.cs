using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.CapaNegocio;
using System;
using System.Windows.Forms;

namespace GaleriaDeArtes
{
    public partial class AgregarPintura : Form
    {
        private readonly PinturaBLL _bll;
        private readonly Pintura?   _pintura;
        private readonly bool       _esEdicion;

        // Agregar
        public AgregarPintura(PinturaBLL bll)
        {
            InitializeComponent();
            _bll       = bll;
            _esEdicion = false;
            Text       = "Agregar Pintura";
        }

        // Editar
        public AgregarPintura(PinturaBLL bll, Pintura pintura)
        {
            InitializeComponent();
            _bll       = bll;
            _pintura   = pintura;
            _esEdicion = true;
            Text       = "Editar Pintura";
        }

        private void AgregarPintura_Load(object sender, EventArgs e)
        {
            // Cargar artistas en el combo
            var artistas = _bll.ObtenerArtistas();
            cmbArtista.DisplayMember = "NombreCompleto";
            cmbArtista.ValueMember   = "IdArtista";
            cmbArtista.DataSource    = artistas;
            cmbArtista.SelectedIndex = -1;

            // Cargar técnicas en el combo
            var tecnicas = _bll.ObtenerTecnicas();
            cmbTecnica.DisplayMember = "NombreTecnica";
            cmbTecnica.ValueMember   = "IdTecnica";
            cmbTecnica.DataSource    = tecnicas;
            cmbTecnica.SelectedIndex = -1;

            if (_esEdicion && _pintura != null)
            {
                txtTitulo.Text           = _pintura.Titulo;
                txtPrecio.Text           = _pintura.PrecioBase.ToString("0.00");
                cmbEstado.Text           = _pintura.EstadoDisponibilidad;
                cmbArtista.SelectedValue = _pintura.IdArtista;
                cmbTecnica.SelectedValue = _pintura.IdTecnica;
                txtAnio.Text             = _pintura.AnioCreacion.HasValue ? _pintura.AnioCreacion.Value.ToString() : "";
                txtDimensiones.Text      = _pintura.Dimensiones;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
            {
                MessageBox.Show("El precio debe ser un número válido (ejemplo: 1500.00).",
                                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            int? anio = null;
            if (!string.IsNullOrWhiteSpace(txtAnio.Text))
            {
                if (!int.TryParse(txtAnio.Text.Trim(), out int anioParsed) || anioParsed < 1 || anioParsed > 9999)
                {
                    MessageBox.Show("El año de creación debe ser un número válido (ejemplo: 1503).",
                                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAnio.Focus();
                    return;
                }
                anio = anioParsed;
            }

            var pintura = new Pintura
            {
                IdPintura            = _esEdicion && _pintura != null ? _pintura.IdPintura : 0,
                Titulo               = txtTitulo.Text.Trim(),
                PrecioBase           = precio,
                EstadoDisponibilidad = cmbEstado.SelectedItem?.ToString() ?? "",
                IdArtista            = cmbArtista.SelectedValue is int idA ? idA : 0,
                IdTecnica            = cmbTecnica.SelectedValue is int idT ? idT : 0,
                AnioCreacion         = anio,
                Dimensiones          = txtDimensiones.Text.Trim()
            };

            var (ok, mensaje) = _esEdicion
                ? _bll.Actualizar(pintura)
                : _bll.Insertar(pintura);

            if (!ok)
            {
                MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
