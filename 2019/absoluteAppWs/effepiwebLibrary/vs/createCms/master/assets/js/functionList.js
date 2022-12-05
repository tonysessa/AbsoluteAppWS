$(document).ready(function () {

    InitiateSimpleDataTable.init();

    $(".lnkAction").on('click', function () {
        var current = $(this);

        var dataUrl = $(current).attr("data-url");
        var dataContent = $(current).attr("data-content");
        var dataUid = $(current).attr("data-uid");
        var dataAction = $(current).attr("data-action");
        var dataReturnUrl = $(current).attr("data-returnUrl");

        var message = "";
        var returnConfirm = "";
        if (dataAction == "delete") {
            message = "Sei sicuro di voler ELIMINARE questo record?";
            returnConfirm = "delete";
        }
        else if (dataAction == "disable") {
            message = "Sei sicuro di voler DISABILITARE questo record?";
            returnConfirm = "draft";
        }
        else if (dataAction == "enable") {
            message = "Sei sicuro di voler ABILITARE questo record?";
            returnConfirm = "stage";
        }
        else if (dataAction == "publish") {
            message = "Sei sicuro di voler PUBBLICARE questo record?";
            returnConfirm = "publish";
        }
        else if (dataAction == "unpublish") {
            message = "Sei sicuro di voler DE-PUBBLICARE questo record?";
            returnConfirm = "unpublish";
        }
        else if (dataAction == "copy") {
            message = "Sei sicuro di voler COPIARE questo record?";
            returnConfirm = "copy";
        }

        /*
        var message = "";
        var returnConfirm = "";
        if (dataAction == "delete") {
            message = "Do you really want to Delete this record?";
            returnConfirm = "delete";
        }
        else if (dataAction == "disable") {
            message = "Do you really want to Save as Draft this record?";
            returnConfirm = "draft";
        }
        else if (dataAction == "enable") {
            message = "Do you really want to Save On Stage this record?";
            returnConfirm = "stage";
        }
        else if (dataAction == "publish") {
            message = "Do you really want to Publish this record?";
            returnConfirm = "publish";
        }
        else if (dataAction == "unpublish") {
            message = "Do you really want to Unpublish this record?";
            returnConfirm = "unpublish";
        }
        else if (dataAction == "copy") {
            message = "Do you really want to copy this record?";
            returnConfirm = "copy";
        }
        */
        if (message != '') {
            bootbox.confirm(message, function (result) {
                if (result) {
                    $.post(dataUrl,
						{
						    dataContent: dataContent,
						    dataUid: dataUid,
						    dataAction: dataAction
						},
						function (data, status) {
						    if (data.trim() == "SUCCESS") {

						        document.location.href = dataReturnUrl + "&confirm=" + returnConfirm;
						    }
						});
                }
            });
        }
    });

    //
    $(".lnkActionList").on('click', function () {

        var current = $(this),
			dataUrl = $(current).attr("data-url"),
			dataContent = $(current).attr("data-content"),
			dataAction = $(current).attr("data-action"),
			dataReturnUrl = $(current).attr("data-returnUrl"),
			message = "",
			returnConfirm = "";

        if (dataAction == "delete") {
            message = "Sei sicuro di voler ELIMINARE i record selezionati?";
            returnConfirm = "delete";
        }
        else if (dataAction == "disable") {
            message = "Sei sicuro di voler SALVARE IN BOZZA i record selezionati?";
            returnConfirm = "draft";
        }
        else if (dataAction == "enable") {
            message = "Sei sicuro di voler SALVARE E PUBBLICARE i record selezionati?";
            returnConfirm = "stage";
        }
        else if (dataAction == "publish") {
            message = "Sei sicuro di voler PUBBLICARE i record selezionati?";
            returnConfirm = "publish";
        }
        else if (dataAction == "unpublish") {
            message = "Sei sicuro di voler DE-PUBBLICARE i record selezionati?";
            returnConfirm = "unpublish";
        }
        else if (dataAction == "copyAll") {
            message = "Sei sicuro di voler COPIARE il record selezionati?";
            returnConfirm = "copyAll";
        }

        /*
        if (dataAction == "delete") {
            message = "Do you really want to Delete selected records?";
            returnConfirm = "delete";
        }
        else if (dataAction == "disable") {
            message = "Do you really want to Save as Draft selected records?";
            returnConfirm = "draft";
        }
        else if (dataAction == "enable") {
            message = "Do you really want to Save On Stage selected records?";
            returnConfirm = "stage";
        }
        else if (dataAction == "publish") {
            message = "Do you really want to Publish selected records?";
            returnConfirm = "publish";
        }
        else if (dataAction == "unpublish") {
            message = "Do you really want to Unpublish selected records?";
            returnConfirm = "unpublish";
        }
        else if (dataAction == "copyAll") {
            message = "Do you really want to copy selected records?";
            returnConfirm = "copyAll";
        }
        */
        //
        var selected = [];
        var lastPosition;

        $('.lnkCheckbox').each(function () {
            if ($(this).prop("checked"))
                selected.push($(this).attr('data-uid'));
        });

        //
        console.log(message);
        if (selected.length > 0) {
            bootbox.confirm(message, function (result) {
                if (result) {
                    var bOk = true;
                    var j = 0;
                    var increment = 0;
                    lastPosition = selected.length;
                    var data = {
                        dataContent: dataContent,
                        dataAction: dataAction
                    };

                    callService(selected, data, increment);

                    if (bOk) {
                        // bootbox.dialog({
                        //     message: $("#modal-success").html(),
                        //     title: "Success",
                        //     className: "",
                        // });
                        //document.location.href = dataReturnUrl;
                    } else {
                        alert("Error");
                    }
                }
            });
        } else {
            // No Rw Selected
            bootbox.dialog({
                message: "No row selected",
                title: "Error",
                className: ""
            });
        }


        function callService(selected, data, increment) {

            console.info(selected, data, increment);
            data.dataUid = selected[increment];
            $.ajax({
                url: dataUrl,
                data: data,
                method: "POST",
            }).then(function (response) {
                console.log("inc= ", increment, "last= ", lastPosition - 1)
                if (increment < (lastPosition - 1)) {

                    increment++;

                    if (typeof callService === "function") {
                        if (response.trim() == "SUCCESS") {
                        	Notify((increment) + ' of ' + lastPosition, 'bottom-right', '2500', 'info', 'fa-info-circle', true);
                        }else if(response.trim() == "WARNING"){
                        	Notify(response, 'bottom-right', '2500', 'warning', 'fa-warning', true);
                        }else if(response.trim() == "ERROR"){
                        	Notify(response, 'bottom-right', '2500', 'error', 'fa-bolt', true);
                        }
                        callService(selected, data, increment);
                    }

                } else {
                    console.info("finito");
                    Notify((increment + 1) + ' of ' + lastPosition, 'bottom-right', '2500', 'success', 'fa-check', true);
                    setTimeout(function () {
                        document.location.href = dataReturnUrl + "&confirm=" + returnConfirm;
                    }, 2500);
                }
            });
        }

    });

    $(".lnkCheckboxHeader").on('click', function () {

        var current = $(this);

        if (current.prop("checked")) {
            $(".lnkCheckbox").prop("checked", true)
        } else {
            $(".lnkCheckbox").prop("checked", false)
        }
    });
});

function SetDraggableRows() {

    //
    var nNewPosition = null;
    var nOldPosition = null;

    $('#Button_ReorderRecord').addClass("disabled");
    $('#Button_ApplyReorder').removeClass("disabled");

    $('#tbody').parent("table").addClass("js--dragactive");


    $(".listsortable").sortable({
        beforeStop: function (event, ui) {

            nNewPosition = ui.item.index();

            // Default Value
            var Action = "prev";
            var sUid = $(ui.item).find('.lnkCheckbox').attr("data-uid");
            var currentList = $(ui.item).parent();
            var thisID = currentList.attr("id");
            var sContentTable = currentList.attr("contenttable");
            var nElement = $('#' + thisID + ' > tr').length;
            var sRefUid = $('#' + thisID + ' > tr').eq(nNewPosition + 2).find('.lnkCheckbox').attr("data-uid");

            console.log("nElement:" + nElement);
            console.log("nNewPosition:" + nNewPosition);
            console.log("nOldPosition:" + nOldPosition);

            if (nNewPosition > nOldPosition) {
                sRefUid = $('#' + thisID + ' > tr').eq(nNewPosition - 1).find('.lnkCheckbox').attr("data-uid");
                Action = "next";
            }

            if (nNewPosition == nElement - 1) {
                sRefUid = $('#' + thisID + ' > tr').eq(nNewPosition - 1).find('.lnkCheckbox').attr("data-uid");
                Action = "";
            }

            if (sUid == undefined)
                sUid = "";

            if (sRefUid == undefined)
                sRefUid = "";

            if (sContentTable == undefined)
                sContentTable = "";

            if (Action == undefined)
                Action = "";

            // Call SetOrder Page
            var href = '/cms/support/setOrder.aspx?Section=' + sContentTable + '&Uid=' + sUid + '&RefUid=' + sRefUid + '&Action=' + Action;
            $.ajax({
                url: href,
                context: document.body
            }).done(function () {
                $('#Button_RequiredPublishOrder').removeClass("disabled");
            });
        },
        activate: function (event, ui) {
            nOldPosition = ui.item.index();
        }
    });
    $(".listSortable").disableSelection();
};

var BO_image_preview = function (el) {
    this.pointer = $(el);
    this.img = this.pointer.find("img");
    this.pointer.css("position", "relative");
    this.previewontainer = $("<div />", { "class": "preview-box" });
    this.previewontainerimg = $("<img />", { "class": "preview-image" }).appendTo(this.previewontainer);
    this.observe();
};
BO_image_preview.prototype.observe = function (event) {
    this.pointer.on("touchstart mouseenter", $.proxy(this.openPreview, this));
    this.pointer.on("touchend mouseleave", $.proxy(this.closePreview, this));
};
BO_image_preview.prototype.setPreviewPosition = function () {

    var OT = this.pointer.position().top;
    var OL = this.pointer.position().left + this.img.width();

    this.previewontainer.css({
        "top": OT,
        "left": OL,
        "margin-top": "-" + (this.previewontainer.find("img").height() / 2) + "px",
    });

};
BO_image_preview.prototype.openPreview = function () {
    this.previewontainerimg.attr("src", this.img.attr("src"));
    //this.img.before(this.previewontainer);
    this.pointer.parents(".table").append(this.previewontainer)
    this.previewontainer.css("position", "absolute");

    var self = this;

    this.previewontainer.fadeIn(250, function () {
        self.setPreviewPosition();
    });

};
BO_image_preview.prototype.closePreview = function () {
    var self = this;

    this.previewontainer.fadeOut(250, function () {
        self.previewontainer.remove();
    });

};
$("td.img-data").each(function (i, el) {
    var t = new BO_image_preview(el);
});
