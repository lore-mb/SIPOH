$(function () {

    CargarDatepickers();

});


function GenerarReporteInicialPorRangoFecha() {

    $("#loading").fadeIn();

    var IdJuzgadoPickList = $("#slctJuzgadoPorCircuito").val();
    var FechaInicialDatePicker = $("#datetimepickerFechaInicial").val();
    var FechaFinalDatePicker = $("#datetimepickerFechaFinal").val();

    // Direcciona al metodo del controlador
    window.location.href = '/Reportes/ReporteInicialesPorFecha/?FechaInicial=' + FechaInicialDatePicker + '&FechaFinal=' + FechaFinalDatePicker + '&IdJuzgado=' + IdJuzgadoPickList;

    $("#loading").fadeOut();
}


function GenerarReporteInicialPorDia() {

    $("#loading").fadeIn();

    // Obtiene Fecha Local
    var Fecha = new Date();
    var Mes = Fecha.getMonth() + 1;
    var Dia = Fecha.getDate();
    var output = Fecha.getFullYear() + '-' +
        (Mes < 10 ? '0' : '') + Mes + '-' +
        (Dia < 10 ? '0' : '') + Dia;

    var IdJuzgadoPickList = $("#slctJuzgadoPorCircuitoDia").val();

    // Direcciona al metodo del controlador
    window.location.href = '/Reportes/ReporteInicialesDeHoy/?FechaHoy=' + output + '&IdJuzgado=' + IdJuzgadoPickList;

    $("#loading").fadeOut();
}

function CargarDatepickers()
{
    //  Iniciales - Rango Fechas 
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

    // Promociones - Rango Fecha

}

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
        if (form.checkValidity() === true && id == "FrmReporteInicialDia")
        {
            GenerarReporteInicialPorDia();
        }
    }, false);
});

