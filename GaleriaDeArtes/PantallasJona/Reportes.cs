using GaleriaDeArtes.CapaDatos;
using GaleriaDeArtes.CapaEntidad;
using GaleriaDeArtes.CapaEntidad.Reportes;
using GaleriaDeArtes.CapaNegocio;
using GaleriaDeArtes.ModuloReportes;
using System.Diagnostics;

namespace GaleriaDeArtes.PantallasJona
{
    public class Reportes : Form
    {
        // ── Paleta ────────────────────────────────────────────────────────────
        private static readonly Color Navy       = Color.FromArgb(21,  77,  113);
        private static readonly Color Blue       = Color.FromArgb(28,  110, 164);
        private static readonly Color FondoPanel = Color.FromArgb(238, 246, 255);
        private static readonly Color Blanco     = Color.White;

        // ── Controles ─────────────────────────────────────────────────────────
        private ComboBox      cboTipoReporte   = null!;
        private GroupBox      grpFiltros       = null!;
        private ComboBox      cboFiltroArtista = null!;
        private ComboBox      cboFiltroEstado  = null!;
        private NumericUpDown nudAnioDesde     = null!;
        private NumericUpDown nudAnioHasta     = null!;
        private TextBox       txtPrecioMin     = null!;
        private TextBox       txtPrecioMax     = null!;
        private Label         lblDescripcion   = null!;
        private Button        btnGenerar       = null!;
        private Button        btnGuardar       = null!;
        private ProgressBar   progressBar      = null!;

        // ── Lógica ────────────────────────────────────────────────────────────
        private readonly ReporteBLL _bll = new ReporteBLL();
        private string? _ultimoPdfTemp = null;

        private static readonly string[] Descripciones =
        {
            "Lista completa de todas las pinturas con artista, técnica, precio y estado.\n" +
            "Admite filtros por artista, estado de disponibilidad, rango de años y precio.",

            "Agrupación de obras por artista: total de pinturas, valor acumulado\n" +
            "y precio promedio. Ordenado de mayor a menor productividad.",

            "Distribución de pinturas según la técnica pictórica utilizada.\n" +
            "Muestra cantidad, valor total y porcentaje sobre el total.",

            "Resumen financiero del inventario agrupado por estado\n" +
            "(Disponible / Vendida / Reservada) con valor total y participación.",

            "Vista ejecutiva en una sola página: KPIs principales, estado del\n" +
            "inventario y datos destacados de la galería.",

            "Detalle de las pinturas incluidas en cada exhibición registrada,\n" +
            "con información de artista, técnica, precio y estado."
        };

        public Reportes()
        {
            ConfiguracionFormulario.Aplicar(this);
            Text = "Módulo de Reportes — Galería de Artes";
            InicializarComponentes();
            CargarArtistasEnFiltro();
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONSTRUCCIÓN DE LA INTERFAZ
        // ─────────────────────────────────────────────────────────────────────

        private void InicializarComponentes()
        {
            // ── Panel izquierdo: selección y filtros ──────────────────────────
            var panelIzq = new Panel
            {
                Width     = 310,
                Dock      = DockStyle.Left,
                BackColor = FondoPanel,
                Padding   = new Padding(16)
            };

            var lblPanelTitulo = new Label
            {
                Text      = "Seleccionar Reporte",
                Font      = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Navy,
                AutoSize  = false,
                Width     = 278,
                Height    = 28,
                Top       = 16,
                Left      = 16
            };

            var lblTipo = new Label
            {
                Text     = "Tipo de reporte:",
                AutoSize = true,
                Left     = 16,
                Top      = 50,
                Font     = new Font("Segoe UI", 9F)
            };

            cboTipoReporte = new ComboBox
            {
                Left          = 16,
                Top           = 70,
                Width         = 278,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 9F)
            };
            cboTipoReporte.Items.AddRange(new object[]
            {
                "1. Catálogo General de Pinturas",
                "2. Resumen por Artista",
                "3. Análisis por Técnica",
                "4. Inventario por Estado",
                "5. Resumen Ejecutivo",
                "6. Pinturas por Exhibición"
            });
            cboTipoReporte.SelectedIndexChanged += CboTipoReporte_SelectedIndexChanged;

            // ── GroupBox de filtros (solo para reporte 1) ─────────────────────
            grpFiltros = new GroupBox
            {
                Text      = "Filtros opcionales",
                Left      = 10,
                Top       = 105,
                Width     = 284,
                Height    = 270,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Navy,
                Visible   = false
            };

            int fy = 22;

            grpFiltros.Controls.Add(Etiqueta("Artista:", 10, fy));
            cboFiltroArtista = new ComboBox
            {
                Left = 10, Top = fy + 18, Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F)
            };
            grpFiltros.Controls.Add(cboFiltroArtista);

            fy += 52;
            grpFiltros.Controls.Add(Etiqueta("Estado de disponibilidad:", 10, fy));
            cboFiltroEstado = new ComboBox
            {
                Left = 10, Top = fy + 18, Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F)
            };
            cboFiltroEstado.Items.AddRange(new object[]
                { "(Todos)", "Disponible", "Vendida", "Reservada" });
            cboFiltroEstado.SelectedIndex = 0;
            grpFiltros.Controls.Add(cboFiltroEstado);

            fy += 52;
            grpFiltros.Controls.Add(Etiqueta("Año creación (desde — hasta):", 10, fy));
            nudAnioDesde = new NumericUpDown
            {
                Left = 10, Top = fy + 18, Width = 120,
                Minimum = 1400, Maximum = DateTime.Now.Year,
                Value = 1900, Font = new Font("Segoe UI", 9F)
            };
            nudAnioHasta = new NumericUpDown
            {
                Left = 140, Top = fy + 18, Width = 130,
                Minimum = 1400, Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year, Font = new Font("Segoe UI", 9F)
            };
            grpFiltros.Controls.Add(nudAnioDesde);
            grpFiltros.Controls.Add(nudAnioHasta);

            fy += 52;
            grpFiltros.Controls.Add(Etiqueta("Precio (mín. — máx.):", 10, fy));
            txtPrecioMin = new TextBox
            {
                Left = 10, Top = fy + 18, Width = 120,
                Font = new Font("Segoe UI", 9F), PlaceholderText = "Mínimo"
            };
            txtPrecioMax = new TextBox
            {
                Left = 140, Top = fy + 18, Width = 130,
                Font = new Font("Segoe UI", 9F), PlaceholderText = "Máximo"
            };
            grpFiltros.Controls.Add(txtPrecioMin);
            grpFiltros.Controls.Add(txtPrecioMax);

            fy += 52;
            var btnLimpiar = new Button
            {
                Text = "Limpiar filtros",
                Left = 10, Top = fy, Width = 260, Height = 28,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Navy,
                BackColor = Blanco
            };
            btnLimpiar.FlatAppearance.BorderColor = Blue;
            btnLimpiar.Click += (_, __) => LimpiarFiltros();
            grpFiltros.Controls.Add(btnLimpiar);

            btnGenerar = new Button
            {
                Text      = "Generar y Abrir PDF",
                Left      = 16, Top = 390, Width = 278, Height = 38,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Blanco,
                BackColor = Navy,
                Cursor    = Cursors.Hand
            };
            btnGenerar.FlatAppearance.BorderSize         = 0;
            btnGenerar.FlatAppearance.MouseOverBackColor = Blue;
            btnGenerar.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 161, 224);
            btnGenerar.Click += BtnGenerar_Click;

            btnGuardar = new Button
            {
                Text      = "Guardar PDF en...",
                Left      = 16, Top = 436, Width = 278, Height = 34,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5f),
                ForeColor = Navy,
                BackColor = Blanco,
                Cursor    = Cursors.Hand,
                Enabled   = false
            };
            btnGuardar.FlatAppearance.BorderColor        = Blue;
            btnGuardar.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 249, 175);
            btnGuardar.Click += BtnGuardar_Click;

            progressBar = new ProgressBar
            {
                Left = 16, Top = 476, Width = 278, Height = 12,
                Style = ProgressBarStyle.Marquee, Visible = false
            };

            panelIzq.Controls.AddRange(new Control[]
            {
                lblPanelTitulo, lblTipo, cboTipoReporte, grpFiltros,
                btnGenerar, btnGuardar, progressBar
            });

            // ── Panel derecho: descripción e instrucciones ────────────────────
            var panelDer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

            var grpDesc = new GroupBox
            {
                Text      = "Descripción del reporte seleccionado",
                Dock      = DockStyle.Top,
                Height    = 160,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Navy
            };
            lblDescripcion = new Label
            {
                Dock      = DockStyle.Fill,
                Padding   = new Padding(8),
                Font      = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(60, 60, 60),
                Text      = "Seleccione un tipo de reporte para ver su descripción."
            };
            grpDesc.Controls.Add(lblDescripcion);

            var lblInstruccion = new Label
            {
                Text =
                    "Instrucciones:\n\n" +
                    "1. Seleccione el tipo de reporte.\n" +
                    "2. Configure los filtros si aplica (solo Catálogo General).\n" +
                    "3. Haga clic en \"Generar y Abrir PDF\" para visualizar.\n" +
                    "4. Use \"Guardar PDF en...\" para elegir dónde guardarlo.",
                AutoSize  = false,
                Dock      = DockStyle.Fill,
                Font      = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(80, 80, 80),
                Padding   = new Padding(0, 10, 0, 0)
            };
            panelDer.Controls.Add(lblInstruccion);
            panelDer.Controls.Add(grpDesc);

            var separador = new Panel
            {
                Width     = 1,
                Dock      = DockStyle.Left,
                BackColor = Color.FromArgb(197, 220, 240)
            };

            // Orden: primero panelDer (Fill), luego separador (Left), luego panelIzq (Left),
            // luego MenuLateral (Left). El último en agregarse queda más a la izquierda.
            Controls.Add(panelDer);
            Controls.Add(separador);
            Controls.Add(panelIzq);
            Controls.Add(new MenuLateral(PaginaActiva.Reportes));

            cboTipoReporte.SelectedIndex = 0;
        }

        // ─────────────────────────────────────────────────────────────────────
        // EVENTOS
        // ─────────────────────────────────────────────────────────────────────

        private void CboTipoReporte_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int idx = cboTipoReporte.SelectedIndex;
            grpFiltros.Visible = (idx == 0);

            if (idx >= 0 && idx < Descripciones.Length)
                lblDescripcion.Text = Descripciones[idx];

            btnGuardar.Enabled = false;
            _ultimoPdfTemp     = null;
        }

        private async void BtnGenerar_Click(object? sender, EventArgs e)
        {
            if (cboTipoReporte.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un tipo de reporte.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string rutaTemp = Path.Combine(Path.GetTempPath(),
                $"GaleriaArtes_Reporte_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            await EjecutarGeneracion(rutaTemp);

            if (_ultimoPdfTemp != null)
                Process.Start(new ProcessStartInfo(_ultimoPdfTemp) { UseShellExecute = true });
        }

        private async void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (_ultimoPdfTemp == null) return;

            using var dlg = new SaveFileDialog
            {
                Title      = "Guardar reporte PDF",
                Filter     = "Archivo PDF (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                FileName   = $"Reporte_{cboTipoReporte.Text.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                File.Copy(_ultimoPdfTemp, dlg.FileName, overwrite: true);
                MessageBox.Show($"Reporte guardado en:\n{dlg.FileName}",
                    "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // GENERACIÓN ASÍNCRONA
        // Los controles se leen en el hilo UI ANTES de Task.Run para evitar
        // acceso cross-thread (InvalidOperationException).
        // ─────────────────────────────────────────────────────────────────────

        private async Task EjecutarGeneracion(string rutaDestino)
        {
            // Capturar valores en hilo UI antes de entrar al Task
            int            tipoReporte = cboTipoReporte.SelectedIndex;
            FiltroPintura? filtro      = (tipoReporte == 0) ? ConstruirFiltro() : null;

            btnGenerar.Enabled  = false;
            btnGuardar.Enabled  = false;
            progressBar.Visible = true;

            try
            {
                await Task.Run(() =>
                {
                    switch (tipoReporte)
                    {
                        case 0:
                            GeneradorPDF.GenerarCatalogoPinturas(
                                _bll.ObtenerCatalogoPinturas(filtro), filtro, rutaDestino);
                            break;
                        case 1:
                            GeneradorPDF.GenerarResumenArtistas(
                                _bll.ObtenerResumenPorArtista(), rutaDestino);
                            break;
                        case 2:
                            GeneradorPDF.GenerarResumenTecnicas(
                                _bll.ObtenerResumenPorTecnica(), rutaDestino);
                            break;
                        case 3:
                            GeneradorPDF.GenerarInventarioPorEstado(
                                _bll.ObtenerInventarioPorEstado(), rutaDestino);
                            break;
                        case 4:
                            GeneradorPDF.GenerarResumenEjecutivo(
                                _bll.ObtenerResumenEjecutivo(), rutaDestino);
                            break;
                        case 5:
                            GeneradorPDF.GenerarReportePorExhibicion(
                                _bll.ObtenerPinturasPorExhibicion(), rutaDestino);
                            break;
                    }
                });

                _ultimoPdfTemp     = rutaDestino;
                btnGuardar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Siempre en hilo UI (continuación del await)
                btnGenerar.Enabled  = true;
                progressBar.Visible = false;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPERS
        // ─────────────────────────────────────────────────────────────────────

        private FiltroPintura ConstruirFiltro()
        {
            var filtro = new FiltroPintura();

            if (cboFiltroArtista.SelectedItem is Artista artSelec && artSelec.IdArtista > 0)
                filtro.IdArtista = artSelec.IdArtista;

            string estado = cboFiltroEstado.SelectedItem?.ToString() ?? "(Todos)";
            if (estado != "(Todos)")
                filtro.EstadoDisponibilidad = estado;

            int anioDesde = (int)nudAnioDesde.Value;
            int anioHasta = (int)nudAnioHasta.Value;
            if (anioDesde > 1400) filtro.AnioDesde = anioDesde;
            if (anioHasta < DateTime.Now.Year) filtro.AnioHasta = anioHasta;

            if (decimal.TryParse(txtPrecioMin.Text, out decimal precioMin) && precioMin > 0)
                filtro.PrecioMinimo = precioMin;
            if (decimal.TryParse(txtPrecioMax.Text, out decimal precioMax) && precioMax > 0)
                filtro.PrecioMaximo = precioMax;

            return filtro;
        }

        private void CargarArtistasEnFiltro()
        {
            try
            {
                var dal   = new PinturaDAL();
                var lista = dal.ObtenerArtistas();
                lista.Insert(0, new Artista { IdArtista = 0, NombreCompleto = "(Todos los artistas)" });

                cboFiltroArtista.DataSource    = lista;
                cboFiltroArtista.DisplayMember = "NombreCompleto";
                cboFiltroArtista.ValueMember   = "IdArtista";
                cboFiltroArtista.SelectedIndex = 0;
            }
            catch
            {
                cboFiltroArtista.Items.Add("(Todos los artistas)");
                cboFiltroArtista.SelectedIndex = 0;
            }
        }

        private void LimpiarFiltros()
        {
            cboFiltroArtista.SelectedIndex = 0;
            cboFiltroEstado.SelectedIndex  = 0;
            nudAnioDesde.Value             = 1900;
            nudAnioHasta.Value             = DateTime.Now.Year;
            txtPrecioMin.Clear();
            txtPrecioMax.Clear();
        }

        private static Label Etiqueta(string texto, int x, int y) => new Label
        {
            Text      = texto,
            Left      = x,
            Top       = y,
            AutoSize  = true,
            Font      = new Font("Segoe UI", 8.5f),
            ForeColor = Color.FromArgb(60, 60, 60)
        };
    }
}
