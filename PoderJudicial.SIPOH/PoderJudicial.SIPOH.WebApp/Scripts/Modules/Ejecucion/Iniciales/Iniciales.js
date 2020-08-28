//#region Varaibles Globales
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var idcircuito = null;


//#endregion

// #region Document Ready [NADA]
$(document).ready(function ()
{
    //Elemntos al Cargado
    ElementosAlCargado();

    //Solicitudes AJAX
    LlenaPickListCircuito();
    
});
// #endregion

// #region ElementosAlCargado [NADA]
function ElementosAlCargado()
{
    //Renderizar Tabla  
    PintarTabla();

    // #region Muestra u oculta div NUV - CAUSA
    $("#slcNumero").change(function ()
    {
        if ($(this).val() == "2")
        {
            $('#divCAU').show();
            $('#divNUC').hide();
        } else
        {
            if ($("#slcNumero").val() == "1")
            {
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
    var validation = Array.prototype.filter.call(forms, function (form)
    {
        form.addEventListener('submit', function (event)
        {
            if (form.checkValidity() === false)
            {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
    // #endregion
}
// #endregion

// #region GetDataCircuito [NADA]
function LlenaPickListCircuito()
{
    SolicitudEstandarAjax("/Iniciales/ObtenerCircuito", "", ListarCircuito);
}
// #endregion

//#region PintarCircuito
function ListarCircuito(data)
{
    if (data.Estatus = EstatusRespuesta.OK)
    {
        const ObjCircuito = [data.Data];
        const ObjCircuitoTr = [data.Data];
        idcircuito = data.Data.Value;

        var $slctCirAc = $('#slctCircuitoAc');
        var $slctCirTr = $('#slctCircuitoTr');

        $.each(ObjCircuito, function (id, circuito)
        {
            $slctCirAc.append('<option value=' + circuito.Value + '>' + circuito.Text + '</option>');
            
        });

        $.each(ObjCircuitoTr, function (id, circuitoTr) {
            $slctCirTr.append('<option value=' + circuitoTr.Value + '>' + circuitoTr.Text + '</option>');
        });

        Circuito_JuzgadoAcusatorio();
        Parametros_Distrito();
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}
//#endregion

// #region PasarParametro Circuito - Acusatorio
function Circuito_JuzgadoAcusatorio()
{
    if (idcircuito != null)
    {
        var parobj = { idCircuito : idcircuito }
        SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoAcusatorio", parobj, ListarJuzgadoAcusatorio);
    }
    else
    {
        alert("Error al obtener los datos");
    }
}
// #endregion

// #region Listar Juzgado Acusatorio
function ListarJuzgadoAcusatorio(data)
{
    if (data.Estatus = EstatusRespuesta.OK)
    {
        const ObjJuzgadoAcu = [data.Data];
        var $slcJuzAcu = $('#slctJuzgado');

        $.each(ObjJuzgadoAcu, function (id, juzgado)
        {
            for (var i = 0; i < juzgado.length; i++)
            {
                $slcJuzAcu.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
            }
        });
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}
// #endregion

//// #region Parametro Distrito
//function Parametros_Distrito() {
//    if (idcircuito != null) {
//        var Parametros = { idCircuito: idcircuito }
//        SolicitudEstandarAjax("/Iniciales/ObtenerDistritosPorCircuito", Parametros, ListarDistrito)
//    } else {
//        alert("Error de parametro");
//    }
//}
//// #endregion


//function ListarDistrito(data) {
//    if (data.Estatus = EstatusRespuesta.OK) {
//        // Aquí estoy revisando, me retorna null
//        alert(JSON.stringify(data));
//        //var Array = [data.Data]
//        //var $pickDistrito = $('#slctDistrito');
//        //$.each(Array, function (id, distrito) {
//        //    for (var i = 0; i < distrito.length; i++) {
//        //        $pickDistrito.append('<option value=' + distrito[i].Value + '>' + distrito[i].Text + '</option>');
//        //    }
//        //});
//    } else if (data.Estatus == EstatusRespuesta.ERROR) {
//        customNotice(data.Mensaje, "Error:", "error", 3350);
//    }
//}

//#region Solicitud Ajax Get Generico
function SolicitudEstandarAjax(url, parametros, funcion)
{
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: parametros,
        beforeSend: function () {
            // $("#loading").fadeIn(); //Animacion Load
        },
        success: function (data)
        {
            funcion(data);
        },
        error: function (xhr)
        {
            alert('Error Ajax: ' + xhr.statusText);
            //  $("#loading").fadeOut();
        }
    });
}
// #endregion

