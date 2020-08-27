// #region Varaibles Globales
var idcircuito = null;
// #endregion

$(document).ready(function () {

    // #region Listar Circuito
    SolicitudEstandarAjax("/Iniciales/ObtenerCircuito", "", ListarCircuito);
    // #endregion
    // #region Iniciar Funciones
    Circuito_JuzgadoAcusatorio();
    // #endregion

});

function Circuito_JuzgadoAcusatorio(){
    if (idcircuito != null) {
        alert("DATA OK")
        var parobj = { idCircuito: idcircuito }
        SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoAcusatorio", parobj, ListarJuzgadoAcusatorio);
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
        idcircuito = data.Data.Value;

        var $slctCirAc = $('#slctCircuitoAc');
        $.each(ObjCircuito, function (id, circuito) {
            $slctCirAc.append('<option value=' + circuito.Value + '>' + circuito.Text + '</option>');
        });
    } else if (data.Estatus == EstatusRespuesta.ERROR) {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}
//#endregion

// #region GetPametroCircuito_Acusatorio
function GetPametroCircuito_Acusatorio() {
    parametros = { idCircuito: 5 }
    SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoAcusatorio", parametros, ListarJuzgadoAcusatorio);

}
// #endregion

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

