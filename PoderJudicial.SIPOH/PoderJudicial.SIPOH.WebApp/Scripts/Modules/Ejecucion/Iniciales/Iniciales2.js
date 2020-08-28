var estructuraTablaCausas = [{ data: 'nJuzgado', title: 'N° Juzgado' },{ data: 'causaNuc', title: 'Causa|Nuc' },{ data: 'ofendido', title: 'Ofendido (s)' },{ data: 'inculpado', title: 'Inculpado (s)' },{ data: 'delito', title: 'Delitos (s)' },{ data: 'eliminar', title: 'Quitar' }];
var dataTable = null;
var causas = [];

function PintarTabla()
{
    dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
}

function ConsultarCausas(tradicional)
{
    var juzgadoId = 198;

    if (!tradicional)
    {
        var nuc = "14-2016-963258";
        var parametros = { idJuzgado: juzgadoId, nuc: nuc };
        SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorNUC", parametros, ListarCausas);
    }
    else
    {
        var numCusa = "";
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

           causa.id = 1;
           causa.nJuzgado = expediente.NombreJuzgado;
           causa.causaNuc = expediente.NumeroCausa == null ? expediente.NUC : expediente.NumeroCausa;
           causa.ofendido = expediente.Ofendidos;
           causa.inculpado = expediente.Inculpados;
           causa.delito = expediente.Delitos;
           causa.eliminar = "BOTON";
           //Agrega Causa al Arreglo de Cuasas
           causas.push(causa);

           dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false); 

           var causa = new Object();

           causa.id = 2;
           causa.nJuzgado = expediente.NombreJuzgado;
           causa.causaNuc = expediente.NumeroCausa == null ? expediente.NUC : expediente.NumeroCausa;
           causa.ofendido = expediente.Ofendidos;
           causa.inculpado = expediente.Inculpados;
           causa.delito = expediente.Delitos;
           causa.eliminar = "BOTON";
           //Agrega Causa al Arreglo de Cuasas
           causas.push(causa);


           dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
           dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
           dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
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
    dtabla = GeneraTablaDatos(dtabla, nombreTabla, causas, estructuraTablaCausas, false, false, false);
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
            }
        }
    });
}