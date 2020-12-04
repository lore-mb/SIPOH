using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.WebApp.Models;
using PoderJudicial.SIPOH.Entidades;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using iText.Layout.Properties;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.IO.Image;
using iText.Layout;
using System.IO;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class ReportesController : BaseController
    {
        #region Inyección de despendecias y mapeado de clases.
        public readonly IReportesProcessor reportesProcessor;
        private readonly IMapper mapper;

        public ReportesController(IReportesProcessor reportesProcessor, IMapper mapper)
        {
            this.reportesProcessor = reportesProcessor;
            this.mapper = mapper;
        }
        #endregion

        #region Metodos publicos.
        public ActionResult Reportes()
        {
            List<Juzgado> ComboListaJuzgadosIniciales = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);
            List<Juzgado> ComboListaJuzgadosPromociones = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);
            ViewBag.ListaCircuitoJuzgadoIniciales = ComboListaJuzgadosIniciales != null ? ComboListaJuzgadosIniciales : new List<Juzgado>();
            ViewBag.ListaCircuitoJuzgadoPromociones = ComboListaJuzgadosPromociones != null ? ComboListaJuzgadosPromociones : new List<Juzgado>();

            ViewBag.Mensaje = TempData["Mensaje"];

            return View();
        }

        public ActionResult FormatoReportePorDia(string fechaHoy, int idJuzgado, int tipoBusqueda)
        {
            // Guardado Temporal en RAM
            MemoryStream _MemorySt = new MemoryStream();

            // Guardado en URL
            PdfWriter _Url = new PdfWriter(_MemorySt);
            PdfDocument _DocumentoReporte = new PdfDocument(_Url);

            // Tamaño y especificaciones del papel 
            Document _DocumentoConfig = new Document(_DocumentoReporte, PageSize.LETTER.Rotate());
            _DocumentoConfig.SetMargins(73, 35, 50, 35);

            Style EstiloEncabezadoRegistros = new Style()
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.DARK_GRAY)
                .SetBackgroundColor(new DeviceRgb(224, 224, 224));

            Style EstiloEncabezadoParametros = new Style()
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.DARK_GRAY);

            Style EstiloCuerpoRegistros = new Style()
                .SetFontSize(7)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.DARK_GRAY);

            Style EstiloPieDePagina = new Style()
                .SetFontSize(8)
                .SetFontColor(ColorConstants.DARK_GRAY);

            Style EstiloTotal = new Style()
                .SetFontSize(10)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.RIGHT);

            Style EstiloTotalParametro = new Style()
                .SetFontSize(10)
                .SetBackgroundColor(new DeviceRgb(224, 224, 244))
                .SetTextAlignment(TextAlignment.CENTER);

            Style EstilosFirma = new Style()
                .SetFontSize(10)
                .SetBorder(Border.NO_BORDER)
                .SetFontColor(ColorConstants.DARK_GRAY)
                .SetTextAlignment(TextAlignment.CENTER);

            Style EstiloTitulosFirma = new Style()
                .SetFontSize(10)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.LEFT);

            // Header del documento
            string LogoPoderJudicial = Server.MapPath("~/Content/Master/Img/PoderJudicialHgo.png");
            string LogoConsejoJudicatura = Server.MapPath("~/Content/Master/Img/LogoCJ1.JPG");

            Image LogoPJ = new Image(ImageDataFactory.Create(LogoPoderJudicial));
            Image LogoCJ = new Image(ImageDataFactory.Create(LogoConsejoJudicatura));

            if (tipoBusqueda == 0)
            {
                string MensajeTituloReporteIniciales = "REPORTE INICIALES DE EJECUCIÓN";
                _DocumentoReporte.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(LogoPJ, LogoCJ, MensajeTituloReporteIniciales));
            }
            else
            {
                string MensajeTituloReportePromociones = "REPORTE PROMOCIONES DE EJECUCIÓN";
                _DocumentoReporte.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(LogoPJ, LogoCJ, MensajeTituloReportePromociones));
            }

            Table TabSubEncabezado = new Table(2).UseAllAvailableWidth();

            Table TableVacia = new Table(1).UseAllAvailableWidth();
            Cell CellVacia = new Cell(1, 1).Add(new Paragraph(""));

            Table TabFechas = new Table(4).UseAllAvailableWidth();

            Table TabEncabezadoRegitros;

            if (tipoBusqueda == 0)
                TabEncabezadoRegitros = new Table(7).UseAllAvailableWidth();
            else
                TabEncabezadoRegitros = new Table(5).UseAllAvailableWidth();

            Table TabTotal = new Table(2).UseAllAvailableWidth();

            List<Juzgado> ListaJuzgadosIniciales = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);

            List<EjecucionCausa> ListaRegistros;

            if (tipoBusqueda == 0)
                ListaRegistros = reportesProcessor.ListaInicialesPromocionesPorDia(Instancia.INICIAL, fechaHoy, idJuzgado);
            else
                ListaRegistros = reportesProcessor.ListaInicialesPromocionesPorDia(Instancia.PROMOCION, fechaHoy, idJuzgado);

            List<ReporteDTO> ListaRegistrosReporteDTO = mapper.Map<List<EjecucionCausa>, List<ReporteDTO>>(ListaRegistros);

            if (ListaRegistros.Count == 0)
            {
                if (tipoBusqueda == 0)
                    TempData.Add("Mensaje", "No se han dado de alta registros de iniciales en el juzgado seleccionado.");
                else
                    TempData.Add("Mensaje", "No se han dado de alta registros de promociones en el juzgado seleccionado");

                return RedirectToAction("Reportes");
            }
            else
            {
                var NombreJuzgadoToCell = "";

                foreach (var ParametroNombreJuzgado in ListaJuzgadosIniciales)
                {
                    if (ParametroNombreJuzgado.IdJuzgado == idJuzgado)
                        NombreJuzgadoToCell = ParametroNombreJuzgado.Nombre.ToString();
                }

                Cell CellNombreJuzgado = new Cell(2, 0).Add(new Paragraph("Nombre de Juzgado")).AddStyle(EstiloEncabezadoRegistros);
                Cell CellNombreJuzgadoParametro = new Cell(2, 0).Add(new Paragraph(NombreJuzgadoToCell)).AddStyle(EstiloEncabezadoParametros);

                Cell CellFechaInicial = new Cell(2, 0).Add(new Paragraph("Fecha del día actual")).AddStyle(EstiloEncabezadoRegistros);
                Cell CellFechaInicialParametro = new Cell(2, 0).Add(new Paragraph(fechaHoy.ToString())).AddStyle(EstiloEncabezadoParametros);

                Cell CellTextoTotal = new Cell();

                if (ListaRegistrosReporteDTO.Count > 0)
                {
                    Cell CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No. Ejecución")).AddStyle(EstiloEncabezadoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    if (tipoBusqueda == 0)
                    {
                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Fecha Ingreso")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Detalle Solicitante")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Solicitud")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Sentenciado")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No. Causa")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("NUC")).AddStyle(EstiloEncabezadoRegistros);

                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        foreach (var Columna in ListaRegistrosReporteDTO)
                        {
                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NumeroEjecucion)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.FechaEjecucion)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.DetalleSolicitante.ToString())).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.DescripcionSolicitud)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NombreBeneficiario + " " + Columna.ApellidoMBeneficiario + " " + Columna.ApellidoMBeneficiario)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NumeroCausa)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            if (Columna.NUC == null)
                            {
                                CellContenidoEncabezado = new Cell().Add(new Paragraph("")).AddStyle(EstiloCuerpoRegistros);
                                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                            }
                            else
                            {
                                CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NUC)).AddStyle(EstiloCuerpoRegistros);
                                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                            }
                        }
                        CellTextoTotal.Add(new Paragraph("Total de Ejecuciones")).AddStyle(EstiloTotal);
                    }
                    else
                    {
                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Fecha Ingreso")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Nombre de Promovente")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Solicitud")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Cantidad Anexos")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        foreach (var Columna in ListaRegistrosReporteDTO)
                        {
                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NumeroEjecucion)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.FechaEjecucion)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.Promovente)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.DescripcionSolicitud)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.Total.ToString())).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                        }
                        CellTextoTotal.Add(new Paragraph("Total de Promociones")).AddStyle(EstiloTotal);
                    }

                    Cell CellTextoTotalParametro = new Cell().Add(new Paragraph(ListaRegistrosReporteDTO.Count.ToString())).AddStyle(EstiloTotalParametro);

                    TabTotal
                        .AddCell(CellTextoTotal)
                        .AddCell(CellTextoTotalParametro);
                }

                Table TabDatosFirma = new Table(4).UseAllAvailableWidth();

                Cell CellRecibi = new Cell(1, 1).Add(new Paragraph("Recibí: ")).AddStyle(EstiloTitulosFirma);
                Cell CellRecibiLinea = new Cell(1, 1).Add(new Paragraph("______________________________________________")).AddStyle(EstilosFirma);

                Cell CellRecibi2 = new Cell(1, 1).Add(new Paragraph("Recibí: ")).AddStyle(EstiloTitulosFirma);
                Cell CellRecibiLinea2 = new Cell(1, 1).Add(new Paragraph("______________________________________________")).AddStyle(EstilosFirma);

                Cell CellNombre = new Cell(1, 1).Add(new Paragraph("Nombre:")).AddStyle(EstiloTitulosFirma);
                Cell CellNombreLinea = new Cell(1, 1).Add(new Paragraph("______________________________________________")).AddStyle(EstilosFirma);

                Cell CellNombre2 = new Cell(1, 1).Add(new Paragraph("Nombre:")).AddStyle(EstiloTitulosFirma);
                Cell CellNombreLinea2 = new Cell(1, 1).Add(new Paragraph("______________________________________________")).AddStyle(EstilosFirma);

                Cell CellFechaFirma = new Cell(0, 1).Add(new Paragraph("Fecha:")).AddStyle(EstiloTitulosFirma);
                Cell CellFechaLineaFirma = new Cell(0, 1).Add(new Paragraph("_______________________________ Hora:  _________")).AddStyle(EstilosFirma);

                Cell CellFechaFirma2 = new Cell(0, 1).Add(new Paragraph("Fecha:")).AddStyle(EstiloTitulosFirma);
                Cell CellFechaLineaFirma2 = new Cell(0, 1).Add(new Paragraph("_______________________________ Hora:  _________")).AddStyle(EstilosFirma);

                Table TabFirma = new Table(2).UseAllAvailableWidth();

                Cell CellLineaFirma = new Cell(1, 1).Add(new Paragraph("__________________________________")).AddStyle(EstilosFirma);
                Cell CellLineaFirma2 = new Cell(1, 1).Add(new Paragraph("__________________________________")).AddStyle(EstilosFirma);

                Cell CellFirmaTexto = new Cell(1, 1).Add(new Paragraph("Firma")).AddStyle(EstilosFirma);
                Cell CellFirmaTexto2 = new Cell(1, 1).Add(new Paragraph("Firma")).AddStyle(EstilosFirma);

                TabSubEncabezado
                    .AddCell(CellNombreJuzgado)
                    .AddCell(CellNombreJuzgadoParametro);

                TableVacia
                    .AddCell(CellVacia);

                TabFechas
                    .AddCell(CellFechaInicial)
                    .AddCell(CellFechaInicialParametro);

                TabDatosFirma
                    .AddCell(CellRecibi)
                    .AddCell(CellRecibiLinea)
                    .AddCell(CellRecibi2)
                    .AddCell(CellRecibiLinea2)
                    .AddCell(CellNombre)
                    .AddCell(CellNombreLinea)
                    .AddCell(CellNombre2)
                    .AddCell(CellNombreLinea2)
                    .AddCell(CellFechaFirma)
                    .AddCell(CellFechaLineaFirma)
                    .AddCell(CellFechaFirma2)
                    .AddCell(CellFechaLineaFirma2);

                TabFirma
                    .AddCell(CellLineaFirma)
                    .AddCell(CellLineaFirma2)
                    .AddCell(CellFirmaTexto)
                    .AddCell(CellFirmaTexto2);

                _DocumentoConfig
                    .Add(new Paragraph("\n"))
                    .Add(TabSubEncabezado)
                    .Add(CellVacia)
                    .Add(TabFechas)
                    .Add(new Paragraph("\n"))
                    .Add(TabEncabezadoRegitros)
                    .Add(new Paragraph("\n"))
                    .Add(TabTotal)
                    .Add(new Paragraph("\n"))
                    .Add(TabDatosFirma)
                    .Add(new Paragraph("\n"))
                    .Add(TabFirma);

                _DocumentoConfig.Close();

                byte[] ByteStream = _MemorySt.ToArray();
                _MemorySt = new MemoryStream();
                _MemorySt.Write(ByteStream, 0, ByteStream.Length);
                _MemorySt.Position = 0;

                if (tipoBusqueda == 0)
                    return File(ByteStream, "application/pdf", "REP_INI_EJEC " + fechaHoy.ToString() + ".pdf");
                else
                    return File(ByteStream, "application/pdf", "REP_PROM_EJEC " + fechaHoy.ToString() + ".pdf");
            }
        }

        public ActionResult FormatoReportePorRangoFecha(string fechaInicial, string fechaFinal, int idJuzgado, int tipoBusqueda)
        {
            // Guardado Temporal en RAM
            MemoryStream _MemorySt = new MemoryStream();

            // Guardado en URL
            PdfWriter _Url = new PdfWriter(_MemorySt);
            PdfDocument _DocumentoReporte = new PdfDocument(_Url);

            // Tamaño y especificaciones del papel 
            Document _DocumentoConfig = new Document(_DocumentoReporte, PageSize.LETTER.Rotate());
            _DocumentoConfig.SetMargins(73, 35, 50, 35);

            Style EstiloEncabezadoRegistros = new Style()
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.DARK_GRAY)
                .SetBackgroundColor(new DeviceRgb(224, 224, 224));

            Style EstiloEncabezadoParametros = new Style()
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.DARK_GRAY);

            Style EstiloCuerpoRegistros = new Style()
                .SetFontSize(7)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.DARK_GRAY);

            Style EstiloPieDePagina = new Style()
                .SetFontSize(8)
                .SetFontColor(ColorConstants.DARK_GRAY);

            Style EstiloTotal = new Style()
                .SetFontSize(10)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.RIGHT);

            Style EstiloTotalParametro = new Style()
                .SetFontSize(10)
                .SetBackgroundColor(new DeviceRgb(224, 224, 244))
                .SetTextAlignment(TextAlignment.CENTER);

            Style EstilosFirma = new Style()
                .SetFontSize(10)
                .SetBorder(Border.NO_BORDER)
                .SetFontColor(ColorConstants.DARK_GRAY)
                .SetTextAlignment(TextAlignment.CENTER);

            Style EstiloTitulosFirma = new Style()
                .SetFontSize(10)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.LEFT);

            // Header del documento
            string LogoPoderJudicial = Server.MapPath("~/Content/Master/Img/PoderJudicialHgo.png");
            string LogoConsejoJudicatura = Server.MapPath("~/Content/Master/Img/LogoCJ1.JPG");

            Image LogoPJ = new Image(ImageDataFactory.Create(LogoPoderJudicial));
            Image LogoCJ = new Image(ImageDataFactory.Create(LogoConsejoJudicatura));

            if (tipoBusqueda == 0)
            {
                string MensajeTituloReporteIniciales = "REPORTE INICIALES DE EJECUCIÓN";
                _DocumentoReporte.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(LogoPJ, LogoCJ, MensajeTituloReporteIniciales));
            }
            else
            {
                string MensajeTituloReportePromociones = "REPORTE PROMOCIONES DE EJECUCIÓN";
                _DocumentoReporte.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(LogoPJ, LogoCJ, MensajeTituloReportePromociones));
            }

            Table TabSubEncabezado = new Table(2).UseAllAvailableWidth();

            Table TableVacia = new Table(1).UseAllAvailableWidth();
            Cell CellVacia = new Cell(1, 1).Add(new Paragraph(""));

            Table TabFechas = new Table(4).UseAllAvailableWidth();

            Table TabEncabezadoRegitros;

            if (tipoBusqueda == 0)
                TabEncabezadoRegitros = new Table(7).UseAllAvailableWidth();
            else
                TabEncabezadoRegitros = new Table(5).UseAllAvailableWidth();

            Table TabTotal = new Table(2).UseAllAvailableWidth();

            List<Juzgado> ListaJuzgadosIniciales = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);

            List<EjecucionCausa> ListaRegistros;

            if (tipoBusqueda == 0)
                ListaRegistros = reportesProcessor.ListaInicialesPromocionesPorRangoFecha(Instancia.INICIAL, fechaInicial, fechaFinal, idJuzgado);
            else
                ListaRegistros = reportesProcessor.ListaInicialesPromocionesPorRangoFecha(Instancia.PROMOCION, fechaInicial, fechaFinal, idJuzgado);

            List<ReporteDTO> ListaRegistrosReporteDTO = mapper.Map<List<EjecucionCausa>, List<ReporteDTO>>(ListaRegistros);


            if (ListaRegistros.Count == 0)
            {
                if (tipoBusqueda == 0)
                    TempData.Add("Mensaje", "No se han dado de alta registros de iniciales en el juzgado seleccionado entre la fecha " + fechaInicial.ToString() + " y " + fechaFinal.ToString() + ".");
                else
                    TempData.Add("Mensaje", "No se han dado de alta registros de promociones en el juzgado seleccionado entre la fecha " + fechaInicial.ToString() + " y " + fechaFinal.ToString() + ".");

                return RedirectToAction("Reportes");
            }
            else
            {
                var NombreJuzgadoToCell = "";

                foreach (var ParametroNombreJuzgado in ListaJuzgadosIniciales)
                {
                    if (ParametroNombreJuzgado.IdJuzgado == idJuzgado)
                    {
                        NombreJuzgadoToCell = ParametroNombreJuzgado.Nombre.ToString();
                    }
                }

                Cell CellNombreJuzgado = new Cell(2, 0).Add(new Paragraph("Nombre de Juzgado")).AddStyle(EstiloEncabezadoRegistros);
                Cell CellNombreJuzgadoParametro = new Cell(2, 0).Add(new Paragraph(NombreJuzgadoToCell)).AddStyle(EstiloEncabezadoParametros);

                Cell CellFechaInicial = new Cell(2, 0).Add(new Paragraph("Período Inicial")).AddStyle(EstiloEncabezadoRegistros);
                Cell CellFechaInicialParametro = new Cell(2, 0).Add(new Paragraph(fechaInicial.ToString())).AddStyle(EstiloEncabezadoParametros);

                Cell CellFechaFinal = new Cell(2, 0).Add(new Paragraph("Período Final ")).AddStyle(EstiloEncabezadoRegistros);
                Cell CellFechaFinalParametro = new Cell(2, 0).Add(new Paragraph(fechaFinal.ToString())).AddStyle(EstiloEncabezadoParametros);

                Cell CellTextoTotal = new Cell();

                if (ListaRegistrosReporteDTO.Count > 0)
                {
                    Cell CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No. Ejecución")).AddStyle(EstiloEncabezadoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    if (tipoBusqueda == 0)
                    {
                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Fecha Ingreso")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Detalle Solicitante")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Solicitud")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Sentenciado")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No. Causa")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("NUC")).AddStyle(EstiloEncabezadoRegistros);

                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        foreach (var Columna in ListaRegistrosReporteDTO)
                        {
                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NumeroEjecucion)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.FechaEjecucion)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.DetalleSolicitante.ToString())).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.DescripcionSolicitud)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NombreBeneficiario + " " + Columna.ApellidoMBeneficiario + " " + Columna.ApellidoMBeneficiario)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NumeroCausa)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            if (Columna.NUC == null)
                            {
                                CellContenidoEncabezado = new Cell().Add(new Paragraph("")).AddStyle(EstiloCuerpoRegistros);
                                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                            }
                            else
                            {
                                CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NUC)).AddStyle(EstiloCuerpoRegistros);
                                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                            }
                        }
                        CellTextoTotal.Add(new Paragraph("Total de Ejecuciones")).AddStyle(EstiloTotal);
                    }
                    else
                    {
                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Fecha Ingreso")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Nombre de Promovente")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Solicitud")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Cantidad Anexos")).AddStyle(EstiloEncabezadoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                        foreach (var Columna in ListaRegistrosReporteDTO)
                        {
                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.NumeroEjecucion)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.FechaEjecucion)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.Promovente)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.DescripcionSolicitud)).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                            CellContenidoEncabezado = new Cell().Add(new Paragraph(Columna.Total.ToString())).AddStyle(EstiloCuerpoRegistros);
                            TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                        }
                        CellTextoTotal.Add(new Paragraph("Total de Promociones")).AddStyle(EstiloTotal);
                    }

                    Cell CellTextoTotalParametro = new Cell().Add(new Paragraph(ListaRegistrosReporteDTO.Count.ToString())).AddStyle(EstiloTotalParametro);

                    TabTotal
                        .AddCell(CellTextoTotal)
                        .AddCell(CellTextoTotalParametro);
                }

                Table TabDatosFirma = new Table(4).UseAllAvailableWidth();

                Cell CellRecibi = new Cell(1, 1).Add(new Paragraph("Recibí: ")).AddStyle(EstiloTitulosFirma);
                Cell CellRecibiLinea = new Cell(1, 1).Add(new Paragraph("______________________________________________")).AddStyle(EstilosFirma);

                Cell CellRecibi2 = new Cell(1, 1).Add(new Paragraph("Recibí: ")).AddStyle(EstiloTitulosFirma);
                Cell CellRecibiLinea2 = new Cell(1, 1).Add(new Paragraph("______________________________________________")).AddStyle(EstilosFirma);

                Cell CellNombre = new Cell(1, 1).Add(new Paragraph("Nombre:")).AddStyle(EstiloTitulosFirma);
                Cell CellNombreLinea = new Cell(1, 1).Add(new Paragraph("______________________________________________")).AddStyle(EstilosFirma);

                Cell CellNombre2 = new Cell(1, 1).Add(new Paragraph("Nombre:")).AddStyle(EstiloTitulosFirma);
                Cell CellNombreLinea2 = new Cell(1, 1).Add(new Paragraph("______________________________________________")).AddStyle(EstilosFirma);

                Cell CellFechaFirma = new Cell(0, 1).Add(new Paragraph("Fecha:")).AddStyle(EstiloTitulosFirma);
                Cell CellFechaLineaFirma = new Cell(0, 1).Add(new Paragraph("_______________________________ Hora:  _________")).AddStyle(EstilosFirma);

                Cell CellFechaFirma2 = new Cell(0, 1).Add(new Paragraph("Fecha:")).AddStyle(EstiloTitulosFirma);
                Cell CellFechaLineaFirma2 = new Cell(0, 1).Add(new Paragraph("_______________________________ Hora:  _________")).AddStyle(EstilosFirma);

                Table TabFirma = new Table(2).UseAllAvailableWidth();

                Cell CellLineaFirma = new Cell(1, 1).Add(new Paragraph("__________________________________")).AddStyle(EstilosFirma);
                Cell CellLineaFirma2 = new Cell(1, 1).Add(new Paragraph("__________________________________")).AddStyle(EstilosFirma);

                Cell CellFirmaTexto = new Cell(1, 1).Add(new Paragraph("Firma")).AddStyle(EstilosFirma);
                Cell CellFirmaTexto2 = new Cell(1, 1).Add(new Paragraph("Firma")).AddStyle(EstilosFirma);

                TabSubEncabezado
                    .AddCell(CellNombreJuzgado)
                    .AddCell(CellNombreJuzgadoParametro);

                TableVacia
                    .AddCell(CellVacia);

                TabFechas
                    .AddCell(CellFechaInicial)
                    .AddCell(CellFechaInicialParametro)
                    .AddCell(CellFechaFinal)
                    .AddCell(CellFechaFinalParametro);

                TabDatosFirma
                    .AddCell(CellRecibi)
                    .AddCell(CellRecibiLinea)
                    .AddCell(CellRecibi2)
                    .AddCell(CellRecibiLinea2)
                    .AddCell(CellNombre)
                    .AddCell(CellNombreLinea)
                    .AddCell(CellNombre2)
                    .AddCell(CellNombreLinea2)
                    .AddCell(CellFechaFirma)
                    .AddCell(CellFechaLineaFirma)
                    .AddCell(CellFechaFirma2)
                    .AddCell(CellFechaLineaFirma2);

                TabFirma
                    .AddCell(CellLineaFirma)
                    .AddCell(CellLineaFirma2)
                    .AddCell(CellFirmaTexto)
                    .AddCell(CellFirmaTexto2);

                _DocumentoConfig
                    .Add(new Paragraph("\n"))
                    .Add(TabSubEncabezado)
                    .Add(CellVacia)
                    .Add(TabFechas)
                    .Add(new Paragraph("\n"))
                    .Add(TabEncabezadoRegitros)
                    .Add(new Paragraph("\n"))
                    .Add(TabTotal)
                    .Add(new Paragraph("\n"))
                    .Add(TabDatosFirma)
                    .Add(new Paragraph("\n"))
                    .Add(TabFirma);

                _DocumentoConfig.Close();

                byte[] ByteStream = _MemorySt.ToArray();
                _MemorySt = new MemoryStream();
                _MemorySt.Write(ByteStream, 0, ByteStream.Length);
                _MemorySt.Position = 0;

                if (tipoBusqueda == 0)
                    return File(ByteStream, "application/pdf", "REP_INI_EJEC " + fechaInicial.ToString() + " a " + fechaFinal.ToString() + ".pdf");
                else
                    return File(ByteStream, "application/pdf", "REP_PROM_EJEC " + fechaInicial.ToString() + " a " + fechaFinal.ToString() + ".pdf");
            }
        }
        public class HeaderEventHandler1 : IEventHandler
        {
            Image LogoPoderJudicial;
            Image LogoConsejoJudicatura;
            string MensajeTituloReporte;

            public HeaderEventHandler1(Image LogoPJ, Image LogoCJ, string MensajeTitulo)
            {
                LogoPoderJudicial = LogoPJ;
                LogoConsejoJudicatura = LogoCJ;
                MensajeTituloReporte = MensajeTitulo;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent DocumentoEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = DocumentoEvent.GetDocument();
                PdfPage Pagina = DocumentoEvent.GetPage();

                Rectangle AreaDibujo = new Rectangle(35, Pagina.GetPageSize().GetTop() - 70, Pagina.GetPageSize().GetRight() - 70, 50);

                Canvas Canvas = new Canvas(Pagina, AreaDibujo);

                Canvas
                    .Add(ObtenerTabla(DocumentoEvent))
                    .ShowTextAligned("Pagina 1", 725, 30, TextAlignment.CENTER)
                    //.ShowTextAligned(DateTime.Today.ToLongDateString(), 211, 30, TextAlignment.RIGHT)
                    .Close();
            }

            public Table ObtenerTabla(PdfDocumentEvent DocumentoEvent)
            {
                float[] AnchoCelda = { 20f, 80f, 20f };

                Table TablaEvent = new Table(UnitValue.CreatePercentArray(AnchoCelda)).UseAllAvailableWidth();

                Style EstiloCelda = new Style()
                    .SetBorder(Border.NO_BORDER)
                    .SetFontColor(ColorConstants.DARK_GRAY);

                Style styleText = new Style()
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10f);

                Cell CellEncabezadoTexto = new Cell().Add(LogoPoderJudicial.SetHeight(52).SetHeight(52));

                TablaEvent.AddCell(CellEncabezadoTexto
                    .AddStyle(EstiloCelda)
                    .SetTextAlignment(TextAlignment.CENTER));

                CellEncabezadoTexto = new Cell().AddStyle(EstiloCelda)
                    .Add(new Paragraph("TRIBUNAL SUPERIOR DE JUSTICIA DEL ESTADO DE HIDALGO").SetFontSize(15).SetTextAlignment(TextAlignment.CENTER))
                    .Add(new Paragraph(MensajeTituloReporte).SetTextAlignment(TextAlignment.CENTER));

                TablaEvent.AddCell(CellEncabezadoTexto);

                Cell ImagenDerecha = new Cell().Add(LogoConsejoJudicatura.SetHeight(45).SetWidth(115)).AddStyle(EstiloCelda);

                TablaEvent.AddCell(ImagenDerecha);

                return TablaEvent;
            }
        }
        #endregion

        #region Metodos Privados.
        private void ValidarJuzgado(List<Juzgado> ListaJuzgados)
        {
            if (ListaJuzgados == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                if (ListaJuzgados.Count > 0)
                {
                    var ListadoJuzgados = ViewHelper.Options(ListaJuzgados, "IdJuzgado", "Nombre");
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = ListadoJuzgados;
                }
                else
                {
                    Respuesta.Data = new object();
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                }
            }
        }
        #endregion
    }
}