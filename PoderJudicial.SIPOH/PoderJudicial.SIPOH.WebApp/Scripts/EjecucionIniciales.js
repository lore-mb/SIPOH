// #region Varaibles Globales
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var IdCircuito = null;
var IdDistrito = null;
var IdOtroSolicitante = null;
var IdOtroSolicitud = null;
var IdOtroAnexos = null;

var DataTableCausas = null;
var DataTableAnex = null;
var DataTableBeneficiario = null;
var DataTableTocas = null;
var DataTableAmparos = null;

//Estructura para data tables
var EstructuraTablaCausas = [{ data: 'CausaNuc', title: 'Causa|Nuc' }, { data: 'NombreJuzgado', title: 'N° Juzgado' }, { data: 'Ofendidos', title: 'Ofendido(s)' }, { data: 'Inculpados', title: 'Inculpado(s)' }, { data: 'Delitos', title: 'Delito(s)'}, { data: 'Eliminar', title: 'Quitar'}];
var Causas = [];

var EstructuraTablaAnexos = [{ data: 'cantidad', title: 'Cantidad', className: "text-center" }, { data: 'descripcion', title: 'Descripción', className: "text-center" }, { data: 'eliminar', title: 'Quitar', className: "text-center" }];
var Anexos = [];

var EstructuraTablaBeneficiarios = [{ data: 'NumeroEjecucion', title: 'No. Ejecución', className: "text-center" }, { data: 'NombreJuzgado', title: 'Juzgado', className: "text-center" }, { data: 'NombreBeneficiario', title: 'Nombre(s)', className: "text-center" }, { data: 'ApellidoPBeneficiario', title: 'Apellido Paterno', className: "text-center" }, { data: 'ApellidoMBeneficiario', title: 'Apellido Materno', className: "text-center" }, { data: 'FechaEjecucion', title: 'Fecha de Ejecución', className: "text-center" }];
var Beneficarios = [];

var EstructuraTablaTocas = [{ data: 'nombreJuzgado', title: 'Sala', className: "text-center" }, { data: 'numeroDeToca', title: 'Número De Toca', className: "text-center" }, { data: 'eliminar', title: 'Quitar', className: "text-center" }];
var Tocas = [];

var EstructuraTablaAmparos = [{ data: 'amparo', title: 'Número de Amparo', className: "text-center"}, { data: 'eliminar', title: 'Quitar', className: "text-center"}];
var Amparos = [];

var EncontroBeneficiarios = false;
var MostrarSeccionesBeneficiario = false;
var FormEjecucionValidado = false;
var EsTradicional = false;
var IdEjecucion = null;

//Variables globales para intentos
var intentos = 0;

// #endregion 

// #region Funciones que se detonan al terminado del renderizado
$(document).ready(function ()
{
    SiguienteInput();
    FormatearInput("#Numero", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpCAUT", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
    FormatearInput("#inpToca", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");

    //Pintar Tablas
    DataTableCausas = GeneraTablaDatos(DataTableCausas, "dataTable", Causas, EstructuraTablaCausas, false, false, false);
    DataTableAnex = GeneraTablaDatos(DataTableAnex, "dataTableAnexos", Anexos, EstructuraTablaAnexos, false, false, false);
    DataTableTocas = GeneraTablaDatos(DataTableTocas, "dataTableTocas", Tocas, EstructuraTablaTocas, false, false, false);
    DataTableAmparos = GeneraTablaDatos(DataTableAmparos, "dataTableAmparos", Anexos, EstructuraTablaAmparos, false, false, false);

    //Obtener Circuito
    IdCircuito = $("#IdCircuitoHDN").val();
    IdOtroSolicitante = $("#IdOtroSolicitanteHDN").val();
    IdOtroSolicitud = $("#IdOtroSolicitudHDN").val();
    IdOtroAnexos = $("#IdOtroAnexoHDN").val();

    //Elemntos al Cargado
    ElementosAlCargado();
});


////Descripcion : Metodo que contiene todos los elementos que tienen una inicializacion al cargado del documento HTML
////Parametros de entrada : NA
////Salida : NA
function ElementosAlCargado()
{
    //Elementos Bloqueados
    $("#Numero").prop('disabled', true); 
    $("#slctNumero").prop('disabled', true);

    $('#slctJuzgado').change(function ()
    {
        var idDistrito = $("#slctJuzgado").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idDistrito != "" && idDistrito != null)
        {
            $("#Numero").prop('disabled', false);
            $("#slctNumero").prop('disabled', false);        
        }
        else
        {
            $("#Numero").val("");
            $("#Numero").prop('disabled', true);
            $("#slctNumero").prop('disabled', true);
        }
    });

    $("#contenedorBeneficiario").hide();
    $("#seccionBeneficiario").hide();
    $("#seccionBusquedaAnexos").hide();
    $("#seccionTablaAnexos").hide();
    $("#seccionBotonGuardar").hide();

    //Funcionalidad para validar formularios
    var forms = document.getElementsByClassName('needs-validation');

    Array.prototype.filter.call(forms, function (form)
    {
        form.addEventListener('submit', function (event)
        {
            var id = form.id;

            event.preventDefault();
            event.stopPropagation();

            if (form.classList.contains("customValidation"))
            {
                //Valida solo campos especificos del formulario
                var validateGroup = form.getElementsByClassName('validate-me');

                for (var i = 0; i < validateGroup.length; i++)
                {
                    validateGroup[i].classList.add('was-validated');
                }
            }
            else
            {
                form.classList.add('was-validated');
            }

            //form.classList.add('was-validated');

            if (form.checkValidity() === true && id == "formBuscaAcusatorioHistoricoCausa")
            {
                AgregarCausaAlDataTable(false)
            }

            if (form.checkValidity() === true && id == "formBuscaTradicionalHistoricoCausa")
            {
                AgregarCausaAlDataTable(true)
            }

            if (form.checkValidity() === true && id == "formAgregaDelito")
            {
                AgregarDelitoAlDataTableDelitos();
            }

            if (form.checkValidity() === true && id == "formAgregarParte")
            {
                AgregarParteAlDataTables();
            }

            if (form.checkValidity() === true && id == "formCausas")
            {               
                ConsultarCausas();
            }

            if (form.checkValidity() === true && id == "formCausasTradicional")
            {
                ConsultarCausas();
            }

            if (form.checkValidity() === true && id == "formTocas")
            {
                AgregarTocas();
            }

            if (form.checkValidity() === true && id == "formAmparos")
            {
                AgregaAmparos();
            }

            if (form.checkValidity() === true && id == "formAnexos")
            {
                AgregarAnexosInicales();
            }

            if (form.checkValidity() === true && id == "formEjecucion")
            {
                var funcion = function ()
                {
                    if (!esConsignacionHistorica)
                    {
                        GenerarEjecucion();
                    }
                    else
                    {
                        GenerarHistoricoDeEjecucion();
                    }
                }

                var mensaje = "Esta a punto de registrar una " + (!esConsignacionHistorica ? "Inicial de Ejecución" : "Consignación Histórica de Ejecución") + ", ¿Desea continuar?";
                MensajeDeConfirmacion(mensaje, "large", funcion);
            }

            if (form.checkValidity() === true && id == "frmBusquedaDeNumeroEjecucion")
            {
                ValidarExistenciaDeNumeroEjecucion();
            }

            if (id == "formEjecucion")
            {
                FormEjecucionValidado = true;
            }

        }, false);
    });

    $("#inpBusquedaSentenciado").val("0");
    
    $("#slctJuzgadoTradi").prop('disabled', true);
    $("#inpCAUT").prop('disabled', true);

    $('#slctDistrito').change(function ()
    {
        IdDistrito = $("#slctDistrito").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (IdDistrito != "" && IdDistrito != null)
        {
            $("#slctJuzgadoTradi").prop('disabled', false);

            var parametros = { idDistrito: IdDistrito }
            SolicitudEstandarAjax("Iniciales/ObtenerJuzgadoTradicional", parametros, ListarJuzgadoTradicional); 
        }
        else
        {
            $("#inpCAUT").val("");
            $("#inpCAUT").prop('disabled', true);
            $("#slctJuzgadoTradi").prop('disabled', true);
            $("#slctJuzgadoTradi").html("");
            $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));
        }
    });

    $('#slctJuzgadoTradi').change(function ()
    {
        IdDistrito = $("#slctJuzgadoTradi").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (IdDistrito != "" && IdDistrito != null)
        {
            $("#inpCAUT").prop('disabled', false);
        }
        else
        {
            $("#inpCAUT").val("");
            $("#inpCAUT").prop('disabled', true);
        }
    });

    if (!esConsignacionHistorica)
    {
        $("#botonMostrarBeneficiarios").click(function () {
            var apellidoPBene = $('#inpApellidoPaterno').val();
            var NombreBene = $('#inpNombreSentenciado').val();

            if (NombreBene != "" && apellidoPBene != "" && EncontroBeneficiarios) {
                $('#ejecucionModal').modal('show');
            }
        });

        $("#botonCheckBeneficiarios").click(function () {
            var apellidoPBene = $('#inpApellidoPaterno').val();
            var NombreBene = $('#inpNombreSentenciado').val();

            if (NombreBene != "" && apellidoPBene != "" && !MostrarSeccionesBeneficiario) {
                MostrarSeccionesBeneficiario = true;
                $("#botonCerrarBeneficiarios").removeClass("btn-secondary");
                $("#botonCerrarBeneficiarios").addClass("btn-danger")
                $("#seccionBeneficiario").show();
                $("#seccionBusquedaAnexos").show();
                $("#seccionTablaAnexos").show();
                $("#seccionBotonGuardar").show();
            }
        });

        $("#botonCerrarBeneficiarios").click(function () {
            MostrarSeccionesBeneficiario = false;
            $("#seccionBeneficiario").hide();
            $("#seccionBusquedaAnexos").hide();
            $("#seccionTablaAnexos").hide();
            $("#seccionBotonGuardar").hide();

            $("#botonCerrarBeneficiarios").removeClass("btn-danger");
            $("#botonCerrarBeneficiarios").addClass("btn-secondary");

            if (FormEjecucionValidado) {
                var form = $('#formEjecucion')[0];
                $(form).removeClass('was-validated');
                FormEjecucionValidado = false;
            }
        });

        $('#inpApellidoPaterno').change(function () {
            ValidarBeneficiarios();
        });

        $('#inpNombreSentenciado').change(function () {
            ValidarBeneficiarios();
        });

        $('#inpApellidoMaterno').change(function () {
            ValidarBeneficiarios();
        });

        $("#btnCancelar").click(function () {
            $("#ejecucionModal").modal("hide");

            $("#botonCheckBeneficiarios").removeClass("btn-success");
            $("#botonCheckBeneficiarios").addClass("btn-secondary");

            $("#botonMostrarBeneficiarios").removeClass("btn-warning");
            $("#botonMostrarBeneficiarios").addClass("btn-secondary");

            Beneficarios = [];
            $('#inpApellidoPaterno').val("");
            $('#inpApellidoMaterno').val("");
            $('#inpNombreSentenciado').val("");

            EncontroBeneficiarios = false;
            $("#inpBusquedaSentenciado").val("0");
            $("#inpBusquedaSentenciado").css('border', function () {
                return '1px solid #b0bec5';
            });

            $("#seccionBeneficiario").hide();
            $("#seccionBusquedaAnexos").hide();
            $("#seccionTablaAnexos").hide();
            $("#seccionBotonGuardar").hide();

            if (FormEjecucionValidado) {
                var form = $('#formEjecucion')[0];
                $(form).removeClass('was-validated');
                FormEjecucionValidado = false;
            }
        });

        $("#btnAceptar").click(function () {
            MostrarSeccionesBeneficiario = true;
            $("#ejecucionModal").modal("hide");
            $("#botonCerrarBeneficiarios").removeClass("btn-secondary");
            $("#botonCerrarBeneficiarios").addClass("btn-danger")
            $("#seccionBeneficiario").show();
            $("#seccionBusquedaAnexos").show();
            $("#seccionTablaAnexos").show();
            $("#seccionBotonGuardar").show();
        });
    }

    $('#slctSolicitud').change(function ()
    {
        var value = $("#slctSolicitud").find('option:selected').val();

        if (value == IdOtroSolicitud)
        {
            $("#inpOtraSolicitud").prop('disabled', false);
            $("#inpOtraSolicitud").prop('required', true);
        }
        else
        {
            $("#inpOtraSolicitud").prop('disabled', true);
            $("#inpOtraSolicitud").prop('required', false);
            $("#inpOtraSolicitud").val("");
        }
    });

    $("#juzgadoT-tab").click(function ()
    {
        //Para que al cargado no se vea el elemento ocultandose
        EsTradicional = true;
        $('#slctSalaTradicional').removeAttr('hidden');

        $('#slctSalaTradicional').show();
        $("#slctSalaTradicional").prop('required', true);

        $('#slctSalaAcusatorio').hide();
        $("#slctSalaAcusatorio").prop('required', false);

        if (esConsignacionHistorica)
        {
            ValidaSeccionDeBeneficiario();
        }
    });

    $("#juzgadoA-tab").click(function ()
    {
        EsTradicional = false;
        $('#slctSalaAcusatorio').show();
        $("#slctSalaAcusatorio").prop('required', true);

        $('#slctSalaTradicional').hide();
        $("#slctSalaTradicional").prop('required', false);

        if (esConsignacionHistorica)
        {
            ValidaSeccionDeBeneficiario();
        }
    });

    $("#inpAddAnexos").prop('disabled', true);

    $('#slctAnexosInicales').change(function ()
    {
        var value = $("#slctAnexosInicales").find('option:selected').val();

        if (value != "" && value != null)
        {
            $("#inpAddAnexos").prop('disabled', false);
        }
        else
        {
            $("#inpAddAnexos").prop('disabled', true);
            $("#inpAddAnexos").val(0);
        }
       
        if (value == IdOtroAnexos)
        {
            $("#inpOtroAnexo").prop('disabled', false);
            $("#inpOtroAnexo").prop('required', true);
        }
        else
        {
            $("#inpOtroAnexo").prop('disabled', true);
            $("#inpOtroAnexo").prop('required', false);
            $("#inpOtroAnexo").val("");
        }
    });

    //CHANGE Juzgado Acusatorio
    $("#slctNumero").change(function ()
    {
        if ($(this).val() == 1)
        {
            //Acusatorio
            $('#Numero').attr('placeholder', '0000/0000');
            $('#Numero').val('');
            FormatearInput("#Numero", "9999/9999", "0000/0000", "[0-9\uFF11-\uFF19]", "/");
            $('#NumeroLabel').html("<strong>Número de Causa</strong>");

        } else if ($(this).val() == 2)
        {
            $('#Numero').attr('placeholder', '00-0000-0000');
            $('#Numero').val('');
            FormatearInput("#Numero", "99-9999-9999", "0000000000", "[0-9\uFF11-\uFF19]", "-");
            $('#NumeroLabel').html("<strong>Número Unico de Caso</strong>");
        }
    });

    if (esConsignacionHistorica)
    {
        ElementosAlCargadoConsignaciones();
    }
}
// #endregion 

// #region Beneficiarios
////Descripcion : Si el usuario ingreso el apellido paterno y nombre del beneficiario, este ejecuta la solicitud ajax para
////que retonar la lista de beneficiarios que coninciden con lo escrito por el usuario, de lo contraria plica estilos al formulario
////Parametros de entrada
////NA
////Salida : NA
function ValidarBeneficiarios()
{
    Beneficarios = [];

    var apellidoPBene = $('#inpApellidoPaterno').val();
    var apellidoMBene = $('#inpApellidoMaterno').val();
    var nombreBene = $('#inpNombreSentenciado').val();

    if (nombreBene != "" && apellidoPBene != "")
    {
        //Pintar en verde Check
        $("#botonCheckBeneficiarios").removeClass("btn-secondary");
        $("#botonCheckBeneficiarios").addClass("btn-success");

        //Consumir Metodo del Controlador
        var parametros = { nombreBene: nombreBene, apellidoPaternoBene: apellidoPBene, apellidoMaternoBene: apellidoMBene }
        SolicitudEstandarAjax("Iniciales/ConsultarSentenciadoBeneficiario", parametros, LlenaTablaConsultaBeneficiarios);
    }
    else
    {
        if (!MostrarSeccionesBeneficiario)
        {
            $("#botonCheckBeneficiarios").removeClass("btn-success");
            $("#botonCheckBeneficiarios").addClass("btn-secondary");
        }

        if (EncontroBeneficiarios)
        {
            $("#botonMostrarBeneficiarios").removeClass("btn-warning");
            $("#botonMostrarBeneficiarios").addClass("btn-secondary");

            $("#inpBusquedaSentenciado").val("0");
            $("#inpBusquedaSentenciado").css('border', function ()
            {
                return '1px solid #b0bec5';
            });
        }
    }
}

////Descripcion : Metodo que recibe la lista de beneficiarios enviado por el metodo del controlador, valida y genera el data table por medio 
////del arreglo de beneficiarios enviado por el objeto tipo respuesta
////Parametros de entrada
////<respuesta : Objeto tipo respuesta que recibe del metodo del controlador por medio de la solicitud ajax>
////Salida : NA
function LlenaTablaConsultaBeneficiarios(respuesta)
{
    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
        EncontroBeneficiarios = true;
        Beneficarios = respuesta.Data.beneficiarios;
       
        $("#inpBusquedaSentenciado").val(respuesta.Data.total);
        
        //Pintar en mostrar Bene
        $("#botonMostrarBeneficiarios").removeClass("btn-secondary");
        $("#botonMostrarBeneficiarios").addClass("btn-warning");

        DataTableBeneficiario = GeneraTablaDatos(DataTableBeneficiario, "dataTableBeneficiarios", Beneficarios, EstructuraTablaBeneficiarios, true, true, false);
    }
    else if (respuesta.Estatus == EstatusRespuesta.SIN_RESPUESTA)
    {
        EncontroBeneficiarios = false;

        $("#inpBusquedaSentenciado").val("0");  
        $("#inpBusquedaSentenciado").css('border', function ()
        {
            return '1px solid #b0bec5';
        });

        $("#botonMostrarBeneficiarios").removeClass("btn-warning");
        $("#botonMostrarBeneficiarios").addClass("btn-secondary");

        $("#botonCerrarBeneficiarios").removeClass("btn-secondary");
        $("#botonCerrarBeneficiarios").addClass("btn-danger");

        $("#seccionBeneficiario").show();
        $("#seccionBusquedaAnexos").show();
        $("#seccionTablaAnexos").show();
        $("#seccionBotonGuardar").show();

        MostrarSeccionesBeneficiario = true;
    }
    else if (respuesta.Estatus == EstatusRespuesta.ERROR)
    {
        EncontroBeneficiarios = false;

        $("#inpBusquedaSentenciado").val("0");
        $("#inpBusquedaSentenciado").css('border', function () {
            return '1px solid #b0bec5';
        });

        $("#botonMostrarBeneficiarios").removeClass("btn-warning");
        $("#botonMostrarBeneficiarios").removeClass("btn-secondary");
        $("#botonMostrarBeneficiarios").addClass("btn-secondary");

        $("#botonCerrarBeneficiarios").removeClass("btn-danger");
        $("#botonCerrarBeneficiarios").removeClass("btn-secondary");
        $("#botonCerrarBeneficiarios").addClass("btn-secondary");

        $("#botonCheckBeneficiarios").removeClass("btn-success");
        $("#botonCheckBeneficiarios").removeClass("btn-secondary");
        $("#botonCheckBeneficiarios").addClass("btn-secondary");

        var funcion = function ()
        {
            ValidarBeneficiarios();
        }

        var mensaje = "Mensaje: " + respuesta.Mensaje + ". Presione Aceptar para intentarlo nuevamente, si el problema continúa vuelva intentarlo mas tarde o consulte a soporte.";
        MensajeDeConfirmacion(mensaje, "large", funcion, null, "Error no Controlado por el Sistema");
    }
}
// #endregion 

// #region Distritos & Juzgados
////Descripcion : Metodo que recibe la lista de juzgados tradicionales enviado por el metodo del controlador, valida y genera 
////el Option Select por medio de la lista recibida
////Parametros de entrada
////<respuesta : Objeto tipo respuesta que recibe del metodo del controlador por medio de la solicitud ajax>
////Salida : NA
function ListarJuzgadoTradicional(respuesta)
{
    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
        var numero = respuesta.Data.length;

        $("#slctJuzgadoTradi").html("");

        if (numero > 1)
        {
            $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));

            $("#inpCAUT").val("");
            $("#inpCAUT").prop('disabled', true);
        }
        if (numero == 1)
        {
            $("#inpCAUT").prop('disabled', false);
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
        var mensaje = "Mensaje : " + respuesta.Mensaje;
        Alerta(mensaje, "large", "Error no Controlado por el Sistema");
    }
}
// #endregion 

// #region Causas
////Descripcion : Funcion para validar que metodo del controlador consumir por medio de la solicitud ajax, valida el numero de cuasa, el
////Nuc y el juzgado seleccionado por el usuario, 
////Parametros de entrada :
////NA
////Salida : NA
function ConsultarCausas()
{
    if (!EsTradicional)
    {
        var causaNucSelect = $("#slctNumero").find('option:selected').val();
        var juzgadoId = $("#slctJuzgado").find('option:selected').val();

        var causaNucText = $('#Numero').val();

        if (causaNucSelect == 2)
        {
            $("#loading").fadeIn();
            var parametros = { idJuzgado: juzgadoId, nuc: causaNucText };
            SolicitudEstandarAjax("Iniciales/ObtenerExpedientePorNUC", parametros, ListarCausas);
        }
        else
        {   
            var anioActual = new Date().getFullYear();
            var anioCausa = causaNucText.substr(5, 4);

            if (anioCausa > anioActual)
            {
                var funcion = function ()
                {
                    LimpiaValidacion("formCausas", "Numero");
                }

                AlertaCallback("El número consecutivo de la causa no debe ser mayor al año actual", funcion);
                return;
            }

            $("#loading").fadeIn();
            var parametros = { idJuzgado: juzgadoId, numeroCausa: causaNucText };
            SolicitudEstandarAjax("Iniciales/ObtenerExpedientePorCausa", parametros, ListarCausas);
        } 
    }
    else
    {
        var causaNucText = $("#inpCAUT").val();
        var juzgadoId = $("#slctJuzgadoTradi").find('option:selected').val();

        var anioActual = new Date().getFullYear();
        var anioCausa = causaNucText.substr(5, 4);

        if (anioCausa > anioActual)
        {
            var funcion = function ()
            {
                LimpiaValidacion("formCausasTradicional", "inpCAUT");
            }

            AlertaCallback("El número consecutivo de la causa no debe ser mayor al año actual", funcion);
            return;
        }

        $("#loading").fadeIn();
        var parametros = { idJuzgado: juzgadoId, numeroCausa: causaNucText };
        SolicitudEstandarAjax("Iniciales/ObtenerExpedientePorCausa", parametros, ListarCausas);
    }
}

////Descripcion : Recibe un objeto de tipo causa enviado por el metodo del controlador, valida y agrega al data table
////la causa, si la vista es ejecutada desde el controlador consignaciones historicas, se valida
////que en caso de no existir la causa ingresada se solicite al usuario si desea agregar una causa de historico a la 
////base de datos.
////Parametros de entrada
////<respuesta : Objeto tipo respuesta que recibe del metodo del controlador por medio de la solicitud ajax>
////Salida : NA
function ListarCausas(respuesta)
{
    $("#loading").fadeOut();
    if (respuesta.Estatus == EstatusRespuesta.OK)
    {
            var expediente = respuesta.Data;           
            var causaNucLabel = expediente.NUC != null ? "NUC " : "Número de Causa ";
            var causaNuc = expediente.NumeroCausa == null ? expediente.NUC : expediente.NumeroCausa;

            if (ValidarCuasaEnTabla(expediente.IdExpediente))
            {
                var mensaje = causaNucLabel + "<b>" + causaNuc + "</b> que intenta agregar, ya se encuentra en la tabla." ;  
    
                var funcion = function ()
                {
                    if (EsTradicional)
                    {
                        LimpiaValidacion("formCausasTradicional", "inpCAUT");
                    }
                    else
                    {
                        LimpiaValidacion("formCausas", "Numero");
                    }
                }

                AlertaCallback(mensaje, funcion)
            }
            else
            {     
                var mensaje = "Mensaje: La consulta generó un resultado exitoso <br><br> El " + causaNucLabel + " <b>" + causaNuc + "</b> se encuentra asignado al juzgado de procedencia <b>" + expediente.NombreJuzgado + "</b> seleccionado.<br><br> ¿Desea Continuar? <br>";

                var funcion = function ()
                {
                    if (Causas.length == 0)
                    {
                        $('#contenedorBeneficiario').removeAttr('hidden');
                        $("#contenedorBeneficiario").show(500);

                        if (esConsignacionHistorica)
                        {
                            if ($("#" + ("slctJuzgadoEjecucion")).find('option:selected').val() == "" && $("#inpNumeroEjecucion").val() == "")
                            {
                                $("#seccionBusquedaBeneficiario").hide();
                            }
                        }
                    }

                    //Id para el DataTable
                    expediente.Id = expediente.IdExpediente;
                    expediente.Eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarCausa(" + expediente.IdExpediente + ")' data-toggle='tooltip' title='Quitar Causa'><i class='icon-bin2'></i></button>";

                    //Agrega Causa al Arreglo de Cuasas
                    Causas.push(expediente);
                    //Generar Tabla 
                    DataTableCausas = GeneraTablaDatos(DataTableCausas, "dataTable", Causas, EstructuraTablaCausas, false, false, false);

                    //Limpiar Formulario CausasPorNumeroCausa
                    if (EsTradicional)
                    {
                        LimpiaValidacion("formCausasTradicional", "inpCAUT");
                    }
                    else
                    {
                        LimpiaValidacion("formCausas", "Numero");
                    }
                }

                //Imprime Mensaje de Confirmacion
                MensajeDeConfirmacion(mensaje, "large", funcion);
            }
    }
    else if (respuesta.Estatus == EstatusRespuesta.ERROR)
    {
        var mensaje = "Mensaje: " + respuesta.Mensaje + "<br><br>Presione <b>Aceptar</b> e intente nuevamente, si el problema continúa vuelva intentarlo mas tarde o consulte a soporte.";
        Alerta(mensaje, "large", "Error no Controlado por el Sistema");
    }
    else if (respuesta.Estatus == EstatusRespuesta.SIN_RESPUESTA)
    {
        //Muestra la palabra JUZGADO cuando se trata del sistema tradicional
        var juzgadoEtiqueta = EsTradicional ? "JUZGADO " : "";

        //Obtiene el nombre de juzgado seleccionado
        var juzgadoNombre = $("#" + (!EsTradicional ? "slctJuzgado" : "slctJuzgadoTradi")).find('option:selected').text();
        juzgadoNombre = juzgadoEtiqueta + juzgadoNombre;

        //Obtiene el numero de causa 
        var causaNuc = $("#" + (!EsTradicional ? "Numero" : "inpCAUT")).val();
        var causaNucSelect = $("#slctNumero").find('option:selected').val();

        var etiqueta = causaNucSelect != 2 ? "El Número de Causa" : "El NUC";
        etiqueta = !EsTradicional ? etiqueta : "El Número de Causa";

        if (!esConsignacionHistorica || causaNucSelect == 2)
        {
            var mensaje = "Mensaje: " + respuesta.Mensaje + ". <br><br>" + etiqueta + " <b>" + causaNuc + "</b> ingresado no se encuentra asignado en el <b>" + juzgadoNombre + "</b>";
            Alerta(mensaje, "large");
        }
        else
        {
            var idJuzgado = $("#" + (!EsTradicional ? "slctJuzgado" : "slctJuzgadoTradi")).find('option:selected').val();

            if (!ValidarCuasaHistoricaEnTabla(idJuzgado, causaNuc))
            {
                var funcionCancelar = function ()
                {
                    LimpiaValidacion((!EsTradicional ? "formCausas" : "formCausasTradicional"), (!EsTradicional ? "Numero" : "inpCAUT"));
                }

                //Metodo que activa la funcionalidad para el formumario de Historico de Causa
                var funcionAceptar = function ()
                {
                    CargaElementosHitoricoCausa();
                }

                var mensaje = "Mensaje: " + respuesta.Mensaje + ". <br><br>" + etiqueta + " <b>" + causaNuc + "</b> ingresado no se encuentra asignado en el <b>" + juzgadoNombre + "</b>, si necesita crear el registro de consignación histórica del número de causa presione <b>Aceptar</b>.";
                MensajeDeConfirmacion(mensaje, "large", funcionAceptar, funcionCancelar);
            }
            else
            {
                var mensaje = etiqueta + " <b>" + causaNuc + "</b> que intenta agregar, ya se encuentra en la tabla.";
                Alerta(mensaje, "small");
            }
        }
    }
}

////Descripcion : Valida si existe una causa en la tabla de Causas, si no existe el metodo retorna un false, 
////si existe retorna true
////Parametros de entrada
////<id : Id de la causa a validar>
////Salida : Tipo Boleano, si la cuasa existe retorna TRUE, si no existe retorna FALSE
function ValidarCuasaEnTabla(id)
{
    var iterarArreglo = Causas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (Causas[index].Id == id)
        {
            return true;
        }
    }
    return false;
}

////Descripcion : Valida si existe una causa de historico en la tabla de Causas, si no existe el metodo retorna un false, 
////si existe retorna true
////Parametros de entrada
////<id : Id de la causa a validar>
////Salida : Tipo Boleano, si la cuasa existe retorna TRUE, si no existe retorna FALSE
function ValidarCuasaHistoricaEnTabla(idJuzgado, numeroCausa)
{
    var iterarArreglo = Causas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (Causas[index].IdJuzgado == idJuzgado && Causas[index].NumeroExpediente == numeroCausa)
        {
            return true;
        }
    }
    return false;
}

////Descripcion : Muestra mensaje al usuario para confirmar la eliminacion de la cuasa, si el usuario preciona aceptar
////elimina la causa del data table 
////Parametros de entrada
////<id : Id de la causa a eliminar>
////Salida : NA
function EliminarCausa(idExpediente) 
{
    var indexCausa = 0;
    var iterarArreglo = Causas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (idExpediente == Causas[index].Id)
        {
            indexCausa = index;
        }
    }

    var funcion = function ()
    {
        Causas.splice(indexCausa, 1);
           
        //Genera nuevamente la tabla
        DataTableCausas = GeneraTablaDatos(DataTableCausas, "dataTable", Causas, EstructuraTablaCausas, false, false, false);

        if (Causas.length == 0)
        {
            $("#contenedorBeneficiario").hide(500);
        }
    }

    var mensaje = "¿Desea retirar la Causa <b>" + Causas[indexCausa].CausaNuc + "</b> de la tabla?"; 

    MensajeDeConfirmacion(mensaje, "small", funcion);
}
// #endregion 

// #region Tocas
////Descripcion : Genera un objeto de tipo Tocas y lo agrega al data table de Tocas, valida que el numero de toca
////ingresado no sea mayor al año actual y no sea repetido
////Parametros de entrada : NA
////Salida : NA
function AgregarTocas()
{
    var numToca = $("#inpToca").val();
    var anioActual = new Date().getFullYear();
    var anioToca = numToca.substr(5, 4); 

    if (anioToca > anioActual)
    {
        var funcion = function ()
        {
            LimpiaValidacion("formTocas", "inpToca");        
        }

        AlertaCallback("El Número de Toca que intenta añadir, es mayor al año actual", funcion);
        return;
    }

    if (ValidarTocaEnTabla(numToca))
    {
        var mensaje = "El Número de Toca <b>" + numToca + "</b> que intenta agregar, ya se encuentra en la tabla.";

        var funcion = function ()
        {
            LimpiaValidacion("formTocas", "inpToca");
        }

        AlertaCallback(mensaje, funcion);
    }
    else
    {
        var nombreSelect = !EsTradicional ? "slctSalaAcusatorio" : "slctSalaTradicional";
            
        var idJuzgado = $("#" + nombreSelect).find('option:selected').val();
        var nombreJuzgado = $("#" + nombreSelect).find('option:selected').text();

        //Se genera un numero aleatorio para asignar un Id a la toca agregada a la tabla
        var numRamdom = Math.floor(Math.random() * 90000) + 10000;

        var toca = new Object();
        toca.id = numRamdom;
        toca.idJuzgado = idJuzgado;
        toca.nombreJuzgado = nombreJuzgado;
        toca.numeroDeToca = numToca;
        toca.eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarToca(" + toca.id + ")' data-toggle='tooltip' title='Quitar Toca'><i class='icon-bin2'></i></button>";
        Tocas.push(toca);

        //Generar Tabla
        DataTableTocas = GeneraTablaDatos(DataTableTocas, "dataTableTocas", Tocas, EstructuraTablaTocas, false, false, false);

        //Limpia form de tocas despues de agregar toca a la tabla
        LimpiaValidacion("formTocas", "inpToca");
    }
}

////Descripcion : Valida si existe una toca en el data table de tocas, si no existe el metodo retorna un false, 
////si existe retorna true
////Parametros de entrada
////<id : Id de la toca a validar>
////Salida : Tipo Boleano, si la cuasa existe retorna TRUE, si no existe retorna FALSE
function ValidarTocaEnTabla(numeroToca)
{
    for (var index = 0; index < Tocas.length; index++)
    {
        if (Tocas[index].numeroDeToca == numeroToca)
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
function EliminarToca(id)
{
    var indexToca = 0;
    var iterarArreglo = Tocas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == Tocas[index].id)
        {
            indexToca = index;
        }
    }

    var funcion = function ()
    {        
        Tocas.splice(indexToca, 1);

        //Genera nuevamente la tabla
        DataTableTocas = GeneraTablaDatos(DataTableTocas, "dataTableTocas", Tocas, EstructuraTablaTocas, false, false, false);
    }

    var mensaje = "¿Desea retirar la Toca <b>" + Tocas[indexToca].numeroDeToca + "</b> de la tabla?";
    MensajeDeConfirmacion(mensaje, "large", funcion);
}
// #endregion 

// #region Amparos
////Descripcion : Genera un objeto de tipo Amparo y lo agrega al data table de Amparos, valida que el numero de amparo
////ingresado no sea repetido
////Parametros de entrada : NA
////Salida : NA
function AgregaAmparos()
{
    var numAmparo = $("#ipnAmparo").val();

    if (ValidarAmparoEnTabla(numAmparo))
    {
        var mensaje = "El Número de Amparo <b>" + numAmparo + "</b> que intenta agregar, ya se encuentra en la tabla.";

        var funcion = function ()
        {
            LimpiaValidacion("formAmparos", "ipnAmparo");
        }

        AlertaCallback(mensaje, funcion);
    }
    else
    {
        //Se genera un numero aleatorio para asignar un Id a la toca agregada a la tabla
        var numRamdom = Math.floor(Math.random() * 90000) + 10000;

        var amparo = new Object();
        amparo.id = numRamdom;
        amparo.amparo = numAmparo;
        amparo.eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarAmparo(" + amparo.id + ")' data-toggle='tooltip' data-placement='top' title='Quitar Sentencia de Amparo'><i class='icon-bin2'></i></button>";
        Amparos.push(amparo);

        //Generar Tabla
        DataTableAmparos = GeneraTablaDatos(DataTableAmparos, "dataTableAmparos", Amparos, EstructuraTablaAmparos, false, false, false);

        LimpiaValidacion("formAmparos", "ipnAmparo");
    }
}

////Descripcion : Valida si existe una Amparo en el data table de Amparos, si no existe el metodo retorna un false, 
////si existe retorna true
////Parametros de entrada
////<id : Id de la Amparo a validar>
////Salida : Tipo Boleano, si la Amparo existe retorna TRUE, si no existe retorna FALSE
function ValidarAmparoEnTabla(numeroAmparo)
{
    for (var index = 0; index < Amparos.length; index++)
    {
        if (Amparos[index].amparo == numeroAmparo)
        {
            return true;
        }
    }
    return false;
}

////Descripcion : Muestra mensaje al usuario para confirmar la eliminacion del Amparo, si el usuario preciona aceptar
////elimina el amparo del data table 
////Parametros de entrada
////<id : Id del Amparo a eliminar>
////Salida : NA
function EliminarAmparo(id)
{
    var iterarArreglo = Amparos;
    var indexAmparo = 0;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == Amparos[index].id)
        {
            indexAmparo = index;
        }
    }

    var funcion = function ()
    {
        Amparos.splice(indexAmparo, 1);
        //Genera nuevamente la tabla
        DataTableAmparos = GeneraTablaDatos(DataTableAmparos, "dataTableAmparos", Amparos, EstructuraTablaAmparos, false, false, false);
    }

    var mensaje = "¿Desea retirar el nuemro de Amparo <b>" + Amparos[indexAmparo].amparo + "</b> de la tabla?";
    MensajeDeConfirmacion(mensaje, "large", funcion);
}

////Descripcion : Genera un arreglo de string con los nuemero de amparos de data table
////Parametros de entrada : NA
////Salida : Arreglo de tipo strings, contiene los numeros de amparos del data table
function GeneraArregloNumeroAmparos()
{
    var numeroAmparos = [];

    for (var index = 0; index < Amparos.length; index++)
    {
        numeroAmparos.push(Amparos[index].amparo);
    }

    return numeroAmparos;
}
// #endregion 

// #region Anexos
////Descripcion : Genera un objeto de tipo Anexo y lo agrega al data table de Anexos, valida que el anexo 
////ingresado no sea repetido
////Parametros de entrada : NA
////Salida : NA
function AgregarAnexosInicales()
{
    var id = $("#slctAnexosInicales").find('option:selected').val();
    var descripcionAnexo = $("#slctAnexosInicales").find('option:selected').text();
    var cantidadAnexo = $("#inpAddAnexos").val(); 
    var idAnexo = id;

    if (id == IdOtroAnexos)
    {
        descripcionAnexo = $("#inpOtroAnexo").val(); 
        idAnexo = Math.floor(Math.random() * 90000) + 10000;
    }

    if (!ValidarAnexoEnTabla(idAnexo, cantidadAnexo))
    {
        var anexoIniciales = new Object();
        anexoIniciales.id = idAnexo;
        anexoIniciales.idAnexo = id;
        anexoIniciales.descripcion = descripcionAnexo;
        anexoIniciales.cantidad = cantidadAnexo;
        anexoIniciales.eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarAnexo(" + anexoIniciales.id + ")' data-toggle='tooltip' data-placement='top' title='Quitar Sentencia de Amparo'><i class='icon-bin2'></i></button>";   
        Anexos.push(anexoIniciales);
    }

    //Generar Tabla
    DataTableAnex = GeneraTablaDatos(DataTableAnex, "dataTableAnexos", Anexos, EstructuraTablaAnexos, false, false, false);

    if (Anexos.length > 0)
    {
        $('#botonEnviar').removeAttr('disabled');
    }

    LimpiaValidacion("formAnexos", "inpAddAnexos");
}

////Descripcion : Muestra mensaje al usuario para confirmar la eliminacion del Anexo, si el usuario preciona aceptar
////elimina el anexo del data table 
////Parametros de entrada
////<id : Id del Anexo a eliminar>
////Salida : NA
function EliminarAnexo(id)
{
    var iterarArreglo = Anexos;
    var indexArreglo = 0;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == Anexos[index].id)
        {
            indexArreglo = index;
        }
    }

    var funcion = function ()
    {
        Anexos.splice(indexArreglo, 1);
        //Genera nuevamente la tabla
        DataTableAnex = GeneraTablaDatos(DataTableAnex, "dataTableAnexos", Anexos, EstructuraTablaAnexos, false, false, false);

        if (Anexos.length == 0)
        {
            $("#botonEnviar").prop('disabled', true);
        }
    }

    var mensaje = "¿Desea retirar el Anexo <b>" + Anexos[indexArreglo].descripcion + " (Cantidad : " + Anexos[indexArreglo].cantidad + ")</b>   de la tabla?";
    MensajeDeConfirmacion(mensaje, "large", funcion);
}

////Descripcion : Valida si existe una anexo en el data table de anexis, si no existe el metodo retorna un false, 
////si existe retorna true
////Parametros de entrada
////<id : Id de la anexi a validar>
////Salida : Tipo Boleano, si la anexo existe retorna TRUE, si no existe retorna FALSE
function ValidarAnexoEnTabla(id, cantidad)
{
    for (var index = 0; index < Anexos.length; index++)
    {
        if (Anexos[index].id == id)
        {
            Anexos[index].cantidad = cantidad;
            return true;
        }
    }
    return false;
}
// #endregion 

// #region Ejecucion
////Descripcion : Genera un obejeto que contiene todos los elementos que solicita el metodo del controlador para
////la creacion del registro de ejecucion valida y genera la solicitud AJAX
////Parametros de entrada : NA
////Salida : NA
function GenerarEjecucion()
{
    $("#loading").fadeIn();

    intentos = intentos + 1;

    var parametros =
    {
        NombreBeneficiario: $("#inpNombreSentenciado").val(),
        ApellidoPBeneficiario: $("#inpApellidoPaterno").val(),
        ApellidoMBeneficiario: $("#inpApellidoMaterno").val(),
        Interno: $('input[name="customRadioInline1"]:checked').val(),
        IdExpedientes: Causas,
        Tocas: Tocas,
        Amparos: GeneraArregloNumeroAmparos(),
        Anexos: Anexos,
        IdSolicitante: $("#slctSolicitante").find('option:selected').val(),
        DetalleSolicitante: $("#ipnDetalleSolicitante").val(),
        IdSolicitud: $("#slctSolicitud").find('option:selected').val(),
        OtraSolicita: $("#inpOtraSolicitud").val()
    };

    SolicitudEstandarPostAjax('Iniciales/CrearEjecucion', parametros, RederizarDetalleSuccess, RederizarDetalleError);
}

////Descripcion : Si la solicitud al controlador fue exitosa, redirecciona a la vista que muestra el detalle de la
////Ejecucion Creada, de lo contrario se le informa al usuario que ocurrio un error y le permite intentar nuevamente el registro,
////se valida el numero de intentos si este es mayor a 3 se interumpe el proceso
////Parametros de entrada : NA
////Salida : NA
function RederizarDetalleSuccess(respuesta)
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
            var mensaje = "Mensaje: " + respuesta.Mensaje + ". <br><br>Intentos: " + intentos + "<br><br><b>Ha superado el número maximo de intentos, vuelva intentarlo mas tarde o consulte a soporte</b";
            intentos = 0;

            Alerta(mensaje, "large");
        }
        else
        {
            var mensaje = "Mensaje: " + respuesta.Mensaje + ", de click en Aceptar para intentar crear el registro nuevamente.<br><br>Intentos: " + intentos;

            reintento = true;
            MensajeDeConfirmacion(mensaje, "large", GenerarEjecucion, null, titulo = "Error no controlado por el sistema");
        }
    }
}

////Descripcion : Si la solicitud al controlador no fue exitosa, se valida el numero de intentos acumulados
////Parametros de entrada : respues de la solicitud ajax
////Salida : NA
function RederizarDetalleError(respuesta)
{
    $("#loading").fadeOut();

    if (intentos > 2)
    {
        var mensaje = "Mensaje: " + respuesta + ". <br><br>Intentos: " + intentos + "<br><br><b>Ha superado el número maximo de intentos, vuelva intentarlo mas tarde o consulte a soporte</b";
        intentos = 0;

        Alerta(mensaje, "large");
    }
    else
    {
        var mensaje = "Mensaje: " + respuesta + ", de click en Aceptar para intentar crear el registro nuevamente.<br><br>Intentos: " + intentos;

        reintento = true;
        MensajeDeConfirmacion(mensaje, "large", GenerarEjecucion, null, titulo = "Error no controlado por el sistema");
    }
}
// #endregion 

// #region Metodos Genericos
function SolicitudEstandarAjax(url, parametros, functionCallbackSuccess, functionCallbackError = null)
{
    $.ajax({
        url: urlBase + url,
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

function LimpiaValidacion(idFormulario, campoLimpiar)
{
    $("#" + campoLimpiar).val("");
    var form = $('#' + idFormulario)[0];
    $(form).removeClass('was-validated');
}

function SiguienteInput()
{
    document.addEventListener('keypress', function (evt)
    {
        // Si el evento NO es una tecla Enter
        if (evt.key !== 'Enter')
        {
            return;
        }

        let element = evt.target;

        // Si el evento NO fue lanzado por un elemento con class "focusNext"
        if (!element.classList.contains('focusNext'))
        {
            return;
        }

        // AQUI logica para encontrar el siguiente
        let tabIndex = element.tabIndex + 1;
        var next = document.querySelector('[tabindex="' + tabIndex + '"]');

        // Si encontramos un elemento
        if (next && !element.classList.contains('focusNextEnd'))
        {
            next.focus();
            event.preventDefault();
        }
    });
}

function SolicitudEstandarPostAjax(url, parameters, functionCallbackSuccess, functionCallbackError = null)
{
    $.ajax({
        url: urlBase + url,
        type: "POST",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(parameters), // Debe obtenerse como JSON.stringify
        dataType: "json",
        cache: true, // sólo para Internet Explorer 8
        beforeSend: function ()
        {
            //$("#loading").fadeIn();
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

            if (functionCallbackSuccess == null)
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

function GeneraTablaDatos(tabla, idTablaHtml, datos, estructuraTabla, ordering, searching, lengthChange)
{
    if (tabla != null)
    {
        tabla.destroy();
        $("#" + idTablaHtml).empty();
    }

    return tabla = $("#" + idTablaHtml).DataTable({
        data: datos,
        columns: estructuraTabla,
        rowId: 'id',
        responsive: true,
        "ordering": ordering,
        "searching": searching,
        "lengthChange": lengthChange,
        "pageLength": 5,
        "lengthMenu": [5, 10, 25, 50],
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
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
            },      


        },
            drawCallback: function (settings) {
            $('[data-toggle="tooltip"]').tooltip();
        }
    }); 
}

function MensajeDeConfirmacion(mensaje, tamanio, funcion, funcionCancelar = null, titulo = null)
{
    titulo = titulo == null ? "Confirmación" : titulo; 

    bootbox.confirm({
        title: "<h3>" + titulo + "</h3>",
        message: mensaje,
        buttons: {
            confirm: {
                label: '<i class="fa fa-check"></i> Aceptar',
                className: 'btn btn-outline-success'
            },
            cancel: {
                label: '<i class="fa fa-times"></i> Cancelar',
                className: 'btn btn-outline-secondary'
            }
        },
        callback: function (result)
        {
            if (result)
            {
                funcion();
            }
            else
            {
                if (funcionCancelar != null)
                {
                    funcionCancelar();
                }
            }
        },
        size: tamanio
    });
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
            ok: {
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
// #endregion 
