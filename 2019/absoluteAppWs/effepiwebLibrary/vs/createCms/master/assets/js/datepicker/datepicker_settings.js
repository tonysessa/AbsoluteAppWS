$(function () {
    $(".from-date").datepicker({
        defaultDate: "+1d",
        minDate: 0,
        onClose: function (selectedDate) {
            $(".to-date").datepicker("option", "minDate", selectedDate);
        }
    });
    $(".to-date").datepicker({
        defaultDate: "+1d",
        minDate: 0,
        onClose: function (selectedDate) {
            $(".from-date").datepicker("option", "maxDate", selectedDate);
        }
    });
    $(".set-date").datepicker();

});