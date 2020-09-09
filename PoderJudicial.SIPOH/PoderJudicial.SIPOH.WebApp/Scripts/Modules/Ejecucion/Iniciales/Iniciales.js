//region Varaibles Globale!
var EstatusRespuesta = { SIN_RESPUESTA: 0, OK: 1, ERROR: 2 }
var idCircuito = null;
var idDistrito = null;

var dataTable = null;
var dataTableAnex = null;
var dataTableBeneficiario = null;
var dataTableTocas = null;
var dataTableAmparos = null;

//Estructura para data tables
var estructuraTablaCausas = [{ data: 'nJuzgado', title: 'N° Juzgado' }, { data: 'causaNuc', title: 'Causa|Nuc' }, { data: 'ofendido', title: 'Ofendido (s)' }, { data: 'inculpado', title: 'Inculpado (s)' }, { data: 'delito', title: 'Delitos (s)' }, { data: 'eliminar', title: 'Quitar' }];
var causas = [];

var estructuraTablaAnexos = [{ data: 'cantidad', title: 'Cantidad' }, { data: 'descripcion', title: 'Descripción' }, { data: 'eliminar', title: 'Quitar' }];
var anexos = [];

var estructuraTablaBeneficiarios = [{ data: 'numeroEjecucion', title: 'No. Ejecución' }, { data: 'nombreJuzgado', title: 'Juzgado', width: "60%" }, { data: 'nombreBeneficiario', title: 'Nombre (s)' }, { data: 'apellidoPaterno', title: 'Apellido Paterno' }, { data: 'apellidoMaterno', title: 'Apellido Materno' }, { data: 'fechaEjecucion', title: 'Fecha de Ejecución' }];
var beneficarios = [];

var estructuraTablaTocas = [{ data: 'sala', title: 'Sala' }, { data: 'numeroToca', title: 'Numero De Toca' }, { data: 'eliminar', title: 'Quitar' }];
var tocas = [];

var estructuraTablaAmparos = [{ data: 'amparo', title: 'Numero de Amparo' }, { data: 'eliminar', title: 'Quitar' }];
var amparos = [];

var encontroBeneficiarios = false;
var mostrarSeccionesBeneficiario = false;
var formEjecucionValidado = false;
var esTradicional = false;

//Funciones que se detonan al terminado del renderizado 
$(document).ready(function ()
{
    //Pintar Tablas
    dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);
    dataTableAnex = GeneraTablaDatos(dataTableAnex, "dataTableAnexos", anexos, estructuraTablaAnexos, false, false, false);
    dataTableTocas = GeneraTablaDatos(dataTableTocas, "dataTableTocas", tocas, estructuraTablaTocas, false, false, false);
    dataTableAmparos = GeneraTablaDatos(dataTableAmparos, "dataTableAmparos", anexos, estructuraTablaAmparos, false, false, false);

    //Obtener Circuito
    idCircuito = $("#IdCircuitoHDN").val();

    //Elemntos al Cargado
    ElementosAlCargado();
});

// #region Variables SELECT Juzgado Acusatorio
var InputNumero = $("#Numero");
var lblNumero = $("#NumeroLabel");
var slctNumero = $("#slctNumero");
// #endregion 

// #region FORMAT LOAD Juzgado Acusatorio
slctNumero.prop('selectedIndex', 0);
InputNumero.val('');
$("#spnNumero").text('CAUSA');
InputNumero.inputmask("9999/9999");
//var ValorSeleccionado = $(this).children("option:selected").val();
// #endregion

// #region CHANGE Juzgado Acusatorio
slctNumero.change(function () {
    if ($(this).val() == 1) {
        // #region Acusatorio
        InputNumero.inputmask("9999/9999");
        InputNumero.val('');
        $("#spnNumero").text('CAUSA');
        lblNumero.html("Numero de Causa:");
        // #endregion
    } else if ($(this).val() == 2) {
        InputNumero.inputmask("99-9999-9999");
        InputNumero.val('');
        $("#spnNumero").text('NUC');
        lblNumero.html("Numero Unico de Caso:");
    }
});
// #endregion

// #region Varibles SELECT Juzgado Tradicional
var slctNumeroT = $("#slctNumeroT");
var InpNumeroT = $("#NumeroT");
var lblNumeroT = $("#NumeroLabelT");
// #endregion

// #region FORMAT LOAD Juzgado Tradicional
slctNumeroT.prop('selectedIndex', 0);
InpNumeroT.val('');
$("#spnNumeroT").text('CAUSA');
InpNumeroT.inputmask("9999/9999");
// #endregion

// #region CHANGE Juzgado Tradicional
slctNumeroT.change(function () {
    if ($(this).val() == 1) {
        InpNumeroT.inputmask("9999/9999");
        InpNumeroT.val('');
        $("#spnNumeroT").text('CAUSA');
        lblNumeroT.html("Numero de Causa:");
    } else if ($(this).val() == 2) {
        InpNumeroT.inputmask("99-9999-9999");
        InpNumeroT.val('');
        $("#spnNumeroT").text('NUC');
        lblNumeroT.html("Numero Unico de Caso:");
    }
});
// #endregion

//Elementos al Cargado
function ElementosAlCargado()
{
    $("#contenedorBeneficiario").hide();
    $("#seccionBeneficiario").hide();
    $("#seccionBusquedaAnexos").hide();
    $("#seccionTablaAnexos").hide();
    $("#seccionBotonGuardar").hide();

    $(document).on("keydown", ":input:not(textarea)", function (event)
    {
        if (event.key == "Enter")
        {
            event.preventDefault();
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
                alert("Se crea el registro de ejecución");   
            }

            if (id == "formEjecucion")
            {
                formEjecucionValidado = true;
            }

            form.classList.add('was-validated');

        }, false);
    });

    $("#inpBusquedaSentenciado").val("Total : 0");   

    $("#slctJuzgadoTradi").prop('disabled', true);

    $('#slctDistrito').change(function ()
    {
        idDistrito = $("#slctDistrito").find('option:selected').val();

        //Metodo que contiene el proceso para llenado de pick List Juzgados Tradicionales
        if (idDistrito != "" && idDistrito != null)
        {
            $("#slctJuzgadoTradi").prop('disabled', false);
            DistritoJuzgadoTradicional();
        }
        else
        {
            $("#slctJuzgadoTradi").prop('disabled', true);
            $("#slctJuzgadoTradi").html("");
            $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));
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

        $("#botonMostrarBeneficiarios").removeClass("btn-info");
        $("#botonMostrarBeneficiarios").addClass("btn-secondary");

        beneficarios = [];
        $('#inpApellidoPaterno').val("");
        $('#inpApellidoMaterno').val("");
        $('#inpNombreSentenciado').val("");

        encontroBeneficiarios = false;
        $("#inpBusquedaSentenciado").val("Total : 0");
        $("#inpBusquedaSentenciado").css('border', function ()
        {
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

        if (value == "O")
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

    $('#slctAnexosInicales').change(function ()
    {
        var value = $("#slctAnexosInicales").find('option:selected').val();

        if (value == "8")
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
}

function ValidarBeneficiarios()
{
    beneficarios = [];

    var apellidoPBene = $('#inpApellidoPaterno').val();
    var apellidoMBene = $('#inpApellidoMaterno').val();
    var nombreBene = $('#inpNombreSentenciado').val();

    if (nombreBene != "" && apellidoPBene != "") {
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
            $("#botonMostrarBeneficiarios").removeClass("btn-info");
            $("#botonMostrarBeneficiarios").addClass("btn-secondary");

            $("#inpBusquedaSentenciado").val("Total : 0");
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
        var iterarArreglo = data.Data.beneficiarios;
       
        for (var index = 0; index < iterarArreglo.length; index++)
        {
            var beneficiario = new Object();
            beneficiario.id = iterarArreglo[index].IdEjecucion;
            beneficiario.numeroEjecucion = iterarArreglo[index].NumeroEjecucion;
            beneficiario.nombreJuzgado = iterarArreglo[index].NombreJuzgado;
            beneficiario.nombreBeneficiario = iterarArreglo[index].NombreBeneficiario;
            beneficiario.apellidoPaterno = iterarArreglo[index].ApellidoPBeneficiario;
            beneficiario.apellidoMaterno = iterarArreglo[index].ApellidoMBeneficiario;
            beneficiario.fechaEjecucion = iterarArreglo[index].FechaEjecucion;
            beneficarios.push(beneficiario);
        }

        $("#inpBusquedaSentenciado").val("Total : " + data.Data.total);
        
        //Pintar en mostrar Bene
        $("#botonMostrarBeneficiarios").removeClass("btn-secondary");
        $("#botonMostrarBeneficiarios").addClass("btn-info");

        dataTableBeneficiario = GeneraTablaDatos(dataTableBeneficiario, "dataTableBeneficiarios", beneficarios, estructuraTablaBeneficiarios, true, true, false);
    }
    else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA)
    {
        encontroBeneficiarios = false;
        $("#inpBusquedaSentenciado").val("Total : 0");  
        $("#inpBusquedaSentenciado").css('border', function ()
        {
            return '1px solid #b0bec5';
        });

        $("#botonMostrarBeneficiarios").removeClass("btn-info");
        $("#botonMostrarBeneficiarios").addClass("btn-secondary");

        $("#botonCerrarBeneficiarios").removeClass("btn-secondary");
        $("#botonCerrarBeneficiarios").addClass("btn-danger")

        $("#seccionBeneficiario").show();
        $("#seccionBusquedaAnexos").show();
        $("#seccionTablaAnexos").show();
        $("#seccionBotonGuardar").show();

        mostrarSeccionesBeneficiario = true;
    }
    else
    {

    }
}

function DistritoJuzgadoTradicional()
{
    if (idDistrito != null)
    {
        var parametros = { idDistrito: idDistrito }
        SolicitudEstandarAjax("/Iniciales/ObtenerJuzgadoTradicional", parametros, ListarJuzgadoTradicional);
    }
    else
    {
        alert("Error al obtener los datos");
    }
}

function ListarJuzgadoTradicional(data)
{
    if (data.Estatus == EstatusRespuesta.OK)
    {
        var numero = data.Data.length;

        $("#slctJuzgadoTradi").html("");

        if (numero > 1)
        {
            $("#slctJuzgadoTradi").append($("<option/>", { value: "", text: "SELECCIONAR OPCIÓN" }));
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
        customNotice(data.Mensaje, "Error:", "error", 3350);
    }
}

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
            var causaNuc = expediente.NumeroCausa == null ? expediente.NUC : expediente.NumeroCausa;
            var causaNucLabel = expediente.NumeroCausa == null ? "NUC " : "Numero de causa ";

            if (ValidarCuasaEnTabla(expediente.IdExpediente))
            {
                var mensaje = causaNucLabel + "<b>" + causaNuc + "</b> que intenta agregar, ya se encuentra en la tabla." ;  
                Alerta(mensaje);
            }
            else
            {     
                var mensaje = "La consulta generó un resultado exitoso para el " + causaNucLabel + " <b>" + causaNuc + "</b> asignado al juzgado de procedencia <b>" + expediente.NombreJuzgado + "</b> seleccionado.<br><br> ¿Desea Continuar? <br>";

                var funcion = function ()
                {
                    if (causas.length == 0)
                    {
                        $('#contenedorBeneficiario').removeAttr('hidden');
                        $("#contenedorBeneficiario").show();
                    }
                    var causa = new Object();
                    causa.id = expediente.IdExpediente;
                    causa.nJuzgado = expediente.NombreJuzgado;
                    causa.causaNuc = causaNuc;
                    causa.ofendido = expediente.Ofendidos;
                    causa.inculpado = expediente.Inculpados;
                    causa.delito = expediente.Delitos;
                    causa.eliminar = "<button class='btn btn-danger btn-sm' onclick='EliminarCausa(" + causa.id + ")'><i class='fas fa-trash-alt'></i></button>";
                    //Agrega Causa al Arreglo de Cuasas
                    causas.push(causa);
                    //Generar Tabla
                    dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);    
                }

                //Imprime Mensaje de Confiracion
                MensajeDeConfirmacion(mensaje, "large",funcion);
            }
        }
        else if (data.Estatus == EstatusRespuesta.ERROR)
        {
            Alerta(data.Mensaje);
        }
        else if (data.Estatus == EstatusRespuesta.SIN_RESPUESTA)
        {
            Alerta(data.Mensaje);
        }
}

function ValidarCuasaEnTabla(id)
{
    var iterarArreglo = causas;

    for (var index = 0; index < iterarArreglo.length; index++)
    {
        if (causas[index].id == id)
        {
            return true;
        }
    }
    return false;
}

function EliminarCausa(id) 
{
    var funcion = function ()
    {
        var iterarArreglo = causas;

        for (var index = 0; index < iterarArreglo.length; index++)
        {
            if (id == causas[index].id)
            {
                causas.splice(index, 1);
            }
        }

        //Genera nuevamente la tabla
        dataTable = GeneraTablaDatos(dataTable, "dataTable", causas, estructuraTablaCausas, false, false, false);

        if (causas.length == 0)
        {
            $("#contenedorBeneficiario").hide();
        }
    }

    var mensaje = "¿Desea retirar la causa de la tabla?"; 
    MensajeDeConfirmacion(mensaje, "small", funcion);
}

function AgregarTocas()
{
    var numToca = $("#inpToca").val();
    var anioActual = new Date().getFullYear();
    var anioToca = numToca.substr(5, 4); 

    if (anioToca > anioActual)
    {
        Alerta("El numero de toca que intenta añadir, es mayor al año actual");
        return;
    }

    if (ValidarTocaEnTabla(numToca))
    {
        var mensaje = "El numero de Toca <b>" + numToca + "</b> que intenta agregar, ya se encuentra en la tabla.";
        Alerta(mensaje);
    }
    else
    {
        var nombreSelect = !esTradicional ? "slctSalaAcusatorio" : "slctSalaTradicional";
            
        var idJuzgado = $("#" + nombreSelect).find('option:selected').val();
        var nombreJuzgado = $("#" + nombreSelect).find('option:selected').text();

        //Se genera un numero aleatorio para asignar un Id a la toca agregada a la tabla
        var numRamdom = Math.floor(Math.random() * 90000) + 10000;

        var toca = new Object();
        toca.id = numToca.substr(0, 4) + numRamdom;
        toca.idJuzgado = idJuzgado;
        toca.sala = nombreJuzgado;
        toca.numeroToca = numToca;
        toca.eliminar = "<button class='btn btn-danger btn-sm' onclick='EliminarToca(" + toca.id + ")'><i class='fas fa-trash-alt'></i></button>";
        tocas.push(toca);

        //Generar Tabla
        dataTableTocas = GeneraTablaDatos(dataTableTocas, "dataTableTocas", tocas, estructuraTablaTocas, false, false, false);
    }
}

function ValidarTocaEnTabla(numeroToca)
{
    for (var index = 0; index < tocas.length; index++)
    {
        if (tocas[index].numeroToca == numeroToca)
        {
            return true;
        }
    }
    return false;
}

function EliminarToca(id)
{
    var funcion = function ()
    {
        var iterarArreglo = tocas;

        for (var index = 0; index < iterarArreglo.length; index++)
        {
            if (id == tocas[index].id)
            {
                tocas.splice(index, 1);
            }
        }

        //Genera nuevamente la tabla
        dataTableTocas = GeneraTablaDatos(dataTableTocas, "dataTableTocas", tocas, estructuraTablaTocas, false, false, false);
    }

    var mensaje = "¿Desea retirar la toca de la tabla?";
    MensajeDeConfirmacion(mensaje, "small", funcion);
}

function AgregaAmparos()
{
    var numAmparo = $("#ipnAmparo").val();

    if (ValidarAmparoEnTabla(numAmparo))
    {
        var mensaje = "El numero de Amparo <b>" + numAmparo + "</b> que intenta agregar, ya se encuentra en la tabla.";
        Alerta(mensaje);
    }
    else
    {
        //Se genera un numero aleatorio para asignar un Id a la toca agregada a la tabla
        var numRamdom = Math.floor(Math.random() * 90000) + 10000;

        var amparo = new Object();
        amparo.id = numRamdom;
        amparo.amparo = numAmparo;
        amparo.eliminar = "<button class='btn btn-danger btn-sm' onclick='EliminarAmparo(" + amparo.id + ")'><i class='fas fa-trash-alt'></i></button>";
        amparos.push(amparo);

        //Generar Tabla
        dataTableAmparos = GeneraTablaDatos(dataTableAmparos, "dataTableAmparos", amparos, estructuraTablaAmparos, false, false, false);
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
    var funcion = function ()
    {
        var iterarArreglo = amparos;

        for (var index = 0; index < iterarArreglo.length; index++)
        {
            if (id == amparos[index].id)
            {
                amparos.splice(index, 1);
            }
        }

        //Genera nuevamente la tabla
        dataTableAmparos = GeneraTablaDatos(dataTableAmparos, "dataTableAmparos", amparos, estructuraTablaAmparos, false, false, false);
    }

    var mensaje = "¿Desea retirar el amparo de la tabla?";
    MensajeDeConfirmacion(mensaje, "small", funcion);
}

function AgregarAnexosInicales()
{
    var tipo = $("#slctAnexosInicales").find('option:selected').val();
    var descripcionAnexo = $("#slctAnexosInicales").find('option:selected').text();
    var cantidadAnexo = $("#inpAddAnexos").val(); 
    var idAnexo = tipo;

    if (tipo == "8")
    {
        descripcionAnexo = $("#inpOtroAnexo").val(); 
        idAnexo = Math.floor(Math.random() * 90000) + 10000;
    }

    if (!ValidarAnexoEnTabla(idAnexo, cantidadAnexo))
    {
        var anexoIniciales = new Object();
        anexoIniciales.id = idAnexo;
        anexoIniciales.descripcion = descripcionAnexo;
        anexoIniciales.cantidad = cantidadAnexo;
        anexoIniciales.eliminar = "<button class='btn btn-danger btn-sm' onclick='EliminarAnexo(" + anexoIniciales.id + ")'><i class='fas fa-trash-alt'></i></button>";

        anexos.push(anexoIniciales);
    }

    //Generar Tabla
    dataTableAnex = GeneraTablaDatos(dataTableAnex, "dataTableAnexos", anexos, estructuraTablaAnexos, false, false, false);
}

function EliminarAnexo(id)
{
    var funcion = function ()
    {
        var iterarArreglo = anexos;

        for (var index = 0; index < iterarArreglo.length; index++)
        {
            if (id == anexos[index].id)
            {
                anexos.splice(index, 1);
            }
        }

        //Genera nuevamente la tabla
        dataTableAnex = GeneraTablaDatos(dataTableAnex, "dataTableAnexos", anexos, estructuraTablaAnexos, false, false, false);
    }

    var mensaje = "¿Desea retirar el anexo de la tabla?";
    MensajeDeConfirmacion(mensaje, "small", funcion);
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
            }
        }
    });
}

function MensajeDeConfirmacion(mensaje, tamanio, funcion)
{
    bootbox.confirm({
        title: "<b>Mensaje de Confirmación</b>",
        message: mensaje,
        buttons: {
            confirm: {
                label: 'Aceptar',
                className: 'btn btn-outline-success'
            },
            cancel: {
                label: 'Cancelar',
                className: 'btn btn-outline-secondary'
            }
        },
        callback: function (result)
        {
            if (result)
            {
                funcion();
            }
        },
        size: tamanio
    });
}

function Alerta(mensaje)
{
    bootbox.alert({
        title: "<b>Mensaje</b>",
        message: mensaje,
        buttons:
        {
            ok:
            {
                label: 'Aceptar',
                className: 'btn btn-outline-success'
            }
        }
    });
}


//// Dev TOCAS Y AMPAROS //

//$("#FormTocas").hide();

//function Parametros_Sala_Tradicional() {
//    SolicitudEstandarAjax("/Iniciales/ObtenerSalaTradicional", "", Obtener_Sala_Tradicional);
//}
//function Obtener_Sala_Tradicional(data) {
//    if (data.Estatus == EstatusRespuesta.OK) {
//        const ArraySalaTradicional = [data.Data];
//        var $SelectSalaAcusatorio = $("#comboSala");
//        $.each(ArraySalaTradicional, function (id, sala) {
//            for (var i = 0; i < sala.length; i++) {
//                $SelectSalaAcusatorio.append('<option value=' + sala[i].Value + '>' + sala[i].Text + '</option>');
//            }
//        });
//        Parametros_Sala_Acusatorio();
//    } else if (data.Estatus == EstatusRespuesta.ERROR) {
//        customNotice(data.Mensaje, "Error:", "error", 3350);
//    }
//}

//function Parametros_Sala_Acusatorio() {
//    SolicitudEstandarAjax("/Iniciales/ObtenerSalaAcusatorio", "", Obtener_Sala_Acusatorio);
//}
//function Obtener_Sala_Acusatorio(data) {
//    if (data.Estatus = EstatusRespuesta.OK) {
//        var Array_Sala_Acusatorio = [data.Data];
//        var $ComboIndex = $("#comboSala_Acusatorio");
//        $.each(Array_Sala_Acusatorio, function (id, sala) {
//            for (var i = 0; i < sala.length; i++) {
//                $ComboIndex.append('<option value=' + sala[i].Value + '>' + sala[i].Text +'</option>');
//            }
//        });
//    } else if (data.Estatus == EstatusRespuesta.ERROR) {
//        customNotice(data.Mensaje, "Error:", "error", 3350);
//    }
//}

//function Obtener_Val_Sala_Acusatorio() {
//    var Val_Combo = $("#comboSala_Acusatorio").find('option:selected').val();
//    var Val_Input = $("#Sala_Acusatorio").val();
//    alert(Val_Combo);
//    alert(Val_Input);
//}


//function Pintar_Tabla_Sala_Acusatorio() {
    
//}