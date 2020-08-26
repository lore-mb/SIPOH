// Ocultar Elementos en FORM
$("#divCAU").hide();

$(document).ready(function () {
    'use strict';

    // Parametros para funcion generica Ajax
    var parametros = { IdCircuito: 5 };

    // Estatus Respuesta
    var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }

    // Solicitud Ajax Get Generico
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

    // Listar Circuito Acusatorio
    SolicitudEstandarAjax("/Iniciales/ObtenerCircuito", "", LitarCircuitoAcusatorio);
    function LitarCircuitoAcusatorio(data) {
        if (data.Estatus = EstatusRespuesta.OK) {
            const objJSONCircuito = [data.Data];
            var $selectAc = $('#slctCircuitoAc');
            var $selectTr = $('#slctCircuitoTr');
            $.each(objJSONCircuito, function (id, circuito) {
                $selectAc.append('<option value=' + circuito.Value + '>' + circuito.Text + '</option>');
                $selectTr.append('<option value=' + circuito.Value + '>' + circuito.Text + '</option>');
            });
        } else if (data.Estatus == EstatusRespuesta.ERROR) {
            customNotice(data.Mensaje, "Error:", "error", 3350);
        }
    }

    // Listar Juzgado de Procedencia en Combo
    SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoAcusatorio", parametros, ListarJuzgado);
    function ListarJuzgado(data) {
        if (data.Estatus = EstatusRespuesta.OK) {
            const objJSONJuzgado = [data.Data];
            var $select = $('#slctJuzgado');
            $.each(objJSONJuzgado, function (id, juzgado) {
                for (var i = 0; i < juzgado.length; i++) {
                    $select.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
                }
            });
        } else if (data.Estatus == EstatusRespuesta.ERROR) {
            customNotice(data.Mensaje, "Error:", "error", 3350);
        }
    }

    // Listar Distrito de procedencia
    SolicitudEstandarAjax("", parametros, ListarDistrito);
    function ListarDistrito(data) {
        if (data.Estatus = EstatusRespuesta.OK) {

        } else if (data.Estatus == EstatusRespuesta.ERROR) {
            customNotice(data.Mensaje, "Error:", "error", 3350);
        }
    }

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

    // DataTable
    $("#dataTable").dataTable(
        {
            responsive: true,
            searching: false,
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

    // Validar Formulario Juzgado Acusatorio
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

