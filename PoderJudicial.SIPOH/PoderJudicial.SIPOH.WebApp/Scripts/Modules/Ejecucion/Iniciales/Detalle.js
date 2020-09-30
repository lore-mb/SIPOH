
$(document).ready(function ()
{
    var proceso = $("#IdProceso").val();
    proceso = proceso.toLowerCase();
    proceso = JSON.parse(proceso);

    var ejecucion = $("#IdEjecucion").val();
    ejecucion = ejecucion.toLowerCase();
    ejecucion = JSON.parse(ejecucion);

    var causas = JSON.parse($("#IdCausas").val().toLowerCase());
    var tocas = JSON.parse($("#IdTocas").val().toLowerCase());
    var anexos = JSON.parse($("#IdAnexos").val().toLowerCase());
    var amparos = JSON.parse($("#IdAmparos").val().toLowerCase());

    //Funcionalidad para abrir Modal
    $("#btnGeneraSello").click(function ()
    {
        ImprimirSello();
    });

    $('#dataTableExpedienteDetalle').DataTable({
        searching: false,
        ordering: false,
        lengthChange: false,
        responsive: true,
        "language":
        {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": (causas ? "Ocurrio un error al consultar la informacion de esta tabla" : "Ningún dato disponible en esta tabla"),
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

    $('#dataTableTocasDetalle').DataTable({
        searching: false,
        ordering: false,
        paging: false,
        info: false,
        "language":
        {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": (tocas ? "Ocurrio un error al consultar la informacion de esta tabla" : "Ningún dato disponible en esta tabla"),
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
        },
        responsive: true
    });

    $('#dataTableAmparosDetalle').DataTable({
        searching: false,
        ordering: false,
        paging: false, 
        info: false,
        "language":
        {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": (amparos ? "Ocurrio un error al consultar la informacion de esta tabla" : "Ningún dato disponible en esta tabla"),
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
        },
        responsive: true
    });

    $('#dataTableAnexosDetalle').DataTable({
        searching: false,
        ordering: false,
        paging: false,
        info: false,
        "language":
        {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": (anexos ? "Ocurrio un error al consultar la informacion de esta tabla" : "Ningún dato disponible en esta tabla"),
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
        },
        responsive: true
    });

    if (!proceso)
    {
        if (ejecucion)
        {
            var mensaje = $("#IdMensajeError").val();
            Alerta(mensaje, "large");
        }
        else
        {
            var mensaje = $("#IdMensajeError").val();
            mensaje = mensaje + ", precione el boton <b>Actualizar</b> para generar el detalle nuevamente, si continua el problema consulte con soporte.";
            Alerta(mensaje, "large");
        }
    }
});

//Funcion para abrir ventana que imprimira sello
function ImprimirSello()
{
    var printContents = document.getElementById('sello').innerHTML;
    var ventana = window.open();
    ventana.document.write("<html><head><title></title>");
    ventana.document.write("<link rel=\"stylesheet\" href=\"/Content/Master/Site/sello.css\" type=\"text/css\"/>");
    ventana.document.write("<script src=\"/Scripts/Master/Jquery/jquery.min.js\"></script>");
    ventana.document.write("<script src=\"/Scripts/Modules/Ejecucion/Iniciales/Sello.js\"></script>");       
    ventana.document.write("</head><body>");
    ventana.document.write(printContents);
    ventana.document.write("</body></html>");
    ventana.document.close(); 
}

function Alerta(mensaje, tamanio = null)
{
    tamanio = tamanio == null ? "small" : tamanio;

    bootbox.alert({
        title: "<h3>¡Atención!</h3>",
        message: mensaje,
        buttons:
        {
            ok: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-success'
            }
        },
        size: tamanio
    });
}