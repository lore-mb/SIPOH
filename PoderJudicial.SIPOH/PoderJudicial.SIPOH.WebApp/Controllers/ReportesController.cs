using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.Entidades.Enum;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
/* Libs Itext7*/
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Events;
using iText.IO.Image;


namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class ReportesController : BaseController
    {

        #region Inyección Dependencias 
        public readonly IReportesProcessor reportesProcessor;

        public ReportesController(IReportesProcessor reportesProcessor)
        {
            this.reportesProcessor = reportesProcessor;
        }
        #endregion

        #region Metodos publicos
        public ActionResult Reportes()
        {
            List<Juzgado> ComboListaJuzgadosIniciales = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);
            List<Juzgado> ComboListaJuzgadosPromociones = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);
            ViewBag.ListaCircuitoJuzgadoIniciales = ComboListaJuzgadosIniciales != null ? ComboListaJuzgadosIniciales : new List<Juzgado>();
            ViewBag.ListaCircuitoJuzgadoPromociones = ComboListaJuzgadosPromociones != null ? ComboListaJuzgadosPromociones : new List<Juzgado>();
            return View();
        }

        [HttpGet]
        public ActionResult ListaJuzgadoPorCircuito(int IdCircuito)
        {
            try
            {
                List<Juzgado> ListaJuzgados = reportesProcessor.ObtenerJuzgadoPorCircuito(IdCircuito);
                ValidarJuzgado(ListaJuzgados);
                Respuesta.Mensaje = reportesProcessor.Mensaje;
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = ex.Message;
                Respuesta.Data = null;
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ReporteInicialesPorFecha(string FechaInicial, string FechaFinal, int IdJuzgado) 
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
                .SetBackgroundColor(new DeviceRgb(224,224,244))
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

            _DocumentoReporte.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(LogoPJ, LogoCJ));

            // T. SubEncabezado
            Table TabSubEncabezado = new Table(2).UseAllAvailableWidth();

            // T. Vacia
            Table TableVacia = new Table(1).UseAllAvailableWidth();
            Cell CellVacia = new Cell(1, 1).Add(new Paragraph(""));

            // T. Fechas
            Table TabFechas = new Table(4).UseAllAvailableWidth();

            // T. Registros
            Table TabEncabezadoRegitros = new Table(7).UseAllAvailableWidth();

            // T. Total
            Table TabTotal = new Table(2).UseAllAvailableWidth();

            // Pintado dinamico de registros 
            List<Juzgado> ListaJuzgadosIniciales = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);
            List<Reporte> ListaRegitros = reportesProcessor.RegistrosReportePorRangoFecha(TipoReporteRangoFecha.INICIAL, FechaInicial, FechaFinal, IdJuzgado);

            var NombreJuzgadoToCell = "";

            foreach (var ParametroNombreJuzgado in ListaJuzgadosIniciales)
            {
                if (ParametroNombreJuzgado.IdJuzgado == IdJuzgado) 
                {
                    NombreJuzgadoToCell = ParametroNombreJuzgado.Nombre.ToString();
                } 
            }

            Cell CellNombreJuzgado = new Cell(2, 0).Add(new Paragraph("Nombre de Juzgado")).AddStyle(EstiloEncabezadoRegistros);
            Cell CellNombreJuzgadoParametro = new Cell(2, 0).Add(new Paragraph(NombreJuzgadoToCell)).AddStyle(EstiloEncabezadoParametros);

            Cell CellFechaInicial = new Cell(2, 0).Add(new Paragraph("Desde")).AddStyle(EstiloEncabezadoRegistros);
            Cell CellFechaInicialParametro = new Cell(2, 0).Add(new Paragraph(FechaInicial.ToString())).AddStyle(EstiloEncabezadoParametros);
            Cell CellFechaFinal = new Cell(2, 0).Add(new Paragraph("Hasta")).AddStyle(EstiloEncabezadoRegistros);
            Cell CellFechaFinalParametro = new Cell(2, 0).Add(new Paragraph(FechaFinal.ToString())).AddStyle(EstiloEncabezadoParametros);

            if (ListaRegitros.Count == 0)
            {

                Cell CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No hay registros de ejecución para el periodo seleccionado.")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

            }
            else if (ListaRegitros.Count > 0) 
            {
                Cell CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No. Ejecución")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Detalle Solicitante")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Fecha Ingreso")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Solicitud")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Sentenciado")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No. Causa")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("NUC")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                foreach (var Item in ListaRegitros)
                {
                    //Revisar Validaciones 

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.NumeroEjecucion)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.DetalleSolicitante.ToString())).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.FechaEjecucion)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.DescripcionSolicitud)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.NombreBeneficiario + " " + Item.ApellidoMBeneficiario + " " + Item.ApellidoMBeneficiario)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.NumeroCausa)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    if (Item.NUC == null)
                    {
                        CellContenidoEncabezado = new Cell().Add(new Paragraph("")).AddStyle(EstiloCuerpoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                    }
                    else
                    {
                        CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.NUC)).AddStyle(EstiloCuerpoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                    }
                }

                Cell CellTextoTotal = new Cell().Add(new Paragraph("Total de Ejecuciones")).AddStyle(EstiloTotal);
                Cell CellTextoTotalParametro = new Cell().Add(new Paragraph(ListaRegitros.Count.ToString())).AddStyle(EstiloTotalParametro);

                // T. Total
                TabTotal
                    .AddCell(CellTextoTotal)
                    .AddCell(CellTextoTotalParametro);

            }

            // T. Datos Firma
            Table TabDatosFirma = new Table(4).UseAllAvailableWidth();

            Cell CellRecibi = new Cell(1, 1).Add(new Paragraph("Recibí: ")).AddStyle(EstiloTitulosFirma);
            Cell CellRecibiLinea = new Cell(1, 1).Add(new Paragraph("_________________________________________________")).AddStyle(EstilosFirma);

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

            // T. Firma
            Table TabFirma = new Table(2).UseAllAvailableWidth();

            Cell CellLineaFirma = new Cell(1, 1).Add(new Paragraph("__________________________________")).AddStyle(EstilosFirma);
            Cell CellLineaFirma2 = new Cell(1, 1).Add(new Paragraph("__________________________________")).AddStyle(EstilosFirma);

            Cell CellFirmaTexto = new Cell(1, 1).Add(new Paragraph("Firma")).AddStyle(EstilosFirma);
            Cell CellFirmaTexto2 = new Cell(1, 1).Add(new Paragraph("Firma")).AddStyle(EstilosFirma);


            // ARMADO DEL DOCUMENTO

            // T. Encabezado
            TabSubEncabezado
                .AddCell(CellNombreJuzgado)
                .AddCell(CellNombreJuzgadoParametro);

            // T. Vacia
            TableVacia
                .AddCell(CellVacia);

            // T. Vacia
            TabFechas
                .AddCell(CellFechaInicial)
                .AddCell(CellFechaInicialParametro)
                .AddCell(CellFechaFinal)
                .AddCell(CellFechaFinalParametro);

            // T. Datos Firma
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

            // T. Firma
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

            // Descarga directamente el PDF despues del render
            return File(ByteStream, "application/pdf", "Reporte Iniciales de Ejecucion " + FechaInicial.ToString() + " a " + FechaFinal.ToString() + ".pdf");

        }

        public ActionResult ReporteInicialesDeHoy(string FechaHoy, int IdJuzgado) 
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

            _DocumentoReporte.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(LogoPJ, LogoCJ));

            // T. SubEncabezado
            Table TabSubEncabezado = new Table(2).UseAllAvailableWidth();

            // T. Vacia
            Table TableVacia = new Table(1).UseAllAvailableWidth();
            Cell CellVacia = new Cell(1, 1).Add(new Paragraph(""));

            // T. Fechas
            Table TabFechas = new Table(4).UseAllAvailableWidth();

            // T. Registros
            Table TabEncabezadoRegitros = new Table(8).UseAllAvailableWidth();

            // T. Total
            Table TabTotal = new Table(2).UseAllAvailableWidth();

            // Pintado dinamico de registros 
            List<Juzgado> ListaJuzgadosIniciales = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);
            List<Reporte> ListaRegitros = reportesProcessor.RegistrosReportePorDia(TipoReporteDia.INICIAL, FechaHoy, IdJuzgado);

            var NombreJuzgadoToCell = "";

            foreach (var ParametroNombreJuzgado in ListaJuzgadosIniciales)
            {
                if (ParametroNombreJuzgado.IdJuzgado == IdJuzgado)
                {
                    NombreJuzgadoToCell = ParametroNombreJuzgado.Nombre.ToString();
                }
            }

            Cell CellNombreJuzgado = new Cell(2, 0).Add(new Paragraph("Nombre de Juzgado")).AddStyle(EstiloEncabezadoRegistros);
            Cell CellNombreJuzgadoParametro = new Cell(2, 0).Add(new Paragraph(NombreJuzgadoToCell)).AddStyle(EstiloEncabezadoParametros);

            Cell CellFechaInicial = new Cell(2, 0).Add(new Paragraph("Fecha de Hoy")).AddStyle(EstiloEncabezadoRegistros);
            Cell CellFechaInicialParametro = new Cell(2, 0).Add(new Paragraph(FechaHoy.ToString())).AddStyle(EstiloEncabezadoParametros);


            if (ListaRegitros.Count == 0)
            {

                Cell CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No hay registros de ejecución para el periodo seleccionado.")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

            }
            else if (ListaRegitros.Count > 0)
            {
                Cell CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No. Ejecución")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Detalle Solicitante")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Fecha Ingreso")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Solicitud")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("Sentenciado")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("No. Causa")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                CellContenidoEncabezado = new Cell(1, 1).Add(new Paragraph("NUC")).AddStyle(EstiloEncabezadoRegistros);
                TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                foreach (var Item in ListaRegitros)
                {
                    //Revisar Validaciones 

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.NumeroEjecucion)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.DetalleSolicitante.ToString())).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.FechaEjecucion)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.DescripcionSolicitud)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.NombreBeneficiario + " " + Item.ApellidoMBeneficiario + " " + Item.ApellidoMBeneficiario)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.NumeroCausa)).AddStyle(EstiloCuerpoRegistros);
                    TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);

                    if (Item.NUC == null)
                    {
                        CellContenidoEncabezado = new Cell().Add(new Paragraph("")).AddStyle(EstiloCuerpoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                    }
                    else
                    {
                        CellContenidoEncabezado = new Cell().Add(new Paragraph(Item.NUC)).AddStyle(EstiloCuerpoRegistros);
                        TabEncabezadoRegitros.AddCell(CellContenidoEncabezado);
                    }
                }


                Cell CellTextoTotal = new Cell().Add(new Paragraph("Total de Ejecuciones")).AddStyle(EstiloTotal);
                Cell CellTextoTotalParametro = new Cell().Add(new Paragraph(ListaRegitros.Count.ToString())).AddStyle(EstiloTotalParametro);

                // T. Total
                TabTotal
                    .AddCell(CellTextoTotal)
                    .AddCell(CellTextoTotalParametro);

            }

            // T. Datos Firma
            Table TabDatosFirma = new Table(4).UseAllAvailableWidth();

            Cell CellRecibi = new Cell(1, 1).Add(new Paragraph("Recibí: ")).AddStyle(EstiloTitulosFirma);
            Cell CellRecibiLinea = new Cell(1, 1).Add(new Paragraph("_________________________________________________")).AddStyle(EstilosFirma);

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

            // T. Firma
            Table TabFirma = new Table(2).UseAllAvailableWidth();

            Cell CellLineaFirma = new Cell(1, 1).Add(new Paragraph("__________________________________")).AddStyle(EstilosFirma);
            Cell CellLineaFirma2 = new Cell(1, 1).Add(new Paragraph("__________________________________")).AddStyle(EstilosFirma);

            Cell CellFirmaTexto = new Cell(1, 1).Add(new Paragraph("Firma")).AddStyle(EstilosFirma);
            Cell CellFirmaTexto2 = new Cell(1, 1).Add(new Paragraph("Firma")).AddStyle(EstilosFirma);


            // ARMADO DEL DOCUMENTO

            // T. Encabezado
            TabSubEncabezado
                .AddCell(CellNombreJuzgado)
                .AddCell(CellNombreJuzgadoParametro);

            // T. Vacia
            TableVacia
                .AddCell(CellVacia);

            // T. Vacia
            TabFechas
                .AddCell(CellFechaInicial)
                .AddCell(CellFechaInicialParametro);

            // T. Datos Firma
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

            // T. Firma
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

            // Descarga directamente el PDF despues del render
            return File(ByteStream, "application/pdf", "Reporte Iniciales de Ejecucion " + FechaHoy.ToString() +".pdf");
        }

        public ActionResult ReportePromocionesPorFecha() 
        {
            return null;
        }

        public ActionResult ReportePromocionesDeHoy() 
        {
            return null;
        }

        public class HeaderEventHandler1 : IEventHandler
        {

            Image LogoPoderJudicial;
            Image LogoConsejoJudicatura;

            public HeaderEventHandler1(Image LogoPJ, Image LogoCJ)
            {
                LogoPoderJudicial = LogoPJ;
                LogoConsejoJudicatura = LogoCJ;

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
                    .Add(new Paragraph("REPORTE DE INICIALES DE EJECUCIÓN").SetTextAlignment(TextAlignment.CENTER));

                TablaEvent.AddCell(CellEncabezadoTexto);

                Cell ImagenDerecha = new Cell().Add(LogoConsejoJudicatura.SetHeight(45).SetWidth(115)).AddStyle(EstiloCelda);

                TablaEvent.AddCell(ImagenDerecha);

                return TablaEvent;
            }
        }

        #endregion

        #region Metodos Privados

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