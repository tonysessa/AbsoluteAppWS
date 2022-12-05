(function(){
    var Polling = function(elem,o){
            
        
        
        this.obj = $(elem);
        this.url = this.obj.attr("data-pollingurl");
        this.time = this.obj.attr("data-pollingtime");
        

        var self = this;
        var options = {
            callback : function(data){
                self.compiling(data);
            }
        }
        
        this.options = $.extend(options,o);
        
        this.starter();

        return {
            start : function(){
                self.starter.call(self);
            },
            end : function(){
                self.ender.call(self);
            }
        }
    };
    Polling.prototype.starter = function(isend){
        var time = 0;
        var _ = this;
        function calltheurl(now){

            if(!time || now - time >= _.time) {

                time = now;

                $.ajax({
                    url: _.url,
                    method: "POST",
                    crossOrigin: "true",
                    dataType: "JSON" 
                }).then(function(data){
                    
                    if( data.status ){
                        _.obj.show();
                        if (typeof _.options.callback === "function"){
                            _.options.callback.call(_,data);
                        }
                    }else{
                        _.obj.hide();
                    }

                });

            }
            
            requestAnimationFrame(calltheurl);    

        }

        requestAnimationFrame(calltheurl);

    };
    Polling.prototype.ender = function(){
        
    };
    Polling.prototype.compiling = function(data){
        var items = data.items;
        for (item in items){
            this.obj.find(items[item].name).text(items[item].value);
        }
    }

    $.fn.extend({
        polling : function(){

            $(this).each(function(i,el){
                
                var i, l;
                    i = arguments;
                    l = arguments.length
                ;
                if ( typeof arguments[0] === "object" ){
                    l = arguments[0];
                }
                return new Polling(el, l);
            })
        }
    });

})(jQuery);