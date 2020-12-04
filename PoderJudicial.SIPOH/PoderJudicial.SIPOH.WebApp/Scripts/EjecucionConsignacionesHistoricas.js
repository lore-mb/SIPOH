// #region Varaibles Globales
var TradicionalHistorico = false;
var IdJuzgadoSeleccionado = null;
var TabHistoricoCausa = false;

var DataTableImputados = null;
var EstructuraTablaImputados = [{ data: 'Nombre', title: 'Nombre o Rezón Social' }, { data: 'Genero', title: 'Genero' }, { data: 'Opciones', title: 'Opciones', className: "text-center" }];
var Imputados = [];

var DataTableOfendidos = null;
var EstructuraOfendidos = [{ data: 'Nombre', title: 'Nombre o Rezón Social' }, { data: 'Genero', title: 'Genero' }, { data: 'Opciones', title: 'Opciones', className: "text-center" }];
var Ofendidos = [];

var DataTableDelitos = null;
var EstructuraDelitos = [{ data: 'Delito', title: 'Delito(s) del Inculpado' }, { data: 'Opciones', title: 'Opciones', className: "text-center" }];
var Delitos = [];

//Descripcion : Metodo que contiene todos los elementos que tienen una inicializacion al cargado del documento HTML
//Parametros de entrada : NA
//Salida : NA
function ElementosAlCargadoConsignaciones()
{
    $('.personaMoral').hide();

    $('#slcTipoPersona').change(function ()
    {
        var idTipoPersona = $("#slcTipoPersona").find('option:selected').val();

        if (idTipoPersona == 1)
        {   
            $(".requeridoPm").prop('required', false);
            $(".requeridoPf").prop('required', true);
            $('.personaFisica').show();
            $('.personaMoral').hide();
        }
        else
        {
            $(".requeridoPm").prop('required', true);
            $(".requeridoPf").prop('required', false);
            $('.personaFisica').hide();
            $('.personaMoral').show()
        }
    });

    $('#slcTipoParte').change(function ()
    {
        var idTipoParte = $("#slcTipoParte").find('option:selected').val();
        if (idTipoParte == 1)
        {
            $(".requeridoPfIn").prop('required', true);
        }
        else
        {     
            $(".requeridoPfIn").prop('required', false);
        }
    });

    $('#datetimepickerFechaAcusatorio').datetimepicker({
        format: 'YYYY-MM-DD'
    });

    $('#datetimepickerFechaTradicional').datetimepicker({
        format: 'YYYY-MM-DD'
    });

    DataTableImputados = GeneraTablaDatos(DataTableImputados, "dataTableImputados", Imputados, EstructuraTablaImputados, false, false, false);
    DataTableOfendidos = GeneraTablaDatos(DataTableOfendidos, "dataTableOfendidos", Ofendidos, EstructuraOfendidos, false, false, false);
    DataTableDelitos = GeneraTablaDatos(DataTableDelitos, "dataTableDelitos", Delitos, EstructuraDelitos, false, false, false);

    $("#juzgado-Historico").prop('disabled', true);

    //Agregar formato a NUC Y Causas 
    FormatearInput("#inpCausaAcusatorioHistoricoCausa", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpCausaTradicionalHistoricoCausa", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpNucHistoricoCausa", "99-9999-9999", "0000000000", "[0-9\uFF11-\uFF19]", "-");
    FormatearInput("#inpNumeroEjecucion", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");

    $('#slctDelitosInputados').change(function ()
    {
        var idDelito = $("#slctDelitosInputados").find('option:selected').val();

        if (idDelito != null || idDelito != "")
        {
            var texto = $("#slctDelitosInputados option:selected").text()
            $(this).attr({ "title": texto });

            $('#checkInputadoInvalido').html('<i class="icon-notification"></i> Seleccione un Delito');
        }
    });

    $("#juzgado-Historico").click(function ()
    {
        var contieneCausa = $("#juzgado-Historico").hasClass("contieneCausa");

        if (contieneCausa)
        {
            EsTradicional = false;
            TabHistoricoCausa = true;

            $("#contenedorBeneficiario").hide();
        }      
    });
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

    $("#juzgado-Historico").prop('disabled', false);
    $("#juzgado-Historico").attr("href", "#pills-Historico");
    $("#juzgado-Historico").addClass("contieneCausa");

    $(".causaAceptada").show();

    $("#contenedorBeneficiario").hide();
    $("#pills-Historico").addClass("show active");
    $('#juzgado-Historico').addClass('active'); 

    if (!EsTradicional)
    {
        $("#datetimepickerFechaAcusatorio").prop('disabled', false);
        $("#datetimepickerFechaAcusatorio").val("");

        $("#btnAgregarHistoricoCausa").attr("form", "formBuscaAcusatorioHistoricoCausa");
        LimpiaFormularioConValidacion("formBuscaAcusatorioHistoricoCausa");

        $("#pills-home").removeClass("show active");
        $('#juzgadoA-tab').removeClass('active');
        $('#inpNucHistoricoCausa').val("");   
        $('#inpCausaAcusatorioHistoricoCausa').val("");  

        var juzgadoId = $("#slctJuzgado").find('option:selected').val();

        $("#inpCausaAcusatorioHistoricoCausa").prop('disabled', true);
        var causaNucText = $('#Numero').val();

        SeteaFormularioHistoricoCausa(causaNucText, null, juzgadoId);
    }
    else
    {
        $("#datetimepickerFechaTradicional").prop('disabled', false);
        $("#datetimepickerFechaTradicional").val("");

        $("#btnAgregarHistoricoCausa").attr("form", "formBuscaTradicionalHistoricoCausa");
        LimpiaFormularioConValidacion("formBuscaTradicionalHistoricoCausa");

        $("#pills-profile").removeClass("show active");
        $('#juzgadoT-tab').removeClass('active');

        var causaText = $("#inpCAUT").val();
        var distrito = $("#slctDistrito").find('option:selected').val();
        var juzgadoId = $("#slctJuzgadoTradi").find('option:selected').val();
        SeteaFormularioHistoricoCausa(causaText, distrito, juzgadoId);
    }
}

function SeteaFormularioHistoricoCausa(numeroCausa, idDistrito, idJuzgado)
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
        SolicitudEstandarAjax("ConsignacionesHistoricas/ObtenerJuzgadoTradicional", parametros, ListarJuzgadoTradicionalFormHistorico);
    }
    else
    {
        $('#contenedorformBuscaTradicionalHistoricoCausa').hide();
        $('#contenedorformBuscaAcusatorioHistoricoCausa').show();
        $("#slctJuzgadoHistoricoCausa option[value='" + idJuzgado + "']").prop('selected', true);
        $("#slctJuzgadoHistoricoCausa").prop('disabled', true);
        $('#inpCausaAcusatorioHistoricoCausa').val(numeroCausa);   
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

    SolicitudEstandarAjax("ConsignacionesHistoricas/ValidaNumeroDeEjecucion", parametros, MostrarFormularioBeneficiarios);
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
                $('#seccionBusquedaBeneficiario').show(500);
                $('#seccionBeneficiario').show(500); 
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

function DeshabilitarNumeroEjecucion()
{
    $("#slctJuzgadoEjecucion").prop('disabled', false);
    $("#inpNumeroEjecucion").prop('disabled', false);
    $("#btnConsultarEjecucion").prop('disabled', false);
    $('#seccionBusquedaBeneficiario').hide(500);
    $('#seccionBeneficiario').hide(500);
    $('#seccionBusquedaAnexos').hide();
    $('#seccionTablaAnexos').hide();
    $('#seccionBotonGuardar').hide(); 
}

function MuestraModalPartesCausa(esImputado)
{
    $(".requeridoPf").prop('required', true);
    $(".requeridoPm").prop('required', false);
    $('.personaFisica').show();
    $('.personaMoral').hide();

    if (!esImputado)
    {
        $("#divAliasParte").hide();
        $(".requeridoPfIn").prop('required', false);
        //$("#formAgregaDelito").hide();
        $("#slcTipoParte option[value='2']").prop('selected', true); 
        //$("#btnAceptarParte").prop('disabled', false);
    }
    else
    {
        $("#divAliasParte").show();
        $(".requeridoPfIn").prop('required', true);
        //$("#btnAceptarParte").prop('disabled', true);
        //$("#formAgregaDelito").show();
        $("#slcTipoParte option[value='1']").prop('selected', true); 
    }

    $("#partesCausaModal").modal("show");
}

function CerrarModalPartes()
{
    //Limpia Formularios del Modal
    LimpiaFormularioConValidacion("formAgregarParte");
    //Cierra Modal Partes
    $("#partesCausaModal").modal("hide");
}

function LimpiaFormularioConValidacion(idForm)
{
    $("#" + idForm)[0].reset();
    var formulario = $('#' + idForm)[0];

    if (formulario.classList.contains("customValidation"))
    {
        //Valida solo campos especificos del formulario
        var validateGroup = formulario.getElementsByClassName('validate-me');

        for (var i = 0; i < validateGroup.length; i++)
        {
            validateGroup[i].classList.remove('was-validated');
        }
    }
    else
    {
        $(formulario).removeClass('was-validated');
    }
}


//Descripcion : Crea un objeto de tipo delito apartir de la opcion seleccionada en el select de delitos, agrega al objeto al array de delitos
//se actualiza y redenriza el dataTable de delitos
//Parametros de entrada: 
//NA
//Salida : NA
function AgregarDelitoAlDataTableDelitos()
{
    var idDelito = $("#slctDelitosInputados").find('option:selected').val();
    var nombreDelito = $("#slctDelitosInputados option:selected").text();

    if (!ValidarDelitoEnTabla(idDelito))
    {
        var delito = new Object();
        delito.Delito = nombreDelito;
        delito.IdDelito = idDelito;
        delito.Opciones = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarDelitoDelDataTable(" + delito.IdDelito + ")' data-toggle='tooltip' title='Quitar Delito'><i class='icon-bin2'></i></button>"
        Delitos.push(delito);

        DataTableDelitos = GeneraTablaDatos(DataTableDelitos, "dataTableDelitos", Delitos, EstructuraDelitos, false, false, false);

        //Habilita boton para agregar historico de causa
        if (Imputados.length > 0 && Ofendidos.length > 0 && Delitos.length > 0)
        {
            $("#btnAgregarHistoricoCausa").prop('disabled', false);
        }
    }
    else
    {
        $('#checkInputadoInvalido').html('<i class="icon-notification"></i> La opción seleccionada ya se encuentra en la tabla');
        $("#slctDelitosInputados option[value='']").prop('selected', true); 
    }
}

////Descripcion : Valida si existe un delito en la tabla de Delitos, si no existe el metodo retorna un false, 
////si existe retorna true
////Parametros de entrada
////<id : Id de la causa a validar>
////Salida : Tipo Boleano, si la cuasa existe retorna TRUE, si no existe retorna FALSE
function ValidarDelitoEnTabla(id)
{
    var iterarArreglo = Delitos;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (Delitos[index].IdDelito == id)
        {
            return true;
        }
    }
    return false;
}

////Descripcion : Muestra mensaje al usuario para confirmar la eliminacion de la toca, si el usuario preciona aceptar
////elimina la causa del data table 
////Parametros de entrada
////<id : Id de la toca a eliminar>
////Salida : NA
function EliminarDelitoDelDataTable(id)
{
    var indexDelito = 0;
    var iterarArreglo = Delitos;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == Delitos[index].id)
        {
            Delitos.splice(indexDelito, 1);
        }
    }

    $('[data-toggle="tooltip"]').tooltip("hide");
    Delitos.splice(indexDelito, 1);
    DataTableDelitos = GeneraTablaDatos(DataTableDelitos, "dataTableDelitos", Delitos, EstructuraDelitos, false, false, false);

    if (Delitos.length == 0)
    {
        $("#btnAgregarHistoricoCausa").prop('disabled', true);
    }
}

////Descripcion : Crea un objeto de tipo Parte, y dependiendo del tipo de parte se agrega al arreglo que armara los data tables de partes
//toma los valores del formulario para armar los objetos
////Parametros de entrada
////NA
////Salida : NA
function AgregarParteAlDataTables()
{
    var tipoParte = $("#slcTipoParte").find('option:selected').val();
    var idTipoPersona = $("#slcTipoPersona").find('option:selected').val();

    //Crea objeto de tipo Parte
    var parte = new Object();
    parte.IdParte = Math.floor(Math.random() * 90000) + 10000;

    //Atributos generales
    parte.Nombre = idTipoPersona == 1 ? ($('#ipnNombreParte').val() + " " + $('#inpApellidoPaternoParte').val() + " " + $('#ipnApellidoMaternoParte').val()) : $('#inpRazonSocial').val();
    parte.NombreParte = idTipoPersona == 1 ? $('#ipnNombreParte').val() : $('#inpRazonSocial').val();

    //atributos cuando es una persona fisica
    parte.ApellidoPParte = idTipoPersona == 1 ? $('#inpApellidoPaternoParte').val() : "";
    parte.ApellidoMParte = idTipoPersona == 1 ? $('#ipnApellidoMaternoParte').val() : "";
    parte.Alias = idTipoPersona == 1 ? $('#ipnAliasParte').val() : "";
    parte.Genero = idTipoPersona == 1 ? $("#slcGenero").find('option:selected').val() : "O";

    //Tipo de parte
    parte.TipoParte = tipoParte == 1 ? "I" : "O";

    if (tipoParte == 1)
    {
        //parte.Delitos = Delitos;
        //parte.CadenaDelitos = GeneraCadenaDelitos(Delitos);
        parte.Opciones = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarParteDelDataTable(" + parte.IdParte + "," + true + ")' data-toggle='tooltip' title='Quitar Imputado'><i class='icon-bin2'></i></button>";
        //"<button type='button' onclick='MuestraDelitos(" + parte.IdParte + ")' class='btn btn-link btn-primary' data-toggle='tooltip' title = 'Ver Delitos'><i class='icon-search'></i></button>"; 
        //+"<button type='button' onclick='alert(" + parte.IdParte + ")' class='btn btn-link btn-warning' data-toggle='tooltip' title = 'Editar'><i class='icon-pencil icon'></i></button>";
        Imputados.push(parte);
        DataTableImputados = GeneraTablaDatos(DataTableImputados, "dataTableImputados", Imputados, EstructuraTablaImputados, false, false, false);
    }
    else
    {
        parte.Opciones = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarParteDelDataTable(" + parte.IdParte + "," + false + ")' data-toggle='tooltip' title='Quitar Imputado'><i class='icon-bin2'></i></button>";
        //"<button type='button' onclick='alert(" + parte.IdParte + ")' class='btn btn-link btn-warning' data-toggle='tooltip' title = 'Editar'><i class='icon-pencil icon'></i></button>";
        Ofendidos.push(parte);
        DataTableOfendidos = GeneraTablaDatos(DataTableOfendidos, "dataTableOfendidos", Ofendidos, EstructuraOfendidos, false, false, false);
    }

    //Cierra y limpia modal
    CerrarModalPartes();

    //Habilita boton para agregar historico de causa
    if (Imputados.length > 0 && Ofendidos.length > 0 && Delitos.length > 0)
    {
        $("#btnAgregarHistoricoCausa").prop('disabled', false);  
    }
}

function GeneraCadenaDelitos(delitos)
{
    var cadenaDelitos = "<ul>";

    for (var index = 0; index < delitos.length; index++)
    {
        if (index == 0)
        {
            cadenaDelitos = cadenaDelitos + "<li>" + delitos[index].Delito + "</li>";
        }
        else if (index > 0)
        {
            cadenaDelitos = cadenaDelitos + "<li>" + delitos[index].Delito + "</li>";
        }
    }

    cadenaDelitos = cadenaDelitos + "</ul>"
    return cadenaDelitos;
}

function MuestraDelitos(id)
{
    var cadenaDelitos = "";
    var nombreImputado = "";

    for (var index = 0; index < Imputados.length; index++)
    {
        if (Imputados[index].IdParte == id)
        {
            cadenaDelitos = Imputados[index].CadenaDelitos;
            nombreImputado = Imputados[index].Nombre;
        }
    }

    var mensaje = "Delitos por <b>" + nombreImputado + "</b><br><br>" + cadenaDelitos;

    Alerta(mensaje, "large", "Delitos");
}

function EliminarParteDelDataTable(id, esInculpado)
{
    var indexParte = 0;
    var iterarArreglo = esInculpado ? Imputados : Ofendidos;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == (esInculpado ? Imputados[index].IdParte : Ofendidos[index].IdParte))
        {
            indexParte = index;
        }
    }

    var funcion = function ()
    {
        //Genera nuevamente la tabla
        if (esInculpado)
        {
            Imputados.splice(indexParte, 1);
            DataTableImputados = GeneraTablaDatos(DataTableImputados, "dataTableImputados", Imputados, EstructuraTablaImputados, false, false, false);
        }
        else
        {
            Ofendidos.splice(indexParte, 1);
            DataTableOfendidos = GeneraTablaDatos(DataTableOfendidos, "dataTableOfendidos", Ofendidos, EstructuraOfendidos, false, false, false);
        }

        if (Imputados.length == 0 || Ofendidos.length == 0)
        {
            $("#btnAgregarHistoricoCausa").prop('disabled', true);
        }
    }

    var nombreParte = esInculpado ? Imputados[indexParte].Nombre : Ofendidos[indexParte].Nombre;
    var nombreTabla = esInculpado ? "Imputados" : "Victimas";

    var mensaje = "¿Desea retirar la parte con nombre <b>" + nombreParte + "</b> de la tabla de " + nombreTabla + "?";
    MensajeDeConfirmacion(mensaje, "large", funcion);
}

function AgregarCausaAlDataTable(tradicional)
{
    //Toma los valores del formulario
    var nombreJuzgado = $("#" + (!tradicional ? "slctJuzgadoHistoricoCausa" : "slctJuzgadoTradiHistoricoCausa")).find('option:selected').text();
    var idJuzgado = $("#" + (!tradicional ? "slctJuzgadoHistoricoCausa" : "slctJuzgadoTradiHistoricoCausa")).find('option:selected').val();
    var fechaRecepcion = $("#" + (!tradicional ? "datetimepickerFechaAcusatorio" : "datetimepickerFechaTradicional")).val();
    var numeroCausa = $("#" + (!tradicional ? "inpCausaAcusatorioHistoricoCausa" : "inpCausaTradicionalHistoricoCausa")).val();  

    //Crea Objeto tipo Cuasa
    var expediente = new Object();

    //Datos del DataTable
    expediente.Id = Math.floor(Math.random() * 90000) + 10000;
    expediente.CausaNuc = numeroCausa;
    expediente.NombreJuzgado = nombreJuzgado
    expediente.Ofendidos = GeneraCadenaNombrePartes(false);
    expediente.Inculpados = GeneraCadenaNombrePartes(true);
    expediente.Delitos = GeneraCadenaDelitosCausa();
    expediente.Eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarCausa(" + expediente.Id + ")' data-toggle='tooltip' title='Quitar Causa'><i class='icon-bin2'></i></button>";

    //Datos a base de datos
    expediente.IdExpediente = 0;
    expediente.NumeroExpediente = numeroCausa;
    expediente.IdJuzgado = idJuzgado;
    expediente.FechaIngreso = fechaRecepcion;
    expediente.IdDelitos = ObtieneIdDelitos();
    expediente.Partes = Ofendidos.concat(Imputados);

    //Agrega Causa al Arreglo de Cuasas
    Causas.push(expediente);
    //Generar Tabla 
    DataTableCausas = GeneraTablaDatos(DataTableCausas, "dataTable", Causas, EstructuraTablaCausas, false, false, false);

    //Bloquea Fecha
    $("#" + (!tradicional ? "datetimepickerFechaAcusatorio" : "datetimepickerFechaTradicional")).prop('disabled', true);

    //Limpia Data Tables
    Ofendidos = [];
    Imputados = [];

    //Actualiza Data Tables
    DataTableImputados = GeneraTablaDatos(DataTableImputados, "dataTableImputados", Imputados, EstructuraTablaImputados, false, false, false);
    DataTableOfendidos = GeneraTablaDatos(DataTableOfendidos, "dataTableOfendidos", Ofendidos, EstructuraOfendidos, false, false, false);

    LimpiaFormularioConValidacion("formAgregaDelito");

    //Limpia tabla delitos del Modal
    Delitos = [];
    DataTableDelitos = GeneraTablaDatos(DataTableDelitos, "dataTableDelitos", Delitos, EstructuraDelitos, false, false, false);

    //Bloquea Boton
    $("#btnAgregarHistoricoCausa").prop('disabled', true);

    //Oculta Secciones
    $(".causaAceptada").hide(500);

    $("#juzgado-Historico").prop('disabled', true);
    $("#juzgado-Historico").removeClass("contieneCausa");
    $("#juzgado-Historico").removeAttr("href");

    $('#contenedorBeneficiario').removeAttr('hidden');

    if ($("#" + ("slctJuzgadoEjecucion")).find('option:selected').val() == "" && $("#inpNumeroEjecucion").val() == "")
    {
        $("#seccionBusquedaBeneficiario").hide();
    }

    $("#contenedorBeneficiario").show(500);
}

function GenerarHistoricoDeEjecucion()
{
    $("#loading").fadeIn();

    intentos = intentos + 1;

    var parametros =
    {
        NumeroEjecucion: $("#inpNumeroEjecucion").val(),
        IdJuzgado: $("#slctJuzgadoEjecucion").find('option:selected').val(),
        NombreBeneficiario : $("#inpNombreSentenciado").val(),
        ApellidoPBeneficiario : $("#inpApellidoPaterno").val(),
        ApellidoMBeneficiario : $("#inpApellidoMaterno").val(),
        Interno : $('input[name="customRadioInline1"]:checked').val(),
        Causas : Causas,
        Tocas : Tocas,
        Amparos : GeneraArregloNumeroAmparos(),
        Anexos : Anexos,
        IdSolicitante : $("#slctSolicitante").find('option:selected').val(),
        DetalleSolicitante : $("#ipnDetalleSolicitante").val(),
        IdSolicitud : $("#slctSolicitud").find('option:selected').val(),
        OtraSolicita : $("#inpOtraSolicitud").val()
    };

    SolicitudEstandarPostAjax('ConsignacionesHistoricas/CreaEjecucion', parametros, RederizarDetalleHistoricoSuccess, RederizarDetalleHistoricoError);
}

function RederizarDetalleHistoricoSuccess(respuesta)
{
    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
        var url = respuesta.Data.Url;

        ////Redirecciona a la vista detalle
        document.location.href = url;
    }
    else if (respuesta.Estatus == EstatusRespuesta.ERROR)
    {
        $("#loading").fadeOut();

        if (intentos > 2)
        {
            var mensaje = "Mensaje: " + respuesta.Mensaje + ". <br><br>Intentos: " + intentos + "<br><br><b>Ha superado el numero maximo de intentos, vuelva intentarlo mas tarde o consulte a soporte</b";
            intentos = 0;

            Alerta(mensaje, "large");
        }
        else
        {
            var mensaje = "Mensaje: " + respuesta.Mensaje + ", de click en Aceptar para intentar crear el registro nuevamente.<br><br>Intentos: " + intentos;

            reintento = true;
            MensajeDeConfirmacion(mensaje, "large", GenerarHistoricoDeEjecucion, null, titulo = "Error no controlado por el sistema");
        }
    }
}

function RederizarDetalleHistoricoError(respuesta)
{
    $("#loading").fadeOut();

    if (intentos > 2)
    {
        var mensaje = "Mensaje: " + respuesta + ". <br><br>Intentos: " + intentos + "<br><br><b>Ha superado el numero maximo de intentos, vuelva intentarlo mas tarde o consulte a soporte</b";
        intentos = 0;

        Alerta(mensaje, "large");
    }
    else
    {
        var mensaje = "Mensaje: " + respuesta + ", de click en Aceptar para intentar crear el registro nuevamente.<br><br>Intentos: " + intentos;

        reintento = true;
        MensajeDeConfirmacion(mensaje, "large", GenerarHistoricoDeEjecucion, null, titulo = "Error no controlado por el sistema");
    }
}

function GeneraCadenaNombrePartes(esImputado)
{
    var cadenaNombrePartes = "";
    var iterarArreglo = esImputado ? Imputados : Ofendidos;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        cadenaNombrePartes = cadenaNombrePartes + (iterarArreglo[index].Nombre + (index == (iterarArreglo.length - 1) ? "" : ", "));
    }

    return cadenaNombrePartes;
}

function GeneraCadenaDelitosCausa()
{
    var cadenaDelitosCausa = "";
    var iterarArreglo = Delitos;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        cadenaDelitosCausa = cadenaDelitosCausa + (iterarArreglo[index].Delito + (index == (iterarArreglo.length - 1) ? "" : ", "));
    }

    return cadenaDelitosCausa;
}

function ObtieneIdDelitos()
{
    var idDelitos = [];

    for (var index = 0; index < Delitos.length; index++)
    {
        var id = Delitos[index].IdDelito;
        idDelitos.push(id);
    }

    return idDelitos;
}
