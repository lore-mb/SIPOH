

$(document).ready(function () {
    var NombreUser = $("#login").val();

    $.notify({
        icon: 'icon-bell',
        title: 'SISTEMA SIPOH:',
        message: "Bienvenido (a) <label class='font-weight-bold'>" + NombreUser + "</label> al Sistema de Informacion Penal Acusatorio y Oral del Estado de Hidalgo. " ,
    }, {
        type: 'success',
        placement: {
            from: "top",
            align: "right"
        },
        time: 2000,
        animate: {
            enter: 'animated fadeInRight',
            exit: 'animated fadeOutRight'
        }
    });

})
