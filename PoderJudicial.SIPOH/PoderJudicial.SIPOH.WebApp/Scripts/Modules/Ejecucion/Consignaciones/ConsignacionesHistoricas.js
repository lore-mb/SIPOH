// #region Varaibles Globales
var TradicionalHistorico = false;
var IdJuzgadoSeleccionado = null;
var TabHistoricoCausa = false;
var CausaValidada = true;
var IngresaCausaManual = false;

//Descripcion : Metodo que contiene todos los elementos que tienen una inicializacion al cargado del documento HTML
//Parametros de entrada : NA
//Salida : NA
function ElementosAlCargadoConsignaciones()
{
    $('#datetimepickerFechaAcusatorio').datetimepicker({
        format: 'YYYY-MM-DD'
    });

    $('#datetimepickerFechaTradicional').datetimepicker({
        format: 'YYYY-MM-DD'
    });

    $("#juzgado-Historico").prop('disabled', true);

    //Agregar formato a NUC Y Causas 
    FormatearInput("#inpCausaAcusatorioHistoricoCausa", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpCausaTradicionalHistoricoCausa", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpNucHistoricoCausa", "99-9999-9999", "0000000000", "[0-9\uFF11-\uFF19]", "-");
    FormatearInput("#inpNumeroEjecucion", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");

    //Funcionalidad al cambio del select #checkSinNuc
    $('#checkSinNuc').change(function ()
    {
        if ($('#checkSinNuc').is(":checked"))
        {
            $("#inpNucHistoricoCausa").prop('disabled', true);
            $("#inpNucHistoricoCausa").val("");
            $("#btnCausaNuc").prop('disabled', true);  
            CausaValidada = true;
        }
        else
        {
            $("#btnCausaNuc").prop('disabled', false);  
            $("#inpNucHistoricoCausa").prop('disabled', false);
            CausaValidada = false;
        }
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
    var idJuzgado = $("#" + ("slctJuzgadoHistoricoCausa")).find('option:selected').val(); 

    //Obtiene el valor de la causa
    var numeroDeCausa = $("#inpCausaAcusatorioHistoricoCausa").val();

    //Obtiene el año actual 
    var anioActual = new Date().getFullYear();
    var anioCausa = numeroDeCausa.substr(5, 4);

    if (anioCausa > anioActual)
    {
        var funcion = function ()
        {
            var form = $('#formBuscaAcusatorioHistoricoCausa')[0];
            $(form).removeClass('was-validated');
            $("#inpCausaAcusatorioHistoricoCausa").val("");
        }

        AlertaCallback("El Numero de Causa que intenta ingresar es mayor al año actual", funcion, "large");
        return;
    }

    var nuc = $("#inpNucHistoricoCausa").val();
    var parametros = { idJuzgado: idJuzgado, numeroDeCausa: numeroDeCausa, nuc: nuc }
    SolicitudEstandarAjax("/ConsignacionesHistoricas/ValidaNumeroDeCausa", parametros, MuestraFormularioHistoricoCausa);
}

//Descripcion : Muestra un mensaje al usuario indicado que la causa que intenta anexanar al formulario 
//ya existe en la base de datos, de lo contrario anexa la causa al formulario
//Parametros de entrada: 
//<respuesta : Objeto que recibe del metodo del controlador>
//Salida : NA
function MuestraFormularioHistoricoCausa(respuesta)
{
    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
        var existe = respuesta.Data;

        if (existe)
        {
            var funcion = function ()
            {
                var elementoIput = IngresaCausaManual ? "inpCausaAcusatorioHistoricoCausa" : "inpNucHistoricoCausa";
                LimpiaValidacion("formBuscaAcusatorioHistoricoCausa", elementoIput);
            }

            var juzgadoNombre = $("#slctJuzgadoHistoricoCausa").find('option:selected').text();
            var nucCausa = IngresaCausaManual ? $("#inpCausaAcusatorioHistoricoCausa").val() : $("#inpNucHistoricoCausa").val();
            var mensaje = IngresaCausaManual ? "El Número de Causa <b>" + nucCausa + "</b> ya se encuentra asignado en el <b>" + juzgadoNombre + "</b>" : "El NUC <b>" + nucCausa + "</b> ya se encuentra asignado en el <b>" + juzgadoNombre + "</b>";

            AlertaCallback(mensaje, funcion, "large");
            return;
        }
        else
        {
            var funcion = function ()
            {

            }
        }
    }
    else if (respuesta.Estatus == EstatusRespuesta.ERROR)
    {
       
    }
}

//Descripcion : Genera las opciones del Select #slctJuzgadoTradi por medio del objeto respuesta que recibe del controlador
//Parametros de entrada: 
//<respuesta : Objeto que recibe del metodo del controlador>
//Salida : NA
function ListarJuzgadoTradicionalFormHistorico(respuesta)
{
    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
        const ObjJuzgadoTra = [respuesta.Data];
        var $slcTradi = $('#slctJuzgadoTradiHistoricoCausa');

        $.each(ObjJuzgadoTra, function (id, juzgado)
        {
            for (var i = 0; i < juzgado.length; i++)
            {
                $slcTradi.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
            }
        });

        $("#slctJuzgadoTradiHistoricoCausa option[value='" + IdJuzgadoSeleccionado + "']").prop('selected', true);
        $("#slctJuzgadoTradiHistoricoCausa").prop('disabled', true);
    }
    else if (respuesta.Estatus == EstatusRespuesta.ERROR)
    {
        var mensaje = "Mensaje : " + respuesta.Mensaje;
        Alerta(mensaje, "large", "Error no Controlado por el Sistema");
    }
}

function CargaElementosHitoricoCausa()
{
    TabHistoricoCausa = true;

    $("#contenedorBeneficiario").hide();
    $("#pills-Historico").addClass("show active");
    $('#juzgado-Historico').addClass('active'); 

    if (!EsTradicional)
    {
        $("#pills-home").removeClass("show active");
        $('#juzgadoA-tab').removeClass('active');
        $('#inpNucHistoricoCausa').val("");   
        $('#inpCausaAcusatorioHistoricoCausa').val("");  

        var causaNucSelect = $("#slctNumero").find('option:selected').val();
        var juzgadoId = $("#slctJuzgado").find('option:selected').val();

        if (causaNucSelect == 1)
        {
            IngresaCausaManual = false;

            $("#checkSinNuc").prop('disabled', false);
            $("#checkSinNuc").prop("checked", true);

            $("#inpCausaAcusatorioHistoricoCausa").prop('disabled', true);
            $("#btnCausaNuc").prop('disabled', true);        
            $("#inpNucHistoricoCausa").prop('disabled', true);

            var causaNucText = $('#Numero').val();
            SeteaFormularioHistoricoCausa(causaNucText, null, null, juzgadoId);
        }
        else
        {
            IngresaCausaManual = true;
            CausaValidada = false;

            $("#inpNucHistoricoCausa").prop('disabled', true);
            $("#inpCausaAcusatorioHistoricoCausa").prop('disabled', false);
            $("#checkSinNuc").prop('disabled', true);
            $("#btnCausaNuc").prop('disabled', false);     

            var causaNucText = $('#Numero').val();  
            SeteaFormularioHistoricoCausa(null, causaNucText, null, juzgadoId);
        }
    }
    else
    {
        $("#pills-profile").removeClass("show active");
        $('#juzgadoT-tab').removeClass('active');

        var causaText = $("#inpCAUT").val();
        var distrito = $("#slctDistrito").find('option:selected').val();
        var juzgadoId = $("#slctJuzgadoTradi").find('option:selected').val();
        SeteaFormularioHistoricoCausa(causaText, null, distrito, juzgadoId);
    }
}

function SeteaFormularioHistoricoCausa(numeroCausa, nuc, idDistrito, idJuzgado)
{
    if (EsTradicional)
    {
        $('#contenedorformBuscaAcusatorioHistoricoCausa').hide();
        $('#contenedorformBuscaTradicionalHistoricoCausa').removeAttr('hidden');
        $('#contenedorformBuscaTradicionalHistoricoCausa').show();

        $("#slctDistritoHistoricoCausa option[value='" + idDistrito + "']").prop('selected', true);
        $('#inpCausaTradicionalHistoricoCausa').val(numeroCausa);   

        IdJuzgadoSeleccionado = idJuzgado;
        var parametros = { idDistrito: idDistrito }
        SolicitudEstandarAjax("/ConsignacionesHistoricas/ObtenerJuzgadoTradicional", parametros, ListarJuzgadoTradicionalFormHistorico);
    }
    else
    {
        $('#contenedorformBuscaTradicionalHistoricoCausa').hide();
        $('#contenedorformBuscaAcusatorioHistoricoCausa').show();
     
        $("#slctJuzgadoHistoricoCausa option[value='" + idJuzgado + "']").prop('selected', true);
        $("#slctJuzgadoHistoricoCausa").prop('disabled', true);

        if (numeroCausa != null)
        {
            $('#inpCausaAcusatorioHistoricoCausa').val(numeroCausa);   
        }

        if (nuc != null)
        {
            $("#checkSinNuc").prop("checked", false);
            $('#inpNucHistoricoCausa').val(nuc);   
        }
    }
}

function ValidaSeccionDeBeneficiario()
{
    if (TabHistoricoCausa)
    {
        TabHistoricoCausa = false;
        var total = Causas.length;
        if (total > 0)
        {
            $("#contenedorBeneficiario").show();
        }
    }
}

function ValidarExistenciaDeNumeroEjecucion()
{
    //Obtiene el valor del numero de Ejecucion
    var numeroEjecucion = $("#inpNumeroEjecucion").val();

    //Obtiene el año actual 
    var anioActual = new Date().getFullYear();
    var anioCausa = numeroEjecucion.substr(5, 4);

    if (anioCausa > anioActual)
    {
        var funcion = function ()
        {
            var form = $('#frmBusquedaDeNumeroEjecucion')[0];
            $(form).removeClass('was-validated');
            $("#inpNumeroEjecucion").val("");
        }

        AlertaCallback("El Numero de Ejecución que intenta ingresar es mayor al año actual", funcion, "large");
        return;
    }

    $("#loading").fadeIn();

    var idJuzgadoEjecucion = $("#" + ("slctJuzgadoEjecucion")).find('option:selected').val(); 
    var parametros = { idJuzgadoEjecucion: idJuzgadoEjecucion, numeroEjecucion: numeroEjecucion }

    SolicitudEstandarAjax("/ConsignacionesHistoricas/ValidaNumeroDeEjecucion", parametros, MostrarFormularioBeneficiarios);
}

function MostrarFormularioBeneficiarios(respuesta)
{
    $("#loading").fadeOut();

    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
        var disponible = respuesta.Data;

        if (!disponible)
        {
            var funcion = function ()
            {
                var form = $('#frmBusquedaDeNumeroEjecucion')[0];
                $(form).removeClass('was-validated');
                $("#inpNumeroEjecucion").val("");
            }

            var mensaje = "Mensaje : " + respuesta.Mensaje;
            AlertaCallback(mensaje, funcion, "large");
        }
        else
        {
            var funcion = function ()
            {
                $("#slctJuzgadoEjecucion").prop('disabled', true);
                $("#inpNumeroEjecucion").prop('disabled', true);
                $("#btnConsultarEjecucion").prop('disabled', true);
                $('#seccionBusquedaBeneficiario').show();
                $('#seccionBeneficiario').show(); 
                $('#seccionBusquedaAnexos').show();
                $('#seccionTablaAnexos').show();
                $('#seccionBotonGuardar').show();
            }

            var mensaje = "Mensaje : " + respuesta.Mensaje + ".<br><br> ¿Desea continuar con el registro?";
            MensajeDeConfirmacion(mensaje, "large", funcion);
        }
    }
    else if (respuesta.Estatus == EstatusRespuesta.ERROR)
    {
        alert("Error");
    }
}

function GenerarHistoricoDeEjecucion()
{
    alert("Se crea historico de ejecucion");
}

function DeshabilitarNumeroEjecucion()
{
    $("#slctJuzgadoEjecucion").prop('disabled', false);
    $("#inpNumeroEjecucion").prop('disabled', false);
    $("#btnConsultarEjecucion").prop('disabled', false);
    $('#seccionBusquedaBeneficiario').hide();
    $('#seccionBeneficiario').hide();
    $('#seccionBusquedaAnexos').hide();
    $('#seccionTablaAnexos').hide();
    $('#seccionBotonGuardar').hide(); 
}