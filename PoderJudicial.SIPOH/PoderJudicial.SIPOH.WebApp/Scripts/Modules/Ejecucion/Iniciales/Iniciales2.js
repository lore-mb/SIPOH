var estructuraTablaCausas =
    [{ data: 'nJuzgado', title: 'N° Juzgado' },
     { data: 'causaNuc', title: 'Causa|Nuc' },
     { data: 'ofendido', title: 'Ofendido (s)' },
     { data: 'inculpado', title: 'Inculpado (s)' },
     { data: 'delito', title: 'Delitos (s)' },
     { data: 'eliminar', title: 'Quitar' }];

var causas = [];
var dtabla = null;

function PintarTabla()
{
    var nombreTabla = "dataTable";

    for (var index = 0; index < 2; index++) {
        var causa = new Object();
        causa.id = index;
        causa.nJuzgado = "Juzgado de Ejecucion Pro Pachuca";
        causa.causaNuc = "0027/2017";
        causa.ofendido = "ANTOLIN ALFREDO ORTIZ GUTIERREZ, JOSE SOCORRO HERNANDEZ VELAZQUEZ";
        causa.inculpado = "FERNANDO FUENTES CRUZ, SERGIO ALBERTO VALENTIN MONZALVO";
        causa.delito = "ASALTO AGRAVADO, ROBO";
        causa.eliminar = '<a href= "#" onclick="imprimirAlert(' + index + ')"><div class="btn btn-danger btn-sm"><i class="fa fa-close"></i></div></a>'
        causas.push(causa);
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

//$("#dataTable").dataTable(
//     {
//        searching: false,
//        language: {
//            "decimal": "",
//            "emptyTable": "No hay información",
//            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
//            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
//            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
//            "infoPostFix": "",
//            "thousands": ",",
//            "lengthMenu": "Mostrar _MENU_ Entradas",
//            "loadingRecords": "Cargando...",
//            "processing": "Procesando...",
//            "search": "Buscar:",
//            "zeroRecords": "Sin resultados encontrados",
//            "paginate": {
//                "first": "Primero",
//                "last": "Ultimo",
//                "next": "Siguiente",
//                "previous": "Anterior"
//            }
//        },
//   }
//);