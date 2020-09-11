//Region Variables Golbales
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var idJuzgado = null;
var idCircuito = null;
var idDistrito = null;

var dataTable = null;
var dataTableCausa = null;
var dataTableAnex = null;
var dataTableDetallEjecucion = null;

//Estructura para Data Tables
var estructuraTablaDetallEjecucion = [{ data: 'nEjecucion', title: 'N° Ejecucion', className: "text-center" }, { data: 'nJuzgado', title: 'Juzgado', className: "text-center" }, { data: 'solicitante', title: 'Solicitante', className: "text-center" }, { data: 'sentebene', title: 'Sentenciado/Beneficiario', className: "text-center" }, { data: 'solicitud', title: 'Solicitud', className: "text-center" }];
var DetallEjec = [];
var estructuraTablaAnexos = [{ data: 'descripcion', title: 'Descripción' }, { data: 'cantidad', title: 'Cantidad' }, { data: 'eliminar', title: 'Quitar' }];
var anexos = [];
var estructuraTablaCausa = [{ data: 'nJuzgado', title: 'N° Juzgado', className: "text-center" }, { data: 'causaNuc', title: 'Causa|Nuc', className: "text-center" }, { data: 'ofendido', title: 'Ofendido (s)', className: "text-center" }, { data: 'inculpado', title: 'Inculpado (s)', className: "text-center" }, { data: 'delito', title: 'Delitos (s)', className: "text-center" }, { data: 'eliminar', title: 'Quitar', className: "text-center" }];
var causas = [];


//Funciones que se detonan al terminado del renderizado 
$(document).ready(function () {
    //Pintar Tablas
    dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
    dataTableAnex = GeneraTablaDatos(dataTableAnex, "dataTableAnexos", anexos, estructuraTablaAnexos, false, false, false);
    dataTableAnex = GeneraTablaDatos(dataTableCausa, "dataTableCausa", causas, estructuraTablaCausa, false, false, false);
    
    //Obtener Circuito
    idCircuito = $("#IdCircuitoHDN").val();

    //Elemntos al Cargado
    ElementosAlCargado();
});


// #region Variables SELECT Juzgados y N°Ejecucion
var InputNumero = $("#Numero");



// #region FORMAT LOAD Juzgado Acusatorio
slctNumero.prop('selectedIndex', 0);
InputNumero.val('');
$('#Numero').attr('placeholder', '0000/0000');
$("#spnNumero").text('N°Ejecucion');
InputNumero.inputmask("9999/9999");
//var ValorSeleccionado = $(this).children("option:selected").val();



function ElementosAlCargado() {
    FormatearElementos();


    $("#seccionTablaAnexos").hide();
    $("#seccionBotonGuardar").hide();

    $(document).on("keydown", ":input:not(textarea)", function (event) {
        if (event.key == "Enter") {
            event.preventDefault();
        }
    });

    //Funcionalidad para validar formularios
    var forms = document.getElementsByClassName('needs-validation');

    Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
            var id = form.id;

            event.preventDefault();
            event.stopPropagation();

            if (form.checkValidity() === true && id == "formselectJuzgado") {
                ConsultarCausas(true);
            }

            if (form.checkValidity() === true && id == "formJAcusatorio") {
                ConsultarCausas(false);
            }

            if (form.checkValidity() === true && id == "formCausasTradicional") {
               
            }

            if (form.checkValidity() === true && id == "Promovente") {
                AgregaPromoventes();
            }

            if (form.checkValidity() === true && id == "formAnexos") {
                AgregarDocumentos();
            }

        }, false);
    });

    function DistritoJuzgadoTradicional() {
        if (idDistrito != null) {
            var parametros = { idDistrito: idDistrito }
            SolicitudEstandarAjax("/Promociones/ObtenerJuzgadoTradicional", parametros, ListarJuzgadoTradicional);
        }
        else {
            alert("Error al obtener los datos");
        }
    }

    function ListarJuzgadoTradicional(data) {
        if (data.Estatus == EstatusRespuesta.OK) {
            var numero = data.Data.length;

            $("#slctJuzgadoTradi").html("");

            if (numero > 1) {
                $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));
            }

            const ObjJuzgadoTra = [data.Data];
            var $slcTradi = $('#slctJuzgadoTradi');

            $.each(ObjJuzgadoTra, function (id, juzgado) {
                for (var i = 0; i < juzgado.length; i++) {
                    $slcTradi.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
                }
            });
        }
        else if (data.Estatus == EstatusRespuesta.ERROR) {
            customNotice(data.Mensaje, "Error:", "error", 3350);
        }
    }
    function ConsultarCausas(esAcusatorio) {
        if (esAcusatorio) {
            var causaNucSelect = $("#slctNumero").find('option:selected').val();
            var juzgadoId = $("#slctJuzgado").find('option:selected').val();

            var causaNucText = $('#Numero').val();

            if (causaNucSelect == 2) {
                var parametros = { idJuzgado: juzgadoId, nuc: causaNucText };
                SolicitudEstandarAjax("/Promociones/ObtenerExpedientePorNUC", parametros, ListarCausas);
            }
            else {
                var parametros = { idJuzgado: juzgadoId, numeroCausa: causaNucText };
                SolicitudEstandarAjax("/Promociones/ObtenerExpedientePorCausa", parametros, ListarCausas);
            }
        }
        else {
            var causaNucText = $("#inpCAUT").val();
            var juzgadoId = $("#slctJuzgadoTradi").find('option:selected').val();

            var parametros = { idJuzgado: juzgadoId, numeroCausa: causaNucText };
            SolicitudEstandarAjax("/Promociones/ObtenerExpedientePorCausa", parametros, ListarCausas);
        }
    }
    function ListarCausas(data) {
        if (data.Estatus == EstatusRespuesta.OK) {
            var expediente = data.Data;
            var causaNuc = expediente.NumeroCausa == null ? expediente.NUC : expediente.NumeroCausa;
            var causaNucLabel = expediente.NumeroCausa == null ? "NUC " : "Numero de causa ";

            if (ValidarCuasaEnTabla(expediente.IdExpediente)) {
                var mensaje = causaNucLabel + "<b>" + causaNuc + "</b> que intenta agregar, ya se encuentra en la tabla.";
                Alerta(mensaje);
            }
            else {
                var mensaje = "La consulta generó un resultado exitoso para el " + causaNucLabel + " <b>" + causaNuc + "</b> asignado al juzgado de procedencia <b>" + expediente.NombreJuzgado + "</b> seleccionado.<br><br> ¿Desea Continuar? <br>";

                var funcion = function () {
                    if (causas.length == 0) {
                        $('#contenedorBeneficiario').removeAttr('hidden');
                        $("#contenedorBeneficiario").show();
                    }
                    var causa = new Object();
                    causa.id = expediente.IdExpediente;
                    causa.nJuzgado = expediente.NombreJuzgado;
                    causa.causaNuc = causaNuc;
                    causa.ofendido = expediente.Ofendidos;
                    causa.inculpado = expediente.Inculpados;
                    causa.delito = expediente.Delitos;
                    causa.eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarCausa(" + causa.id + ")' data-toggle='tooltip' data-placement='top' title='Eliminar'><i class='icon-bin2'></i></button>";
                    //Agrega Causa al Arreglo de Cuasas
                    causas.push(causa);
                    //Generar Tabla
                    dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
                }

                //Imprime Mensaje de Confirmacion
                MensajeDeConfirmacion(mensaje, "large", funcion);
            }
        }
        else if (data.Estatus == EstatusRespuesta.ERROR) {
            Alerta(data.Mensaje);
        }
        else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA) {
            Alerta(data.Mensaje);
        }
    }

    function ValidarCuasaEnTabla(id) {
        var iterarArreglo = causas;

        for (var index = 0; index < iterarArreglo.length; index++) {
            if (causas[index].id == id) {
                return true;
            }
        }
        return false;
    }

    function EliminarCausa(id) {
        var funcion = function () {
            var iterarArreglo = causas;

            for (var index = 0; index < iterarArreglo.length; index++) {
                if (id == causas[index].id) {
                    causas.splice(index, 1);
                }
            }
        }

    function Alerta(mensaje) {
            bootbox.alert({
                title: "<h3>¡Atención!</h3>",
                message: mensaje,
                buttons:
                {
                    ok: {
                        label: '<i class="fa fa-check"></i> Confirmar',
                        className: 'btn btn-outline-success'
                    }
                }
            });
        }
    }