// #region Varaibles Globales
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var Tradicional = false;

//Descripcion : Metodo de JQuery que se ejecuta cada ves que se renderiza el documento HTML, destinado para inicializar 
//funcionalidad a nuestros elementos 
//Parametros de entrada : NA
//Salida : NA
$(document).ready(function ()
{
    ElementosAlCargado();
});

//Descripcion : Metodo que contiene todos los elementos que tienen una inicializacion al cargado del documento HTML
//Parametros de entrada : NA
//Salida : NA
function ElementosAlCargado()
{
    //Agregar formato a NUC Y Causas
    FormatearInput("#inpCausaAcusatorio", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpCausaTradicional", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpNuc", "99-9999-9999", "0000000000", "[0-9\uFF11-\uFF19]", "-");

    //Inhabilita elementos del formulario
    $("#inpCausaAcusatorio").prop('disabled', true);
    $("#inpNuc").prop('disabled', true);
    $("#checkSinNuc").prop('disabled', true);
    $("#slctJuzgadoTradi").prop('disabled', true);
    $("#inpCausaTradicional").prop('disabled', true);

    //Funcionalidad al cambio del select #slctJuzgado
    $('#slctJuzgado').change(function ()
    {
        var idJuzgado = $("#slctJuzgado").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idJuzgado != "" && idJuzgado != null)
        {
            $("#inpCausaAcusatorio").prop('disabled', false);
            $("#checkSinNuc").prop('disabled', false);
        }
        else
        {
            $("#inpCausaAcusatorio").prop('disabled', true);
            $("#inpCausaAcusatorio").val("");
            $("#inpNuc").prop('disabled', true);
            $("#checkSinNuc").prop('disabled', true);
            $("#checkSinNuc").prop("checked", true);
            $("#inpNuc").val("");
        }
    });

    //Funcionalidad al cambio del select #checkSinNuc
    $('#checkSinNuc').change(function ()
    {
        if ($('#checkSinNuc').is(":checked"))
        {
            $("#inpNuc").prop('disabled', true);
            $("#inpNuc").val("");
        }
        else
        {
            $("#inpNuc").prop('disabled', false);
        }
    });

    //Funcionalidad al cambio del select #slctDistrito
    $('#slctDistrito').change(function ()
    {
        var idDistrito = $("#slctDistrito").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idDistrito != "" && idDistrito != null)
        {
            $("#slctJuzgadoTradi").prop('disabled', false);   

            var parametros = { idDistrito: idDistrito }
            SolicitudEstandarAjax("/ConsignacionesHistoricas/ObtenerJuzgadoTradicional", parametros, ListarJuzgadoTradicional);
        }
        else
        {
            $("#inpCausaTradicional").val("");
            $("#inpCausaTradicional").prop('disabled', true);
            $("#slctJuzgadoTradi").prop('disabled', true);
            $("#slctJuzgadoTradi").html("");
            $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));
        }
    });

    //Funcionalidad al cambio del select #slctJuzgadoTradi
    $('#slctJuzgadoTradi').change(function ()
    {
        var idJuzgado = $("#slctJuzgadoTradi").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idJuzgado != "" && idJuzgado != null)
        {
            $("#inpCausaTradicional").prop('disabled', false);
        }
        else
        {
            $("#inpCausaTradicional").val("");
            $("#inpCausaTradicional").prop('disabled', true);
        }
    });

    //Funcionalidad para validar formularios
    var forms = document.getElementsByClassName('needs-validation');

    Array.prototype.filter.call(forms, function (form)
    {
        form.addEventListener('submit', function (event)
        {
            var id = form.id;

            event.preventDefault();
            event.stopPropagation();

            form.classList.add('was-validated');

            if (form.checkValidity() === true && id == "formBuscaAcusatorioCausas")
            {
                ValidarQueExisteCausaEnJuzgado()
            }

            if (form.checkValidity() === true && id == "formBuscaCausasTradicional")
            {
                ValidarQueExisteCausaEnJuzgado();
            }

        }, false);
    });

    //Se le setea valor verdadero a la variable tradicional cada que se le de clic en el tab Tradicional
    $("#juzgadoT-tab").click(function ()
    {
        Tradicional = true;
    });

    //Se le setea valor falso a la variable tradicional cada que se le de clic en el tab de Acusatorio
    $("#juzgadoA-tab").click(function ()
    {
        Tradicional = false;
    });
}

//Descripcion : Valida si el tipo juzgado es tradicional o acusatorio y genera un objeto que contiene los parametros
//que recibe el contrlador de consignaciones historicas con los valores obtenidos por medio del fomulario
//Parametros de entrada
//<tradicional : valor de tipo boleano que regresenta si es un juzgado tradicional o no>
//Salida : NA
function ValidarQueExisteCausaEnJuzgado()
{
    //Obtiene el valor del juzgado seleccionado
    var idJuzgado = $("#" + (!Tradicional ? "slctJuzgado" : "slctJuzgadoTradi")).find('option:selected').val(); 

    //Obtiene el valor de la causa
    var numeroDeCausa = $("#" + (!Tradicional ? "inpCausaAcusatorio" : "inpCausaTradicional")).val();

    //Obtiene el año actual
    var anioActual = new Date().getFullYear();
    var anioCausa = numeroDeCausa.substr(5, 4);

    if (anioCausa > anioActual)
    {
        var funcion = function ()
        {
            var form = $('#' + (!Tradicional ? "formBuscaAcusatorioCausas" : "formBuscaCausasTradicional"))[0];
            $(form).removeClass('was-validated');

            $("#" + (!Tradicional ? "inpCausaAcusatorio" : "inpCausaTradicional")).val("");
        }

        AlertaCallback("El Numero de Causa que intenta ingresar es mayor al año actual", funcion, "large");
        return;
    }

    if (!Tradicional)
    {
        if (!$('#checkSinNuc').is(":checked"))
        {
            var nuc = $("#inpNuc").val();
            var parametros = { idJuzgado: idJuzgado, numeroDeCausa: numeroDeCausa, nuc: nuc }
            SolicitudEstandarAjax("/ConsignacionesHistoricas/ValidaCausaEnJuzgadoPorNumeroNUC", parametros, AgregaCausaAlFormulario);
        }
        else
        {
            var parametros = { idJuzgado: idJuzgado, numeroDeCausa: numeroDeCausa}
            SolicitudEstandarAjax("/ConsignacionesHistoricas/ValidaCausaEnJuzgadoPorNumeroCausa", parametros, AgregaCausaAlFormulario);
        }
    }
    else
    {
        var parametros = { idJuzgado: idJuzgado, numeroDeCausa: numeroDeCausa }
        SolicitudEstandarAjax("/ConsignacionesHistoricas/ValidaCausaEnJuzgadoPorNumeroCausa", parametros, AgregaCausaAlFormulario);
    }
}

//Descripcion : Muestra un mensaje al usuario indicado que la causa que intenta anexanar al formulario 
//ya existe en la base de datos, de lo contrario anexa la causa al formulario
//Parametros de entrada: 
//<respuesta : Objeto que recibe del metodo del controlador>
//Salida : NA
function AgregaCausaAlFormulario(respuesta)
{
    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
        var existe = respuesta.Data;

        if (existe)
        {
            if (!Tradicional)
            {
                var funcion = function ()
                {
                    var form = $('#formBuscaAcusatorioCausas')[0];
                    $(form).removeClass('was-validated');

                    $("#inpCausaAcusatorio").val("");

                    if (!$('#checkSinNuc').is(":checked"))
                    {
                        $("#inpNuc").val("");
                    }
                }

                var juzgadoNombre = $("#slctJuzgado option:selected").text();
                var causa = $("#inpCausaAcusatorio").val();
                var mensajeNuc = "";

                if (!$('#checkSinNuc').is(":checked"))
                {
                    mensajeNuc = "o Numero Unico de Caso <b>" + $("#inpNuc").val() + "</b>";
                }

                var mensaje = "<b>" + juzgadoNombre + "</b> ya tiene registrado un expediente con Numero de Causa <b>" + causa + "</b> " + mensajeNuc;
                AlertaCallback(mensaje, funcion, "large");
            }
            else
            {
                var funcion = function ()
                {
                    var form = $('#formBuscaCausasTradicional')[0];
                    $(form).removeClass('was-validated');

                    $("#inpCausaTradicional").val("");
                }

                var juzgadoNombre = $("#slctJuzgadoTradi option:selected").text();
                var causa = $("#inpCausaTradicional").val();

                var mensaje = "<b>" + juzgadoNombre + "</b> ya tiene registrado un expediente con Numero de Causa <b>" + causa + "</b>";
                AlertaCallback(mensaje, funcion, "large");
            }
        }
        else
        {
            if (!Tradicional)
            {
                $("#slctJuzgado").prop('disabled', true);
                $("#inpCausaAcusatorio").prop('disabled', true);
                $("#inpNuc").prop('disabled', true);
                $("#checkSinNuc").prop('disabled', true);
                $("#btnBuscarCuasaAcusatorio").prop('disabled', true);
                $("#juzgadoA-tab").prop('disabled', true);
                $("#juzgadoT-tab").prop('disabled', true);
            }
            else
            {
                $("#slctDistrito").prop('disabled', true);
                $("#slctJuzgadoTradi").prop('disabled', true);
                $("#inpCausaTradicional").prop('disabled', true);
                $("#btnBuscarCuasaTradicional").prop('disabled', true);
                $("#juzgadoT-tab").prop('disabled', true);
                $("#juzgadoA-tab").prop('disabled', true);
            }
        }
    }
    else if (respuesta.Estatus == EstatusRespuesta.ERROR)
    {
        var mensaje = "Mensaje : " + data.Mensaje;
        Alerta(mensaje, "large", "Error no Controlado por el Sistema");
    }
}

//Descripcion : Genera las opciones del Select #slctJuzgadoTradi por medio del objeto respuesta que recibe del controlador
//Parametros de entrada: 
//<respuesta : Objeto que recibe del metodo del controlador>
//Salida : NA
function ListarJuzgadoTradicional(respuesta)
{
    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
        var numero = respuesta.Data.length;

        $("#slctJuzgadoTradi").html("");

        if (numero > 1)
        {
            $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));

            $("#inpCausaTradicional").val("");
            $("#inpCausaTradicional").prop('disabled', true);
        }
        if (numero == 1)
        {
            $("#inpCausaTradicional").prop('disabled', false);
        }

        const ObjJuzgadoTra = [respuesta.Data];
        var $slcTradi = $('#slctJuzgadoTradi');

        $.each(ObjJuzgadoTra, function (id, juzgado)
        {
            for (var i = 0; i < juzgado.length; i++)
            {
                $slcTradi.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
            }
        });
    }
    else if (respuesta.Estatus == EstatusRespuesta.ERROR)
    {
        var mensaje = "Mensaje : " + data.Mensaje;
        Alerta(mensaje, "large", "Error no Controlado por el Sistema");
    }
}



// #region Metodos Genericos
function SolicitudEstandarAjax(url, parametros, functionCallbackSuccess, functionCallbackError = null)
{
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: parametros,
        beforeSend: function ()
        {
            // $("#loading").fadeIn(); //Animacion Load
        },
        success: function (data)
        {
            functionCallbackSuccess(data);
        },
        error: function (jqXHR)
        {
            $("#loading").fadeOut();

            var mensaje = '';

            if (jqXHR.status === 0)
            {
                mensaje = 'No esta conectado, verifique su conexión.';
            }
            else if (jqXHR.status == 404)
            {
                mensaje = 'No se encontró la página solicitada, ERROR:404';
            }
            else if (jqXHR.status == 500)
            {
                mensaje = "Error interno del servidor, ERROR:500";
            }
            else if (exception === 'timeout')
            {
                mensaje = 'Error de Time Out.';
            }
            else if (exception === 'abort')
            {
                mensaje = 'Solicitud AJAX Abortada.';
            }
            else
            {
                mensaje = 'Error no detectado : ' + jqXHR.responseText;
            }

            if (functionCallbackError == null)
            {
                Alerta(mensaje, "large", "Error ");
            }

            if (functionCallbackError != null)
            {
                var data = mensaje;
                functionCallbackError(data);
            }
        }
    });
}


function FormatearInput(selector, mask, placeholder, validatorRegEx, radixPoint)
{
    Inputmask(mask, {
        positionCaretOnClick: "select",
        radixPoint: radixPoint,
        _radixDance: true,
        numericInput: true,
        placeholder: placeholder,
        definitions:
        {
            "0":
            {
                validator: validatorRegEx
            }
        }
    }).mask(selector);
}

function Alerta(mensaje, tamanio = null, titulo = null)
{
    titulo = titulo == null ? "¡Atención!" : titulo;
    tamanio = tamanio == null ? "small" : tamanio;

    bootbox.alert({
        title: "<h3>" + titulo + "</h3>",
        message: mensaje,
        buttons:
        {
            ok:
            {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-success'
            }
        },
        size: tamanio
    });
}

function AlertaCallback(mensaje, funcion, tamanio = null, titulo = null)
{
    titulo = titulo == null ? "¡Atención!" : titulo;
    tamanio = tamanio == null ? "small" : tamanio;

    bootbox.alert({
        title: "<h3>" + titulo + "</h3>",
        message: mensaje,
        buttons:
        {
            ok: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-success'
            }
        },
        callback: function ()
        {
            funcion();
        },
        size: tamanio
    });
}