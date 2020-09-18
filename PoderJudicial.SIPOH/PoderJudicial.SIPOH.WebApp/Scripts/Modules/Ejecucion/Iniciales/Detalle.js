
$(document).ready(function ()
{
    //Funcionalidad para abrir Modal
    $("#btnGeneraSello").click(function ()
    {
        ImprimirSello();
    });

    $('#dataTableCausas').DataTable({
        searching: false,
        ordering: false,
        lengthChange: false,
        responsive: true,
        "language":
        {
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
        }
    });

    $('#dataTableTocas').DataTable({
        searching: false,
        ordering: false,
        paging: false,
        info: false,
        responsive: true,
    });

    $('#dataTableAmparos').DataTable({
        searching: false,
        ordering: false,
        paging: false, 
        info: false,
        responsive: true,
    });

    $('#dataTableAnexos').DataTable({
        searching: false,
        ordering: false,
        paging: false,
        info: false,
        responsive: true,
    });
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