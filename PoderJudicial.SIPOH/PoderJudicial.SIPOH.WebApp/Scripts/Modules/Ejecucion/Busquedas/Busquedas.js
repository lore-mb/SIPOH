// #region Varaibles Globales
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var Nuc = null;


// #region FUNCIONALIDADES DE CARGADO
$(document).ready(function () {
    TablaPartesCausa = Consumir_DataTable(TablaPartesCausa, "_DataTablaPartesCausa", partescausas, estructuraTablaPartesCausa, false, false, false);

});

//Estructura para data tables 
var estructuraTablaPartesCausa = [{ data: 'NoEjecucion', title: 'N° Ejecucion', className: "text-center" }, { data: 'JuzgEjec', title: 'Juzgado Ejecucion', className: "text-center" }, { data: 'Nombre', title: 'Nombre (s)', className: "text-center" }, { data: 'Type', title: 'Tipo', className: "text-center" }, { data: 'DSolictante', title: 'Detalle Solicitante', className: "text-center" }, { data: 'Fecha', title: 'Fecha', className: "text-center" }, { data: 'Solictud', title: 'Solictud', className: "text-center" }, { data: 'Beneficiario', title: 'Beneficiario', className: "text-center" }, { data: 'TypeExpediente', title: 'Tipo Expediente', className: "text-center" }];
var partescausas = [];

//var estructuraTablaSentenBene = [{ data: 'NoEjecucion', title: 'N°Ejecucion', className: "text-center" }, { data: 'DetalleSolic', title: 'Detalle del Solicitante', className: "text-center" }, { data: 'fecha', title: 'Fecha', className: "text-center" }, { data: 'Solicitud', title: 'Solicitud', className: "text-center" }, { data: 'Beneficiario', title: 'Beneficiario', className: "text-center" }, { data: 'TypeExpe', title: 'Tipo Expediente', className: "text-center" }, { data: 'Detalle', title: '', className: "text-center" }];
//var beneficarios = [];

//var estructuraTablaNumCausa = [{ data: 'NoEjecucion', title: 'No. Ejecución', className: "text-center" }, { data: 'JuzgEjec', title: 'Juzgado', className: "text-center" }, { data: 'nombreBeneficiario', title: 'Nombre (s)', className: "text-center" }, { data: 'DetalleSolic', title: 'Detalle del Solicitante', className: "text-center" }, { data: 'fecha', title: 'Fecha', className: "text-center" }, { data: 'Solicitud', title: 'Solicitud', className: "text-center" }, { data: 'Beneficiario', title: 'Beneficiario', className: "text-center" }, { data: 'TypeExpe', title: 'Tipo Expediente', className: "text-center" }, { data: 'Detalle', title: '', className: "text-center" }];
//var Numcausa = [];

//var estructuraTablaNUC = [{ data: 'NoEjecucion', title: 'No. Ejecución', className: "text-center" }, { data: 'JuzgEjec', title: 'Juzgado', className: "text-center" }, { data: 'nombreBeneficiario', title: 'Nombre (s)', className: "text-center" }, { data: 'DetalleSolic', title: 'Detalle del Solicitante', className: "text-center" }, { data: 'fecha', title: 'Fecha', className: "text-center" }, { data: 'Solicitud', title: 'Solicitud', className: "text-center" }, { data: 'Beneficiario', title: 'Beneficiario', className: "text-center" }, { data: 'TypeExpe', title: 'Tipo Expediente', className: "text-center" }, { data: 'Detalle', title: '', className: "text-center" }];
//var Nuc = [];

//var estructuraTablaSolicitante = [{ data: 'NoEjecucion', title: 'No. Ejecución', className: "text-center" }, { data: 'JuzgEjec', title: 'Juzgado', className: "text-center" }, { data: 'nombreBeneficiario', title: 'Nombre (s)', className: "text-center" }, { data: 'DetalleSolic', title: 'Detalle del Solicitante', className: "text-center" }, { data: 'fecha', title: 'Fecha', className: "text-center" }, { data: 'Solicitud', title: 'Solicitud', className: "text-center" }, { data: 'Beneficiario', title: 'Beneficiario', className: "text-center" }, { data: 'TypeExpe', title: 'Tipo Expediente', className: "text-center" }, { data: 'Detalle', title: '', className: "text-center" }];
//var Solicitante = [];

//var estructuraTablaDetalleSolicitante = [{ data: 'NoEjecucion', title: 'No. Ejecución', className: "text-center" }, { data: 'JuzgEjec', title: 'Juzgado', className: "text-center" }, { data: 'nombreBeneficiario', title: 'Nombre (s)', className: "text-center" }, { data: 'DetalleSolic', title: 'Detalle del Solicitante', className: "text-center" }, { data: 'fecha', title: 'Fecha', className: "text-center" }, { data: 'Solicitud', title: 'Solicitud', className: "text-center" }, { data: 'Beneficiario', title: 'Beneficiario', className: "text-center" }, { data: 'TypeExpe', title: 'Tipo Expediente', className: "text-center" }, { data: 'Detalle', title: '', className: "text-center" }];
//var DSolicitante = [];


var TablaPartesCausa = null;
//var TablaSentenciadoBeneficiario = null;
//var TablaNumeroCausa = null;
//var TablaNUC = null;
//var TablaSolicitante = null;
//var TablaDetalleSolicitante = null;


// #region FUNCIÓN GENERAL: DataTable
function Consumir_DataTable(tabla, idTablaHtml, datos, estructuraTabla, ordering, searching, lengthChange) {
    if (tabla != null) {
        tabla.destroy();
        $("#" + idTablaHtml).empty();

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
}





