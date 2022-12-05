//
function InitAction(_divList) {

    //
    // alert(_divList);

    //
    var currentRelated = $('#' + _divList + " .relatedActive").data("uid");

    if (currentRelated != undefined) {

        var ContentTable = _divList;
        ContentTable = ContentTable.replace("Panel_Related_", "");
        ContentTable = ContentTable.replace("_List", "");

        var currentItemIndex = $('#' + _divList + " .relatedActive").index();
        var nextItemIndex = currentItemIndex + 1;

        var nextItemUid;
        if (nextItemIndex < $('#' + _divList).find('.card').length) {
            nextItemUid = $('#' + _divList).find('.card').eq(currentItemIndex + 1).attr("data-uid");
            $('#HiddenField_' + ContentTable + '_NextUid').val(nextItemUid);
            $('#Button_Related_' + ContentTable + '_SaveAndNext').removeClass("disabled");            
        }
    }

    // Click
    $('#' + _divList + " .lnkAction" ).off('click');
    $('#' + _divList + " .lnkAction" ).on('click', function () {

        //
        var current = $(this);

        //
        var dataUrl = $(current).attr("data-url");
        var dataContent = $(current).attr("data-content");
        var dataUid = $(current).attr("data-uid");
        var dataUidRelated = $(current).attr("data-uidRelated");
        var dataAction = $(current).attr("data-action");
        var dataReturnUrl = $(current).attr("data-returnUrl");

        //
        var message = "";
        if (dataAction == "delete")
            message = "Do you really want to Delete selected records?";
        if (dataAction == "deleteRelated")
            message = "Do you really want to Delete selected records?";
        if (dataAction == "disable")
            message = "Do you really want to Save as Draft selected records?";
        if (dataAction == "enable")
            message = "Do you really want to Save On Stage selected records?";
        if (dataAction == "publish")
            message = "Do you really want to Publish selected records?";
        if (dataAction == "unpublish")
            message = "Do you really want to Unpublish selected records?";

        //
        bootbox.confirm(message, function (result) {
            if (result) {
                $.post(dataUrl,
                    {
                        dataContent: dataContent,
                        dataUid: dataUid,
                        dataUidRelated: dataUidRelated,
                        dataAction: dataAction
                    },
                    function (data, status) {
                        if (data.trim() != '') {
                            console.log(data)
                            $("#Panel_Related_" + dataContent + "_List").html( $(data).html() );
                            InitAction('Panel_Related_' + dataContent + '_List');
                        }
                    });
            }
        });

    });
    $("#"+_divList + ' .lnkActionList').off('click');
    $("#"+_divList + ' .lnkActionList').on('click', function (event) {

        var current = $(this),
            dataUrl = $(current).attr("data-url"),
            dataContent = $("#"+_divList + " .card-list").attr("contenttable"),
            dataAction = $(current).attr("data-action"),
            dataReturnUrl = $(current).attr("data-returnUrl"),
            message = "",
            returnConfirm = "";

        if (dataAction == "delete") {
            message = "Do you really want to Delete selected records?";
            returnConfirm = "delete";
        }
        if (dataAction == "deleterelated") {
            message = "Do you really want to Delete selected records?";
            returnConfirm = "deleterelated";
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
        else if (dataAction == "new") {
            message = "Do you really want Enable selected records?";
            returnConfirm = "copyAll";
        }
        else if (dataAction == "unnew") {
            message = "Do you really want Disable selected records?";
            returnConfirm = "copyAll";
        }

        //
        var selected = [];
        $('.lnkCheckbox').each(function () {
            if ( $(this).prop("checked") ){

                var obj = {};
                var $element = $(this).parents(".card-block").find(".lnkAction");

                $element = $element.eq(1);

                    obj.dataurl = $element[0].dataset.url;
                    obj.datauid = $element[0].dataset.uid;
                    obj.returnurl = $element[0].dataset.returnurl;
                    obj.datacontent = $element[0].dataset.content;
                    obj.datauidrelated = $element[0].dataset.uidrelated;

                    selected.push( obj );

            }
        });

        //
        if (selected.length > 0) {
            bootbox.confirm(message, function (result) {
                if (result) {
                    var bOk = true;
                    var j = 0;
                    for ( var i = 0; i < selected.length; i++) {

                        $.post(selected[i].dataurl,
                            {
                                dataContent: selected[i].datacontent,
                                dataUidRelated: selected[i].datauidrelated,
                                dataUid: selected[i].datauid,
                                dataAction: dataAction
                            }).done(
                                function (data) {
                                    j ++;

                                    console.log( j , selected.length);

                                    if (j == selected.length){
                                        $("#Panel_Related_" + dataContent + "_List").html( $(data).html() );
                                        InitAction('Panel_Related_' + dataContent + '_List');
                                    }

                            });

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
    });

}

function enableSortable(_divList) {

    //
    var nNewPosition = null;
    var nOldPosition = null;
    // 
    $('#' + _divList + " .lnkReorder").addClass('disabled');
    $('#' + _divList + " .lnkApply").removeClass('disabled');

    // Sort    
    $('#' + _divList + " .listsortable").sortable({
        beforeStop: function (event, ui) {

            //
            nNewPosition = ui.item.index();
            console.log("nNewPosition:" + nNewPosition);
            if (nNewPosition != nOldPosition) {
                // Default Value
                var Action = "prev";
                var sUid = $(ui.item).find('.lnkCheckbox').attr("data-uid");
                var currentList = $(ui.item).parent();
                var sContentTable = currentList.attr("contenttable");
                var nElement = $(currentList).find('.card').length - 1;
                var sRefUid = $(currentList).find('.card').eq(nNewPosition + 2).find('.lnkCheckbox').attr("data-uid");

                console.log("nElement:" + nElement);
                console.log("nNewPosition:" + nNewPosition);
                console.log("nOldPosition:" + nOldPosition);

                if (nNewPosition > nOldPosition) {
                    sRefUid = $(currentList).find('.card').eq(nNewPosition - 1).find('.lnkCheckbox').attr("data-uid");
                    Action = "next";
                }

                if (nNewPosition == nElement - 1) {
                    sRefUid = $(currentList).find('.card').eq(nNewPosition - 1).find('.lnkCheckbox').attr("data-uid");
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

                console.log("sUid:" + sUid);
                console.log("sRefUid:" + sRefUid);
                console.log("Action:" + Action);

                // Call SetOrder Page
                var href = '/cms/support/setOrder.aspx?Section=' + sContentTable + '&Uid=' + sUid + '&RefUid=' + sRefUid + '&Action=' + Action;
                $.ajax({
                    url: href,
                    context: document.body
                }).done(function () {
                    //
                    $('#Button_RequiredPublishOrder').removeClass("disabled");
                    Notify('Order updated successfully', 'bottom-right', '5000', 'palegreen', 'fa-power-off', true);
                });
            }
        },
        activate: function (event, ui) {
            nOldPosition = ui.item.index();
        }
    });
    $('#' + _divList + " .listSortable").disableSelection();
}

function disableSortable(_divList) {

    // 
    $('#' + _divList + " .lnkReorder").removeClass('disabled');
    $('#' + _divList + " .lnkApply").addClass('disabled');

    //
    $('#' + _divList + " .listsortable").sortable("destroy");

    //
    eval(_divList + "_Bind();");
}

    

        var MultiCheckbox = function(context, input){
            
            this.context = $(context);
            this.input = $("#"+input);
            this.selectAll = this.context.find(".select-all-checkbox input");
            this.checkbox = this.context.find(".checkbox-to-check input[type='checkbox']");
            this.events();
        };
        MultiCheckbox.prototype.events = function(){
            this.selectAll.on("change", $.proxy(this.triggerAll , this) );
            this.checkbox.on("change", $.proxy(this.createArray , this) );
        };
        MultiCheckbox.prototype.triggerAll = function(event){
            if( $(event.currentTarget).prop("checked") ){
                this.checkbox.each(function(i,checkbox){
                    $(checkbox).prop("checked", true).change();
                });
            }else{
                this.checkbox.each(function(i,checkbox){
                    $(checkbox).prop("checked", false).change();
                });
            }
        };
        MultiCheckbox.prototype.createArray = function(){
            var self = this;
            var checkboxstring = "";
            this.checkbox.each(function(i,check){

                if( $(check).prop("checked") ){
                    if (i > 0){
                        checkboxstring += "," + $(check).val();
                    }else{
                        checkboxstring += $(check).val();
                    }
                }

            });

            this.changeInputValue(checkboxstring);
            
        };
        MultiCheckbox.prototype.changeInputValue = function(value){
            this.input.val(value).change();
        };

        $(".has-multicheckbox").each(function(){
            var IdField = $(this).attr("data-input-id")
            new MultiCheckbox ( this, IdField );
        });
        