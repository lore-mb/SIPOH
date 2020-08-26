
// Llamar Funcion ListarCircuito, pasar parametros vacios y url
SolicitudEstandarAjax("/Iniciales/ObtenerCircuito", "", LitarCircuito);
var parametros = { IdCircuito: 5 };
SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoAcusatorio", parametros, ListarJuzgado);


// Estatus Respuesta
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }

//Solicitud Ajax Get Generico
function SolicitudEstandarAjax(url, parametros, funcion) {
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: parametros,
        beforeSend: function () {
            $("#loading").fadeIn(); //Animacion Load
        },
        success: function (data) {
            funcion(data);
        },
        error: function (xhr) {
            alert('Error Ajax: ' + xhr.statusText);
            $("#loading").fadeOut();
        }
    });
}

function LitarCircuito(data) {
    if (data.Estatus = EstatusRespuesta.OK) {
        const ObjJSON = [data.Data];
        var $select = $('#slctCircuito');
        $.each(ObjJSON, function (id, circuito) {
            $select.append('<option value=' + circuito.Value + '>' + circuito.Name + '</option>');
        });
    } else if (data.Estatus == EstatusRespuesta.ERROR) {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}

function ListarJuzgado(data) {
    if (data.Estatus = EstatusRespuesta.OK) {
        const ObjJSONs = [data.Data];

        /*alert(JSON.stringify(ObjJSONs));*/

        var $select = $('#slctJuzgado');

        $.each(ObjJSONs, function (id, juzgado) {
            for (var i = 0; i < juzgado.length; i++) {
                $select.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
            }
        });
    } else if (data.Estatus == EstatusRespuesta.ERROR) {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}

$(document).ready(function () {
    'use strict';
    

    // Ocultar Elementos en FORM
    $("#divCAU").hide();

    // Muestra u oculta los inputs de NUC y CAUSA segun selección.
    $("#slcNumero").change(function () {
        if ($(this).val() == "2") {
            $('#divCAU').show();
            $('#divNUC').hide();
        } else {
            if ($("#slcNumero").val() == "1") {
                $('#divCAU').hide();
                $('#divNUC').show();
            }
        }
    });

    // DataTable Fuc
    $("#dataTable").dataTable(
        {
            responsive: true,
            search: false,
            language: {
                "decimal": "",
                "emptyTable": "No hay información",
                "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
                "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
                "infoFiltered": "(Filtrado de _MAX_ total entradas)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ Entradas",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "Buscar:",
                "zeroRecords": "Sin resultados encontrados",
                "paginate": {
                    "first": "Primero",
                    "last": "Ultimo",
                    "next": "Siguiente",
                    "previous": "Anterior"
                }
            },
        }
    );

    // Validar Formulario
    var forms = document.getElementsByClassName('needs-validation');
    var validation = Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
            if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
});

