// Mostrar group numero NUC - CAUSA

$("#divCAU").hide();
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


$(document).ready(function () {
    'use strict';

    $("#dataTable").dataTable(
        {
            responsive: true,
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

