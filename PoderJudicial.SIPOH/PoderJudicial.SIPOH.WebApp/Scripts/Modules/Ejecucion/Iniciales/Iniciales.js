 // #region Varaibles Globales
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var idCircuito = null;
var idDistrito = null;
var idOtroSolicitante = null;
var idOtroSolicitud = null;
var idOtroAnexos = null;

var dataTable = null;
var dataTableAnex = null;
var dataTableBeneficiario = null;
var dataTableTocas = null;
var dataTableAmparos = null;

//Estructura para data tables
var EstructuraTablaCausas = [{ data: 'CausaNuc', title: 'Causa|Nuc' }, { data: 'NombreJuzgado', title: 'N° Juzgado' }, { data: 'Ofendidos', title: 'Ofendido(s)' }, { data: 'Inculpados', title: 'Inculpado(s)' }, { data: 'Delitos', title: 'Delito(s)'}, { data: 'Eliminar', title: 'Quitar'}];
var Causas = [];

var estructuraTablaAnexos = [{ data: 'cantidad', title: 'Cantidad', className: "text-center" }, { data: 'descripcion', title: 'Descripción', className: "text-center" }, { data: 'eliminar', title: 'Quitar', className: "text-center" }];
var anexos = [];

var estructuraTablaBeneficiarios = [{ data: 'NumeroEjecucion', title: 'No. Ejecución', className: "text-center" }, { data: 'NombreJuzgado', title: 'Juzgado', className: "text-center" }, { data: 'NombreBeneficiario', title: 'Nombre(s)', className: "text-center" }, { data: 'ApellidoPBeneficiario', title: 'Apellido Paterno', className: "text-center" }, { data: 'ApellidoMBeneficiario', title: 'Apellido Materno', className: "text-center" }, { data: 'FechaEjecucion', title: 'Fecha de Ejecución', className: "text-center" }];
var beneficarios = [];

var estructuraTablaTocas = [{ data: 'nombreJuzgado', title: 'Sala', className: "text-center" }, { data: 'numeroDeToca', title: 'Numero De Toca', className: "text-center" }, { data: 'eliminar', title: 'Quitar', className: "text-center" }];
var tocas = [];

var estructuraTablaAmparos = [{ data: 'amparo', title: 'Numero de Amparo', className: "text-center"}, { data: 'eliminar', title: 'Quitar', className: "text-center"}];
var amparos = [];

var encontroBeneficiarios = false;
var mostrarSeccionesBeneficiario = false;
var formEjecucionValidado = false;
var esTradicional = false;
var idEjecucion = null;

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
    dataTable = GeneraTablaDatos(dataTable, "dataTable", Causas, EstructuraTablaCausas, false, false, false);
    dataTableAnex = GeneraTablaDatos(dataTableAnex, "dataTableAnexos", anexos, estructuraTablaAnexos, false, false, false);
    dataTableTocas = GeneraTablaDatos(dataTableTocas, "dataTableTocas", tocas, estructuraTablaTocas, false, false, false);
    dataTableAmparos = GeneraTablaDatos(dataTableAmparos, "dataTableAmparos", anexos, estructuraTablaAmparos, false, false, false);

    //Obtener Circuito
    idCircuito = $("#IdCircuitoHDN").val();
    idOtroSolicitante = $("#IdOtroSolicitanteHDN").val();
    idOtroSolicitud = $("#IdOtroSolicitudHDN").val();
    idOtroAnexos = $("#IdOtroAnexoHDN").val();

    //Elemntos al Cargado
    ElementosAlCargado();
});

//Elementos al Cargado
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

            form.classList.add('was-validated');

            if (form.checkValidity() === true && id == "formCausas")
            {
                ConsultarCausas(true);
            }

            if (form.checkValidity() === true && id == "formCausasTradicional")
            {
                ConsultarCausas(false);
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
                GenerarEjecucion();
            }

            if (id == "formEjecucion")
            {
                formEjecucionValidado = true;
            }

        }, false);
    });

    $("#inpBusquedaSentenciado").val("0");

    $("#slctJuzgadoTradi").prop('disabled', true);
    $("#inpCAUT").prop('disabled', true);

    $('#slctDistrito').change(function ()
    {
        idDistrito = $("#slctDistrito").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idDistrito != "" && idDistrito != null)
        {
            $("#slctJuzgadoTradi").prop('disabled', false);

            var parametros = { idDistrito: idDistrito }
            SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoTradicional", parametros, ListarJuzgadoTradicional); 
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
        idDistrito = $("#slctJuzgadoTradi").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idDistrito != "" && idDistrito != null)
        {
            $("#inpCAUT").prop('disabled', false);
        }
        else
        {
            $("#inpCAUT").val("");
            $("#inpCAUT").prop('disabled', true);
        }
    });


    $("#botonMostrarBeneficiarios").click(function ()
    {
        var apellidoPBene = $('#inpApellidoPaterno').val();
        var NombreBene = $('#inpNombreSentenciado').val();

        if (NombreBene != "" && apellidoPBene != "" && encontroBeneficiarios)
        {
            $('#ejecucionModal').modal('show');
        }
    });

    $("#botonCheckBeneficiarios").click(function ()
    {
        var apellidoPBene = $('#inpApellidoPaterno').val();
        var NombreBene = $('#inpNombreSentenciado').val();

        if (NombreBene != "" && apellidoPBene != "" && !mostrarSeccionesBeneficiario)
        {
            mostrarSeccionesBeneficiario = true;
            $("#botonCerrarBeneficiarios").removeClass("btn-secondary");
            $("#botonCerrarBeneficiarios").addClass("btn-danger")
            $("#seccionBeneficiario").show();
            $("#seccionBusquedaAnexos").show();
            $("#seccionTablaAnexos").show();
            $("#seccionBotonGuardar").show();
        }
    });

    $("#botonCerrarBeneficiarios").click(function ()
    {
        mostrarSeccionesBeneficiario = false;
        $("#seccionBeneficiario").hide();
        $("#seccionBusquedaAnexos").hide();
        $("#seccionTablaAnexos").hide();
        $("#seccionBotonGuardar").hide();

        $("#botonCerrarBeneficiarios").removeClass("btn-danger");
        $("#botonCerrarBeneficiarios").addClass("btn-secondary");

        if (formEjecucionValidado)
        {
            var form = $('#formEjecucion')[0];
            $(form).removeClass('was-validated');
            formEjecucionValidado = false;
        }
    });

    $("#btnCancelar").click(function ()
    {
        $("#ejecucionModal").modal("hide");

        $("#botonCheckBeneficiarios").removeClass("btn-success");
        $("#botonCheckBeneficiarios").addClass("btn-secondary");

        $("#botonMostrarBeneficiarios").removeClass("btn-warning");
        $("#botonMostrarBeneficiarios").addClass("btn-secondary");

        beneficarios = [];
        $('#inpApellidoPaterno').val("");
        $('#inpApellidoMaterno').val("");
        $('#inpNombreSentenciado').val("");

        encontroBeneficiarios = false;
        $("#inpBusquedaSentenciado").val("0");
        $("#inpBusquedaSentenciado").css('border', function () {
            return '1px solid #b0bec5';
        });

        $("#seccionBeneficiario").hide();
        $("#seccionBusquedaAnexos").hide();
        $("#seccionTablaAnexos").hide();
        $("#seccionBotonGuardar").hide();

        if (formEjecucionValidado)
        {
            var form = $('#formEjecucion')[0];
            $(form).removeClass('was-validated');
            formEjecucionValidado = false;
        }
    });

    $("#btnAceptar").click(function ()
    {
        mostrarSeccionesBeneficiario = true;
        $("#ejecucionModal").modal("hide");
        $("#botonCerrarBeneficiarios").removeClass("btn-secondary");
        $("#botonCerrarBeneficiarios").addClass("btn-danger")
        $("#seccionBeneficiario").show();
        $("#seccionBusquedaAnexos").show();
        $("#seccionTablaAnexos").show();
        $("#seccionBotonGuardar").show();
    });

    $('#inpApellidoPaterno').change(function ()
    {
        ValidarBeneficiarios();
    });

    $('#inpNombreSentenciado').change(function ()
    {
        ValidarBeneficiarios();
    });

    $('#inpApellidoMaterno').change(function ()
    {
        ValidarBeneficiarios();
    });

    $('#slctSolicitud').change(function ()
    {
        var value = $("#slctSolicitud").find('option:selected').val();

        if (value == idOtroSolicitud)
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
        esTradicional = true;
        $('#slctSalaTradicional').removeAttr('hidden');

        $('#slctSalaTradicional').show();
        $("#slctSalaTradicional").prop('required', true);

        $('#slctSalaAcusatorio').hide();
        $("#slctSalaAcusatorio").prop('required', false);
    });

    $("#juzgadoA-tab").click(function ()
    {
        esTradicional = false;
        $('#slctSalaAcusatorio').show();
        $("#slctSalaAcusatorio").prop('required', true);

        $('#slctSalaTradicional').hide();
        $("#slctSalaTradicional").prop('required', false);
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
       
        if (value == idOtroAnexos)
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
            $('#NumeroLabel').html("<strong>Numero de Causa</strong>");

        } else if ($(this).val() == 2)
        {
            $('#Numero').attr('placeholder', '00-0000-0000');
            $('#Numero').val('');
            FormatearInput("#Numero", "99-9999-9999", "0000000000", "[0-9\uFF11-\uFF19]", "-");
            $('#NumeroLabel').html("<strong>Numero Unico de Caso</strong>");
        }
    });
}
// #endregion 

// #region Beneficiarios
function ValidarBeneficiarios()
{
    beneficarios = [];

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
        SolicitudEstandarAjax("/Iniciales/ConsultarSentenciadoBeneficiario", parametros, LlenaTablaConsultaBeneficiarios);
    }
    else
    {
        if (!mostrarSeccionesBeneficiario)
        {
            $("#botonCheckBeneficiarios").removeClass("btn-success");
            $("#botonCheckBeneficiarios").addClass("btn-secondary");
        }

        if (encontroBeneficiarios)
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

function LlenaTablaConsultaBeneficiarios(data)
{
    if (data.Estatus == EstatusRespuesta.OK)
    {
        encontroBeneficiarios = true;
        beneficarios = data.Data.beneficiarios;
       
        $("#inpBusquedaSentenciado").val(data.Data.total);
        
        //Pintar en mostrar Bene
        $("#botonMostrarBeneficiarios").removeClass("btn-secondary");
        $("#botonMostrarBeneficiarios").addClass("btn-warning");

        dataTableBeneficiario = GeneraTablaDatos(dataTableBeneficiario, "dataTableBeneficiarios", beneficarios, estructuraTablaBeneficiarios, true, true, false);
    }
    else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA)
    {
        encontroBeneficiarios = false;

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

        mostrarSeccionesBeneficiario = true;
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        encontroBeneficiarios = false;

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

        var mensaje = "Mensaje: " + data.Mensaje + ". Precione Aceptar para intentarlo nuevamente, si el problema continua vuelva intentarlo mas tarde o consulte a soporte.";
        MensajeDeConfirmacion(mensaje, "large", funcion, null, "Error no Controlado por el Sistema");
    }
}
// #endregion 

// #region Distritos & Juzgados
function ListarJuzgadoTradicional(data)
{
    if (data.Estatus == EstatusRespuesta.OK)
    {
        var numero = data.Data.length;

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

        const ObjJuzgadoTra = [data.Data];
        var $slcTradi = $('#slctJuzgadoTradi');

        $.each(ObjJuzgadoTra, function (id, juzgado)
        {
            for (var i = 0; i < juzgado.length; i++)
            {
                $slcTradi.append('<option value=' + juzgado[i].Value + '>' + juzgado[i].Text + '</option>');
            }
        });
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        var mensaje = "Mensaje : " + data.Mensaje;
        Alerta(mensaje, "large", "Error no Controlado por el Sistema");
    }
}
// #endregion 

// #region Causas
function ConsultarCausas(esAcusatorio)
{
    if (esAcusatorio)
    {
        var causaNucSelect = $("#slctNumero").find('option:selected').val();
        var juzgadoId = $("#slctJuzgado").find('option:selected').val();

        var causaNucText = $('#Numero').val();

        if (causaNucSelect == 2)
        {
            var parametros = { idJuzgado: juzgadoId, nuc: causaNucText };
            SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorNUC", parametros, ListarCausas);
        }
        else
        {   
            var parametros = { idJuzgado: juzgadoId, numeroCausa: causaNucText };
            SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorCausa", parametros, ListarCausas);
        } 
    }
    else
    {
        var causaNucText = $("#inpCAUT").val();
        var juzgadoId = $("#slctJuzgadoTradi").find('option:selected').val();

        var parametros = { idJuzgado: juzgadoId, numeroCausa: causaNucText };
        SolicitudEstandarAjax("/Iniciales/ObtenerExpedientePorCausa", parametros, ListarCausas);
    }
}

function ListarCausas(data)
{    
    if (data.Estatus == EstatusRespuesta.OK)
    {
            var expediente = data.Data;           
            var causaNucLabel = expediente.NUC != null ? "NUC " : "Numero de causa ";
            var causaNuc = expediente.NumeroCausa == null ? expediente.NUC : expediente.NumeroCausa;

            if (ValidarCuasaEnTabla(expediente.IdExpediente))
            {
                var mensaje = causaNucLabel + "<b>" + causaNuc + "</b> que intenta agregar, ya se encuentra en la tabla." ;  
    
                var funcion = function ()
                {
                    if (esTradicional)
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
                var mensaje = "La consulta generó un resultado exitoso para el " + causaNucLabel + " <b>" + causaNuc + "</b> asignado al juzgado de procedencia <b>" + expediente.NombreJuzgado + "</b> seleccionado.<br><br> ¿Desea Continuar? <br>";

                var funcion = function ()
                {
                    if (Causas.length == 0)
                    {
                        $('#contenedorBeneficiario').removeAttr('hidden');
                        $("#contenedorBeneficiario").show();
                    }

                    var causa = new Object();
                    expediente.Eliminar = "<button type='button' class='btn btn-link btn-danger btn-sm' onclick='EliminarCausa(" + expediente.IdExpediente + ")' data-toggle='tooltip' title='Quitar Causa'><i class='icon-bin2'></i></button>";

                    //Agrega Causa al Arreglo de Cuasas
                    Causas.push(expediente);
                    //Generar Tabla 
                    dataTable = GeneraTablaDatos(dataTable, "dataTable", Causas, EstructuraTablaCausas, false, false, false);

                    //Limpiar Formulario CausasPorNumeroCausa
                    if (esTradicional)
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
        else if (data.Estatus == EstatusRespuesta.ERROR)
        {
            var mensaje = "Mensaje: " + data.Mensaje + ". Precione Aceptar para intentarlo nuevamente, si el problema continua vuelva intentarlo mas tarde o consulte a soporte.";      
            Alerta(mensaje, "large", "Error no Controlado por el Sistema");
        }
        else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA)
        {
            Alerta(data.Mensaje);
        }
}

function ValidarCuasaEnTabla(id)
{
    var iterarArreglo = Causas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (Causas[index].IdExpediente == id)
        {
            return true;
        }
    }
    return false;
}

function EliminarCausa(idExpediente) 
{
    var indexCausa = 0;
    var iterarArreglo = Causas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (idExpediente == Causas[index].IdExpediente)
        {
            indexCausa = index;
        }
    }

    var funcion = function ()
    {
        Causas.splice(indexCausa, 1);
           
        //Genera nuevamente la tabla
        dataTable = GeneraTablaDatos(dataTable, "dataTable", Causas, EstructuraTablaCausas, false, false, false);

        if (Causas.length == 0)
        {
            $("#contenedorBeneficiario").hide();
        }
    }

    var mensaje = "¿Desea retirar la Causa <b>" + Causas[indexCausa].CausaNuc + "</b> de la tabla?"; 

    MensajeDeConfirmacion(mensaje, "large", funcion);
}
// #endregion 

// #region Tocas
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

        AlertaCallback("El numero de toca que intenta añadir, es mayor al año actual", funcion);
        return;
    }

    if (ValidarTocaEnTabla(numToca))
    {
        var mensaje = "El numero de Toca <b>" + numToca + "</b> que intenta agregar, ya se encuentra en la tabla.";

        var funcion = function ()
        {
            LimpiaValidacion("formTocas", "inpToca");
        }

        AlertaCallback(mensaje, funcion);
    }
    else
    {
        var nombreSelect = !esTradicional ? "slctSalaAcusatorio" : "slctSalaTradicional";
            
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
        tocas.push(toca);

        //Generar Tabla
        dataTableTocas = GeneraTablaDatos(dataTableTocas, "dataTableTocas", tocas, estructuraTablaTocas, false, false, false);

        //Limpia form de tocas despues de agregar toca a la tabla
        LimpiaValidacion("formTocas", "inpToca");
    }
}

function ValidarTocaEnTabla(numeroToca)
{
    for (var index = 0; index < tocas.length; index++)
    {
        if (tocas[index].numeroDeToca == numeroToca)
        {
            return true;
        }
    }
    return false;
}

function EliminarToca(id)
{
    var indexToca = 0;
    var iterarArreglo = tocas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == tocas[index].id)
        {
            indexToca = index;
        }
    }

    var funcion = function ()
    {        
        tocas.splice(indexToca, 1);

        //Genera nuevamente la tabla
        dataTableTocas = GeneraTablaDatos(dataTableTocas, "dataTableTocas", tocas, estructuraTablaTocas, false, false, false);
    }

    var mensaje = "¿Desea retirar la Toca <b>" + tocas[indexToca].numeroDeToca + "</b> de la tabla?";
    MensajeDeConfirmacion(mensaje, "large", funcion);
}
// #endregion 

// #region Amparos
function AgregaAmparos()
{
    var numAmparo = $("#ipnAmparo").val();

    if (ValidarAmparoEnTabla(numAmparo))
    {
        var mensaje = "El numero de Amparo <b>" + numAmparo + "</b> que intenta agregar, ya se encuentra en la tabla.";

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
        amparos.push(amparo);

        //Generar Tabla
        dataTableAmparos = GeneraTablaDatos(dataTableAmparos, "dataTableAmparos", amparos, estructuraTablaAmparos, false, false, false);

        LimpiaValidacion("formAmparos", "ipnAmparo");
    }
}

function ValidarAmparoEnTabla(numeroAmparo)
{
    for (var index = 0; index < amparos.length; index++)
    {
        if (amparos[index].amparo == numeroAmparo)
        {
            return true;
        }
    }
    return false;
}

function EliminarAmparo(id)
{
    var iterarArreglo = amparos;
    var indexAmparo = 0;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == amparos[index].id)
        {
            indexAmparo = index;
        }
    }

    var funcion = function ()
    {
        amparos.splice(indexAmparo, 1);
        //Genera nuevamente la tabla
        dataTableAmparos = GeneraTablaDatos(dataTableAmparos, "dataTableAmparos", amparos, estructuraTablaAmparos, false, false, false);
    }

    var mensaje = "¿Desea retirar el nuemro de Amparo <b>" + amparos[indexAmparo].amparo + "</b> de la tabla?";
    MensajeDeConfirmacion(mensaje, "large", funcion);
}

function GeneraArregloNumeroAmparos()
{
    var numeroAmparos = [];

    for (var index = 0; index < amparos.length; index++)
    {
        numeroAmparos.push(amparos[index].amparo);
    }

    return numeroAmparos;
}
// #endregion 

// #region Anexos
function AgregarAnexosInicales()
{
    var id = $("#slctAnexosInicales").find('option:selected').val();
    var descripcionAnexo = $("#slctAnexosInicales").find('option:selected').text();
    var cantidadAnexo = $("#inpAddAnexos").val(); 
    var idAnexo = id;

    if (id == idOtroAnexos)
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
        anexos.push(anexoIniciales);
    }

    //Generar Tabla
    dataTableAnex = GeneraTablaDatos(dataTableAnex, "dataTableAnexos", anexos, estructuraTablaAnexos, false, false, false);

    if (anexos.length > 0)
    {
        $('#botonEnviar').removeAttr('disabled');
    }

    LimpiaValidacion("formAnexos", "inpAddAnexos");
}

function EliminarAnexo(id)
{
    var iterarArreglo = anexos;
    var indexArreglo = 0;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (id == anexos[index].id)
        {
            indexArreglo = index;
        }
    }

    var funcion = function ()
    {
        anexos.splice(indexArreglo, 1);
        //Genera nuevamente la tabla
        dataTableAnex = GeneraTablaDatos(dataTableAnex, "dataTableAnexos", anexos, estructuraTablaAnexos, false, false, false);

        if (anexos.length == 0)
        {
            $("#botonEnviar").prop('disabled', true);
        }
    }

    var mensaje = "¿Desea retirar el Anexo <b>" + anexos[indexArreglo].descripcion + " (Cantidad : " + anexos[indexArreglo].cantidad + ")</b>   de la tabla?";
    MensajeDeConfirmacion(mensaje, "large", funcion);
}

function ValidarAnexoEnTabla(id, cantidad)
{
    for (var index = 0; index < anexos.length; index++)
    {
        if (anexos[index].id == id)
        {
            var cantidadActual = parseInt(anexos[index].cantidad);
            var total = cantidadActual + parseInt(cantidad);
            anexos[index].cantidad = total;
            return true;
        }
    }
    return false;
}
// #endregion 

// #region Ejecucion
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
        Causas: Causas,
        Tocas: tocas,
        Amparos: GeneraArregloNumeroAmparos(),
        Anexos: anexos,
        IdSolicitante: $("#slctSolicitante").find('option:selected').val(),
        DetalleSolicitante: $("#ipnDetalleSolicitante").val(),
        IdSolicitud: $("#slctSolicitud").find('option:selected').val(),
        OtraSolicita: $("#inpOtraSolicitud").val()
    };

    SolicitudEstandarPostAjax('/Iniciales/CrearEjecucion', parametros, RederizarDetalleSuccess, RederizarDetalleError);
}

function RederizarDetalleSuccess(data)
{
    if (data.Estatus == EstatusRespuesta.OK)
    {
        var url = data.Data.Url;

        ////Redirecciona a la vista detalle
        document.location.href = url; 
    }
    else if (data.Estatus == EstatusRespuesta.ERROR)
    {
        $("#loading").fadeOut();

        if (intentos > 2)
        {
            var mensaje = "Mensaje: " + data.Mensaje + ". <br><br>Intentos: " + intentos + "<br><br><b>Ha superado el numero maximo de intentos, vuelva intentarlo mas tarde o consulte a soporte</b";
            intentos = 0;

            Alerta(mensaje, "large");
        }
        else
        {
            var mensaje = "Mensaje: " + data.Mensaje + ", de click en Aceptar para intentar crear el registro nuevamente.<br><br>Intentos: " + intentos;

            reintento = true;
            MensajeDeConfirmacion(mensaje, "large", GenerarEjecucion, null, titulo = "Error no controlado por el sistema");
        }
    }
}

function RederizarDetalleError(data)
{
    $("#loading").fadeOut();

    if (intentos > 2)
    {
        var mensaje = "Mensaje: " + data + ". <br><br>Intentos: " + intentos + "<br><br><b>Ha superado el numero maximo de intentos, vuelva intentarlo mas tarde o consulte a soporte</b";
        intentos = 0;

        Alerta(mensaje, "large");
    }
    else
    {
        var mensaje = "Mensaje: " + data + ", de click en Aceptar para intentar crear el registro nuevamente.<br><br>Intentos: " + intentos;

        reintento = true;
        MensajeDeConfirmacion(mensaje, "large", GenerarEjecucion, null, titulo = "Error no controlado por el sistema");
    }
}
// #endregion 

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

function SolicitudEstandarPostAjax(urlAction, parameters, functionCallbackSuccess, functionCallbackError = null)
{
    $.ajax({
        url: urlAction,
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
