// #region Varaibles Globales
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var estructuraTablaNumeroEjecucion = [{ data: 'noEjecucion', title: 'N° Ejecución', className: "text-center" }, { data: 'juzgadoEjecucion', title: 'Juzgado de Ejecución', className: "text-center" }, { data: 'fecha', title: 'Fecha Ejecución', className: "text-center" }, { data: 'beneficiario', title: 'Beneficiario', className: "text-center" }, { data: 'tipoExpediente', title: 'Tipo Expediente', className: "text-center" }, { data: 'causas', title: 'Detalle', className: "text-center" }];
var numeroEjecucionDatos = [];
var tablaNumeroEjecucion = null;

$(document).ready(function ()
{
    //Pinta la tabla en el ejecucion
    tablaNumeroEjecucion = GeneraTablaDatos(tablaNumeroEjecucion, "dataTableNumeroEjecucion", numeroEjecucionDatos, estructuraTablaNumeroEjecucion, false, false, false);
});

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

