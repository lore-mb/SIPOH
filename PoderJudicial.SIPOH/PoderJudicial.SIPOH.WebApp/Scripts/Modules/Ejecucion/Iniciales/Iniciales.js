//region Varaibles Globale!
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var idcircuito = null;
var idDistrito = null;
var esJuzgadoAcusatorio = true;

//Estructura para data tables
var estructuraTablaCausas = [{ data: 'nJuzgado', title: 'N° Juzgado' }, { data: 'causaNuc', title: 'Causa|Nuc' }, { data: 'ofendido', title: 'Ofendido (s)' }, { data: 'inculpado', title: 'Inculpado (s)' }, { data: 'delito', title: 'Delitos (s)' }, { data: 'eliminar', title: 'Quitar' }];
var dataTable = null;
var causas = [];

$(document).ready(function ()
{
    //Pintar Tabla
    dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
    
    //Elemntos al Cargado
    ElementosAlCargado();

    //Solicitudes AJAX
    LlenaPickListCircuito();  
});

//Elementos al Cargado
function ElementosAlCargado()
{
    //Funcionalidad para mostrar u ocultar NUC o Causa
    $("#slcNumero").change(function ()
    {
        if ($(this).val() == "2")
        {
            $('#inpCAU').val("");
            $('#divCAU').show();
            $('#divNUC').hide();

            //Trampa
            $('#inpNumNUC').val("AA");
        }
        else if ($(this).val() == "1")
        {
            $('#inpNumNUC').val("");
            $('#divNUC').show();
            $('#divCAU').hide();

            //Trampa
            $('#inpCAU').val("BB");           
        }
    });
    $("#divCAU").hide();
    $('#inpCAU').val("BB");

    //Funcionalidad para validar formularios
    var forms = document.getElementsByClassName('needs-validation');
    Array.prototype.filter.call(forms, function (form)
    {
        form.addEventListener('submit', function (event)
        {
            event.preventDefault();
            event.stopPropagation();

            if (form.checkValidity() === true)
            {
                //alert("CORRECTO, ES ACUSATORIO ? " + esJuzgadoAcusatorio);
                ConsultarCausas(esJuzgadoAcusatorio);
            }            
           
            form.classList.add('was-validated');

        }, false);
    });

    $('#slctDistrito').change(function ()
    {
        idDistrito = $("#slctDistrito").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        Distrito_JuzgadoTradicional();
    });

    $('#inpNumNUC').change(function ()
    {
        esJuzgadoAcusatorio = true;

    });

    $('#inpCAU').change(function ()
    {
        esJuzgadoAcusatorio = true;
    });

    $('#inpCAUT').change(function ()
    {
        esJuzgadoAcusatorio = false;
    });
}

function LlenaPickListCircuito()
{
    SolicitudEstandarAjax("/Iniciales/ObtenerCircuito", "", ListarCircuito);
}

function ListarCircuito(data)
{
    if (data.Estatus = EstatusRespuesta.OK)
    {
        const ObjCircuito = [data.Data];
        const ObjCircuitoTr = [data.Data];
        idcircuito = data.Data.Value;

        var $slctCirAc = $('#slctCircuitoAc');
        var $slctCirTr = $('#slctCircuitoTr');

        $.each(ObjCircuito, function (id, circuito)
        {
            $slctCirAc.append('<option value=' + circuito.Value + '>' + circuito.Text + '</option>');
            
        });

        $.each(ObjCircuitoTr, function (id, circuitoTr) {
            $slctCirTr.append('<option value=' + circuitoTr.Value + '>' + circuitoTr.Text + '</option>');
        });

        Circuito_JuzgadoAcusatorio();
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}

function Circuito_JuzgadoAcusatorio()
{
    if (idcircuito != null)
    {
        var parobj = { idCircuito : idcircuito }
        SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoAcusatorio", parobj, ListarJuzgadoAcusatorio);
    }
    else
    {
        alert("Error al obtener los datos");
    }
}

function Distrito_JuzgadoTradicional()
{
    if (idDistrito != null)
    {
        var parametros = { idDistrito: idDistrito }
        SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoTradicional", parametros, ListarJuzgadoTradicional);
    }
    else
    {
        alert("Error al obtener los datos");
    }
}

function ListarJuzgadoAcusatorio(data)
{
    if (data.Estatus = EstatusRespuesta.OK)
    {
        const ObjJuzgadoAcu = [data.Data];
        var $slcJuzAcu = $('#slctJuzgado');

        $.each(ObjJuzgadoAcu, function (id, juzgado)
        {
            for (var i = 0; i < juzgado.length; i++)
            {
                $slcJuzAcu.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
            }
        });

        Parametros_Distrito();
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}

function ListarJuzgadoTradicional(data)
{
    if (data.Estatus = EstatusRespuesta.OK)
    {
        $("#slctJuzgadoTradi").html("");
        $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));

        const ObjJuzgadoTra = [data.Data];
        var $slcTradi = $('#slctJuzgadoTradi');

        $.each(ObjJuzgadoTra, function (id, juzgado)
        {
            for (var i = 0; i < juzgado.length; i++)
            {
                $slcTradi.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
            }
        });   
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}


// #region Parametro Distrito
function Parametros_Distrito()
{
    if (idcircuito != null)
    {
        var Parametros = { idCircuito: idcircuito }
        SolicitudEstandarAjax("/Iniciales/ObtenerDistritosPorCircuito", Parametros, ListarDistrito)
    }
    else
    {
        alert("Error de parametro");
    }
}
// #endregion


function ListarDistrito(data)
{
    if (data.Estatus = EstatusRespuesta.OK)
    {
        var Array = [data.Data]
        var $pickDistrito = $('#slctDistrito');

        $.each(Array, function (id, distrito)
        {
            for (var i = 0; i < distrito.length; i++)
            {
                $pickDistrito.append('<option value=' + distrito[i].Value + '>' + distrito[i].Text + '</option>');
            }
        });
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}

function ConsultarCausas(esAcusatorio)
{
    if (esAcusatorio)
    {
        var nucCusa = $("#slcNumero").find('option:selected').val();
        var juzgadoId = $("#slctJuzgado").find('option:selected').val();

        if (nucCusa == 1)
        {
            var nuc = $('#inpNumNUC').val();
            var parametros = { idJuzgado: juzgadoId, nuc: nuc };
            SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorNUC", parametros, ListarCausas);
        }
        else
        {
            var numCusa = $('#inpCAU').val();
            var parametros = { idJuzgado: juzgadoId, numeroCausa: numCusa };
            SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorCausa", parametros, ListarCausas);
        } 
    }
    else
    {
        var numCusa = $("#inpCAUT").val();
        var juzgadoId = $("#slctJuzgadoTradi").find('option:selected').val();

        var parametros = { idJuzgado: juzgadoId, numeroCausa: numCusa };
        SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorCausa", parametros, ListarCausas);
    }
}

function ListarCausas(data)
{
    try
    {
        if (data.Estatus == EstatusRespuesta.OK)
        {
            var expediente = data.Data;

            var causa = new Object();
            causa.id = expediente.IdExpediente;
            causa.nJuzgado = expediente.NombreJuzgado;
            causa.causaNuc = expediente.NumeroCausa == null ? expediente.NUC : expediente.NumeroCausa;
            causa.ofendido = expediente.Ofendidos;
            causa.inculpado = expediente.Inculpados;
            causa.delito = expediente.Delitos;
            causa.eliminar = "<a href='#' class='btn btn-danger btn-sm' onclick='EliminarCausa(" + causa.id + ")'><i class='fas fa-trash-alt'></i></a>";

            //Agrega Causa al Arreglo de Cuasas
            causas.push(causa);

            dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
        }
        else if (data.Estatus == EstatusRespuesta.ERROR)
        {
            alert(data.Mensaje);
        }
    }
    catch (e)
    {
        alert("Ocurrio una excepción InicialesJs.ListarCausas: " + e.message)
    }
}

function EliminarCausa(id) 
{
    alert(id);
}

//#region Solicitud Ajax Get Generico
function SolicitudEstandarAjax(url, parametros, funcion)
{
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
        success: function (data)
        {
            funcion(data);
        },
        error: function (xhr)
        {
            alert('Error Ajax: ' + xhr.statusText);
            //  $("#loading").fadeOut();
        }
    });
}
// #endregion

function GeneraTablaDatos(tabla, idTablaHtml, datos, estructuraTabla, ordering, searching, lengthChange)
{
    if (tabla != null)
    {
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
            }
        }
    });
}