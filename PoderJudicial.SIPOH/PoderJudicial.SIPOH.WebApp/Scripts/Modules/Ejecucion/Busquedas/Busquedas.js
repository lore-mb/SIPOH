
$("#divPartes").hide();
$("#divPartes").hide();
$("#divPartest").hide();
$("#divSentenciado").hide();
$("#divSentenciadot").hide();
$("#divCirNumC").hide();
$("#divJNumC").hide();
$("#divInpCausa").hide();
$("#divNumCt").hide();
$("#divBotonC").hide();
$("#divCirNUC").hide();
$("#divInpNUC").hide();
$("#divSolic").hide();
$("#divSolict").hide();
$("#divDsolicitante").hide();
$("#divDsolicitantet").hide();
$("#divInpDsolicitante").hide();


/*Mostrar, Ocultar elementos respecto a TIPO de busqueda*/
$(document).ready(function () {

    $("#inlineFormCustomSelect").change(function () {
        if ($(this).val() == "1") {
            $('#divPartes').show();
            $('#divPartest').show();
            $("#divSentenciado").hide();
            $("#divSentenciadot").hide();
            $("#divCirNumC").hide();
            $("#divNumCt").hide();
            $("#divBotonC").hide();
            $("#divCirNUC").hide();
            $("#divInpNUC").hide();
            $("#divSolic").hide();
            $("#divSolict").hide();
            $("#divDsolicitante").hide();
            $("#divDsolicitantet").hide();
            $("#divInpDsolicitante").hide();

        } else if ($("#inlineFormCustomSelect").val() == "0") {
            $("#divPartes").hide();
            $("#divPartest").hide();
            $("#divSentenciado").hide();
            $("#divSentenciadot").hide();
            $("#divCirNumC").hide();
            $("#divNumCt").hide();
            $("#divBotonC").hide();
            $("#divCirNUC").hide();
            $("#divInpNUC").hide();
            $("#divSolic").hide();
            $("#divSolict").hide();
            $("#divDsolicitante").hide();
            $("#divDsolicitantet").hide();
            $("#divInpDsolicitante").hide();

        } else if ($("#inlineFormCustomSelect").val() == "2") {
            $('#divPartes').hide();
            $('#divPartest').hide();
            $("#divSentenciado").show();
            $("#divSentenciadot").show();
            $("#divCirNumC").hide();
            $("#divNumCt").hide();
            $("#divBotonC").hide();
            $("#divCirNUC").hide();
            $("#divInpNUC").hide();
            $("#divSolic").hide();
            $("#divSolict").hide();
            $("#divDsolicitante").hide();
            $("#divDsolicitantet").hide();
            $("#divInpDsolicitante").hide();

        } else if ($("#inlineFormCustomSelect").val() == "3") {
            $('#divPartes').hide();
            $('#divPartest').hide();
            $("#divSentenciado").hide();
            $("#divSentenciadot").hide();
            $("#divCirNumC").show();
            $("#divNumCt").show();
            $("#divJNumC").show();
            $("#divInpCausa").show();
            $("#divBotonC").show();
            $("#divCirNUC").hide();
            $("#divInpNUC").hide();
            $("#divSolic").hide();
            $("#divSolict").hide();
            $("#divDsolicitante").hide();
            $("#divDsolicitantet").hide();
            $("#divInpDsolicitante").hide();

        } else if ($("#inlineFormCustomSelect").val() == "4") {
            $('#divPartes').hide();
            $('#divPartest').hide();
            $("#divSentenciado").hide();
            $("#divSentenciadot").hide();
            $("#divCirNumC").hide();
            $("#divNumCt").hide();
            $("#divJNumC").hide();
            $("#divInpCausa").hide();
            $("#divBotonC").hide();
            $("#divCirNUC").show();
            $("#divInpNUC").show();
            $("#divSolic").hide();
            $("#divSolict").hide();
            $("#divDsolicitante").hide();
            $("#divDsolicitantet").hide();
            $("#divInpDsolicitante").hide();

        } else if ($("#inlineFormCustomSelect").val() == "5") {
            $('#divPartes').hide();
            $('#divPartest').hide();
            $("#divSentenciado").hide();
            $("#divSentenciadot").hide();
            $("#divCirNumC").hide();
            $("#divNumCt").hide();
            $("#divJNumC").hide();
            $("#divInpCausa").hide();
            $("#divBotonC").hide();
            $("#divCirNUC").hide();
            $("#divInpNUC").hide();
            $("#divSolic").show();
            $("#divSolict").show();
            $("#divDsolicitante").hide();
            $("#divDsolicitantet").hide();
            $("#divInpDsolicitante").hide();



        } else if ($("#inlineFormCustomSelect").val() == "6") {
            $('#divPartes').hide();
            $('#divPartest').hide();
            $("#divSentenciado").hide();
            $("#divSentenciadot").hide();
            $("#divCirNumC").hide();
            $("#divNumCt").hide();
            $("#divJNumC").hide();
            $("#divInpCausa").hide();
            $("#divBotonC").hide();
            $("#divCirNUC").hide();
            $("#divInpNUC").hide();
            $("#divSolic").hide();
            $("#divSolict").hide();
            $("#divDsolicitante").show();
            $("#divDsolicitantet").show();
            $("#divInpDsolicitante").show();
        }


    });
});


