$(document).ready(function ()
{
    $("#frm").submit(function (e)
    {
        //Logica para validar campos de LogIn
        var usuario = $("#inputEmail").val();
        var contrasenia = $("#txtPassword").val();

        if ((usuario == null || usuario == "") && (contrasenia == null || contrasenia == ""))
        {
            alert("Valor introducido no válido");

            //Impide enviar la solicitud al controlor
            e.preventDefault();
        }
    });

    
//    //CheckBox mostrar contraseña
//    $('#ShowPassword').click(function ()
//    {
//        $('#Password').attr('type', $(this).is(':checked') ? 'text' : 'password');
//    });
//});


//function mostrarPassword()
//{
//    var cambio = document.getElementById("txtPassword");
//    if (cambio.type == "password")
//    {
//        cambio.type = "text";
//        $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
//    }
//    else
//    {
//        cambio.type = "password";
//        $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
//    }
//}

