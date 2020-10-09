$(function () {
    $('#datetimepicker').datetimepicker({
        useCurrent: false 
    });
    $("#datetimepicker").on("dp.change", function (e) {
        $('#datetimepicker').data("DateTimePicker").minDate(e.date);
    });
  
});