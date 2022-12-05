/* SORT LIST */

function SetDraggableRows() {
    
    $('#Button_ReorderRecord').addClass("disabled");
    $('#Button_ApplyReorder').removeClass("disabled");

    $('#tbody').parent("table").addClass("js--dragactive");

    
    $(".listsortable").sortable({
        beforeStop: function (event, ui) {

            // Default Value
            var Action = "next";
            var nNewPosition = ui.item.index();
            var sUid = $(ui.item).find('.lnkCheckbox').attr("data-uid");
            var currentList = $(ui.item).parent();

            var thisID = currentList.attr("id");
            var sContentTable = currentList.attr("ContentTable");            
            var nElement = $('#' + thisID + ' > tr').length;

            var sRefUid = $('#' + thisID + ' > tr').eq(nNewPosition - 1).find('.lnkCheckbox').attr("data-uid");
            
            if ((nNewPosition == 0) || (nNewPosition == nElement - 1)) {
                sRefUid = $('#' + thisID + ' > tr').eq(nNewPosition + 2).find('.lnkCheckbox').attr("data-uid");
                Action = "prev";
            }

            alert(sUid)
            alert(sRefUid)

            if (sUid == undefined)
                sUid = "";

            if (sRefUid == undefined)
                sRefUid = "";

            if (sContentTable == undefined)
                sContentTable = "";

            if (Action == undefined)
                Action = "";

            // Call SetOrder Page
            var href = '/support/setOrder.aspx?Section=' + sContentTable + '&Uid=' + sUid + '&RefUid=' + sRefUid + '&Action=' + Action;
            $.ajax({
                url: href,
                context: document.body
            }).done(function () {
                $('#Button_RequiredPublishOrder').removeClass("disabled");
            });
        }
    });
    $(".listSortable").disableSelection();
}


var BO_image_preview = function(el){
    this.pointer = $(el);
    this.img = this.pointer.find("img");
    this.pointer.css("position","relative");
    this.previewontainer = $("<div />" , { "class": "preview-box" });
    this.previewontainerimg = $("<img />" , { "class": "preview-image" }).appendTo(this.previewontainer);
    this.observe();
};
BO_image_preview.prototype.observe = function(event) {
    this.pointer.on("touchstart mouseenter", $.proxy( this.openPreview , this ) );
    this.pointer.on("touchend mouseleave", $.proxy( this.closePreview , this ) );
}; 
BO_image_preview.prototype.openPreview = function(){
    this.previewontainerimg.attr("src", this.img.attr("src") );
    this.img.before(this.previewontainer);
    this.previewontainer.fadeIn(250);
};
BO_image_preview.prototype.closePreview = function(){
    var self = this;
    this.previewontainer.fadeOut(250, function(){
        self.previewontainer.remove();    
    })
    
};
$("td.img-data").each(function(i,el){
    new BO_image_preview(el);
});