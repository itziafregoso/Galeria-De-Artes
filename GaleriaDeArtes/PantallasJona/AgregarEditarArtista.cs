using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.CapaNegocio;
using GaleriaDeArtes.PantallasJona;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace GaleriaDeArtes
{
    public partial class AgregarEditarArtista : Form
    {
        private readonly ArtistaBLL _bll;
        private readonly Artista? _artista;
        private readonly bool _esEdicion;

        // Agregar
        public AgregarEditarArtista(ArtistaBLL bll)
        {
            InitializeComponent();
            _bll = bll;
            _esEdicion = false;
            Text = "Agregar Artista";
        }

        // Editar
        public AgregarEditarArtista(ArtistaBLL bll, Artista artista)
        {
            InitializeComponent();
            _bll = bll;
            _artista = artista;
            _esEdicion = true;
            Text = "Editar Artista";
        }

        private void AgregarEditarArtista_Load(object sender, EventArgs e)
        {
            if (_esEdicion && _artista != null)
            {
                txtNombre.Text       = _artista.NombreCompleto;
                txtNacionalidad.Text = _artista.Nacionalidad;
                txtEstilo.Text       = _artista.EstiloPredominante;
                txtBiografia.Text    = _artista.Biografia;

                txtFecha.Text = _artista.FechaNacimiento.HasValue
                    ? _artista.FechaNacimiento.Value.ToString("dd/MM/yyyy")
                    : string.Empty;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            DateTime? fechaNac = null;

            if (!string.IsNullOrWhiteSpace(txtFecha.Text))
            {
                if (!DateTime.TryParseExact(txtFecha.Text.Trim(), "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaParsed))
                {
                    MessageBox.Show("Formato de fecha inválido. Use dd/MM/yyyy (ejemplo: 15/04/1452).",
                                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFecha.Focus();
                    return;
                }
                fechaNac = fechaParsed;
            }

            var artista = new Artista
            {
                IdArtista          = _esEdicion && _artista != null ? _artista.IdArtista : 0,
                NombreCompleto     = txtNombre.Text.Trim(),
                Nacionalidad       = txtNacionalidad.Text.Trim(),
                FechaNacimiento    = fechaNac,
                EstiloPredominante = txtEstilo.Text.Trim(),
                Biografia          = txtBiografia.Text.Trim()
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
