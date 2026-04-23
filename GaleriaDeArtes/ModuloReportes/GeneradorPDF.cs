using GaleriaDeArtes.CapaEntidad.Reportes;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GaleriaDeArtes.ModuloReportes
{
    /// <summary>
    /// Genera los reportes en PDF usando QuestPDF.
    /// Paleta: Navy #154D71 / Blue #1C6EA4 / Sea #33A1E0.
    /// </summary>
    public static class GeneradorPDF
    {
        // ── Paleta ────────────────────────────────────────────────────────────
        private static readonly string ColorPrimario    = "#154D71";
        private static readonly string ColorSecundario  = "#1C6EA4";
        private static readonly string ColorAcento      = "#33A1E0";
        private static readonly string ColorFondoClaro  = "#EEF6FF";
        private static readonly string ColorFondaTabla  = "#E0F0FF";
        private static readonly string ColorTextoBlanco = "#FFFFFF";
        private static readonly string ColorTextoGris   = "#6B6B6B";
        private static readonly string ColorBorde       = "#C5DCF0";

        // ─────────────────────────────────────────────────────────────────────
        // 1. VENTAS POR PERIODO
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarVentasPorPeriodo(
            List<FilaVentaPeriodo> datos,
            FiltroFechas?          filtro,
            string                 rutaDestino)
        {
            string subtitulo = filtro?.DescripcionPeriodo() ?? "Todas las fechas";

            Document.Create(c =>
            {
                c.Page(p =>
                {
                    p.Size(PageSizes.A4.Landscape());
                    p.Margin(1.5f, Unit.Centimetre);
                    p.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    p.Header().Element(h => EncabezadoPagina(h, "Ventas por Periodo", subtitulo));

                    p.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Total ventas",
                                datos.Count.ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Ingresos totales",
                                datos.Sum(d => d.PrecioVenta).ToString("C"), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Promedio por venta",
                                (datos.Any() ? datos.Average(d => d.PrecioVenta) : 0).ToString("C"),
                                "#1A6E8E");
                        });

                        col.Item().PaddingTop(8).Table(t =>
                        {
                            t.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3f);
                                cols.RelativeColumn(2.5f);
                                cols.ConstantColumn(100);
                                cols.ConstantColumn(100);
                            });

                            t.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "Pintura");
                                CeldaEncabezado(h.Cell(), "Cliente");
                                CeldaEncabezado(h.Cell(), "Fecha de Venta");
                                CeldaEncabezado(h.Cell(), "Precio de Venta");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                CeldaDato(t.Cell(), fila.Titulo,                           fondo, bold: true);
                                CeldaDato(t.Cell(), fila.Cliente,                          fondo);
                                CeldaDato(t.Cell(), fila.FechaVenta.ToString("dd/MM/yyyy"), fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.PrecioVenta.ToString("C"),        fondo, centrado: true);
                            }
                        });
                    });

                    p.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 2. TOP 10 PINTURAS MÁS VENDIDAS
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarTopPinturas(
            List<FilaTopPintura> datos,
            FiltroFechas?        filtro,
            string               rutaDestino)
        {
            string subtitulo = filtro?.TieneFiltros == true
                ? $"Periodo: {filtro.DescripcionPeriodo()}"
                : "Ordenadas por cantidad de ventas descendente";

            Document.Create(c =>
            {
                c.Page(p =>
                {
                    p.Size(PageSizes.A4);
                    p.Margin(1.5f, Unit.Centimetre);
                    p.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    p.Header().Element(h => EncabezadoPagina(h, "Top 10 Pinturas Más Vendidas", subtitulo));

                    p.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Pinturas en ranking",
                                datos.Count.ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Ventas acumuladas",
                                datos.Sum(d => d.VecesVendida).ToString(), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Ingresos totales",
                                datos.Sum(d => d.IngresoTotal).ToString("C"), "#1A6E8E");
                        });

                        col.Item().PaddingTop(8).Table(t =>
                        {
                            t.ColumnsDefinition(cols =>
                            {
                                cols.ConstantColumn(35);
                                cols.RelativeColumn(3f);
                                cols.RelativeColumn(2.5f);
                                cols.ConstantColumn(80);
                                cols.ConstantColumn(100);
                            });

                            t.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "#");
                                CeldaEncabezado(h.Cell(), "Pintura");
                                CeldaEncabezado(h.Cell(), "Artista");
                                CeldaEncabezado(h.Cell(), "Núm. Ventas");
                                CeldaEncabezado(h.Cell(), "Ingreso Total");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                CeldaDato(t.Cell(), fila.Posicion.ToString(),        fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.Titulo,                     fondo, bold: true);
                                CeldaDato(t.Cell(), fila.Artista,                    fondo);
                                CeldaDato(t.Cell(), fila.VecesVendida.ToString(),    fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.IngresoTotal.ToString("C"), fondo, centrado: true);
                            }
                        });
                    });

                    p.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 3. INVENTARIO ACTUAL
        // Incluye columna Stock = piezas disponibles (tabla PIEZA).
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarInventarioActual(
            List<FilaInventario> datos,
            string               rutaDestino)
        {
            int disponibles  = datos.Count(d => d.Estado.Equals("Disponible",  StringComparison.OrdinalIgnoreCase));
            int vendidas     = datos.Count(d => d.Estado.Equals("Vendida",     StringComparison.OrdinalIgnoreCase));
            int reservadas   = datos.Count(d => d.Estado.Equals("Reservada",   StringComparison.OrdinalIgnoreCase));
            int totalPiezas  = datos.Sum(d => d.Stock);

            Document.Create(c =>
            {
                c.Page(p =>
                {
                    p.Size(PageSizes.A4.Landscape());
                    p.Margin(1.5f, Unit.Centimetre);
                    p.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    p.Header().Element(h => EncabezadoPagina(h,
                        "Inventario Actual",
                        $"Total: {datos.Count} obras · Piezas disponibles: {totalPiezas}"));

                    p.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Total obras",
                                datos.Count.ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Piezas disponibles",
                                totalPiezas.ToString(), "#2E7D5C");
                            TarjetaStat(row.RelativeItem(), "Disponibles",
                                disponibles.ToString(), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Vendidas",
                                vendidas.ToString(), "#C0392B");
                        });

                        col.Item().PaddingTop(8).Table(t =>
                        {
                            t.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3f);   // Título
                                cols.RelativeColumn(2.5f); // Artista
                                cols.RelativeColumn(1.8f); // Técnica
                                cols.RelativeColumn(1.5f); // Dimensiones
                                cols.ConstantColumn(80);   // Precio
                                cols.ConstantColumn(55);   // Stock
                                cols.ConstantColumn(75);   // Estado
                            });

                            t.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "Título");
                                CeldaEncabezado(h.Cell(), "Artista");
                                CeldaEncabezado(h.Cell(), "Técnica");
                                CeldaEncabezado(h.Cell(), "Dimensiones");
                                CeldaEncabezado(h.Cell(), "Precio Base");
                                CeldaEncabezado(h.Cell(), "Stock");
                                CeldaEncabezado(h.Cell(), "Estado");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                CeldaDato(t.Cell(), fila.Titulo,                   fondo, bold: true);
                                CeldaDato(t.Cell(), fila.Artista,                  fondo);
                                CeldaDato(t.Cell(), fila.Tecnica,                  fondo);
                                CeldaDato(t.Cell(), fila.Dimensiones,              fondo);
                                CeldaDato(t.Cell(), fila.PrecioBase.ToString("C"), fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.Stock.ToString(),         fondo, centrado: true,
                                    color: fila.Stock == 0 ? "#C0392B" : "#155724");
                                CeldaEstado(t.Cell(), fila.Estado);
                            }
                        });
                    });

                    p.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 4. COMPRAS POR PROVEEDOR
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarComprasPorProveedor(
            List<FilaCompraPorProveedor> datos,
            FiltroFechas?                filtro,
            string                       rutaDestino)
        {
            string subtitulo = ConstruirSubtituloFiltro(filtro, "proveedor");

            Document.Create(c =>
            {
                c.Page(p =>
                {
                    p.Size(PageSizes.A4);
                    p.Margin(1.5f, Unit.Centimetre);
                    p.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    p.Header().Element(h => EncabezadoPagina(h, "Compras por Proveedor", subtitulo));

                    p.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Proveedores",
                                datos.Count.ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Total compras",
                                datos.Sum(d => d.TotalCompras).ToString(), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Monto total",
                                datos.Sum(d => d.MontoTotal).ToString("C"), "#1A6E8E");
                        });

                        col.Item().PaddingTop(8).Table(t =>
                        {
                            t.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3f);
                                cols.ConstantColumn(90);
                                cols.ConstantColumn(110);
                                cols.ConstantColumn(100);
                            });

                            t.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "Proveedor");
                                CeldaEncabezado(h.Cell(), "Núm. Compras");
                                CeldaEncabezado(h.Cell(), "Monto Total");
                                CeldaEncabezado(h.Cell(), "Última Compra");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                CeldaDato(t.Cell(), fila.Proveedor,                                   fondo, bold: true);
                                CeldaDato(t.Cell(), fila.TotalCompras.ToString(),                      fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.MontoTotal.ToString("C"),                     fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.UltimaCompra?.ToString("dd/MM/yyyy") ?? "—",  fondo, centrado: true);
                            }

                            // Fila de totales
                            t.Cell().ColumnSpan(2).Background(ColorPrimario).Padding(4)
                                .Text("TOTALES").FontColor(ColorTextoBlanco).Bold().FontSize(9);
                            t.Cell().Background(ColorPrimario).Padding(4)
                                .Text(datos.Sum(d => d.MontoTotal).ToString("C"))
                                .FontColor(ColorTextoBlanco).Bold().FontSize(9).AlignCenter();
                            t.Cell().Background(ColorPrimario).Padding(4)
                                .Text("—").FontColor(ColorTextoBlanco).FontSize(9).AlignCenter();
                        });
                    });

                    p.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 5. VENTAS POR CLIENTE
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarVentasPorCliente(
            List<FilaVentaPorCliente> datos,
            FiltroFechas?             filtro,
            string                    rutaDestino)
        {
            string subtitulo = ConstruirSubtituloFiltro(filtro, "cliente");

            Document.Create(c =>
            {
                c.Page(p =>
                {
                    p.Size(PageSizes.A4);
                    p.Margin(1.5f, Unit.Centimetre);
                    p.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    p.Header().Element(h => EncabezadoPagina(h, "Ventas por Cliente", subtitulo));

                    p.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Clientes",
                                datos.Count.ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Total ventas",
                                datos.Sum(d => d.TotalVentas).ToString(), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Monto total",
                                datos.Sum(d => d.MontoTotal).ToString("C"), "#1A6E8E");
                        });

                        col.Item().PaddingTop(8).Table(t =>
                        {
                            t.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3f);
                                cols.ConstantColumn(90);
                                cols.ConstantColumn(120);
                            });

                            t.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "Cliente");
                                CeldaEncabezado(h.Cell(), "Núm. Ventas");
                                CeldaEncabezado(h.Cell(), "Total Gastado");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                CeldaDato(t.Cell(), fila.Cliente,                  fondo, bold: true);
                                CeldaDato(t.Cell(), fila.TotalVentas.ToString(),   fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.MontoTotal.ToString("C"), fondo, centrado: true);
                            }

                            // Fila de totales
                            t.Cell().Background(ColorPrimario).Padding(4)
                                .Text("TOTALES").FontColor(ColorTextoBlanco).Bold().FontSize(9);
                            t.Cell().Background(ColorPrimario).Padding(4)
                                .Text(datos.Sum(d => d.TotalVentas).ToString())
                                .FontColor(ColorTextoBlanco).Bold().FontSize(9).AlignCenter();
                            t.Cell().Background(ColorPrimario).Padding(4)
                                .Text(datos.Sum(d => d.MontoTotal).ToString("C"))
                                .FontColor(ColorTextoBlanco).Bold().FontSize(9).AlignCenter();
                        });
                    });

                    p.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 6. VENTAS POR MES  (sin columna "Participación" — siempre 100%)
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarVentasPorMes(
            List<FilaVentaPorMes> datos,
            FiltroMes             filtro,
            string                rutaDestino)
        {
            string periodoLabel =
                $"{char.ToUpper(filtro.NombreMes[0])}{filtro.NombreMes[1..]} {filtro.Anio}";

            Document.Create(c =>
            {
                c.Page(p =>
                {
                    p.Size(PageSizes.A4);
                    p.Margin(1.5f, Unit.Centimetre);
                    p.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    p.Header().Element(h => EncabezadoPagina(h,
                        $"Reporte de Ventas — {periodoLabel}",
                        datos.Count > 0
                            ? $"Total: {datos.Sum(d => d.MontoTotal):C} · {datos.Sum(d => d.TotalVentas)} ventas"
                            : "Sin ventas registradas para el periodo seleccionado"));

                    p.Content().PaddingTop(10).Column(col =>
                    {
                        if (!datos.Any())
                        {
                            col.Item().PaddingTop(40).AlignCenter()
                                .Text($"No se encontraron ventas para {periodoLabel}.")
                                .FontColor(ColorTextoGris).FontSize(11).Italic();
                            return;
                        }

                        col.Item().Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Núm. ventas",
                                datos.Sum(d => d.TotalVentas).ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Ingresos totales",
                                datos.Sum(d => d.MontoTotal).ToString("C"), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Promedio por venta",
                                (datos.Sum(d => d.TotalVentas) > 0
                                    ? datos.Sum(d => d.MontoTotal) / datos.Sum(d => d.TotalVentas)
                                    : 0).ToString("C"),
                                "#1A6E8E");
                        });

                        col.Item().PaddingTop(8).Table(t =>
                        {
                            t.ColumnsDefinition(cols =>
                            {
                                cols.ConstantColumn(110); // Mes
                                cols.ConstantColumn(100); // # Ventas
                                cols.ConstantColumn(130); // Monto
                            });

                            t.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "Mes (yyyy-MM)");
                                CeldaEncabezado(h.Cell(), "Núm. Ventas");
                                CeldaEncabezado(h.Cell(), "Monto Total");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                CeldaDato(t.Cell(), fila.Mes,                      fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.TotalVentas.ToString(),   fondo, centrado: true);
                                CeldaDato(t.Cell(), fila.MontoTotal.ToString("C"), fondo, centrado: true);
                            }

                            // Fila de totales
                            t.Cell().Background(ColorPrimario).Padding(4)
                                .Text("TOTALES").FontColor(ColorTextoBlanco).Bold().FontSize(9);
                            t.Cell().Background(ColorPrimario).Padding(4)
                                .Text(datos.Sum(d => d.TotalVentas).ToString())
                                .FontColor(ColorTextoBlanco).Bold().FontSize(9).AlignCenter();
                            t.Cell().Background(ColorPrimario).Padding(4)
                                .Text(datos.Sum(d => d.MontoTotal).ToString("C"))
                                .FontColor(ColorTextoBlanco).Bold().FontSize(9).AlignCenter();
                        });
                    });

                    p.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // COMPONENTES REUTILIZABLES
        // ─────────────────────────────────────────────────────────────────────

        private static string ConstruirSubtituloFiltro(FiltroFechas? filtro, string entidad)
        {
            var partes = new List<string>();
            if (filtro?.FechaDesde.HasValue == true || filtro?.FechaHasta.HasValue == true)
                partes.Add($"Periodo: {filtro!.DescripcionPeriodo()}");
            if (!string.IsNullOrWhiteSpace(filtro?.TextoBusqueda))
                partes.Add($"Búsqueda {entidad}: \"{filtro!.TextoBusqueda}\"");
            return partes.Count > 0 ? string.Join(" · ", partes) : "Todos los registros";
        }

        private static void EncabezadoPagina(IContainer c, string titulo, string subtitulo)
        {
            c.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.ConstantItem(140).Background(ColorPrimario).Padding(8).Column(inner =>
                    {
                        inner.Item().Text("GALERÍA DE ARTES")
                            .FontColor(ColorTextoBlanco).Bold().FontSize(10);
                        inner.Item().Text("Sistema de Gestión")
                            .FontColor(ColorAcento).FontSize(7);
                    });

                    row.RelativeItem().Background(ColorFondoClaro).Padding(8).Column(inner =>
                    {
                        inner.Item().Text(titulo)
                            .FontColor(ColorPrimario).Bold().FontSize(14);
                        inner.Item().Text(subtitulo)
                            .FontColor(ColorTextoGris).FontSize(8);
                    });

                    row.ConstantItem(110).Background(ColorFondoClaro)
                        .AlignRight().Padding(8).Column(inner =>
                    {
                        inner.Item().Text(DateTime.Now.ToString("dd/MM/yyyy"))
                            .FontColor(ColorPrimario).Bold().FontSize(9);
                        inner.Item().Text(DateTime.Now.ToString("HH:mm"))
                            .FontColor(ColorTextoGris).FontSize(8);
                    });
                });

                col.Item().Height(3).Background(ColorAcento);
            });
        }

        private static void PiePagina(IContainer c)
        {
            c.Column(col =>
            {
                col.Item().Height(1).Background(ColorBorde);
                col.Item().PaddingTop(4).Row(row =>
                {
                    row.RelativeItem()
                        .Text("Galería de Artes — Reporte generado automáticamente")
                        .FontColor(ColorTextoGris).FontSize(7);
                    row.ConstantItem(80).AlignRight()
                        .Text(x =>
                        {
                            x.Span("Página ").FontColor(ColorTextoGris).FontSize(7);
                            x.CurrentPageNumber().FontColor(ColorPrimario).FontSize(7).Bold();
                            x.Span(" de ").FontColor(ColorTextoGris).FontSize(7);
                            x.TotalPages().FontColor(ColorPrimario).FontSize(7).Bold();
                        });
                });
            });
        }

        private static void TarjetaStat(IContainer c, string etiqueta, string valor, string colorFondo)
        {
            c.Padding(3).Border(1).BorderColor(ColorBorde)
                .Background(colorFondo).Padding(8).Column(col =>
            {
                col.Item().Text(etiqueta).FontColor(ColorTextoBlanco).FontSize(8);
                col.Item().PaddingTop(4).Text(valor)
                    .FontColor(ColorTextoBlanco).Bold().FontSize(14);
            });
        }

        private static void CeldaEncabezado(IContainer c, string texto)
        {
            c.Background(ColorPrimario).Padding(5)
             .Text(texto).FontColor(ColorTextoBlanco).Bold().FontSize(8.5f);
        }

        private static void CeldaDato(IContainer c, string texto, string fondo,
            bool bold = false, bool centrado = false, string? color = null)
        {
            var style = c.Background(fondo).BorderBottom(1).BorderColor(ColorBorde)
                .Padding(4).Text(texto)
                .FontColor(color ?? "#1A1A1A")
                .FontSize(8.5f);

            if (bold)     style.Bold();
            if (centrado) style.AlignCenter();
        }

        private static void CeldaEstado(IContainer c, string estado)
        {
            string fondo = estado.ToLower() switch
            {
                "disponible" => "#D4EDDA",
                "vendida"    => "#F8D7DA",
                "reservada"  => "#FFF3CD",
                _            => "#E9ECEF"
            };
            string texto = estado.ToLower() switch
            {
                "disponible" => "#155724",
                "vendida"    => "#721C24",
                "reservada"  => "#856404",
                _            => "#495057"
            };

            c.Background(fondo).BorderBottom(1).BorderColor(ColorBorde)
                .Padding(4).Text(estado).FontColor(texto).Bold().FontSize(8f).AlignCenter();
        }
    }
}
