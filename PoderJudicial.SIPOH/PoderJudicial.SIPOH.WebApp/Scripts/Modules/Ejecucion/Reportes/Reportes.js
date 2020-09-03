function ElementosCargado() {

    $("#reporFecha").hide();
    $("#calenFecha").hide();
    $("#btnFecha").hide();
    $("#reporDia").hide();

}


/*Mostrar, Ocultar elementos respecto a seleccion de reporte*/
$(document).ready(function () {

    ElementosCargado();

    $("#inlineFormCustomSelect").change(function () {
        if ($(this).val() == "1") {
            $('#divPartesreporFecha').show();
            $('#calenFecha').show();
            $("#btnFecha").show();
            $("#reporDia").hide();


        } else if ($("#inlineFormCustomSelect").val() == "0") {
            $("#divPartesreporFecha").hide();
            $("#calenFecha").hide();
            $("#btnFecha").hide();
            $("#reporDia").hide();


        } else if ($("#inlineFormCustomSelect").val() == "2") {
            $("#divPartesreporFecha").hide();
            $("#calenFecha").hide();
            $("#btnFecha").hide();
            $("#reporDia").show();
        }
    });
});
