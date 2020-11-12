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
    $("#juzgado-Historico").prop('disabled', true);

    //Agregar formato a NUC Y Causas 
    FormatearInput("#inpCausaAcusatorioHistoricoCausa", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpCausaTradicionalHistoricoCausa", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpNucHistoricoCausa", "99-9999-9999", "0000000000", "[0-9\uFF11-\uFF19]", "-");

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
    SolicitudEstandarAjax("/ConsignacionesHistoricas/ValidaCausaEnJuzgadoPorNumeroNUC", parametros, AgregaCausaAlFormulario);
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
            var tipo = IngresaCausaManual ? "La Causa ya se encuentra asignada en el juzgado morocho" : "El NUC ya se encuentra asignado al juzgado morocho";
            alert(tipo)
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
        var mensaje = "Mensaje : " + data.Mensaje;
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
