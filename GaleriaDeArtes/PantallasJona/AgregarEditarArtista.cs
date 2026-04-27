using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.CapaNegocio;
using System;
using System.Windows.Forms;

namespace GaleriaDeArtes
{
    public partial class AgregarEditarArtista : Form
    {
        private readonly ArtistaBLL _bll;
        private readonly Artista? _artista;
        private readonly bool _esEdicion;

        // Modo agregar
        public AgregarEditarArtista(ArtistaBLL bll)
        {
            InitializeComponent();
            _bll = bll;
            _esEdicion = false;
            lblTitulo.Text = "Agregar Artista";
        }

        // Modo editar
        public AgregarEditarArtista(ArtistaBLL bll, Artista artista)
        {
            InitializeComponent();
            _bll = bll;
            _artista = artista;
            _esEdicion = true;
            lblTitulo.Text = "Editar Artista";
        }

        private void AgregarEditarArtista_Load(object sender, EventArgs e)
        {
            if (_esEdicion && _artista != null)
            {
                txtNombre.Text       = _artista.Nombre;
                txtApellidoP.Text    = _artista.ApellidoPaterno;
                txtApellidoM.Text    = _artista.ApellidoMaterno;
                txtCorreo.Text       = _artista.Correo;
                txtTelefono.Text     = _artista.Telefono;
                txtCalle.Text        = _artista.Calle;
                txtColonia.Text      = _artista.Colonia;
                txtCiudad.Text       = _artista.Ciudad;
                txtCodigoPostal.Text = _artista.CodigoPostal;

                if (_artista.FechaNacimiento.HasValue)
                {
                    dtpFecha.Checked = true;
                    dtpFecha.Value   = _artista.FechaNacimiento.Value;
                }
                else
                {
                    dtpFecha.Checked = false;
                }
            }
            ActualizarEdad();
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e) => ActualizarEdad();

        private void ActualizarEdad()
        {
            if (dtpFecha.Checked)
            {
                var hoy = DateTime.Today;
                int edad = hoy.Year - dtpFecha.Value.Year;
                if (dtpFecha.Value.Date > hoy.AddYears(-edad)) edad--;
                txtEdad.Text = edad >= 0 ? edad.ToString() : "0";
            }
            else
            {
                txtEdad.Text = string.Empty;
            }
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var artista = new Artista
            {
                IdArtista       = _esEdicion && _artista != null ? _artista.IdArtista : 0,
                Nombre          = txtNombre.Text.Trim(),
                ApellidoPaterno = txtApellidoP.Text.Trim(),
                ApellidoMaterno = txtApellidoM.Text.Trim(),
                Correo          = txtCorreo.Text.Trim(),
                Telefono        = txtTelefono.Text.Trim(),
                FechaNacimiento = dtpFecha.Checked ? dtpFecha.Value.Date : null,
                Calle           = txtCalle.Text.Trim(),
                Colonia         = txtColonia.Text.Trim(),
                Ciudad          = txtCiudad.Text.Trim(),
                CodigoPostal    = txtCodigoPostal.Text.Trim(),
                // Conservar campos heredados si se está editando
                Nacionalidad       = _artista?.Nacionalidad ?? "",
                EstiloPredominante = _artista?.EstiloPredominante ?? "",
                Biografia          = _artista?.Biografia ?? ""
            };

            var (ok, mensaje) = _esEdicion
                ? _bll.Actualizar(artista)
                : _bll.Insertar(artista);

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
