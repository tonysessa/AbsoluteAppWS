var NotificationError = {
	init: function(){
		var html = document.getElementsByTagName("HTML");
        this.documentLang = html[0].lang;
        
		this.load();
        return this;
	},
	load: function(){
		var url, xmlhttp, self;

		url = globalVar.CmsContents + this.documentLang + ".json";
    	xmlhttp = new XMLHttpRequest();
    	self = this;

		xmlhttp.onreadystatechange = function() {
			if (xmlhttp.readyState == XMLHttpRequest.DONE ) {
				if (xmlhttp.status == 200) {
                    self.read(JSON.parse(xmlhttp.responseText));
				}
				else if (xmlhttp.status == 400) {
					new Error("i don't find the correct location for the path");
				}
				else {
					new Error("i don't know why but the request dosen't work");
				}
	        }
	    };

	    xmlhttp.open("GET", url, true);
	    xmlhttp.send();

	},
	read: function(data){

		window.errorObject = data;

		return data;

	},
	searchError: function(errorName){
        var error;

		$.each(window.errorObject, function(index, value){ 
			error = value.errorName || value[errorName];
			error.toString();
		});

        return error;
	},
	typeError: function(errorName){
		var array = errorName.split("err");
		return array[0].toString();
	}
}
;

NotificationError.init();
