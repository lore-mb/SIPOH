

$(document).ready(function () {
    // Variables
    var NombreUser = $("#login").val();

    // Bienvenido al Sistema
	BootstrapNotifyLoggedIn("SISTEMA SIPO", "Bienvenido (a) <label class='font-weight-bold'>" + NombreUser + "</label> al Sistema de Informacion Penal Acusatorio y Oral del Estado de Hidalgo. ", "success", "bottom", "right");
    	
})

// Funcion Generaral de Notificación
function BootstrapNotifyLoggedIn(titlemessage, messagenotify, typecolor, position, aligndisplay) {

	$.notify({
		icon: 'icon-bell',
		title: titlemessage,
		message: messagenotify,
	}, {
		type: typecolor,
		placement: {
			from: position,
			align: aligndisplay
		},
		time: 1500,
		animate: {
			enter: 'animated fadeInRight',
			exit: 'animated fadeOutRight'
		}
	});
}


   