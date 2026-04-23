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
        private ComboBox       cboTipoReporte  = null!;
        private GroupBox       grpFiltros      = null!;   // fechas (reportes 0,1,3,4)
        private DateTimePicker dtpFechaDesde   = null!;
        private DateTimePicker dtpFechaHasta   = null!;
        private GroupBox       grpFiltroTexto  = null!;   // cliente/proveedor (reportes 3,4)
        private Label          lblFiltroTexto  = null!;
        private TextBox        txtFiltroTexto  = null!;
        private GroupBox       grpFiltroMes    = null!;   // mes/año (reporte 5)
        private ComboBox       cboMes          = null!;
        private ComboBox       cboAnio         = null!;
        private Label          lblDescripcion  = null!;
        private Button         btnGenerar      = null!;
        private Button         btnGuardar      = null!;
        private ProgressBar    progressBar     = null!;

        // ── Lógica ────────────────────────────────────────────────────────────
        private readonly ReporteBLL _bll = new ReporteBLL();
        private string? _ultimoPdfTemp = null;

        private static readonly string[] Descripciones =
        {
            // 0 - Ventas por Periodo
            "Lista todas las ventas dentro de un rango de fechas.\n" +
            "Muestra la pintura vendida, el cliente, la fecha y el precio.\n" +
            "Filtra por fecha de inicio y fecha de fin.",

            // 1 - Top 10
            "Ranking de las 10 pinturas con mayor número de ventas.\n" +
            "Filtra opcionalmente por periodo de tiempo.\n" +
            "Incluye número de ventas e ingreso total por obra.",

            // 2 - Inventario
            "Existencias actuales con stock de piezas disponibles.\n" +
            "Muestra título, artista, técnica, precio, estado y cantidad\n" +
            "de piezas disponibles (tabla PIEZA).",

            // 3 - Compras por Proveedor
            "Resumen de compras agrupadas por proveedor.\n" +
            "Filtra por periodo de tiempo y/o nombre de proveedor.\n" +
            "Incluye número de compras, monto total y última compra.",

            // 4 - Ventas por Cliente
            "Total de compras realizadas por cada cliente.\n" +
            "Filtra por periodo de tiempo y/o nombre de cliente.\n" +
            "Incluye número de ventas y monto total gastado.",

            // 5 - Ventas por Mes
            "Ventas del mes y año seleccionados.\n" +
            "Muestra cantidad de ventas e ingresos del periodo.\n" +
            "Selecciona mes y año para filtrar el resultado."
        };

        public Reportes()
        {
            ConfiguracionFormulario.Aplicar(this);
            Text = "Módulo de Reportes — Galería de Artes";
            InicializarComponentes();
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONSTRUCCIÓN DE LA INTERFAZ
        // ─────────────────────────────────────────────────────────────────────

        private void InicializarComponentes()
        {
            // ── Panel izquierdo ───────────────────────────────────────────────
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
                "1. Ventas por Periodo",
                "2. Top 10 Pinturas Más Vendidas",
                "3. Inventario Actual",
                "4. Compras por Proveedor",
                "5. Ventas por Cliente",
                "6. Ventas por Mes"
            });
            cboTipoReporte.SelectedIndexChanged += CboTipoReporte_SelectedIndexChanged;

            // ── GroupBox fechas (reportes 0,1,3,4) ────────────────────────────
            grpFiltros = new GroupBox
            {
                Text      = "Filtros de fecha",
                Left      = 10,
                Top       = 105,
                Width     = 284,
                Height    = 162,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Navy,
                Visible   = false
            };

            grpFiltros.Controls.Add(Etiqueta("Fecha inicio:", 10, 22));
            dtpFechaDesde = new DateTimePicker
            {
                Left   = 10, Top = 40, Width = 260,
                Format = DateTimePickerFormat.Short,
                Value  = DateTime.Now.AddMonths(-1),
                Font   = new Font("Segoe UI", 9F)
            };
            grpFiltros.Controls.Add(dtpFechaDesde);

            grpFiltros.Controls.Add(Etiqueta("Fecha fin:", 10, 72));
            dtpFechaHasta = new DateTimePicker
            {
                Left   = 10, Top = 90, Width = 260,
                Format = DateTimePickerFormat.Short,
                Value  = DateTime.Now,
                Font   = new Font("Segoe UI", 9F)
            };
            grpFiltros.Controls.Add(dtpFechaHasta);

            var btnLimpiar = new Button
            {
                Text      = "↺  Restablecer fechas",
                Left      = 10,
                Top       = 126,
                Width     = 260,
                Height    = 28,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Navy,
                BackColor = Color.FromArgb(225, 238, 250),
                Cursor    = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnLimpiar.FlatAppearance.BorderColor        = Blue;
            btnLimpiar.FlatAppearance.BorderSize         = 1;
            btnLimpiar.FlatAppearance.MouseOverBackColor = Color.FromArgb(197, 220, 240);
            btnLimpiar.Click += (_, __) =>
            {
                dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
                dtpFechaHasta.Value = DateTime.Now;
            };
            grpFiltros.Controls.Add(btnLimpiar);

            // ── GroupBox búsqueda cliente/proveedor (reportes 3,4) ────────────
            grpFiltroTexto = new GroupBox
            {
                Text      = "Buscar",
                Left      = 10,
                Top       = 275,   // justo debajo de grpFiltros (105+162+8)
                Width     = 284,
                Height    = 78,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Navy,
                Visible   = false
            };

            lblFiltroTexto = new Label
            {
                Text     = "Nombre:",
                Left     = 10, Top = 22,
                AutoSize = true,
                Font     = new Font("Segoe UI", 8.5f)
            };

            txtFiltroTexto = new TextBox
            {
                Left            = 10, Top = 40, Width = 260,
                PlaceholderText = "Dejar vacío para todos...",
                Font            = new Font("Segoe UI", 9F)
            };

            grpFiltroTexto.Controls.Add(lblFiltroTexto);
            grpFiltroTexto.Controls.Add(txtFiltroTexto);

            // ── GroupBox mes/año (reporte 5) ──────────────────────────────────
            grpFiltroMes = new GroupBox
            {
                Text      = "Filtros de mes y año",
                Left      = 10,
                Top       = 105,
                Width     = 284,
                Height    = 130,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Navy,
                Visible   = false
            };

            grpFiltroMes.Controls.Add(Etiqueta("Mes:", 10, 22));
            cboMes = new ComboBox
            {
                Left          = 10, Top = 40, Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 9F)
            };
            cboMes.Items.AddRange(new object[]
            {
                "01 - Enero",   "02 - Febrero", "03 - Marzo",    "04 - Abril",
                "05 - Mayo",    "06 - Junio",   "07 - Julio",    "08 - Agosto",
                "09 - Septiembre", "10 - Octubre", "11 - Noviembre", "12 - Diciembre"
            });
            cboMes.SelectedIndex = DateTime.Now.Month - 1;
            grpFiltroMes.Controls.Add(cboMes);

            grpFiltroMes.Controls.Add(Etiqueta("Año:", 10, 72));
            cboAnio = new ComboBox
            {
                Left          = 10, Top = 90, Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 9F)
            };
            for (int y = DateTime.Now.Year; y >= DateTime.Now.Year - 5; y--)
                cboAnio.Items.Add(y);
            cboAnio.SelectedIndex = 0;
            grpFiltroMes.Controls.Add(cboAnio);

            // ── Botones (posición fija bajo todos los posibles grupos) ─────────
            btnGenerar = new Button
            {
                Text      = "Generar y Abrir PDF",
                Left      = 16, Top = 368, Width = 278, Height = 38,
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
                Left      = 16, Top = 414, Width = 278, Height = 34,
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
                Left = 16, Top = 455, Width = 278, Height = 12,
                Style = ProgressBarStyle.Marquee, Visible = false
            };

            panelIzq.Controls.AddRange(new Control[]
            {
                lblPanelTitulo, lblTipo, cboTipoReporte,
                grpFiltros, grpFiltroTexto, grpFiltroMes,
                btnGenerar, btnGuardar, progressBar
            });

            // ── Panel derecho ─────────────────────────────────────────────────
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
                    "1. Seleccione el tipo de reporte en el panel izquierdo.\n" +
                    "2. Configure el rango de fechas si aplica (reportes 1, 2, 4, 5).\n" +
                    "3. En reportes 4 y 5 puede buscar por proveedor o cliente específico.\n" +
                    "4. Haga clic en \"Generar y Abrir PDF\" para visualizar el reporte.\n" +
                    "5. Use \"Guardar PDF en...\" para elegir dónde guardar el archivo.",
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

            // Fechas: visible para reportes 0 (Ventas Periodo), 1 (Top 10),
            //                               3 (Compras Proveedor), 4 (Ventas Cliente)
            grpFiltros.Visible     = idx == 0 || idx == 1 || idx == 3 || idx == 4;
            // Búsqueda texto: solo para 3 (proveedor) y 4 (cliente)
            grpFiltroTexto.Visible = idx == 3 || idx == 4;
            // Mes/año: solo para 5 (Ventas por Mes)
            grpFiltroMes.Visible   = idx == 5;

            if (idx == 3)
            {
                grpFiltroTexto.Text   = "Buscar proveedor";
                lblFiltroTexto.Text   = "Nombre de proveedor:";
                txtFiltroTexto.PlaceholderText = "Dejar vacío para todos...";
            }
            else if (idx == 4)
            {
                grpFiltroTexto.Text   = "Buscar cliente";
                lblFiltroTexto.Text   = "Nombre de cliente:";
                txtFiltroTexto.PlaceholderText = "Dejar vacío para todos...";
            }

            txtFiltroTexto.Clear();

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
        // Los valores de los controles se leen en el hilo UI antes de Task.Run.
        // ─────────────────────────────────────────────────────────────────────

        private async Task EjecutarGeneracion(string rutaDestino)
        {
            int          idx        = cboTipoReporte.SelectedIndex;
            FiltroFechas filtro     = ConstruirFiltro(idx);
            FiltroMes?   filtroMes  = ConstruirFiltroMes(idx);

            if (filtroMes == null) return;  // validación ya mostró el mensaje

            btnGenerar.Enabled  = false;
            btnGuardar.Enabled  = false;
            progressBar.Visible = true;

            try
            {
                await Task.Run(() =>
                {
                    switch (idx)
                    {
                        case 0:
                            GeneradorPDF.GenerarVentasPorPeriodo(
                                _bll.ObtenerVentasPorPeriodo(filtro), filtro, rutaDestino);
                            break;
                        case 1:
                            GeneradorPDF.GenerarTopPinturas(
                                _bll.ObtenerTopPinturas(filtro), filtro, rutaDestino);
                            break;
                        case 2:
                            GeneradorPDF.GenerarInventarioActual(
                                _bll.ObtenerInventarioActual(), rutaDestino);
                            break;
                        case 3:
                            GeneradorPDF.GenerarComprasPorProveedor(
                                _bll.ObtenerComprasPorProveedor(filtro), filtro, rutaDestino);
                            break;
                        case 4:
                            GeneradorPDF.GenerarVentasPorCliente(
                                _bll.ObtenerVentasPorCliente(filtro), filtro, rutaDestino);
                            break;
                        case 5:
                            GeneradorPDF.GenerarVentasPorMes(
                                _bll.ObtenerVentasPorMes(filtroMes!), filtroMes!, rutaDestino);
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
                btnGenerar.Enabled  = true;
                progressBar.Visible = false;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPERS
        // ─────────────────────────────────────────────────────────────────────

        private FiltroFechas ConstruirFiltro(int idx)
        {
            // Sin fechas para reportes que no las usan (inventario=2, mes=5)
            if (idx == 2 || idx == 5)
                return new FiltroFechas();

            var f = new FiltroFechas
            {
                FechaDesde = dtpFechaDesde.Value.Date,
                FechaHasta = dtpFechaHasta.Value.Date
            };

            // Texto de búsqueda solo para proveedor/cliente
            if (idx == 3 || idx == 4)
                f.TextoBusqueda = string.IsNullOrWhiteSpace(txtFiltroTexto.Text)
                    ? null
                    : txtFiltroTexto.Text.Trim();

            return f;
        }

        private FiltroMes? ConstruirFiltroMes(int idx)
        {
            if (idx != 5)
                return new FiltroMes { Mes = 1, Anio = DateTime.Now.Year };

            if (cboMes.SelectedIndex < 0 || cboAnio.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione mes y año para generar el reporte.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return new FiltroMes
            {
                Mes  = cboMes.SelectedIndex + 1,
                Anio = (int)cboAnio.SelectedItem!
            };
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
