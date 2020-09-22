/*---------- TABLAS DINAMICAS ----------*/
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }

var TablaCausasEjecucion = null;
var TablaAnexos = null;

var FormatoTablaCausasEjecucion = [{ data: '_NombreJuzgado', title: 'JUZGADO', className: "text-center" },
                                   { data: '_NumeroCausa', title: 'CAUSA', className: "text-center" },
                                   { data: '_NUC', title: 'NUC', className: "text-center" },
                                   { data: '_Ofendidos', title: 'OFENDIDOS (S)', className: "text-center" },
                                   { data: '_Inculpados', title: 'INCULPADO (S)', className: "text-center" },
                                   { data: '_Delitos', title: 'DELITO (S)', className: "text-center" },
                                   { data: '_Eliminar', title: 'ACCIONES', className: "text-center" }];
var CausasEjecucion = [];

var FormatoTablasAnexosEjecucion = [{ data: '_Descripcion', tittle: 'Descripción', className: "text-center"},
                                    { data: '_Cantidad', tittle: 'Cantidad', className: "text-center" }];
var AnexosEjecucion = [];

/*-------- CARGADO DE LA PAGINA --------*/

$(document).ready(function () {
    OcultarElementos();
    DesactivarElementos();
    NuevaConsulta();
    TablaCausasEjecucion = TablaDatos(TablaCausasEjecucion, "_TablaCausasEjecucion", CausasEjecucion, FormatoTablaCausasEjecucion, false, false, false);   
    TablaAnexos = TablaDatos(TablaAnexos, "_TablaAnexosHTM", AnexosEjecucion, FormatoTablasAnexosEjecucion, false, false, false);
});

/*----------- FUNCIONES PRINCIPALES ----------*/
function ConsultarPromocion() {
        $("#divResultadoPromocion").show();
        var IdSelect = $("#slctJuzgadoPorCircuito").children("option:selected").val();
        var IdInput = $("#inpNumeroEjecucion").val();
        var Par_DatosGenralesEjecucion = { Juzgado: IdSelect, NoEjecucion: IdInput }
        SolicitudEstandarAjaxGET('/Promociones/ObtenerEjecucionPorJuzgado', Par_DatosGenralesEjecucion, ConsultarDatosGeneralesEjecucion);
        //$("#slctJuzgadoPorCircuito").prop("disabled", true);
        //$("#inpNumeroEjecucion").prop("disabled", true);
}

function ConsultarDatosGeneralesEjecucion(data) {
    if (data.Estatus == EstatusRespuesta.OK) {
        var ArrayData = data.Data.ListaInformacion;
        var ArrayDataNE = ArrayData[0].NumeroEjecucion;
        var ArrayDataNJ = ArrayData[0].NombreJuzgado;
        console.log(JSON.stringify(ArrayData));
        if (ValidarCausaEjecucion(ArrayData.NumeroEjecucion)) {
            var Mensaje = "El numero de ejecucion " + "<b>" + ArrayDataNE + "</b>" + " ya se encuentra en la tabla."
            var FuncionPostNot = function () {
                return true;
            }
            MensajeDeConfirmacion(Mensaje, "", FuncionPostNot);
        } else {
            var Mensaje = "La consulta ejecutada ha encontrado exitosamente resultados del expediente con numero " + "<b>" + ArrayDataNE + "</b>" + " perteneciente al Juzgado " + "<b>" + ArrayDataNJ + "</b>";
            var FuncionPost = function () {
                if (CausasEjecucion.length == 0) {
                    $("#divResultadoPromocion").hide();
                }
            }
            $("#NumeroEjecucion").val(ArrayData[0].NumeroEjecucion);
            $("#NombreJuzgado").val(ArrayData[0].NombreJuzgado);
            $("#DecSolicitante").val(ArrayData[0].DescripcionSolicitante);
            $("#NombreBeneficiario").val(ArrayData[0].NombreBeneficiario + ' ' + ArrayData[0].ApellidoPBeneficiario + ' ' + ArrayData[0].ApellidoMBeneficiario);
            $("#DecSolicitud").val(ArrayData[0].DescripcionSolicitud);
            var dataIdExpediente = ArrayData[0].IdExpediente;
            EjecutarConsultaDTCausas(dataIdExpediente);
            $(".disabled").prop("disabled", true);
            MensajeDeConfirmacion(Mensaje, "", FuncionPost);
        }    
    } else if (data.Estatus == EstatusRespuesta.ERROR) {
        MensajeDeConfirmacion(data.Mensaje);
    } else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA) {
        MensajeDeConfirmacion(data.Mensaje)
    }   
}   

function EjecutarConsultaDTCausas(dataIdExpediente) {
    Par_DatosExpedienteEjecucion = { IdExpediente: dataIdExpediente }
    SolicitudEstandarAjaxGET("/Promociones/ObtenerExpedienteEjecucionCausa", Par_DatosExpedienteEjecucion, DTCausasRelacionadasEjecucion);
}

function DTCausasRelacionadasEjecucion(data) {
    if (data.Estatus == EstatusRespuesta.OK) {
        var CausaEjecucion = data.Data
        $("#_TablaCausasEjecucion").removeAttr('hidden');
        $("#_TablaCausasEjecucion").show();
        var causaEjecucion = new Object();
        causaEjecucion._IdExpediente = CausaEjecucion.IdExpediente;
        causaEjecucion._NombreJuzgado = CausaEjecucion.NombreJuzgado;
        causaEjecucion._NumeroCausa = CausaEjecucion.NumeroCausa;
        causaEjecucion._NUC = CausaEjecucion.NUC;
        causaEjecucion._Ofendidos = CausaEjecucion.Ofendidos;
        causaEjecucion._Inculpados = CausaEjecucion.Inculpados;
        causaEjecucion._Eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarItemTabla(" + causaEjecucion.IdExpediente + ")' data-toggle='tooltip' title='Quitar Causa'><i class='icon-bin2'></i></button>";
        causaEjecucion._Delitos = CausaEjecucion.Delitos;
        CausasEjecucion.push(causaEjecucion);
        TablaCausasEjecucion = TablaDatos(TablaCausasEjecucion, "_TablaCausasEjecucion", CausasEjecucion, FormatoTablaCausasEjecucion, false, false, false);
    } else if (data.Estatus == EstatusRespuesta.ERROR) {
        MensajeDeConfirmacion(data.mensaje);
    } else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA) {
        MensajeDeConfirmacion(data.Mensaje);
    }
}

function DTAnexosEjecucion() {
    var DataAnexo = data.Data;
    $("#_TablaAnexosHTM").removeAttr('hidden');
    $("#_TablaAnexosHTM").show();
    var AnexoEjecucion = new Object();
    AnexoEjecucion._IdAnexo = DataAnexo.IdAnexo;
    AnexoEjecucion._Descripcion = DataAnexo.Descripcion;
    AnexoEjecucion._Cantidad = DataAnexo.Cantidad;
    AnexoEjecucion._Eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarItemTabla(" + AnexoEjecucion.IdAnexo + ")' data-toggle='tooltip' title='Quitar Causa'><i class='icon-bin2'></i></button>";
    AnexosEjecucion.push(AnexoEjecucion);
    TablaAnexos = TablaDatos(TablaAnexos, "_TablaAnexosHTM", AnexosEjecucion, FormatoTablasAnexosEjecucion,false,false,false);
}

function OcultarElementos() {
    $("#divResultadoPromocion").hide();
}

function DesactivarElementos() {
    $('.disoff').prop("disabled", true);
}

function NuevaConsulta() {
    $("#btnNuevaConsultaPromocion").click(function () {
        $(".disabled").prop("disabled", false);
        $("#divResultadoPromocion").hide();
    });
}

/*--------- FUNCIONES GENERALES --------*/

/* Mensaje de Confirmacion */
function MensajeDeConfirmacion(mensaje, tamanio, funcion) {
    bootbox.confirm({
        title: "<h3>Confirmación</h3>",
        message: mensaje,
        buttons: { 
            confirm: {
                label: '<i class="fa fa-check"></i> Confirmar',
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

/* Consume Metodos de Controlador */
function SolicitudEstandarAjaxGET(url, parametros, funcion) {
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: parametros,
        beforeSend: function () {
        },
        success: function (data) {
            funcion(data);
        },
        error: function (xhr) {
            alert('Error Ajax: ' + xhr.statusText);
        }
    });
}

/* Genera DataTable */
function TablaDatos(tabla, idTablaHtml, datos, estructuraTabla, ordering, searching, lengthChange) {
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

/* Valida Formularios */
var forms = $('.needs-validation');
Array.prototype.filter.call(forms, function (form) {
    form.addEventListener('submit', function (event) {
        var id = form.id;
        event.preventDefault();
        event.stopPropagation();
        if (form.checkValidity() === true && id == "FrmCausaEjecucion") {
            ConsultarPromocion(true);
        }
        form.classList.add('was-validated');
    }, false);
});

// Eliminar Items de Tabla 
function EliminarItemTabla(IdExpediente) {
    var Funcion = function () {

        var SeleccionCausaFuncion = CausasEjecucion;

        for (var i = 0; i < SeleccionCausaFuncion.length; i++) {
            if (IdExpediente == CausasEjecucion[i].IdExpediente) {
                CausasEjecucion.splice(i, 1);
            }
        }
        TablaCausasEjecucion = TablaDatos(TablaCausasEjecucion, "_TablaCausasEjecucion", CausasEjecucion, FormatoTablaCausasEjecucion, false, false, false); 
    }
    var Mensaje = "¿Desea retirar la casua relacionada?";
    MensajeDeConfirmacion(Mensaje, "small", Funcion);
}

function ValidarCausaEjecucion(IdCausa) {
    var ItemArrelgoCE = CausasEjecucion;
    for (var i = 0; i < ItemArrelgoCE.length; i++) {
        if (CausasEjecucion[i].IdExpediente == IdCausa) {
            return true;
        }
    }
}
