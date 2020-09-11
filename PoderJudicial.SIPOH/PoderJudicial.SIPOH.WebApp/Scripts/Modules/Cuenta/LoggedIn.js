

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

function BootstrapNotifyWork() {
	$.notify({
		// options
		icon: 'glyphicon glyphicon-warning-sign',
		title: 'Bootstrap notify',
		message: 'Turning standard Bootstrap alerts into "notify" like notifications',
		url: 'https://github.com/mouse0270/bootstrap-notify',
		target: '_blank'
	}, {
		// settings
		element: 'body',
		position: null,
		type: "info",
		allow_dismiss: true,
		newest_on_top: false,
		showProgressbar: true,
		placement: {
			from: "top",
			align: "right"
		},
		offset: 20,
		spacing: 10,
		z_index: 1031,
		delay: 5000,
		timer: 3000,
		url_target: '_blank',
		mouse_over: null,
		animate: {
			enter: 'animated fadeInDown',
			exit: 'animated fadeOutUp'
		}
	});
}
   