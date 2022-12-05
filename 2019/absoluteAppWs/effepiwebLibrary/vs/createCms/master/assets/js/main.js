var AccordionOrders = function (block) {
    this.block = $(block);
    this.button = this.block.find(".show-all");
    this.blockContent = this.block.find(".orders-list");
    this.lists = this.block.find(".orders-list li");
    this.header = this.block.find(".orders-header");
    this.events();
};
AccordionOrders.prototype.events = function () {
    $(window).on("load", $.proxy(this.activeHide, this));
    this.button.on("click", $.proxy(this.returnNumberToShow, this));
    this.header.on("click", $.proxy(this.headerCollapse, this));
};
AccordionOrders.prototype.activeHide = function () {

    this.block.addClass("is-open");

    this.lists.each(function (i, element) {
        if ($(element).hasClass("to-hide")) {
            $(element).addClass("hide");
        }
    });

};
AccordionOrders.prototype.returnNumberToShow = function (event) {
    
    event.preventDefault();
    if (this.block.hasClass("is-open")) {
        this.lists.each(function (i, element) {
            if ($(element).hasClass("to-hide")) {
                $(element).removeClass("hide");
            }
        });
        this.button.hide();
        this.header.addClass("is-collapsed");
        this.header.trigger("click");
    } else {

        this.lists.each(function (i, element) {
            if ($(element).hasClass("to-hide")) {
                $(element).addClass("hide");
            }
        });

        this.block.addClass("is-open");
    }
};
AccordionOrders.prototype.headerCollapse = function(){
    // ritorno alla situazione iniziale
    if( this.header.hasClass("is-collapsed") ){
        this.blockContent.slideDown();
        this.header.removeClass("is-collapsed")
    }else{
        this.header.addClass("is-collapsed")
        this.blockContent.slideUp();
        this.lists.each(function (i, element) {
            if ($(element).hasClass("to-hide")) {
                $(element).addClass("hide");
            }
        });
        this.button.show();
    }
    
}


var AccordionMessage = function (el) {
    this.block = $(el);

    console.log(this.block.length);

    this.blockHandler = this.block.find(".orders-header");
    this.blockContent = this.block.find(".orders-list");

    console.log(this.blockHandler.length, this.blockContent.length)

    this.events();
};
AccordionMessage.prototype.events = function () {
    this.blockHandler.on("click", $.proxy(this.toggleAccordion, this));
};

AccordionMessage.prototype.toggleAccordion = function () {
    var self = this;
    
    if (this.blockContent.hasClass("is-hide")) {
        
        this.blockContent.slideDown(250, function () {
            self.blockContent.removeClass("is-hide");
        });
    
    } else {
        
        this.blockContent.slideUp(250, function () {
            self.blockContent.addClass("is-hide")
        });

    }

    return false;

};

if ($(".orders-container").length) {

    $(".orders-container.has-show-all").each(function () {
        new AccordionOrders(this);
    });

    $(".orders-container.has-accordion").each(function () {
        new AccordionMessage(this);
    });

}