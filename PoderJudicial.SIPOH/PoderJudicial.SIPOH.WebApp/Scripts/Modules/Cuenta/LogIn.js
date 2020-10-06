$(document).ready(function () {
    $("#frm").submit(function (e) {
        var usuario = $("#inputEmail").val();
        var contrasenia = $("#txtPassword").val();

        if ((usuario == null || usuario == "") && (contrasenia == null || contrasenia == "")) {
            alert("Valor introducido no válido");

            //Impide enviar la solicitud al controlor
            e.preventDefault();
        }
    });
});
    
// #region Metodos para ocultar y/o Mostrar contraseña

$('#mostrarContrasena').click(function () {
    var cajaTextoContrasena = $("#txtPassword").attr('type');
    if (cajaTextoContrasena == 'password') {
        $('#mostrarContrasena').removeClass('icon-eye').addClass('icon-eye-blocked');
    } else {
        $('#mostrarContrasena').removeClass('icon-eye-blocked').addClass('icon-eye');
    }
});

// #endregion
