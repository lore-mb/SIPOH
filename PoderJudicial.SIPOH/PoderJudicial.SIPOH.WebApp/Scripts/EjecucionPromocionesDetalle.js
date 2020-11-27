
$(document).ready(function () {

    var proceso = $("#IdProceso").val();
    proceso = proceso.toLowerCase();
    proceso = JSON.parse(proceso);

    var ejecucion = $("#IdEjecucion").val();
    ejecucion = ejecucion.toLowerCase();
    ejecucion = JSON.parse(ejecucion);

    var anexos = JSON.parse($("#IdAnexos").val().toLowerCase());

    //Funcionalidad para abrir Modal
    $("#btnGeneraSello").click(function () {
        ImprimirSello();
    });

    $('#dataTableAnexosDetalle').DataTable({
        searching: false,
        ordering: false,
        paging: false,
        info: false,
        "language":
        {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": (anexos ? "Ocurrio un error al consultar la informacion de esta tabla" : "Ningún dato disponible en esta tabla"),
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
            }
        },
        responsive: true
    });

    if (!proceso) {
        if (ejecucion) {
            var mensaje = $("#IdMensajeError").val();
            Alerta(mensaje, "large");
        }
        else {
            var mensaje = $("#IdMensajeError").val();
            mensaje = mensaje + ", precione el boton <b>Actualizar</b> para generar el detalle nuevamente, si continua el problema consulte con soporte.";
            Alerta(mensaje, "large");
        }
    }
});

//Funcion para abrir ventana que imprimira sello
function ImprimirSello() {
    var sello = "";

    var selloArregloSeccion1 = [JustificarEspacio("TRIBUNAL SUPERIOR", 35),
    JustificarEspacio("DE JUSTICIA", 35),
    JustificarEspacio("DEL ESTADO DE HIDALGO", 35),
    JustificarEspacio("ATENCION CIUDADANA", 35),
    JustificarEspacio("SENTENCIA EJECUTORIADA", 35),
    JustificarEspacio("PROMOCIÓN", 35),
    JustificarEspacio("------------------", 35)];

    var juzgadoSello = RemoverAcentos($("#juzgadoSello").text());
    var juzgadoArreglo = JustificarEspacioMaximo(juzgadoSello, 35);

    selloArregloSeccion1.forEach(function (valor, indice, array) {
        sello = sello + valor;
    });

    juzgadoArreglo.forEach(function (valor, indice, array) {
        sello = sello + valor;
    });
    
    var numeroEjecucionSello = $("#numeroEjecucionSello").text() + "<br>";
    var folioSello = $("#folioSello").text() + "<br>";
    var fechaEjecucion = $("#fechaIngreso").text() + "<br>";

    var selloArregloSeccion2 = [JustificarEspacio("------------------", 35),
        numeroEjecucionSello,
        folioSello,
        fechaEjecucion];

    selloArregloSeccion2.forEach(function (valor, indice, array) {
        sello = sello + valor;
    });

    sello = sello + "  <br>";

    var totalAnexos = $("#total").text();
    totalAnexos = parseInt(totalAnexos);

    var cantidadTotal = 0;
    var datosAnexo = [];

    for (var i = 0; i < totalAnexos; i++) {
        var descripcion = RemoverAcentos($("#descripcion" + i).text());
        var cantidad = $("#cantidad" + i).text();

        cantidadTotal = cantidadTotal + parseInt(cantidad);

        var objeto = { descripcion: descripcion, cantidad: cantidad }
        datosAnexo.push(objeto);
    }

    var objeto = { descripcion: "TOTAL", cantidad: cantidadTotal }
    datosAnexo.push(objeto);

    var selloArregloSeccion3 = GeneraAnexos(datosAnexo, 35);

    selloArregloSeccion3.forEach(function (valor, indice, array) {
        sello = sello + valor;
    });

    //Necesario para Google Chrome
    sello = "  <BR>" + sello + "<BR>   <BR>"

    var ventana = window.open();
    ventana.document.write("<html><head><title></title>");
    ventana.document.write("<script src=\"/Scripts/Lib_JQuery.min.js\"></script>");
    ventana.document.write("<script src=\"/Scripts/EjecucionInicialesSello.js\"></script>");
    ventana.document.write("</head><body>");
    ventana.document.write("<PRE>" + sello + "</PRE>");
    ventana.document.write("</body></html>");
    ventana.document.close();
}

function Alerta(mensaje, tamanio = null) {
    tamanio = tamanio == null ? "small" : tamanio;
    bootbox.alert({
        title: "<h3>¡Atención!</h3>",
        message: mensaje,
        buttons:
        {
            ok: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-success'
            }
        },
        size: tamanio
    });
}

function JustificarEspacio(cadena, cantidad) {
    var totalCadena = cadena.length;
    var resto = cantidad - totalCadena;
    var divisor = resto % 2;

    if (divisor == 0) {
        var rest = resto / 2;
        return ' '.repeat(rest) + cadena + '<br>';
    }
    else {
        var izquierda = (resto - divisor) / 2;
        var derecha = izquierda + divisor;
        return ' '.repeat(izquierda) + cadena + '<br>';
    }
}

function JustificarEspacioMaximo(cadena, maximo) {
    var arreglo = cadena.split(' ');
    var total = arreglo.length;
    var cadenaNew = "";
    var cadenaAnte = "";
    var newArreglo = [];

    for (var i = 0; i < total; i++) {
        cadenaNew = cadenaNew + (i == 0 ? arreglo[i] : " " + arreglo[i]);

        if (cadenaNew.length < maximo) {
            cadenaAnte = cadenaNew;

            if ((i + 1) == total) {
                newArreglo.push(JustificarEspacio(cadenaAnte, maximo));
            }
        }

        if (cadenaNew.length >= maximo) {
            newArreglo.push(JustificarEspacio(cadenaAnte, maximo));
            cadenaAnte = arreglo[i];
            cadenaNew = cadenaAnte;

            if ((i + 1) == total) {
                newArreglo.push(JustificarEspacio(cadenaNew, maximo));
            }
        }
    }

    return newArreglo;
}

function GeneraAnexos(datos, cantidadMaxima) {
    var anexos = [];
    var total2 = datos.length;

    for (var a = 0; a < total2; a++) {
        var arreglo = datos[a].descripcion.split(' ');
        var total = arreglo.length;
        var cadenaNew = "";
        var cadenaAnte = "";


        for (var i = 0; i < total; i++) {
            cadenaNew = cadenaNew + (i == 0 ? arreglo[i] : " " + arreglo[i]);

            if (cadenaNew.length < cantidadMaxima) {
                cadenaAnte = cadenaNew;

                if ((i + 1) == total) {
                    anexos.push(AgregarPuntos(cadenaAnte, cantidadMaxima, datos[a].cantidad));
                }
            }

            if (cadenaNew.length >= cantidadMaxima) {
                anexos.push(cadenaAnte + '<br>');
                cadenaAnte = arreglo[i];
                cadenaNew = cadenaAnte;

                if ((i + 1) == total) {
                    anexos.push(AgregarPuntos(cadenaNew, cantidadMaxima, datos[a].cantidad));
                }
            }
        }
    }

    return anexos;
}

function AgregarPuntos(cadena, cantidad, cantidadAnexo) {
    var totalCadena = cadena.length;
    var resto = cantidad - totalCadena;
    var lengCantidadAnexo = cantidadAnexo.toString().length;
    var puntosLengt = resto - lengCantidadAnexo;
    puntosLengt = puntosLengt - 2;

    var res = cadena + ' ' + '.'.repeat(puntosLengt) + ' ' + cantidadAnexo + '<br>';

    return res;
}

function RemoverAcentos(str) {
    return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
}
