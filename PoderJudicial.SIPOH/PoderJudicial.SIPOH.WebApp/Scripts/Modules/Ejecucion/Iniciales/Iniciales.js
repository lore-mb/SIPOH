//region Varaibles Globale!
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var idcircuito = null;
var idDistrito = null;
var esJuzgadoAcusatorio = true;

//Estructura para data tables
var dataTable = null;
var dataTableAnex = null;
var dataTableBeneficiario = null;

var estructuraTablaCausas = [{ data: 'nJuzgado', title: 'N° Juzgado' }, { data: 'causaNuc', title: 'Causa|Nuc' }, { data: 'ofendido', title: 'Ofendido (s)' }, { data: 'inculpado', title: 'Inculpado (s)' }, { data: 'delito', title: 'Delitos (s)' }, { data: 'eliminar', title: 'Quitar' }];
var causas = [];

var estructuraTablaAnexos = [{ data: 'cantidad', title: 'Cantidad' }, { data: 'descripcion', title: 'Descripción' }];
var anexos = [];

var estructuraTablaBeneficiarios = [{ data: 'cantidad', title: 'Cantidad' }, { data: 'descripcion', title: 'Descripción' }];
var beneficarios = [];

//Funciones que se detonan al terminado del renderizado 
$(document).ready(function ()
{
    //Pintar Tabla
    dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
    dataTableAnex = GeneraTablaDatos(dataTableAnex, "dataTableAnexos", anexos, estructuraTablaAnexos, false, false, false);

    //Elemntos al Cargado
    ElementosAlCargado();

    //Solicitudes AJAX
    LlenaPickListCircuito();  
});

//Elementos al Cargado
function ElementosAlCargado()
{
   //$("#contenedorBeneficiario").hide();
   //$("#seccionBeneficiario").hide();
    $("#seccionBusquedaAnexos").hide();
    $("#seccionTablaAnexos").hide();

    //Funcionalidad para validar formularios
    var forms = document.getElementsByClassName('needs-validation');

    Array.prototype.filter.call(forms, function (form)
    {
        form.addEventListener('submit', function (event)
        {
            var id = form.id;
            
            event.preventDefault();
            event.stopPropagation();

            if (form.checkValidity() === true && (id == "formCausas" || id == "formCausasTradicional"))
            {
                ConsultarCausas(esJuzgadoAcusatorio);
            }            

            if (form.checkValidity() === true && id == "formAnexos")
            {
                alert(id);
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

    $('#inpCAUT').change(function ()
    {
        esJuzgadoAcusatorio = false;
    });

    $("#botonMostrarBeneficiarios").click(function ()
    {
        MostrarBeneficiarios();
    });

    $("#btnCancelar").click(function ()
    {
        $("#ejecucionModal").modal("hide");
    });

    $('#inpApellidoPaterno').change(function ()
    {
        ValidarBeneficiarios();
    });

    $('#inpNombreSentenciado').change(function ()
    {
        ValidarBeneficiarios();
    });

}

function MostrarBeneficiarios()
{
    var apellidoPBene = $('#inpApellidoPaterno').val();
    var NombreBene = $('#inpNombreSentenciado').val();

    if (NombreBene != "" && apellidoPBene != "")
    {
        $('#ejecucionModal').modal('show');
    }
}

function ValidarBeneficiarios()
{
    var apellidoPBene = $('#inpApellidoPaterno').val();
    var apellidoMBene = $('#inpApellidoMaterno').val();
    var NombreBene = $('#inpNombreSentenciado').val();

    if (NombreBene != "" && apellidoPBene != "")
    {
        //Consumir Metodo del Controlador
        var parametros = { nombreBene: NombreBene, apellidoPaternoBene: apellidoPBene, apellidoMaternoBene: apellidoMBene}
        SolicitudEstandarAjax("/Iniciales/ConsultarSentenciadoBeneficiario", parametros, LlenaTablaConsultaBeneficiarios);
    }
}

function LlenaTablaConsultaBeneficiarios(data)
{
    if (data.Estatus = EstatusRespuesta.OK)
    {
        var data = data.Data;
    }
    else
    {

    }
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

        $.each(ObjCircuitoTr, function (id, circuitoTr)
        {
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
        var numero = data.Data.length;
       
        if (numero == 1)
        {
            $("#slctJuzgado").html("");
        }
     
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
        var numero = data.Data.length;

        $("#slctJuzgadoTradi").html("");

        if (numero > 1)
        {
            $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));
        }

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
        var causaNucSelect = $("#slcNumero").find('option:selected').val();
        var juzgadoId = $("#slctJuzgado").find('option:selected').val();

        var causaNucText = $('#inpNumero').val();

        if (causaNucSelect == 1)
        {
            var parametros = { idJuzgado: juzgadoId, nuc: causaNucText };
            SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorNUC", parametros, ListarCausas);
        }
        else
        {   
            var parametros = { idJuzgado: juzgadoId, numeroCausa: causaNucText };
            SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorCausa", parametros, ListarCausas);
        } 
    }
    else
    {
        var causaNucText = $("#inpCAUT").val();
        var juzgadoId = $("#slctJuzgadoTradi").find('option:selected').val();

        var parametros = { idJuzgado: juzgadoId, numeroCausa: causaNucText };
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

            if (causas.length == 0)
            $("#contenedorBeneficiario").show();

            if (!ValidarCuasaEnTabla(expediente.IdExpediente))
            {
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
                //Generar Tabla
                dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
            }
            else
            {
                alert("La causa con Numero de Expediente " + (expediente.NumeroCausa == null ? expediente.NUC : expediente.NumeroCausa ) + " ya se encuentra seleccionada");
            }
        }
        else if (data.Estatus == EstatusRespuesta.ERROR)
        {
            alert(data.Mensaje);
        }
        else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA)
        {
            alert(data.Mensaje);
        }
    }
    catch (e)
    {
        alert("Ocurrio una excepción InicialesJs.ListarCausas: " + e.message)
    }
}

function ValidarCuasaEnTabla(id)
{
    var iterarArreglo = causas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (causas[index].id == id)
        {
            return true;
        }
    }
    return false;
}

function EliminarCausa(id) 
{
    var iterarArreglo = causas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == causas[index].id)
        {
            causas.splice(index, 1);
        }
    }

    //Genera nuevamente la tabla
    dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);

    if (causas.length == 0)
        $("#contenedorBeneficiario").hide();
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