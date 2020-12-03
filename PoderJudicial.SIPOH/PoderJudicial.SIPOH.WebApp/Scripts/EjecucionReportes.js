$(document).ready(function () {

    CargarDatepickers();
    CargarHoraLocal();
    ValidarFormularios();

    var ViewBagText = $("#valorViewBag").data("value");

    if (ViewBagText != null) {
        MostrarAlertaError("<b>" + "SISTEMA: " + "</b>" + ViewBagText + "</br>"
                           + "<b>" + "SUGERENCIA:" + "</b>" + " De clic en " + "<b class='text-danger'>" + "Aceptar " + "</b>" + " e intente consultar mas tarde.", null, "Atención");
    }

});

function GenerarReporteInicialPorDia() {

    var fechaInput = $("#inpFechaHoy").val();
    var idJuzgadoPickList = $("#slctJuzgadoPorCircuitoDia").val();
    var tipoBusqueda = $('#slcFormatoReporte').val();

    window.location.href = urlBase + 'Reportes/FormatoReportePorDia/?FechaHoy=' + fechaInput +
        '&IdJuzgado=' + idJuzgadoPickList +
        '&TipoBusqueda=' + tipoBusqueda;

}

function GenerarReporteInicialPorRangoFecha() {

    var IdJuzgadoPickList = $("#slctJuzgadoPorCircuito").val();
    var FechaInicialDatePicker = $("#datetimepickerFechaInicial").val();
    var FechaFinalDatePicker = $("#datetimepickerFechaFinal").val();
    var TipoBusqueda = $('#slcFormatoReporteFecha').val();

    window.location.href = urlBase + 'Reportes/FormatoReportePorRangoFecha/?FechaInicial=' + FechaInicialDatePicker +
        '&FechaFinal=' + FechaFinalDatePicker +
        '&IdJuzgado=' + IdJuzgadoPickList +
        '&TipoBusqueda=' + TipoBusqueda;

}

function CargarDatepickers() {

    $('#datetimepickerFechaInicial').datetimepicker({
        format: 'YYYY-MM-DD'
    });
    $('#datetimepickerFechaFinal').datetimepicker({
        format: 'YYYY-MM-DD'
    });

    $("#datetimepickerFechaInicial").on("dp.change", function (e) {
        $('#datetimepickerFechaFinal').data("DateTimePicker").minDate(e.date);
    });
    $("#datetimepickerFechaFinal").on("dp.change", function (e) {
        $('#datetimepickerFechaInicial').data("DateTimePicker").maxDate(e.date);
    });

}

function CargarHoraLocal() {
    var Fecha = new Date();
    var Mes = Fecha.getMonth() + 1;
    var Dia = Fecha.getDate();
    var SalidaFormatoFecha = Fecha.getFullYear() + '-' +
        (Mes < 10 ? '0' : '') + Mes + '-' +
        (Dia < 10 ? '0' : '') + Dia;

    $('#inpFechaHoy').val(SalidaFormatoFecha);
}

function ValidarFormularios() {
    var forms = document.getElementsByClassName('needs-validation');
    Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
            var id = form.id;
            event.preventDefault();
            event.stopPropagation();
            form.classList.add('was-validated');
            if (form.checkValidity() === true && id == "FrmReporteInicialRangoFechas") {
                GenerarReporteInicialPorRangoFecha();
            }
            if (form.checkValidity() === true && id == "FrmReporteInicialDia") {
                GenerarReporteInicialPorDia();
            }
        }, false);
    });
}

function MostrarAlertaError(mensaje, tamanio, titulo) {
    bootbox.alert({
        title: "<h2>" + "<i class='icon-info text-danger'></i> " + titulo + "</h2>",
        message: mensaje,
        buttons:
        {
            ok: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-danger'
            }
        },
        size: tamanio
    });
}
