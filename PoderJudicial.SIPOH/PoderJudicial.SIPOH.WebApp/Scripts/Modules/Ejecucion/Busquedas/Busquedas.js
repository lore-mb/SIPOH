var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }

var estructuraTablaNumeroEejecucionPartes =
    [{ data: 'noEjecucion', title: 'N° Ejecución', className: "text-center" },
     { data: 'juzgadoEjecucion', title: 'Juzgado de Ejecución', className: "text-center" },
     { data: 'fecha', title: 'Fecha Ejecución', className: "text-center" },
     { data: 'beneficiario', title: 'Beneficiario', className: "text-center" },   
     { data: 'tipoExpediente', title: 'Tipo Expediente', className: "text-center" },
     { data: 'parteRelacionada', title: 'Parte Causas', className: "text-center" },
     { data: 'parteRelacionada', title: 'Tipo Parte', className: "text-center" },
     { data: 'causas', title: 'Detalle', className: "text-center" }];
var estructuraTablaNumeroEjecucion = [{ data: 'noEjecucion', title: 'N° Ejecución', className: "text-center" }, { data: 'juzgadoEjecucion', title: 'Juzgado de Ejecución', className: "text-center" }, { data: 'fecha', title: 'Fecha Ejecución', className: "text-center" }, { data: 'beneficiario', title: 'Beneficiario', className: "text-center" }, { data: 'tipoExpediente', title: 'Tipo Expediente', className: "text-center" }, { data: 'causas', title: 'Detalle', className: "text-center" }];
var numeroEjecucionDatos = [];
var tablaNumeroEjecucion = null;

$(document).ready(function ()
{
    //Pinta la tabla en el ejecucion
    tablaNumeroEjecucion = GeneraTablaDatos(tablaNumeroEjecucion, "dataTableNumeroEjecucion", numeroEjecucionDatos, estructuraTablaNumeroEjecucion, false, false, false);

    //Funcionalidad de Elementos al Cargado
    ElementosAlCargado();
});

function ElementosAlCargado()
{
    //Deshabilita option para juzgado acusatorio
    $("#slctJuzgadoPorDistritos").prop('disabled', true);
    $("#inpCausa").prop('disabled', true);

    $('#slctDistrito').change(function ()
    {
        var idDistrito = $("#slctDistrito").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idDistrito != "" && idDistrito != null)
        {
            $("#slctJuzgadoPorDistritos").prop('disabled', false);
            RecuperaJuzgadosAcusatorioTradicional(idDistrito);
        }
        else
        {
            $("#slctJuzgadoPorDistritos").prop('disabled', true);
            $("#slctJuzgadoPorDistritos").html("");
            $("#slctJuzgadoPorDistritos").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));
            $("#inpCausa").prop('disabled', true);
            $("#inpCausa").val("");
        }
    });

    $('#slctJuzgadoPorDistritos').change(function ()
    {
        var idJuzgado = $("#slctJuzgadoPorDistritos").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idJuzgado != "" && idJuzgado != null)
        {
            $("#inpCausa").prop('disabled', false);    
        }
        else
        {
            $("#inpCausa").prop('disabled', true);
            $("#inpCausa").val("");
        }
    });

    $("#inpNuc").prop('disabled', true);

    $('#slctJuzgadoAcusatorio').change(function ()
    {
        var idJuzgado = $("#slctJuzgadoAcusatorio").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idJuzgado != "" && idJuzgado != null)
        {
            $("#inpNuc").prop('disabled', false);
        }
        else
        {
            $("#inpNuc").prop('disabled', true);
            $("#inpNuc").val("");
        }
    });


    //Funcionalidad para validar formularios
    var forms = document.getElementsByClassName('needs-validation');

    Array.prototype.filter.call(forms, function (form)
    {
        form.addEventListener('submit', function (event)
        {
            var id = form.id;

            event.preventDefault();
            event.stopPropagation();

            if (id == "formPartesCausa" || id == "formBeneficiario")
            {
                //Valida solo campos especificos del formulario
                var validateGroup = form.getElementsByClassName('validate-me');

                for (var i = 0; i < validateGroup.length; i++)
                {
                    validateGroup[i].classList.add('was-validated');
                }
            }
            else
            {
               //Valida todos los campos del formulario
               form.classList.add('was-validated');
            }

            if (form.checkValidity() === true && id == "formPartesCausa")
            {
                BuscarEjecucionPorPartesBeneficiarios(true);
            }

            if (form.checkValidity() === true && id == "formBeneficiario")
            {
                BuscarEjecucionPorPartesBeneficiarios(false);
            }

            if (form.checkValidity() === true && id == "formCausasEjecucion")
            {
                alert("entro");
            }

            if (form.checkValidity() === true && id == "formNucEjecucion")
            {
                alert("entro");
            }

            if (form.checkValidity() === true && id == "formSolicitanteEjecucion")
            {
                alert("entro");
            } 

            if (form.checkValidity() === true && id == "formDetalleSolicitanteEjecucion")
            {
                alert("entro");
            } 

        }, false);
    });
}

function RecuperaJuzgadosAcusatorioTradicional(idDistrito)
{
    if (idDistrito != null)
    {
        var parametros = { idDistrito: idDistrito }
        SolicitudEstandarAjax("/Busquedas/ObtenerJuzgadosPorDistrito", parametros, GenerarOptionSelectJuzgadoPorDistrito);
    }
    else
    {
        alert("Error al obtener los datos");
    }
}

function GenerarOptionSelectJuzgadoPorDistrito(data)
{
    if (data.Estatus == EstatusRespuesta.OK)
    {
        var numero = data.Data.length;

        $("#slctJuzgadoPorDistritos").html("");

        if (numero > 1)
        {
            $("#slctJuzgadoPorDistritos").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));
        }

        const ObjJuzgadoTra = [data.Data];
        var $slcTradi = $('#slctJuzgadoPorDistritos');

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

function BuscarEjecucionPorPartesBeneficiarios(partes)
{
    var idNombre = partes ? "inpNombreParte" : "inpNombreBeneficiario";
    var idApellidoPaterno = partes ? "inpApellidoPaternoParte" : "inpApellidoPaternoBeneficiario";
    var idApellidoMaterno = partes ? "inpApellidoMaternoPartes" : "inpApellidoMaternoBeneficiario";

    var nombre = $('#' + idNombre).val();
    var apellidoPaterno = $('#' + idApellidoPaterno).val();
    var apellidoMaterno = $('#' + idApellidoMaterno).val();

    var parametros = { nombre: nombre, apellidoPaterno: apellidoPaterno, apellidoMaterno: apellidoMaterno };

    if (partes)
    {
        SolicitudEstandarAjax("/Busquedas/BusquedaPartesCausa", parametros, ListarNumerosDeEjecucion);
    }
    else
    {
        SolicitudEstandarAjax("/Busquedas/BusquedaPorBeneficiario", parametros, ListarNumerosDeEjecucion);
    } 
}

function ListarNumerosDeEjecucion(data)
{
    if (data.Estatus == EstatusRespuesta.OK)
    {
        var partesDeCausaEjecucion = data.Data.busquedaPartes;

    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {

    }
}

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
            },
        },
        drawCallback: function (settings)
        {
            $('[data-toggle="tooltip"]').tooltip();
        }
    });
}

function SolicitudEstandarAjax(url, parametros, functionCallbackSuccess, functionCallbackError = null)
{
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: parametros,
        beforeSend: function ()
        {
            // $("#loading").fadeIn(); //Animacion Load
        },
        success: function (data)
        {
            functionCallbackSuccess(data);
        },
        error: function (jqXHR)
        {
            $("#loading").fadeOut();

            var mensaje = '';

            if (jqXHR.status === 0)
            {
                mensaje = 'No esta conectado, verifique su conexión.';
            }
            else if (jqXHR.status == 404)
            {
                mensaje = 'No se encontró la página solicitada, ERROR:404';
            }
            else if (jqXHR.status == 500)
            {
                mensaje = "Error interno del servidor, ERROR:500";
            }
            else if (exception === 'timeout')
            {
                mensaje = 'Error de Time Out.';
            }
            else if (exception === 'abort')
            {
                mensaje = 'Solicitud AJAX Abortada.';
            }
            else
            {
                mensaje = 'Error no detectado : ' + jqXHR.responseText;
            }

            if (functionCallbackError == null)
            {
                Alerta(mensaje, "large", "Error ");
            }

            if (functionCallbackError != null)
            {
                var data = mensaje;
                functionCallbackError(data);
            }
        }
    });
}


