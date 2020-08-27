// #region Varaibles Globales
var idcircuito = null;
var saludo = null;
// #endregion

$(document).ready(function () {

    // #region Listar Circuito
    SolicitudEstandarAjax("/Iniciales/ObtenerCircuito", "", ListarCircuito);
    // #endregion

    // #region Iniciar Funciones
    LLenarPickListJuzgado();
    seteaSaludo();
    HolaMundo();
    // #endregion
});
function seteaSaludo()
{
    saludo = "Hola Mundo";
}

function HolaMundo()
{
    if (saludo != null) {
        alert(saludo);
    }

}

function LLenarPickListJuzgado() {
    if (idcircuito != null) {
        var parametros = { idCircuito: idcircuito }
        SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoAcusatorio", parametros, ListarJuzgadoAcusatorio);
    }
}

// #region Status Respuesta
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
// #endregion

// #region Solicitud Ajax Get Generico
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
    // #endregion

//#region ListarCircuito
function ListarCircuito(data) {
    if (data.Estatus = EstatusRespuesta.OK) {
        const ObjCircuito = [data.Data];
        var $slctCirAc = $('#slctCircuitoAc');
        idcircuito = data.Data.Value;
        $.each(ObjCircuito, function (id, circuito) {
            $slctCirAc.append('<option value=' + circuito.Value + '>' + circuito.Text + '</option>');
        });
    } else if (data.Estatus == EstatusRespuesta.ERROR) {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}
//#endregion

// #region Listar Juzgado [Acusatorio]
function ListarJuzgadoAcusatorio(data) {
    if (data.Estatus = EstatusRespuesta.OK) {
        const ObjJuzgadoAcu = [data.Data];
        var $slcJuzAcu = $('#slctJuzgado');
        $.each(ObjJuzgadoAcu, function (id, juzgado) {
            for (var i = 0; i < juzgado.length; i++) {
                $slcJuzAcu.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
            }
        });
    } else if (data.Estatus == EstatusRespuesta.ERROR) {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}
// #endregion

// #region Muestra u oculta div NUV - CAUSA
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
// #endregion

// #region Ocultar Elementos
$("#divCAU").hide();
// #endregion

// #region Validar Formulario
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
    // #endregion

