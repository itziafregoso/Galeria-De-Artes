using GaleriaDeArtes.CapaEntidad.Reportes;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GaleriaDeArtes.ModuloReportes
{
    /// <summary>
    /// Genera todos los reportes en formato PDF usando QuestPDF.
    /// Cada método público retorna la ruta del archivo generado.
    /// Paleta de colores: tonos vinotinto/marrón para tema galería de arte.
    /// </summary>
    public static class GeneradorPDF
    {
        // ── Paleta de colores del sistema (#154D71 Navy / #1C6EA4 Blue / #33A1E0 Sea / #FFF9AF Yellow)
        private static readonly string ColorPrimario    = "#154D71";   // Navy oscuro
        private static readonly string ColorSecundario  = "#1C6EA4";   // Blue medio
        private static readonly string ColorAcento      = "#33A1E0";   // Sea / celeste
        private static readonly string ColorFondoClaro  = "#EEF6FF";   // Azul muy claro
        private static readonly string ColorFondaTabla  = "#E0F0FF";   // Azul tabla alternado
        private static readonly string ColorTextoBlanco = "#FFFFFF";
        private static readonly string ColorTextoGris   = "#6B6B6B";
        private static readonly string ColorBorde       = "#C5DCF0";   // Borde azul suave

        // ─────────────────────────────────────────────────────────────────────
        // 1. CATÁLOGO GENERAL DE PINTURAS
        // ─────────────────────────────────────────────────────────────────────

        /// <param name="datos">Filas del catálogo.</param>
        /// <param name="filtro">Filtro aplicado (puede ser null).</param>
        /// <param name="rutaDestino">Ruta completa del archivo PDF a generar.</param>
        public static void GenerarCatalogoPinturas(
            List<FilaCatalogoPintura> datos,
            FiltroPintura?            filtro,
            string                    rutaDestino)
        {
            Document.Create(contenedor =>
            {
                contenedor.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4.Landscape());
                    pagina.Margin(1.5f, Unit.Centimetre);
                    pagina.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    pagina.Header().Element(c => EncabezadoPagina(c,
                        "Catálogo General de Pinturas",
                        filtro?.TieneFiltros == true ? "Con filtros aplicados" : "Sin filtros"));

                    pagina.Content().PaddingTop(10).Column(col =>
                    {
                        // Estadísticas rápidas
                        col.Item().Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Total obras",
                                datos.Count.ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Valor total",
                                datos.Sum(d => d.PrecioBase).ToString("C"), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Precio promedio",
                                (datos.Any() ? datos.Average(d => d.PrecioBase) : 0).ToString("C"),
                                "#1A6E8E");
                            TarjetaStat(row.RelativeItem(), "Disponibles",
                                datos.Count(d => d.Estado.Equals("Disponible",
                                    StringComparison.OrdinalIgnoreCase)).ToString(),
                                "#2E7D5C");
                        });

                        col.Item().PaddingTop(8).Table(tabla =>
                        {
                            // Definición de columnas
                            tabla.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3.0f); // Título
                                cols.RelativeColumn(2.5f); // Artista
                                cols.RelativeColumn(1.8f); // Técnica
                                cols.ConstantColumn(45);   // Año
                                cols.RelativeColumn(1.5f); // Dimensiones
                                cols.ConstantColumn(80);   // Precio
                                cols.ConstantColumn(72);   // Estado
                                cols.RelativeColumn(2.5f); // Exhibiciones
                            });

                            // Encabezado de tabla
                            tabla.Header(header =>
                            {
                                CeldaEncabezado(header.Cell(), "Título");
                                CeldaEncabezado(header.Cell(), "Artista");
                                CeldaEncabezado(header.Cell(), "Técnica");
                                CeldaEncabezado(header.Cell(), "Año");
                                CeldaEncabezado(header.Cell(), "Dimensiones");
                                CeldaEncabezado(header.Cell(), "Precio");
                                CeldaEncabezado(header.Cell(), "Estado");
                                CeldaEncabezado(header.Cell(), "Exhibición(es)");
                            });

                            // Filas
                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;

                                CeldaDato(tabla.Cell(), fila.Titulo,          fondo, bold: true);
                                CeldaDato(tabla.Cell(), fila.Artista,         fondo);
                                CeldaDato(tabla.Cell(), fila.Tecnica,         fondo);
                                CeldaDato(tabla.Cell(), fila.AnioCreacion?.ToString() ?? "—", fondo, centrado: true);
                                CeldaDato(tabla.Cell(), fila.Dimensiones,     fondo);
                                CeldaDato(tabla.Cell(), fila.PrecioBase.ToString("C"), fondo, centrado: true);
                                CeldaEstado(tabla.Cell(),fila.Estado);
                                CeldaDato(tabla.Cell(), fila.Exhibiciones,    fondo, color: ColorTextoGris);
                            }
                        });
                    });

                    pagina.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 2. RESUMEN POR ARTISTA
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarResumenArtistas(
            List<FilaResumenArtista> datos,
            string                   rutaDestino)
        {
            Document.Create(contenedor =>
            {
                contenedor.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4);
                    pagina.Margin(1.5f, Unit.Centimetre);
                    pagina.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    pagina.Header().Element(c => EncabezadoPagina(c,
                        "Producción por Artista",
                        $"{datos.Count} artistas registrados"));

                    pagina.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Artistas", datos.Count.ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Obras totales",
                                datos.Sum(d => d.TotalPinturas).ToString(), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Valor total",
                                datos.Sum(d => d.ValorTotal).ToString("C"), "#1A6E8E");
                        });

                        col.Item().PaddingTop(8).Table(tabla =>
                        {
                            tabla.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3f);   // Artista
                                cols.RelativeColumn(2f);   // Nacionalidad
                                cols.RelativeColumn(2f);   // Estilo
                                cols.ConstantColumn(65);   // Obras
                                cols.ConstantColumn(95);   // Valor total
                                cols.ConstantColumn(95);   // Precio promedio
                            });

                            tabla.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "Artista");
                                CeldaEncabezado(h.Cell(), "Nacionalidad");
                                CeldaEncabezado(h.Cell(), "Estilo Predominante");
                                CeldaEncabezado(h.Cell(), "Obras");
                                CeldaEncabezado(h.Cell(), "Valor Total");
                                CeldaEncabezado(h.Cell(), "Precio Promedio");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                CeldaDato(tabla.Cell(), fila.NombreArtista,                    fondo, bold: true);
                                CeldaDato(tabla.Cell(), fila.Nacionalidad,                     fondo);
                                CeldaDato(tabla.Cell(), fila.EstiloPredominante,               fondo);
                                CeldaDato(tabla.Cell(), fila.TotalPinturas.ToString(),         fondo, centrado: true);
                                CeldaDato(tabla.Cell(), fila.ValorTotal.ToString("C"),         fondo, centrado: true);
                                CeldaDato(tabla.Cell(), fila.PrecioPromedio.ToString("C"),     fondo, centrado: true);
                            }
                        });
                    });

                    pagina.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 3. ANÁLISIS POR TÉCNICA
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarResumenTecnicas(
            List<FilaResumenTecnica> datos,
            string                   rutaDestino)
        {
            int totalObras = datos.Sum(d => d.TotalPinturas);

            Document.Create(contenedor =>
            {
                contenedor.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4);
                    pagina.Margin(1.5f, Unit.Centimetre);
                    pagina.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    pagina.Header().Element(c => EncabezadoPagina(c,
                        "Análisis por Técnica Pictórica",
                        $"{datos.Count} técnicas · {totalObras} obras en total"));

                    pagina.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3f);
                                cols.ConstantColumn(70);
                                cols.ConstantColumn(70);  // porcentaje
                                cols.ConstantColumn(110);
                                cols.ConstantColumn(110);
                            });

                            tabla.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "Técnica");
                                CeldaEncabezado(h.Cell(), "Obras");
                                CeldaEncabezado(h.Cell(), "% del total");
                                CeldaEncabezado(h.Cell(), "Valor Total");
                                CeldaEncabezado(h.Cell(), "Precio Promedio");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                double pct = totalObras > 0
                                    ? fila.TotalPinturas * 100.0 / totalObras
                                    : 0;
                                CeldaDato(tabla.Cell(), fila.NombreTecnica,            fondo, bold: true);
                                CeldaDato(tabla.Cell(), fila.TotalPinturas.ToString(), fondo, centrado: true);
                                CeldaDato(tabla.Cell(), $"{pct:F1}%",                  fondo, centrado: true);
                                CeldaDato(tabla.Cell(), fila.ValorTotal.ToString("C"), fondo, centrado: true);
                                CeldaDato(tabla.Cell(), fila.PrecioPromedio.ToString("C"), fondo, centrado: true);
                            }
                        });
                    });

                    pagina.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 4. INVENTARIO POR ESTADO
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarInventarioPorEstado(
            List<FilaInventarioEstado> datos,
            string                     rutaDestino)
        {
            Document.Create(contenedor =>
            {
                contenedor.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4);
                    pagina.Margin(1.5f, Unit.Centimetre);
                    pagina.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    pagina.Header().Element(c => EncabezadoPagina(c,
                        "Inventario por Estado de Disponibilidad",
                        $"Valor total del inventario: {datos.Sum(d => d.ValorTotal):C}"));

                    pagina.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(2.5f);
                                cols.ConstantColumn(75);
                                cols.ConstantColumn(85);
                                cols.ConstantColumn(110);
                                cols.ConstantColumn(110);
                            });

                            tabla.Header(h =>
                            {
                                CeldaEncabezado(h.Cell(), "Estado");
                                CeldaEncabezado(h.Cell(), "Cantidad");
                                CeldaEncabezado(h.Cell(), "% Participación");
                                CeldaEncabezado(h.Cell(), "Valor Total");
                                CeldaEncabezado(h.Cell(), "Precio Promedio");
                            });

                            bool par = false;
                            foreach (var fila in datos)
                            {
                                string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                par = !par;
                                CeldaDato(tabla.Cell(), fila.Estado,                     fondo, bold: true);
                                CeldaDato(tabla.Cell(), fila.Cantidad.ToString(),         fondo, centrado: true);
                                CeldaDato(tabla.Cell(), $"{fila.Porcentaje:F1}%",         fondo, centrado: true);
                                CeldaDato(tabla.Cell(), fila.ValorTotal.ToString("C"),    fondo, centrado: true);
                                CeldaDato(tabla.Cell(), fila.PrecioPromedio.ToString("C"),fondo, centrado: true);
                            }

                            // Fila de totales
                            tabla.Cell().ColumnSpan(2)
                                .Background(ColorPrimario).Padding(4)
                                .Text("TOTALES").FontColor(ColorTextoBlanco).Bold().FontSize(9);
                            tabla.Cell().Background(ColorPrimario).Padding(4)
                                .Text($"{datos.Sum(d => d.Porcentaje):F1}%")
                                .FontColor(ColorTextoBlanco).Bold().FontSize(9).AlignCenter();
                            tabla.Cell().Background(ColorPrimario).Padding(4)
                                .Text(datos.Sum(d => d.ValorTotal).ToString("C"))
                                .FontColor(ColorTextoBlanco).Bold().FontSize(9).AlignCenter();
                            tabla.Cell().Background(ColorPrimario).Padding(4)
                                .Text("—").FontColor(ColorTextoBlanco).FontSize(9).AlignCenter();
                        });
                    });

                    pagina.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 5. RESUMEN EJECUTIVO
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarResumenEjecutivo(
            ResumenEjecutivo resumen,
            string           rutaDestino)
        {
            Document.Create(contenedor =>
            {
                contenedor.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4);
                    pagina.Margin(2f, Unit.Centimetre);
                    pagina.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(10));

                    pagina.Header().Element(c => EncabezadoPagina(c,
                        "Resumen Ejecutivo de la Galería",
                        $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}"));

                    pagina.Content().PaddingTop(15).Column(col =>
                    {
                        // ── Bloque 1: KPIs principales ─────────────────────
                        col.Item().Text("Indicadores Clave").FontSize(12).Bold()
                            .FontColor(ColorPrimario);
                        col.Item().PaddingTop(6).Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Artistas registrados",
                                resumen.TotalArtistas.ToString(), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Pinturas en catálogo",
                                resumen.TotalPinturas.ToString(), ColorSecundario);
                            TarjetaStat(row.RelativeItem(), "Exhibiciones activas",
                                resumen.TotalExhibiciones.ToString(), "#1A6E8E");
                        });

                        col.Item().PaddingTop(8).Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Valor total inventario",
                                resumen.ValorTotalInventario.ToString("C"), ColorPrimario);
                            TarjetaStat(row.RelativeItem(), "Precio promedio por obra",
                                resumen.PrecioPromedio.ToString("C"), ColorSecundario);
                        });

                        // ── Bloque 2: Disponibilidad ───────────────────────
                        col.Item().PaddingTop(16)
                            .Text("Estado del Inventario").FontSize(12).Bold()
                            .FontColor(ColorPrimario);
                        col.Item().PaddingTop(6).Row(row =>
                        {
                            TarjetaStat(row.RelativeItem(), "Disponibles",
                                resumen.PinturasDisponibles.ToString(), "#2E7D5C");
                            TarjetaStat(row.RelativeItem(), "Vendidas",
                                resumen.PinturasVendidas.ToString(), "#C0392B");
                            TarjetaStat(row.RelativeItem(), "Reservadas",
                                resumen.PinturasReservadas.ToString(), "#2980B9");
                        });

                        // ── Bloque 3: Destacados ───────────────────────────
                        col.Item().PaddingTop(16)
                            .Text("Destacados").FontSize(12).Bold()
                            .FontColor(ColorPrimario);
                        col.Item().PaddingTop(6).Border(1).BorderColor(ColorBorde)
                            .Background(ColorFondoClaro).Padding(12).Column(inner =>
                        {
                            FilaDestacado(inner, "Artista más prolífico",
                                resumen.ArtistaConMasObras);
                            FilaDestacado(inner, "Técnica más utilizada",
                                resumen.TecnicaMasUsada);
                        });
                    });

                    pagina.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 6. PINTURAS POR EXHIBICIÓN
        // ─────────────────────────────────────────────────────────────────────

        public static void GenerarReportePorExhibicion(
            List<FilaExhibicion> datos,
            string               rutaDestino)
        {
            Document.Create(contenedor =>
            {
                contenedor.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4.Landscape());
                    pagina.Margin(1.5f, Unit.Centimetre);
                    pagina.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                    pagina.Header().Element(c => EncabezadoPagina(c,
                        "Pinturas por Exhibición",
                        $"{datos.Count} exhibiciones · " +
                        $"{datos.Sum(e => e.Pinturas.Count)} obras exhibidas"));

                    pagina.Content().PaddingTop(10).Column(col =>
                    {
                        foreach (var exhibicion in datos)
                        {
                            // Subtítulo por exhibición
                            col.Item().PaddingTop(6)
                                .Background(ColorSecundario).Padding(5)
                                .Text(exhibicion.NombreExhibicion)
                                .FontColor(ColorTextoBlanco).Bold().FontSize(10);

                            if (!exhibicion.Pinturas.Any())
                            {
                                col.Item().Padding(6)
                                    .Text("Sin pinturas registradas en esta exhibición.")
                                    .FontColor(ColorTextoGris).Italic();
                                continue;
                            }

                            col.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(cols =>
                                {
                                    cols.RelativeColumn(3f);
                                    cols.RelativeColumn(2.5f);
                                    cols.RelativeColumn(1.8f);
                                    cols.ConstantColumn(45);
                                    cols.RelativeColumn(1.5f);
                                    cols.ConstantColumn(90);
                                    cols.ConstantColumn(80);
                                });

                                tabla.Header(h =>
                                {
                                    CeldaEncabezadoSub(h.Cell(), "Título");
                                    CeldaEncabezadoSub(h.Cell(), "Artista");
                                    CeldaEncabezadoSub(h.Cell(), "Técnica");
                                    CeldaEncabezadoSub(h.Cell(), "Año");
                                    CeldaEncabezadoSub(h.Cell(), "Dimensiones");
                                    CeldaEncabezadoSub(h.Cell(), "Precio");
                                    CeldaEncabezadoSub(h.Cell(), "Estado");
                                });

                                bool par = false;
                                foreach (var p in exhibicion.Pinturas)
                                {
                                    string fondo = par ? ColorFondaTabla : ColorTextoBlanco;
                                    par = !par;
                                    CeldaDato(tabla.Cell(), p.Titulo,   fondo, bold: true);
                                    CeldaDato(tabla.Cell(), p.Artista,  fondo);
                                    CeldaDato(tabla.Cell(), p.Tecnica,  fondo);
                                    CeldaDato(tabla.Cell(), p.AnioCreacion?.ToString() ?? "—", fondo, centrado: true);
                                    CeldaDato(tabla.Cell(), p.Dimensiones, fondo);
                                    CeldaDato(tabla.Cell(), p.PrecioBase.ToString("C"), fondo, centrado: true);
                                    CeldaEstado(tabla.Cell(), p.Estado);
                                }
                            });
                        }
                    });

                    pagina.Footer().Element(PiePagina);
                });
            }).GeneratePdf(rutaDestino);
        }

        // ─────────────────────────────────────────────────────────────────────
        // COMPONENTES REUTILIZABLES
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Encabezado estándar de página con logo textual y título.</summary>
        private static void EncabezadoPagina(IContainer c, string titulo, string subtitulo)
        {
            c.Column(col =>
            {
                col.Item().Row(row =>
                {
                    // Franja izquierda con nombre de la galería
                    row.ConstantItem(140).Background(ColorPrimario).Padding(8).Column(inner =>
                    {
                        inner.Item().Text("GALERÍA DE ARTES")
                            .FontColor(ColorTextoBlanco).Bold().FontSize(10);
                        inner.Item().Text("Sistema de Gestión")
                            .FontColor(ColorAcento).FontSize(7);
                    });

                    // Título del reporte
                    row.RelativeItem().Background(ColorFondoClaro).Padding(8).Column(inner =>
                    {
                        inner.Item().Text(titulo)
                            .FontColor(ColorPrimario).Bold().FontSize(14);
                        inner.Item().Text(subtitulo)
                            .FontColor(ColorTextoGris).FontSize(8);
                    });

                    // Fecha en esquina derecha
                    row.ConstantItem(110).Background(ColorFondoClaro)
                        .AlignRight().Padding(8).Column(inner =>
                    {
                        inner.Item().Text(DateTime.Now.ToString("dd/MM/yyyy"))
                            .FontColor(ColorPrimario).Bold().FontSize(9);
                        inner.Item().Text(DateTime.Now.ToString("HH:mm"))
                            .FontColor(ColorTextoGris).FontSize(8);
                    });
                });

                // Línea decorativa bajo el encabezado
                col.Item().Height(3).Background(ColorAcento);
            });
        }

        /// <summary>Pie de página con numeración.</summary>
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

        /// <summary>Tarjeta estadística de resumen (título + valor en caja coloreada).</summary>
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

        /// <summary>Celda de encabezado principal (vinotinto oscuro).</summary>
        private static void CeldaEncabezado(IContainer c, string texto)
        {
            c.Background(ColorPrimario).Padding(5)
             .Text(texto).FontColor(ColorTextoBlanco).Bold().FontSize(8.5f);
        }

        /// <summary>Celda de encabezado secundario (vinotinto medio, para sub-secciones).</summary>
        private static void CeldaEncabezadoSub(IContainer c, string texto)
        {
            c.Background(ColorSecundario).Padding(4)
             .Text(texto).FontColor(ColorTextoBlanco).Bold().FontSize(8f);
        }

        /// <summary>Celda de dato genérico con fondo alternado.</summary>
        private static void CeldaDato(IContainer c, string texto, string fondo,
            bool bold = false, bool centrado = false, string? color = null)
        {
            var textStyle = c.Background(fondo).BorderBottom(1).BorderColor(ColorBorde)
                .Padding(4).Text(texto)
                .FontColor(color ?? "#1A1A1A")
                .FontSize(8.5f);

            if (bold)     textStyle.Bold();
            if (centrado) textStyle.AlignCenter();
        }

        /// <summary>Celda de estado con color semafórico (disponible/vendida/reservada).</summary>
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

        /// <summary>Fila de clave-valor para el bloque de destacados del ejecutivo.</summary>
        private static void FilaDestacado(ColumnDescriptor col, string clave, string valor)
        {
            col.Item().PaddingBottom(6).Row(row =>
            {
                row.ConstantItem(180)
                    .Text(clave + ":").FontColor(ColorTextoGris).FontSize(10);
                row.RelativeItem()
                    .Text(valor).FontColor(ColorPrimario).Bold().FontSize(10);
            });
        }
    }
}
