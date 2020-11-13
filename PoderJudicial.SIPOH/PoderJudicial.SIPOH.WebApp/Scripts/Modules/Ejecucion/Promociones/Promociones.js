/* ELEMENTOS AL CARGADO */

// #region DOCUMENT READY

$(document).ready(function () {
    HabilitarFormulariAnexos();
    FormatoInputs();
    OcultarFormulario();
    OtrosAnexosSelect();
    TablaCausas = Consumir_DataTable(TablaCausas, "_TablaCausasEjecucion", Arreglo_TablaCausas, EstructuraTabla_Causas, false, false, false);
    TablaAnexos = Consumir_DataTable(TablaAnexos, "_DataTableAnexos", Arreglo_TablaAnexos, EstructuraTabla_Anexos, false, false, false);
    FuncionalidadesListas();
});

// #endregion 

/* FUNCIONALIDADES DE FORM & DOM */


// #region FUNCIONALIDADES AL CARGADO
function FuncionalidadesListas() {

    // Convierte minusculas a mayusculas
    $(".UpperCase").on("keypress", function () {
        $textoInput = $(this);
        setTimeout(function () {
            $textoInput.val($textoInput.val().toUpperCase());
        }, 50);
    })

    // Button Disabled
    $("#btnNuevaConsultaPromocion").prop("disabled", true);
    $("#inpOtroAnexo").prop("disabled", true);
    $("#btnGuardarAnexos").prop("disabled", true);

    // Click Button
    $("#btnNuevaConsultaPromocion").click(function (e) {
        e.preventDefault();
        Resultados_NEW();
    });

}
// #endregion

// #region FUNCIONALIDAD: Ocultar Formulario
function OcultarFormulario() {
    $("#divResultadoPromocion").hide();
}
// #endregion

// #region FUNCIONALIDAD: Habilitar/Dsh Anexos
function HabilitarFormulariAnexos() {

    $("#formulario input").keyup(function () {

        $NombrePromovente = $("#inpNombrePromovente").val();
        $ApellidoPaterno = $("#inpPromoventeAP").val();
        $ApellidoMaterno = $("#inpPromoventeMA").val();

        if ($NombrePromovente.length <= 0 || $ApellidoPaterno.length <= 0 || $ApellidoMaterno.length <= 0) {
            $("#slctAnexoEjecucion").prop('disabled', true);
            $("#inpCantidadAnexos").prop('disabled', true);
            $("#btnAgregarAnexo").prop('disabled', true);
            $("#btnGuardarAnexos").prop('disabled', true);
        }
        else {
            $("#slctAnexoEjecucion").prop('disabled', false);
            $("#inpCantidadAnexos").prop('disabled', false);
            $("#btnAgregarAnexo").prop('disabled', false);
        }
    });
}
// #endregion

// #region FUNCIONALIDAD: Mostrar Formulario
function MostrarFormulario() {
    $('#divResultadoPromocion').removeAttr('hidden');
    $("#divResultadoPromocion").show();
    $(".disabled").prop('disabled', true);
}
// #endregion

// #region FUNCIONALIDAD: Encontró Resultados
function Resultados_OK() {

    $(".resultOK").prop('disabled', true);
    $("#btnNuevaConsultaPromocion").prop("disabled", false);

}
// #endregion

// #region FUNCIONALIDAD: Nueva Consulta
function Resultados_NEW() {

    OcultarFormulario();

    $(".resultOK").prop('disabled', false);

    $(".clean").val("");

    var form = $('#' + "FrmCausaEjecucion")[0];
    $('#slctJuzgadoPorCircuito').prop('selectedIndex', 0);

    var form = $('#FrmCausaEjecucion')[0];
    $(form).removeClass('was-validated');

    $("#_TablaCausasEjecucion").dataTable().fnClearTable();
    Arreglo_TablaCausas = [];

    $("#_DataTableAnexos").dataTable().fnClearTable();
    Arreglo_TablaAnexos = [];

    $("#btnNuevaConsultaPromocion").prop("disabled", true);

    $('#btnNuevaConsultaPromocion').tooltip('hide');

    $('#slctAnexoEjecucion').prop('selectedIndex', 0);


    var form = $('#Frm_Anexos')[0];
    $(form).removeClass('was-validated');

    $("#slctAnexoEjecucion").prop('disabled', true);
    $("#inpCantidadAnexos").prop('disabled', true);
    $("#btnAgregarAnexo").prop('disabled', true);
    $("#btnGuardarAnexos").prop('disabled', true);



}
// #endregion

// #region FUNCIONALIDAD: Cargar InputMask
function FormatoInputs() {

    FormatearInput("#inpNumeroEjecucion", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]");
}

// #endregion

// #region FUNCIONALIDAD: A-D Input Otro Anexo
function OtrosAnexosSelect() {

    $("#slctAnexoEjecucion").on('change', function () {
        var IdSelect = this.value;
        if (IdSelect == 8) {
            $("#inpOtroAnexo").prop('required', true);
            $("#inpOtroAnexo").prop('disabled', false);
        } else {
            $("#inpOtroAnexo").prop('disabled', true);
            $("#inpOtroAnexo").prop('required', false);
        }
    });

}
// #endregion

// #region FUNCIONALIDAD: Guardar datos en el LocalStorage
function AlmacenarIdEjecucion(IdEjecucionAnexo) {
    sessionStorage.setItem("IdEjecucionAnexo", IdEjecucionAnexo);
}
// #endregion

/* FUNCIONALIDADES PRINCIPALES */

// #region FUNCIONALIDAD: LISTAR DATOS GENERALES

function ListarDatosGenerales() {

    var slctJuzgado = $("#slctJuzgadoPorCircuito").val();
    var inpNoEjecucion = $("#inpNumeroEjecucion").val();
    var objParametros = { Juzgado: slctJuzgado, NoEjecucion: inpNoEjecucion };
    SolicitudEstandarGetAjax("/Promociones/ObtenerEjecucionPorJuzgado", objParametros, ConsumirMetodo_CrearPromocion);

}

function ConsumirMetodo_CrearPromocion(data) {

    if (data.Estatus == EstatusRespuesta.OK) {
        var Array = data.Data.ListaInformacion;
        var MensajeConfirmacion = "Se encontrarón coincidencias para el número de ejecución " + "<b>" + Array[0].NumeroEjecucion + "</b>" + " perteneciente al " + "<b>" + Array[0].NombreJuzgado + "</b>";
        var Funcion_MensajeOK = function () {
            Resultados_OK();
            MostrarFormulario();
            $("#NumeroEjecucion").html(Array[0].NumeroEjecucion);
            $("#NombreJuzgado").html(Array[0].NombreJuzgado);
            $("#DecSolicitante").html(Array[0].DescripcionSolicitante);
            $("#NombreBeneficiario").html(Array[0].NombreBeneficiario + " " + Array[0].ApellidoPBeneficiario + " " + Array[0].ApellidoMBeneficiario);
            $("#DecSolicitud").html(Array[0].DescripcionSolicitud);
            var idEjecucion = (Array[0].IdEjecucion);
            ConsumirMetodo_ObtenerExpedientesPorEjecucion(idEjecucion);
            var IdEjecucionAnexo = Array[0].IdEjecucion;
            AlmacenarIdEjecucion(IdEjecucionAnexo);

        }
        MensajeNotificacionOK(MensajeConfirmacion, "", Funcion_MensajeOK);
    } else if (data.Estatus == EstatusRespuesta.ERROR) {

        alert(data.Mensaje);

    } else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA) {

        var MensajeNoResult = "" + data.Mensaje + " para el numero de ejecución solicitado.";
        var Funcion_MensajeNoResult = function () {
            // Nothing
        }
        MensajeNotificacionNoResult(MensajeNoResult, "", Funcion_MensajeNoResult);
    }
}

// #endregion

// #region FUNCIONALIDAD: LISTAR CASUAS RELACIONADAS A DATOS GENERALES

function ConsumirMetodo_ObtenerExpedientesPorEjecucion(idEjecucion) {

    var ObjParametros = { idEjecucion: idEjecucion };
    SolicitudEstandarGetAjax("/Promociones/ObtenerExpedientesPorEjecucion", ObjParametros, ListarCausas);

}

function ListarCausas(data) {
    if (data.Estatus = EstatusRespuesta.OK) {

        var ArrayCausas = data.Data.ObtenerEPE;
        Arreglo_TablaCausas = []
        for (var index = 0; index < ArrayCausas.length; index++) {
            var Objct_TablaCausas = new Object();
            Objct_TablaCausas._NombreJuzgado = ArrayCausas[index].NombreJuzgado;
            Objct_TablaCausas._NumeroCausa = ArrayCausas[index].NumeroCausa;
            Objct_TablaCausas._Nuc = ArrayCausas[index].NUC;
            Objct_TablaCausas._Ofendidos = ArrayCausas[index].Ofendidos;
            Objct_TablaCausas._Inculpados = ArrayCausas[index].Inculpados;
            Objct_TablaCausas._Delitos = ArrayCausas[index].Delitos;
            Arreglo_TablaCausas.push(Objct_TablaCausas);
        }
        TablaCausas = Consumir_DataTable(TablaCausas, "_TablaCausasEjecucion", Arreglo_TablaCausas, EstructuraTabla_Causas, false, false, false);
    } else if (data.Estatus == EstatusRespuesta.ERROR) {
        alert("Hay un error de comunicación");
    } else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA) {
        alert("Sin respuesta");
    }

}
// #endregion

// #region FUNCIONALIDAD: AGREGAR ANEXOS
function AgregarAnexos() {

    var TxtAnexo = $("#slctAnexoEjecucion").children("option:selected").text();
    var NumeroAnexo = $("#slctAnexoEjecucion").children("option:selected").val();
    var NoCantidad = $("#inpCantidadAnexos").val();
    var TxtOtro = $("#inpOtroAnexo").val();

    $("#btnGuardarAnexos").prop('disabled', false);

    if (NoCantidad != 0) {
        if (Validar_AnexosTabla(NumeroAnexo)) {

            var ArregloTablaAnexos = Arreglo_TablaAnexos;
            var AnexoValor = NumeroAnexo;

            for (var index = 0; index < ArregloTablaAnexos.length; index++) {

                if (AnexoValor == ArregloTablaAnexos[index].IdAnexo) {
                    // Valida cantidad para actualizar 
                    ArregloTablaAnexos[index].Cantidad = NoCantidad;
                }
            }
        }
        else {
            var Objct_TablaAnexos = new Object();
            if (NumeroAnexo == 8) {
                Objct_TablaAnexos.Descripcion = TxtOtro;
            }
            else {
                Objct_TablaAnexos.Descripcion = TxtAnexo;
            }
            Objct_TablaAnexos.IdAnexo = NumeroAnexo;
            Objct_TablaAnexos.Cantidad = NoCantidad;
            Objct_TablaAnexos.Acciones = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='Quitar_Anexos(" + Objct_TablaAnexos.IdAnexo + ")' data-toggle='tooltip' data-placement='top' title='Quitar Anexo'><i class='icon-bin2'></i></button>";
            Arreglo_TablaAnexos.push(Objct_TablaAnexos);
        }
        TablaAnexos = Consumir_DataTable(TablaAnexos, "_DataTableAnexos", Arreglo_TablaAnexos, EstructuraTabla_Anexos, false, false, false);
    }
    else {
        var Mensaje_Existe = "No se puede asignar el anexo " + " <b>" + TxtAnexo + "</b>" + " con " + "<b> " + NoCantidad + "</b> " + " copias certificadas, ingrese una cantidad valida.";
        var Funcion_Existe = function () { }
        MensajeNotificacionNoResult(Mensaje_Existe, "", Funcion_Existe);
    }


}


//#endregion

// #region GUARDAR ANEXOS

function GuardarAnexos() {

    $NombrePromovente = $("#inpNombrePromovente").val() + " " + $("#inpPromoventeAP").val() + " " + $("#inpPromoventeMA").val()
    $IdCatAnexo = $("#slctAnexoEjecucion").find('option:selected').val()
    $Cantidad = $("#inpCantidadAnexos").val()

    MensajeDatos = "Esta a punto de registrar una promocion, ¿Desea continuar?";



    var Funcion_Ejecutar = function () {

        $("#loading").fadeIn();

        intentos = intentos + 1;

        /* Recupera datos del LocalStore */
        var IdAnexoEjecucion = sessionStorage.getItem("IdEjecucionAnexo");

        var objetoparametro = {
            IdEjecucion: IdAnexoEjecucion,
            Promovente: $NombrePromovente,
            Anexos: Arreglo_TablaAnexos
        }

        SolicitudEstandarPostAjax("/Promociones/GuardarAnexosPostEjecucion", objetoparametro, RederizarDetalleSuccess, RederizarDetalleError);
    }

    MensajeNotificacionGuardar(MensajeDatos, "", Funcion_Ejecutar);


}

$("#btnGuardarAnexos").click(function (e) {

    e.preventDefault();
    GuardarAnexos();

});

// #endregion

/*----- FUNCIONES GENERALES -----*/

// #region ESTATUS: Validación de petición AJAX
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 };
// #endregion

// #region VARIABLES GLOBALES
var intentos = 0;
// #endregion

// #region ESTRUCTURAS: DataTable

var EstructuraTabla_Causas = [
    { data: '_NombreJuzgado', title: 'JUZGADO', className: "text-justity" },
    { data: '_NumeroCausa', title: 'CAUSA', className: "text-justity" },
    { data: '_Nuc', title: 'NUC', className: "text-justity" },
    { data: '_Ofendidos', title: 'OFENDIDOS (S)', className: "text-justity" },
    { data: '_Inculpados', title: 'INCULPADO (S)', className: "text-justity" },
    { data: '_Delitos', title: 'DELITO (S)', className: "text-justity" }];

var Arreglo_TablaCausas = [];

var EstructuraTabla_Anexos = [
    { data: 'Descripcion', title: "DESCRIPCIÓN", className: "text-center" },
    { data: 'Cantidad', title: "CANTIDAD", className: "text-center" },
    { data: 'Acciones', title: "ACCIONES", className: "text-center" }];

var Arreglo_TablaAnexos = [];

var TablaCausas = null;
var TablaAnexos = null;

// #endregion

// #region FUNCION: Solicitud Ajax Get
function SolicitudEstandarGetAjax(url, parametros, funcion) {
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: parametros,
        beforeSend: function () {
            // $("#loading").fadeIn(); //Animacion Load
        },
        success: function (data) {
            funcion(data);
        },
        error: function (xhr) {
            alert('Error Ajax: ' + xhr.statusText);
            //  $("#loading").fadeOut();
        }
    });
}
// #endregion

// #region FUNCION: Solicitud Ajax Post
function SolicitudEstandarPostAjax(urlAction, parameters, functionCallbackSuccess) {
    $.ajax({
        url: urlAction,
        type: "POST",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(parameters),
        dataType: "json",
        cache: true,
        beforeSend: function () {
            //$("#loading").fadeIn();
        },
        success: function (data) {
            functionCallbackSuccess(data);
        },
        error: function (jqXHR) {
            $("#loading").fadeOut();

            var mensaje = '';

            if (jqXHR.status === 0) {
                mensaje = 'No esta conectado, verifique su conexión.';
            }
            else if (jqXHR.status == 404) {
                mensaje = 'No se encontró la página solicitada, ERROR:404';
            }
            else if (jqXHR.status == 500) {
                mensaje = "Error interno del servidor, ERROR:500";
            }
            else if (exception === 'timeout') {
                mensaje = 'Error de Time Out.';
            }
            else if (exception === 'abort') {
                mensaje = 'Solicitud AJAX Abortada.';
            }
            else {
                mensaje = 'Error no detectado : ' + jqXHR.responseText;
            }

            if (functionCallbackSuccess == null) {
                Alerta(mensaje, "large", "Error ");
            }

            if (functionCallbackError != null) {
                var data = mensaje;
                functionCallbackError(data);
            }
        }
    });
}
// #endregion

// #region FUNCION: Validación de formularios
var forms = document.getElementsByClassName('needs-validation');

Array.prototype.filter.call(forms, function (form) {
    form.addEventListener('submit', function (event) {
        var id = form.id;
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        if (form.checkValidity() === true && id == "FrmCausaEjecucion") {
            ListarDatosGenerales();
        }
        if (form.checkValidity() === true && id == "Frm_Anexos") {
            AgregarAnexos();
        }
    }, false);
});
// #endregion

// #region FUNCIÓN: DataTable
function Consumir_DataTable(tabla, idTablaHtml, datos, estructuraTabla, ordering, searching, lengthChange) {
    if (tabla != null) {
        tabla.destroy();
        $("#" + idTablaHtml).empty();
    }
    return tabla = $("#" + idTablaHtml).DataTable({
        data: datos,
        columns: estructuraTabla,
        rowId: 'id',
        responsive: true,
        "ordering": ordering,
        "searching": searching,
        "lengthChange": lengthChange,
        "pageLength": 5,
        "lengthMenu": [5, 10, 25, 50],
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "_START_ al _END_ de _TOTAL_",
            "sInfoEmpty": "0 al 0 de 0",
            "sInfoFiltered": "(Total _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            },


        },
        drawCallback: function (settings) {
            $('[data-toggle="tooltip"]').tooltip();
        }
    });
}

// #endregion

// #region FUNCION: Formato Inputs & Mask
function FormatearInput(selector, mask, placeholder, validatorRegEx) {
    Inputmask(mask, {
        positionCaretOnClick: "select",
        radixPoint: "/",
        _radixDance: true,
        numericInput: true,
        placeholder: placeholder,
        definitions: {
            "0": {
                validator: validatorRegEx
            }
        }
    }).mask(selector);
}
// #endregion

// #region FUNCIÓN: Notificacion OK
function MensajeNotificacionOK(mensaje, tamanio, funcion) {
    bootbox.confirm({
        title: "<h3>Confirmación</h3>",
        message: mensaje,
        buttons: {
            confirm: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-success'
            },
            cancel: {
                label: '<i class="fa fa-times"></i> Cancelar',
                className: 'btn btn-outline-secondary'
            }
        },
        callback: function (result) {
            if (result) {
                funcion();
            }
        },
        size: tamanio
    });
}

// #endregion 

// #region FUNCIÓN: Notificacion NO Result

function MensajeNotificacionNoResult(mensaje) {
    bootbox.alert({
        title: "<h3>¡Atención!</h3>",
        message: mensaje,
        buttons:
        {
            ok: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-danger'
            }
        }
    });
}

// #endregion

// #region FUNCIÓN: Validar Anexos en Tabla
function Validar_AnexosTabla(IdAnexoTabla) {
    var Tabla = Arreglo_TablaAnexos;
    for (var i = 0; i < Tabla.length; i++) {
        if (Arreglo_TablaAnexos[i].IdAnexo == IdAnexoTabla) {
            return true;
        }
    }
    return false;
}
// #endregion

// #region FUNCIÓN: Quitar Anexos
function Quitar_Anexos(Id_ItemTabla) {
    // Obtiene el Nombre de Estructura
    var ArregloTabla = Arreglo_TablaAnexos;
    var FuncionQuitar = function () {
        for (var i = 0; i < ArregloTabla.length; i++) {
            if (Id_ItemTabla == Arreglo_TablaAnexos[i].IdAnexo) {
                Arreglo_TablaAnexos.splice(i, 1);
            }

            // Valida si la tabla tiene N Elementos
            if (ArregloTabla.length == 0) {
                $("#btnGuardarAnexos").prop("disabled", true);
            }

            TablaAnexos = Consumir_DataTable(TablaAnexos, "_DataTableAnexos", Arreglo_TablaAnexos, EstructuraTabla_Anexos, false, false, false);
        }
    }

    for (var index = 0; index < Arreglo_TablaAnexos.length; index++) {
        if (Id_ItemTabla == Arreglo_TablaAnexos[index].IdAnexo) {
            var MensajeQuitar = "Se removera el anexo " + "<b>" + ArregloTabla[index].Descripcion + "</b>" + " con: " + "<b>" + ArregloTabla[index].Cantidad + "</b>" + " copia (s) asignada (s). ¿Desea continuar? ";

        }
    }
    MensajeNotificacionOK(MensajeQuitar, "default", FuncionQuitar);
}

// #endregion

// #region FUNCIÓN: Limpiar formularios Firefox

function LimpiarElementosFirefox() {

}

// #endregion

// #region FUNCIÓN: Renderizar detalle Error
function RederizarDetalleError(data) {

    var MensajeData = data.Mensaje;

    $("#loading").fadeOut();

    if (intentos > 2) {
        var mensaje = "<b>" + "SISTEMA: " + "</b>" + MensajeData + ". <br><br>Numero de Intentos Ejecutados: " + "<b class='text-danger'>" + intentos + "</b>" + "<br><br><b class='text-danger'>Ha superado el numero maximo de intentos, vuelva intentarlo mas tarde o consulte a soporte</b";
        intentos = 0;
        Alerta(mensaje, "");
    }
    else {
        var mensaje = "<b>" + "SISTEMA: " + "</b>" + MensajeData + ", de click en " + "<b class='text-success'>" + "Reintentar " + "</b>" + " para ejecutar nuevamente.<br><br>Numero de Intentos Actuales: " + "<b>" + intentos + "</b>";
        reintento = true;
        MensajeDeConfirmacion(mensaje, "", GuardarAnexos, null, titulo = "<i class='icon-warning text-warning'></i> Error no controlado por el sistema");
    }
}
// #endregion

// #region FUNCIÓN: Renderizar Detalle Success
function RederizarDetalleSuccess(data) {
    var MensajeData = data.Mensaje;

    if (data.Estatus == EstatusRespuesta.OK) {
        AlmacenarIdEjecucion(null);
        var url = data.Data.Url;
        /* Redirecciona a la vista detalle */
        document.location.href = url;
    }
    else if (data.Estatus == EstatusRespuesta.ERROR) {
        $("#loading").fadeOut();

        if (intentos > 2) {

            var mensaje = "<b>" + "SISTEMA: " + "</b>" + MensajeData + ". <br><br>Numero de Intentos Ejecutados: " + "<b class='text-danger'>" + intentos + "</b>" + "<br><br><b class='text-danger'>Ha superado el numero maximo de intentos, vuelva intentarlo mas tarde o consulte a soporte</b";

            intentos = 0;

            Alerta(mensaje, "");
        } else {

            var mensaje = "<b>" + "SISTEMA: " + "</b>" + MensajeData + ", de click en " + "<b class='text-success'>" + "Reintentar " + "</b>" + " para ejecutar nuevamente.<br><br>Numero de Intentos Actuales: " + "<b>" + intentos + "</b>";

            reintento = true;

            MensajeDeConfirmacion(mensaje, "", GuardarAnexos, null, titulo = "<i class='icon-warning text-warning'></i> Error no controlado por el sistema");
        }
    }
}
//#endregion 

// #region FUNCIÓN: Mensaje de Confirmación
function MensajeDeConfirmacion(mensaje, tamanio, funcion, funcionCancelar = null, titulo = null) {

    titulo = titulo == null ? "Confirmación" : titulo;

    bootbox.confirm({
        title: "<h3>" + titulo + "</h3>",
        message: mensaje,
        buttons: {
            confirm: {
                label: '<i class="icon-undo"></i> Reintentar',
                className: 'btn btn-outline-success'
            },
            cancel: {
                label: '<i class="fa fa-times"></i> Cancelar',
                className: 'btn btn-outline-secondary'
            }
        },
        callback: function (result) {
            if (result) {
                funcion();
            }
            else {
                if (funcionCancelar != null) {
                    funcionCancelar();
                }
            }
        },
        size: tamanio
    });
}
//#endregion 

// #region FUNCIÓN: Mensaje Alerta

function Alerta(mensaje, tamanio = null, titulo = null) {

    titulo = titulo == null ? "<i class='icon-warning text-danger'></i> ¡Ups! No podemos contactar con el servidor" : titulo;
    tamanio = tamanio == null ? "small" : tamanio;

    bootbox.alert({
        title: "<h3>" + titulo + "</h3>",
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

//#endregion 


function MensajeNotificacionGuardar(mensaje, tamanio, funcion) {
    bootbox.confirm({
        title: "<h3>Confirmación</h3>",
        message: mensaje,
        buttons: {
            confirm: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-success'
            },
            cancel: {
                label: '<i class="fa fa-times"></i> Cancelar',
                className: 'btn btn-outline-secondary'
            }
        },
        callback: function (result) {
            if (result) {
                funcion();
            }
        },
        size: tamanio
    });
}