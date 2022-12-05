

var MultiCheckboxPush = function (context, input) {
    this.context = $(context);
    this.input = $("#" + input);
    this.selectAll = this.context.find(".select-all-checkbox input");
    this.checkbox = this.context.find(".checkbox-to-check input[type='checkbox']");
    this.events();
};
MultiCheckboxPush.prototype.events = function () {
    this.selectAll.on("change", $.proxy(this.triggerAll, this));
    this.checkbox.on("change", $.proxy(this.createArray, this));
};
MultiCheckboxPush.prototype.triggerAll = function (event) {
    if ($(event.currentTarget).prop("checked")) {
        this.checkbox.each(function (i, checkbox) {
            $(checkbox).prop("checked", true).change();
        });
    } else {
        this.checkbox.each(function (i, checkbox) {
            $(checkbox).prop("checked", false).change();
        });
    }
};
MultiCheckboxPush.prototype.createArray = function () {
    var self = this;
    var checkboxstring = "";
    this.checkbox.each(function (i, check) {

        if ($(check).prop("checked")) {
            if (i > 0) {
                checkboxstring += "," + $(check).val();
            } else {
                checkboxstring += $(check).val();
            }
        }

    });
    this.changeInputValue(checkboxstring);
};
MultiCheckboxPush.prototype.changeInputValue = function (value) {
    this.input.val(value).change();
};

$(".has-multicheckbox-push").each(function () {
    
    var IdField = $(this).attr("data-input-id")
    new MultiCheckboxPush(this, IdField);
});
