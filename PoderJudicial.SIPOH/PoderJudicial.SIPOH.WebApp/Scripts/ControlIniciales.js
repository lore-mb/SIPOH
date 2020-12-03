$(document).ready(function ()
{
    ValidarFormularios();
    CargarDatePicker();
});


function ValidarFormularios() {
    var forms = document.getElementsByClassName('needs-validation');
    Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
            var id = form.id;
            event.preventDefault();
            event.stopPropagation();
            form.classList.add('was-validated');
            if (form.checkValidity() === true && id == "FrmAmparoConCausa") {
                RegistrarAmparoConCausa();
            }
            if (form.checkValidity() === true && id == "FrmAmparoSinCausa") {
                RegistrarAmparoSinCausa();
            }
        }, false);
    });
}

function RegistrarAmparoConCausa()
{

    MensajeNotificacionGuardar("Desea guardar la informacion registrada?", null, DatosGuardados);

    function DatosGuardados ()
    {
        $("#loading").fadeIn(2500);
        $("#loading").fadeOut();
    }
}

function MensajeNotificacionGuardar(mensaje, tamanio, funcion) {
    bootbox.confirm({
        title: "<h2>" +"<i class='icon-question text-success'></i>"+" Verificacion de datos a guardar</h2>",
        message: mensaje,
        buttons: {
            confirm: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-success'
            },
            cancel: {
                label: '<i class="fa fa-times"></i> Cancelar',
                className: 'btn btn-outline-secondary'
            }
        },
        callback: function (result) {
            if (result) {
                funcion();
            }
        },
        size: tamanio
    });
}

function CargarDatePicker()
{
    $('#inpFechaRecepcion').datetimepicker({
        format: 'YYYY-MM-DD'
    });
    $('#inpFechaDevolucion').datetimepicker({
        format: 'YYYY-MM-DD'
    });
}