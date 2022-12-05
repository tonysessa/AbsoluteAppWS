$(document).ready(function () {

    //
    //$('#eventDateRange').daterangepicker();

    //
    $('#external-events div.external-event').each(function () {
        var eventObject = {
            title: $.trim($(this).text()),
            categoryTitle: $.trim($(this).text()),
            description: "",
            idCategory: $(this).attr('data-id'),
            uri: $(this).attr('data-uri'),
        };
        $(this).data('eventObject', eventObject);
        $(this).draggable({
            zIndex: 999,
            revert: true,
            revertDuration: 0
        });
    });

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $('#calendar').fullCalendar({
        header: {
            right: 'agendaDay,agendaWeek,month',
            center: 'prev, next'
        },
        defaultView: 'month',
        editable: true,
        allDaySlot: false,
        slotDuration: '00:30:00',
        minTime: '08:00:00',
        maxTime: '22:00:00',

        //CUSTOM ITA
        monthNames: ['Gennaio', 'Febbraio', 'Marzo', 'Aprile', 'Maggio', 'Giugno', 'Luglio', 'Agosto', 'Settembre', 'Ottobre', 'Novembre', 'Dicembre'],
        monthNamesShort: ['Gen', 'Feb', 'Mar', 'Apr', 'Mag', 'Giu', 'Lug', 'Ago', 'Set', 'Ott', 'Nov', 'Dic'],
        dayNames: ['Domenica', 'Lunedi', 'Martedi', 'Mercoledi', 'Giovedi', 'Venerdi', 'Sabato'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mer', 'Gio', 'Ven', 'Sab'],
        allDayDefault: true,
        ignoreTimezone: true,
        isRTL: false,
        firstDay: 0,
        // event ajax
        lazyFetching: true,
        startParam: 'start',
        endParam: 'end',
        axisFormat: 'HH:mm',
        timeFormat: {
            agenda: 'H:mm{ - h:mm}'
        },
        //

        buttonText: {
            prev: '< prec',
            next: 'succ >',
            today: 'Oggi',
            month: 'Mese',
            week: 'Settimana',
            day: 'Giorno'
        },
        droppable: true,
        drop: function (date, allDay) {
            var originalEventObject = $(this).data('eventObject');
            var copiedEventObject = $.extend({}, originalEventObject);
            //
            copiedEventObject.start = date;
            copiedEventObject.allDay = allDay;
            copiedEventObject.descrizione = '';
            //
            if (originalEventObject.uri.toUpperCase() == "MULTI") {
                // Multi
                $('#calendar').fullCalendar('renderEvent', copiedEventObject, false);
            }
            else
                UpsertEvent(copiedEventObject);
        },
        events: {
            //
            url: AppWsUrl + calendarMethod + "?uid_CmsNlsContext=" + Uid_CmsNlsContext + "&uid_CmsUsers=" + Uid_CmsUsers,
            error: function () {
                alert('error');
            },
            success: function () {
                // console.log("success");
            }
        },
        loading: function (bool) {
            //
            $('#loading').toggle(bool);
        },
        eventDrop: function (event, element) {
            //
            currentEvent = null;
            //
            UpdateAppointment(event, element);
        },
        eventResize: function (event, delta, revertFunc, jsEvent, ui, view) {

            //
            currentEvent = null;

            //
            UpdateAppointment(event);
        },
        eventRender: function (event, element) {
            //
            // Abilito Layer Per Allievo
            $(element).attr('data-toggle', 'modal');
            if (event.uri.toUpperCase() == 'MULTI') {
                $(element).attr('data-target', '.lnkModalRange');
            } else {
                $(element).attr('data-target', '.lnkModalDelails');
            }
            //
            if (!event.currentCalendar) {
                var sClass = $(element).attr('class') + ' other';
                $(element).attr('class', sClass);
            }

            //
            currentEvent = event;
        },
        eventClick: function (event, element) {
            currentEvent = event;   

            if (event.uri.toUpperCase() == 'MULTI') {
                var svaldate = Padder(currentEvent.start.getDate().toString(), 2, "0") + "/" + Padder((currentEvent.start.getMonth() + 1).toString(), 2, "0") + "/" + currentEvent.start.getFullYear().toString();
                $('#eventDateRange').val(svaldate + " - " + svaldate);
            }

            $('#eventId').val(event.Id.trim());
            $('#eventTitle').val(event.title.trim());
            $('#eventDescription').val(event.description.trim());
            $('#myLargeModalLabel').text(event.category.trim());
        }
    });

});

$("#modalSave").click(function () {
    currentEvent.title = $('#eventTitle').val();
    currentEvent.description = $('#eventDescription').val();
    UpdateModalEvent(currentEvent);
});

$("#modalSaveRange").click(function () {

    currentEvent.title = $('#eventTitle').val();
    currentEvent.description = $('#eventDescription').val();
    InsertEventRange(currentEvent);
});

$("#modalRemove").click(function () {
    RemoveEvent(currentEvent);
});

$("#modalReset").click(function () {
    $('button[type=button].close').trigger('click');
    //
    $('#calendar').fullCalendar('refetchEvents');
});

$("#modalResetRange").click(function () {
    $('button[type=button].close').trigger('click');
    //
    $('#calendar').fullCalendar('refetchEvents');
});


//
function UpsertEvent(event, element) {

    var parsedDate = $.fullCalendar.parseDate(event.start);
    var parsedDateEnd = $.fullCalendar.parseDate(event.end);
    var formatedDate = $.fullCalendar.formatDate(parsedDate, "dd-MM-yyyy HH:mm:ss");
    var formatedDateEnd = $.fullCalendar.formatDate(parsedDateEnd, "dd-MM-yyyy HH:mm:ss");

    var sId = "";
    if (event.Id)
        sId = event.Id;

    var sUrlWs = AppWsUrl + 'UpsertAppointment';
    var sData = "uid_CmsNlsContext=" + Uid_CmsNlsContext + "&uid_CmsUsers=" + Uid_CmsUsers + "&uid_TipologieAppuntamenti=" + event.idCategory + "&DataOraInizio=" + formatedDate + "&DataOraFine=" + formatedDateEnd + "&TuttoGiornoFlag=" + event.allDay + "&Titolo=" + event.title + "&Descrizione=" + event.description + "&uid=" + sId;

    $.ajax({
        url: sUrlWs,
        type: 'POST',
        dataType: 'json',
        data: sData,
        success: function (data) {
            if (data.msg) {
                // Error
                alert('error: ' + data.msg);
            }
            else {
                if (sId == "") {
                    //$('#calendar').fullCalendar('renderEvent', data, true);
                    $('#calendar').fullCalendar('refetchEvents');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 500) {
                alert('Internal error: ' + jqXHR.responseText);
            } else {
                alert('Unexpected error.');
            }
        }
    });
}

//
function UpdateAppointment(event, element) {

    var parsedDate = $.fullCalendar.parseDate(event.start);
    var parsedDateEnd = $.fullCalendar.parseDate(event.end);
    var formatedDate = $.fullCalendar.formatDate(parsedDate, "dd-MM-yyyy HH:mm:ss");
    var formatedDateEnd = $.fullCalendar.formatDate(parsedDateEnd, "dd-MM-yyyy HH:mm:ss");

    var sId = "";
    if (event.Id)
        sId = event.Id;

    $.ajax({
        url: AppWsUrl + 'UpdateAppointment',
        type: 'POST',
        dataType: 'json',
        data: "uid_CmsNlsContext=" + Uid_CmsNlsContext + "&uid_CmsUsers=" + Uid_CmsUsers + "&uid_TipologieAppuntamenti=" + event.idCategory + "&DataOraInizio=" + formatedDate + "&DataOraFine=" + formatedDateEnd + "&TuttoGiornoFlag=" + event.allDay + "&uid=" + sId,
        success: function (data) {
            if (data.msg) {
                // Error
                alert('error: ' + data.msg);
            }
            else {
                if (sId == "") {
                    //$('#calendar').fullCalendar('renderEvent', data, true);
                    $('#calendar').fullCalendar('updateEvent', event);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 500) {
                alert('Internal error: ' + jqXHR.responseText);
            } else {
                alert('Unexpected error.');
            }
        }
    });
}

//
function UpdateModalEvent(event) {

    var sUrlWs = AppWsUrl + 'UpdateAppointmentDescription';
    var sData = "uid_CmsNlsContext=" + Uid_CmsNlsContext + "&uid_CmsUsers=" + Uid_CmsUsers + "&Titolo=" + event.title + "&Descrizione=" + event.description + "&uid=" + event.Id;

    $.ajax({
        url: sUrlWs,
        type: 'POST',
        dataType: 'json',
        data: sData,
        success: function (data) {
            if (data.msg) {
                // Error
                alert('error: ' + data.msg);
            }
            else {
                //
                $('button[type=button].close').trigger('click');
                event.name = data.name;
                event.title = data.title;
                //
                $('#calendar').fullCalendar('updateEvent', event);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 500) {
                alert('Internal error: ' + jqXHR.responseText);
            } else {
                alert('Unexpected error.');
            }
        }
    });
}

//
function InsertEventRange(event) {

    // String uid_CmsNlsContext, String uid_Users, String uid_TipologieAppuntamenti, String dateStart, String dateEnd, String allDay, String name, String description, String dateRange
    //
    var parsedDate = $.fullCalendar.parseDate(event.start);
    var parsedDateEnd = $.fullCalendar.parseDate(event.end);
    var formatedDate = $.fullCalendar.formatDate(parsedDate, "dd-MM-yyyy HH:mm:ss");
    var formatedDateEnd = $.fullCalendar.formatDate(parsedDateEnd, "dd-MM-yyyy HH:mm:ss");

    //
    var idCategory = $('#eventCategoryRange').val();
    var dateRange = $('#eventDateRange').val();
    var description = $('#eventDescriptionRange').val();
    var title = $('#eventTitleRange').val();

    var sUrlWs = AppWsUrl + 'InsertMultiAppointment';
    var sData = "uid_CmsNlsContext=" + Uid_CmsNlsContext + "&uid_CmsUsers=" + Uid_CmsUsers + "&uid_TipologieAppuntamenti=" + idCategory + "&dateStart=" + formatedDate + "&dateEnd=" + formatedDateEnd + "&allDay=" + event.allDay + "&title=" + title + "&description=" + description + "&dateRange=" + dateRange;

    $.ajax({
        url: sUrlWs,
        type: 'POST',
        dataType: 'json',
        data: sData,
        success: function (data) {
            if (data.msg) {
                // Error
                alert('error: ' + data.msg);
            }
            else {
                //
                $('button[type=button].close').trigger('click');

                //
                $('#calendar').fullCalendar('refetchEvents');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 500) {
                alert('Internal error: ' + jqXHR.responseText);
            } else {
                alert('Unexpected error.');
            }
        }
    });
}

//
function RemoveEvent(event) {

    var sId = "";
    if (event.Id)
        sId = event.Id;

    $.ajax({
        url: AppWsUrl + 'RemoveAppointment',
        type: 'POST',
        dataType: 'json',
        data: "uid_CmsNlsContext=" + Uid_CmsNlsContext + "&uid_CmsUsers=" + Uid_CmsUsers + "&uid=" + sId,
        success: function (data) {
            if (data.msg) {
                // Error
                alert('error: ' + data.msg);
            }
            else {
                $('button[type=button].close').trigger('click');

                //
                $('#calendar').fullCalendar('removeEvents', event._id);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 500) {
                alert('Internal error: ' + jqXHR.responseText);
            } else {
                alert('Unexpected error.');
            }
        }
    });
}

// date.getDay(), 2, "0"
function Padder(str, len, pad) {
    if (str.length < len) {
        str = '0' + str;
    }
    return str;
}
