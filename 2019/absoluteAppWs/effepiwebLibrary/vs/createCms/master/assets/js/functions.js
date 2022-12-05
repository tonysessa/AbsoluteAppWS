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
/*! device.js 0.2.7 */

(function(){var a,b,c,d,e,f,g,h,i,j;b=window.device,a={},window.device=a,d=window.document.documentElement,j=window.navigator.userAgent.toLowerCase(),a.ios=function(){return a.iphone()||a.ipod()||a.ipad()},a.iphone=function(){return!a.windows()&&e("iphone")},a.ipod=function(){return e("ipod")},a.ipad=function(){return e("ipad")},a.android=function(){return!a.windows()&&e("android")},a.androidPhone=function(){return a.android()&&e("mobile")},a.androidTablet=function(){return a.android()&&!e("mobile")},a.blackberry=function(){return e("blackberry")||e("bb10")||e("rim")},a.blackberryPhone=function(){return a.blackberry()&&!e("tablet")},a.blackberryTablet=function(){return a.blackberry()&&e("tablet")},a.windows=function(){return e("windows")},a.windowsPhone=function(){return a.windows()&&e("phone")},a.windowsTablet=function(){return a.windows()&&e("touch")&&!a.windowsPhone()},a.fxos=function(){return(e("(mobile;")||e("(tablet;"))&&e("; rv:")},a.fxosPhone=function(){return a.fxos()&&e("mobile")},a.fxosTablet=function(){return a.fxos()&&e("tablet")},a.meego=function(){return e("meego")},a.cordova=function(){return window.cordova&&"file:"===location.protocol},a.nodeWebkit=function(){return"object"==typeof window.process},a.mobile=function(){return a.androidPhone()||a.iphone()||a.ipod()||a.windowsPhone()||a.blackberryPhone()||a.fxosPhone()||a.meego()},a.tablet=function(){return a.ipad()||a.androidTablet()||a.blackberryTablet()||a.windowsTablet()||a.fxosTablet()},a.desktop=function(){return!a.tablet()&&!a.mobile()},a.television=function(){var a;for(television=["googletv","viera","smarttv","internet.tv","netcast","nettv","appletv","boxee","kylo","roku","dlnadoc","roku","pov_tv","hbbtv","ce-html"],a=0;a<television.length;){if(e(television[a]))return!0;a++}return!1},a.portrait=function(){return window.innerHeight/window.innerWidth>1},a.landscape=function(){return window.innerHeight/window.innerWidth<1},a.noConflict=function(){return window.device=b,this},e=function(a){return-1!==j.indexOf(a)},g=function(a){var b;return b=new RegExp(a,"i"),d.className.match(b)},c=function(a){var b=null;g(a)||(b=d.className.replace(/^\s+|\s+$/g,""),d.className=b+" "+a)},i=function(a){g(a)&&(d.className=d.className.replace(" "+a,""))},a.ios()?a.ipad()?c("ios ipad tablet"):a.iphone()?c("ios iphone mobile"):a.ipod()&&c("ios ipod mobile"):a.android()?c(a.androidTablet()?"android tablet":"android mobile"):a.blackberry()?c(a.blackberryTablet()?"blackberry tablet":"blackberry mobile"):a.windows()?c(a.windowsTablet()?"windows tablet":a.windowsPhone()?"windows mobile":"desktop"):a.fxos()?c(a.fxosTablet()?"fxos tablet":"fxos mobile"):a.meego()?c("meego mobile"):a.nodeWebkit()?c("node-webkit"):a.television()?c("television"):a.desktop()&&c("desktop"),a.cordova()&&c("cordova"),f=function(){a.landscape()?(i("portrait"),c("landscape")):(i("landscape"),c("portrait"))},h=Object.prototype.hasOwnProperty.call(window,"onorientationchange")?"orientationchange":"resize",window.addEventListener?window.addEventListener(h,f,!1):window.attachEvent?window.attachEvent(h,f):window[h]=f,f(),"function"==typeof define&&"object"==typeof define.amd&&define.amd?define(function(){return a}):"undefined"!=typeof module&&module.exports?module.exports=a:window.device=a}).call(this);
var events = {
	events : {},
	on : function(eventName, fn) {
		this.events[eventName] = this.events[eventName] || [];
		this.events[eventName].push(fn);
	},
	off : function(eventName, fn) {
		var i;
		if (this.events[eventName]) {
			for (i = 0; i < this.events[eventName].length; i++) {
				if (this.events[eventName][i] === fn) {
					this.events[eventName].splice(i, 1);
					break;
				}
			};
		}
	},
	emit : function(eventName, data) {
		if (this.events[eventName]) {
			this.events[eventName].forEach(function(fn) {
				fn(data);
			});
		}
	}
};
/*! jQuery UI - v1.11.4+CommonJS - 2015-08-28
* http://jqueryui.com
* Includes: widget.js
* Copyright 2015 jQuery Foundation and other contributors; Licensed MIT */


(function( factory ) {
	if ( typeof define === "function" && define.amd ) {

		// AMD. Register as an anonymous module.
		define([ "jquery" ], factory );

	} else if ( typeof exports === "object" ) {

		// Node/CommonJS
		factory( require( "jquery" ) );

	} else {

		// Browser globals
		factory( jQuery );
	}
}(function( $ ) {
/*!
 * jQuery UI Widget 1.11.4
 * http://jqueryui.com
 *
 * Copyright jQuery Foundation and other contributors
 * Released under the MIT license.
 * http://jquery.org/license
 *
 * http://api.jqueryui.com/jQuery.widget/
 */


var widget_uuid = 0,
	widget_slice = Array.prototype.slice;

$.cleanData = (function( orig ) {
	return function( elems ) {
		var events, elem, i;
		for ( i = 0; (elem = elems[i]) != null; i++ ) {
			try {

				// Only trigger remove when necessary to save time
				events = $._data( elem, "events" );
				if ( events && events.remove ) {
					$( elem ).triggerHandler( "remove" );
				}

			// http://bugs.jquery.com/ticket/8235
			} catch ( e ) {}
		}
		orig( elems );
	};
})( $.cleanData );

$.widget = function( name, base, prototype ) {
	var fullName, existingConstructor, constructor, basePrototype,
		// proxiedPrototype allows the provided prototype to remain unmodified
		// so that it can be used as a mixin for multiple widgets (#8876)
		proxiedPrototype = {},
		namespace = name.split( "." )[ 0 ];

	name = name.split( "." )[ 1 ];
	fullName = namespace + "-" + name;

	if ( !prototype ) {
		prototype = base;
		base = $.Widget;
	}

	// create selector for plugin
	$.expr[ ":" ][ fullName.toLowerCase() ] = function( elem ) {
		return !!$.data( elem, fullName );
	};

	$[ namespace ] = $[ namespace ] || {};
	existingConstructor = $[ namespace ][ name ];
	constructor = $[ namespace ][ name ] = function( options, element ) {
		// allow instantiation without "new" keyword
		if ( !this._createWidget ) {
			return new constructor( options, element );
		}

		// allow instantiation without initializing for simple inheritance
		// must use "new" keyword (the code above always passes args)
		if ( arguments.length ) {
			this._createWidget( options, element );
		}
	};
	// extend with the existing constructor to carry over any static properties
	$.extend( constructor, existingConstructor, {
		version: prototype.version,
		// copy the object used to create the prototype in case we need to
		// redefine the widget later
		_proto: $.extend( {}, prototype ),
		// track widgets that inherit from this widget in case this widget is
		// redefined after a widget inherits from it
		_childConstructors: []
	});

	basePrototype = new base();
	// we need to make the options hash a property directly on the new instance
	// otherwise we'll modify the options hash on the prototype that we're
	// inheriting from
	basePrototype.options = $.widget.extend( {}, basePrototype.options );
	$.each( prototype, function( prop, value ) {
		if ( !$.isFunction( value ) ) {
			proxiedPrototype[ prop ] = value;
			return;
		}
		proxiedPrototype[ prop ] = (function() {
			var _super = function() {
					return base.prototype[ prop ].apply( this, arguments );
				},
				_superApply = function( args ) {
					return base.prototype[ prop ].apply( this, args );
				};
			return function() {
				var __super = this._super,
					__superApply = this._superApply,
					returnValue;

				this._super = _super;
				this._superApply = _superApply;

				returnValue = value.apply( this, arguments );

				this._super = __super;
				this._superApply = __superApply;

				return returnValue;
			};
		})();
	});
	constructor.prototype = $.widget.extend( basePrototype, {
		// TODO: remove support for widgetEventPrefix
		// always use the name + a colon as the prefix, e.g., draggable:start
		// don't prefix for widgets that aren't DOM-based
		widgetEventPrefix: existingConstructor ? (basePrototype.widgetEventPrefix || name) : name
	}, proxiedPrototype, {
		constructor: constructor,
		namespace: namespace,
		widgetName: name,
		widgetFullName: fullName
	});

	// If this widget is being redefined then we need to find all widgets that
	// are inheriting from it and redefine all of them so that they inherit from
	// the new version of this widget. We're essentially trying to replace one
	// level in the prototype chain.
	if ( existingConstructor ) {
		$.each( existingConstructor._childConstructors, function( i, child ) {
			var childPrototype = child.prototype;

			// redefine the child widget using the same prototype that was
			// originally used, but inherit from the new version of the base
			$.widget( childPrototype.namespace + "." + childPrototype.widgetName, constructor, child._proto );
		});
		// remove the list of existing child constructors from the old constructor
		// so the old child constructors can be garbage collected
		delete existingConstructor._childConstructors;
	} else {
		base._childConstructors.push( constructor );
	}

	$.widget.bridge( name, constructor );

	return constructor;
};

$.widget.extend = function( target ) {
	var input = widget_slice.call( arguments, 1 ),
		inputIndex = 0,
		inputLength = input.length,
		key,
		value;
	for ( ; inputIndex < inputLength; inputIndex++ ) {
		for ( key in input[ inputIndex ] ) {
			value = input[ inputIndex ][ key ];
			if ( input[ inputIndex ].hasOwnProperty( key ) && value !== undefined ) {
				// Clone objects
				if ( $.isPlainObject( value ) ) {
					target[ key ] = $.isPlainObject( target[ key ] ) ?
						$.widget.extend( {}, target[ key ], value ) :
						// Don't extend strings, arrays, etc. with objects
						$.widget.extend( {}, value );
				// Copy everything else by reference
				} else {
					target[ key ] = value;
				}
			}
		}
	}
	return target;
};

$.widget.bridge = function( name, object ) {
	var fullName = object.prototype.widgetFullName || name;
	$.fn[ name ] = function( options ) {
		var isMethodCall = typeof options === "string",
			args = widget_slice.call( arguments, 1 ),
			returnValue = this;

		if ( isMethodCall ) {
			this.each(function() {
				var methodValue,
					instance = $.data( this, fullName );
				if ( options === "instance" ) {
					returnValue = instance;
					return false;
				}
				if ( !instance ) {
					return $.error( "cannot call methods on " + name + " prior to initialization; " +
						"attempted to call method '" + options + "'" );
				}
				if ( !$.isFunction( instance[options] ) || options.charAt( 0 ) === "_" ) {
					return $.error( "no such method '" + options + "' for " + name + " widget instance" );
				}
				methodValue = instance[ options ].apply( instance, args );
				if ( methodValue !== instance && methodValue !== undefined ) {
					returnValue = methodValue && methodValue.jquery ?
						returnValue.pushStack( methodValue.get() ) :
						methodValue;
					return false;
				}
			});
		} else {

			// Allow multiple hashes to be passed on init
			if ( args.length ) {
				options = $.widget.extend.apply( null, [ options ].concat(args) );
			}

			this.each(function() {
				var instance = $.data( this, fullName );
				if ( instance ) {
					instance.option( options || {} );
					if ( instance._init ) {
						instance._init();
					}
				} else {
					$.data( this, fullName, new object( options, this ) );
				}
			});
		}

		return returnValue;
	};
};

$.Widget = function( /* options, element */ ) {};
$.Widget._childConstructors = [];

$.Widget.prototype = {
	widgetName: "widget",
	widgetEventPrefix: "",
	defaultElement: "<div>",
	options: {
		disabled: false,

		// callbacks
		create: null
	},
	_createWidget: function( options, element ) {
		element = $( element || this.defaultElement || this )[ 0 ];
		this.element = $( element );
		this.uuid = widget_uuid++;
		this.eventNamespace = "." + this.widgetName + this.uuid;

		this.bindings = $();
		this.hoverable = $();
		this.focusable = $();

		if ( element !== this ) {
			$.data( element, this.widgetFullName, this );
			this._on( true, this.element, {
				remove: function( event ) {
					if ( event.target === element ) {
						this.destroy();
					}
				}
			});
			this.document = $( element.style ?
				// element within the document
				element.ownerDocument :
				// element is window or document
				element.document || element );
			this.window = $( this.document[0].defaultView || this.document[0].parentWindow );
		}

		this.options = $.widget.extend( {},
			this.options,
			this._getCreateOptions(),
			options );

		this._create();
		this._trigger( "create", null, this._getCreateEventData() );
		this._init();
	},
	_getCreateOptions: $.noop,
	_getCreateEventData: $.noop,
	_create: $.noop,
	_init: $.noop,

	destroy: function() {
		this._destroy();
		// we can probably remove the unbind calls in 2.0
		// all event bindings should go through this._on()
		this.element
			.unbind( this.eventNamespace )
			.removeData( this.widgetFullName )
			// support: jquery <1.6.3
			// http://bugs.jquery.com/ticket/9413
			.removeData( $.camelCase( this.widgetFullName ) );
		this.widget()
			.unbind( this.eventNamespace )
			.removeAttr( "aria-disabled" )
			.removeClass(
				this.widgetFullName + "-disabled " +
				"ui-state-disabled" );

		// clean up events and states
		this.bindings.unbind( this.eventNamespace );
		this.hoverable.removeClass( "ui-state-hover" );
		this.focusable.removeClass( "ui-state-focus" );
	},
	_destroy: $.noop,

	widget: function() {
		return this.element;
	},

	option: function( key, value ) {
		var options = key,
			parts,
			curOption,
			i;

		if ( arguments.length === 0 ) {
			// don't return a reference to the internal hash
			return $.widget.extend( {}, this.options );
		}

		if ( typeof key === "string" ) {
			// handle nested keys, e.g., "foo.bar" => { foo: { bar: ___ } }
			options = {};
			parts = key.split( "." );
			key = parts.shift();
			if ( parts.length ) {
				curOption = options[ key ] = $.widget.extend( {}, this.options[ key ] );
				for ( i = 0; i < parts.length - 1; i++ ) {
					curOption[ parts[ i ] ] = curOption[ parts[ i ] ] || {};
					curOption = curOption[ parts[ i ] ];
				}
				key = parts.pop();
				if ( arguments.length === 1 ) {
					return curOption[ key ] === undefined ? null : curOption[ key ];
				}
				curOption[ key ] = value;
			} else {
				if ( arguments.length === 1 ) {
					return this.options[ key ] === undefined ? null : this.options[ key ];
				}
				options[ key ] = value;
			}
		}

		this._setOptions( options );

		return this;
	},
	_setOptions: function( options ) {
		var key;

		for ( key in options ) {
			this._setOption( key, options[ key ] );
		}

		return this;
	},
	_setOption: function( key, value ) {
		this.options[ key ] = value;

		if ( key === "disabled" ) {
			this.widget()
				.toggleClass( this.widgetFullName + "-disabled", !!value );

			// If the widget is becoming disabled, then nothing is interactive
			if ( value ) {
				this.hoverable.removeClass( "ui-state-hover" );
				this.focusable.removeClass( "ui-state-focus" );
			}
		}

		return this;
	},

	enable: function() {
		return this._setOptions({ disabled: false });
	},
	disable: function() {
		return this._setOptions({ disabled: true });
	},

	_on: function( suppressDisabledCheck, element, handlers ) {
		var delegateElement,
			instance = this;

		// no suppressDisabledCheck flag, shuffle arguments
		if ( typeof suppressDisabledCheck !== "boolean" ) {
			handlers = element;
			element = suppressDisabledCheck;
			suppressDisabledCheck = false;
		}

		// no element argument, shuffle and use this.element
		if ( !handlers ) {
			handlers = element;
			element = this.element;
			delegateElement = this.widget();
		} else {
			element = delegateElement = $( element );
			this.bindings = this.bindings.add( element );
		}

		$.each( handlers, function( event, handler ) {
			function handlerProxy() {
				// allow widgets to customize the disabled handling
				// - disabled as an array instead of boolean
				// - disabled class as method for disabling individual parts
				if ( !suppressDisabledCheck &&
						( instance.options.disabled === true ||
							$( this ).hasClass( "ui-state-disabled" ) ) ) {
					return;
				}
				return ( typeof handler === "string" ? instance[ handler ] : handler )
					.apply( instance, arguments );
			}

			// copy the guid so direct unbinding works
			if ( typeof handler !== "string" ) {
				handlerProxy.guid = handler.guid =
					handler.guid || handlerProxy.guid || $.guid++;
			}

			var match = event.match( /^([\w:-]*)\s*(.*)$/ ),
				eventName = match[1] + instance.eventNamespace,
				selector = match[2];
			if ( selector ) {
				delegateElement.delegate( selector, eventName, handlerProxy );
			} else {
				element.bind( eventName, handlerProxy );
			}
		});
	},

	_off: function( element, eventName ) {
		eventName = (eventName || "").split( " " ).join( this.eventNamespace + " " ) +
			this.eventNamespace;
		element.unbind( eventName ).undelegate( eventName );

		// Clear the stack to avoid memory leaks (#10056)
		this.bindings = $( this.bindings.not( element ).get() );
		this.focusable = $( this.focusable.not( element ).get() );
		this.hoverable = $( this.hoverable.not( element ).get() );
	},

	_delay: function( handler, delay ) {
		function handlerProxy() {
			return ( typeof handler === "string" ? instance[ handler ] : handler )
				.apply( instance, arguments );
		}
		var instance = this;
		return setTimeout( handlerProxy, delay || 0 );
	},

	_hoverable: function( element ) {
		this.hoverable = this.hoverable.add( element );
		this._on( element, {
			mouseenter: function( event ) {
				$( event.currentTarget ).addClass( "ui-state-hover" );
			},
			mouseleave: function( event ) {
				$( event.currentTarget ).removeClass( "ui-state-hover" );
			}
		});
	},

	_focusable: function( element ) {
		this.focusable = this.focusable.add( element );
		this._on( element, {
			focusin: function( event ) {
				$( event.currentTarget ).addClass( "ui-state-focus" );
			},
			focusout: function( event ) {
				$( event.currentTarget ).removeClass( "ui-state-focus" );
			}
		});
	},

	_trigger: function( type, event, data ) {
		var prop, orig,
			callback = this.options[ type ];

		data = data || {};
		event = $.Event( event );
		event.type = ( type === this.widgetEventPrefix ?
			type :
			this.widgetEventPrefix + type ).toLowerCase();
		// the original event may come from any element
		// so we need to reset the target on the new event
		event.target = this.element[ 0 ];

		// copy original event properties over to the new event
		orig = event.originalEvent;
		if ( orig ) {
			for ( prop in orig ) {
				if ( !( prop in event ) ) {
					event[ prop ] = orig[ prop ];
				}
			}
		}

		this.element.trigger( event, data );
		return !( $.isFunction( callback ) &&
			callback.apply( this.element[0], [ event ].concat( data ) ) === false ||
			event.isDefaultPrevented() );
	}
};

$.each( { show: "fadeIn", hide: "fadeOut" }, function( method, defaultEffect ) {
	$.Widget.prototype[ "_" + method ] = function( element, options, callback ) {
		if ( typeof options === "string" ) {
			options = { effect: options };
		}
		var hasOptions,
			effectName = !options ?
				method :
				options === true || typeof options === "number" ?
					defaultEffect :
					options.effect || defaultEffect;
		options = options || {};
		if ( typeof options === "number" ) {
			options = { duration: options };
		}
		hasOptions = !$.isEmptyObject( options );
		options.complete = callback;
		if ( options.delay ) {
			element.delay( options.delay );
		}
		if ( hasOptions && $.effects && $.effects.effect[ effectName ] ) {
			element[ method ]( options );
		} else if ( effectName !== method && element[ effectName ] ) {
			element[ effectName ]( options.duration, options.easing, callback );
		} else {
			element.queue(function( next ) {
				$( this )[ method ]();
				if ( callback ) {
					callback.call( element[ 0 ] );
				}
				next();
			});
		}
	};
});

var widget = $.widget;



}));
(function ($) {

	$.event.special.doubletap = {
		bindType: 'touchend',
		delegateType: 'touchend',

		handle: function (event) {
			var handleObj = event.handleObj,
				targetData = jQuery.data(event.target),
				now = new Date().getTime(),
				delta = targetData.lastTouch ? now - targetData.lastTouch : 0,
				delay = delay == null ? 300 : delay;

			if ( delta < delay && delta > 30 ) {
				targetData.lastTouch = null;
				event.type = handleObj.origType;
				['clientX', 'clientY', 'pageX', 'pageY'].forEach(function (property) {
					event[property] = event.originalEvent.changedTouches[0][property];
				})

				// let jQuery handle the triggering of "doubletap" event handlers
				handleObj.handler.apply(this, arguments);
			} else {
				targetData.lastTouch = now;
			}
		}
	};

})(jQuery);
/*
 * jQuery File Upload Plugin
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/* jshint nomen:false */
/* global define, require, window, document, location, Blob, FormData */


(function (factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        // Register as an anonymous AMD module:
        define([
            'jquery',
            'jquery.ui.widget'
        ], factory);
    } else if (typeof exports === 'object') {
        // Node/CommonJS:
        factory(
            require('jquery'),
            require('./vendor/jquery.ui.widget')
        );
    } else {
        // Browser globals:
        factory(window.jQuery);
    }
}(function ($) {
    'use strict';

    // Detect file input support, based on
    // http://viljamis.com/blog/2012/file-upload-support-on-mobile/
    $.support.fileInput = !(new RegExp(
        // Handle devices which give false positives for the feature detection:
        '(Android (1\\.[0156]|2\\.[01]))' +
            '|(Windows Phone (OS 7|8\\.0))|(XBLWP)|(ZuneWP)|(WPDesktop)' +
            '|(w(eb)?OSBrowser)|(webOS)' +
            '|(Kindle/(1\\.0|2\\.[05]|3\\.0))'
    ).test(window.navigator.userAgent) ||
        // Feature detection for all other devices:
        $('<input type="file">').prop('disabled'));

    // The FileReader API is not actually used, but works as feature detection,
    // as some Safari versions (5?) support XHR file uploads via the FormData API,
    // but not non-multipart XHR file uploads.
    // window.XMLHttpRequestUpload is not available on IE10, so we check for
    // window.ProgressEvent instead to detect XHR2 file upload capability:
    $.support.xhrFileUpload = !!(window.ProgressEvent && window.FileReader);
    $.support.xhrFormDataFileUpload = !!window.FormData;

    // Detect support for Blob slicing (required for chunked uploads):
    $.support.blobSlice = window.Blob && (Blob.prototype.slice ||
        Blob.prototype.webkitSlice || Blob.prototype.mozSlice);

    // Helper function to create drag handlers for dragover/dragenter/dragleave:
    function getDragHandler(type) {
        var isDragOver = type === 'dragover';
        return function (e) {
            e.dataTransfer = e.originalEvent && e.originalEvent.dataTransfer;
            var dataTransfer = e.dataTransfer;
            if (dataTransfer && $.inArray('Files', dataTransfer.types) !== -1 &&
                    this._trigger(
                        type,
                        $.Event(type, {delegatedEvent: e})
                    ) !== false) {
                e.preventDefault();
                if (isDragOver) {
                    dataTransfer.dropEffect = 'copy';
                }
            }
        };
    }

    // The fileupload widget listens for change events on file input fields defined
    // via fileInput setting and paste or drop events of the given dropZone.
    // In addition to the default jQuery Widget methods, the fileupload widget
    // exposes the "add" and "send" methods, to add or directly send files using
    // the fileupload API.
    // By default, files added via file input selection, paste, drag & drop or
    // "add" method are uploaded immediately, but it is possible to override
    // the "add" callback option to queue file uploads.
    $.widget('blueimp.fileupload', {

        options: {
            // The drop target element(s), by the default the complete document.
            // Set to null to disable drag & drop support:
            dropZone: $(document),
            // The paste target element(s), by the default undefined.
            // Set to a DOM node or jQuery object to enable file pasting:
            pasteZone: undefined,
            // The file input field(s), that are listened to for change events.
            // If undefined, it is set to the file input fields inside
            // of the widget element on plugin initialization.
            // Set to null to disable the change listener.
            fileInput: undefined,
            // By default, the file input field is replaced with a clone after
            // each input field change event. This is required for iframe transport
            // queues and allows change events to be fired for the same file
            // selection, but can be disabled by setting the following option to false:
            replaceFileInput: true,
            // The parameter name for the file form data (the request argument name).
            // If undefined or empty, the name property of the file input field is
            // used, or "files[]" if the file input name property is also empty,
            // can be a string or an array of strings:
            paramName: undefined,
            // By default, each file of a selection is uploaded using an individual
            // request for XHR type uploads. Set to false to upload file
            // selections in one request each:
            singleFileUploads: true,
            // To limit the number of files uploaded with one XHR request,
            // set the following option to an integer greater than 0:
            limitMultiFileUploads: undefined,
            // The following option limits the number of files uploaded with one
            // XHR request to keep the request size under or equal to the defined
            // limit in bytes:
            limitMultiFileUploadSize: undefined,
            // Multipart file uploads add a number of bytes to each uploaded file,
            // therefore the following option adds an overhead for each file used
            // in the limitMultiFileUploadSize configuration:
            limitMultiFileUploadSizeOverhead: 512,
            // Set the following option to true to issue all file upload requests
            // in a sequential order:
            sequentialUploads: false,
            // To limit the number of concurrent uploads,
            // set the following option to an integer greater than 0:
            limitConcurrentUploads: undefined,
            // Set the following option to true to force iframe transport uploads:
            forceIframeTransport: false,
            // Set the following option to the location of a redirect url on the
            // origin server, for cross-domain iframe transport uploads:
            redirect: undefined,
            // The parameter name for the redirect url, sent as part of the form
            // data and set to 'redirect' if this option is empty:
            redirectParamName: undefined,
            // Set the following option to the location of a postMessage window,
            // to enable postMessage transport uploads:
            postMessage: undefined,
            // By default, XHR file uploads are sent as multipart/form-data.
            // The iframe transport is always using multipart/form-data.
            // Set to false to enable non-multipart XHR uploads:
            multipart: true,
            // To upload large files in smaller chunks, set the following option
            // to a preferred maximum chunk size. If set to 0, null or undefined,
            // or the browser does not support the required Blob API, files will
            // be uploaded as a whole.
            maxChunkSize: undefined,
            // When a non-multipart upload or a chunked multipart upload has been
            // aborted, this option can be used to resume the upload by setting
            // it to the size of the already uploaded bytes. This option is most
            // useful when modifying the options object inside of the "add" or
            // "send" callbacks, as the options are cloned for each file upload.
            uploadedBytes: undefined,
            // By default, failed (abort or error) file uploads are removed from the
            // global progress calculation. Set the following option to false to
            // prevent recalculating the global progress data:
            recalculateProgress: true,
            // Interval in milliseconds to calculate and trigger progress events:
            progressInterval: 100,
            // Interval in milliseconds to calculate progress bitrate:
            bitrateInterval: 500,
            // By default, uploads are started automatically when adding files:
            autoUpload: true,

            // Error and info messages:
            messages: {
                uploadedBytes: 'Uploaded bytes exceed file size'
            },

            // Translation function, gets the message key to be translated
            // and an object with context specific data as arguments:
            i18n: function (message, context) {
                message = this.messages[message] || message.toString();
                if (context) {
                    $.each(context, function (key, value) {
                        message = message.replace('{' + key + '}', value);
                    });
                }
                return message;
            },

            // Additional form data to be sent along with the file uploads can be set
            // using this option, which accepts an array of objects with name and
            // value properties, a function returning such an array, a FormData
            // object (for XHR file uploads), or a simple object.
            // The form of the first fileInput is given as parameter to the function:
            formData: function (form) {
                return form.serializeArray();
            },

            // The add callback is invoked as soon as files are added to the fileupload
            // widget (via file input selection, drag & drop, paste or add API call).
            // If the singleFileUploads option is enabled, this callback will be
            // called once for each file in the selection for XHR file uploads, else
            // once for each file selection.
            //
            // The upload starts when the submit method is invoked on the data parameter.
            // The data object contains a files property holding the added files
            // and allows you to override plugin options as well as define ajax settings.
            //
            // Listeners for this callback can also be bound the following way:
            // .bind('fileuploadadd', func);
            //
            // data.submit() returns a Promise object and allows to attach additional
            // handlers using jQuery's Deferred callbacks:
            // data.submit().done(func).fail(func).always(func);
            add: function (e, data) {
                if (e.isDefaultPrevented()) {
                    return false;
                }
                if (data.autoUpload || (data.autoUpload !== false &&
                        $(this).fileupload('option', 'autoUpload'))) {
                    data.process().done(function () {
                        data.submit();
                    });
                }
            },

            // Other callbacks:

            // Callback for the submit event of each file upload:
            // submit: function (e, data) {}, // .bind('fileuploadsubmit', func);

            // Callback for the start of each file upload request:
            // send: function (e, data) {}, // .bind('fileuploadsend', func);

            // Callback for successful uploads:
            // done: function (e, data) {}, // .bind('fileuploaddone', func);

            // Callback for failed (abort or error) uploads:
            // fail: function (e, data) {}, // .bind('fileuploadfail', func);

            // Callback for completed (success, abort or error) requests:
            // always: function (e, data) {}, // .bind('fileuploadalways', func);

            // Callback for upload progress events:
            // progress: function (e, data) {}, // .bind('fileuploadprogress', func);

            // Callback for global upload progress events:
            // progressall: function (e, data) {}, // .bind('fileuploadprogressall', func);

            // Callback for uploads start, equivalent to the global ajaxStart event:
            // start: function (e) {}, // .bind('fileuploadstart', func);

            // Callback for uploads stop, equivalent to the global ajaxStop event:
            // stop: function (e) {}, // .bind('fileuploadstop', func);

            // Callback for change events of the fileInput(s):
            // change: function (e, data) {}, // .bind('fileuploadchange', func);

            // Callback for paste events to the pasteZone(s):
            // paste: function (e, data) {}, // .bind('fileuploadpaste', func);

            // Callback for drop events of the dropZone(s):
            // drop: function (e, data) {}, // .bind('fileuploaddrop', func);

            // Callback for dragover events of the dropZone(s):
            // dragover: function (e) {}, // .bind('fileuploaddragover', func);

            // Callback for the start of each chunk upload request:
            // chunksend: function (e, data) {}, // .bind('fileuploadchunksend', func);

            // Callback for successful chunk uploads:
            // chunkdone: function (e, data) {}, // .bind('fileuploadchunkdone', func);

            // Callback for failed (abort or error) chunk uploads:
            // chunkfail: function (e, data) {}, // .bind('fileuploadchunkfail', func);

            // Callback for completed (success, abort or error) chunk upload requests:
            // chunkalways: function (e, data) {}, // .bind('fileuploadchunkalways', func);

            // The plugin options are used as settings object for the ajax calls.
            // The following are jQuery ajax settings required for the file uploads:
            processData: false,
            contentType: false,
            cache: false,
            timeout: 0
        },

        // A list of options that require reinitializing event listeners and/or
        // special initialization code:
        _specialOptions: [
            'fileInput',
            'dropZone',
            'pasteZone',
            'multipart',
            'forceIframeTransport'
        ],

        _blobSlice: $.support.blobSlice && function () {
            var slice = this.slice || this.webkitSlice || this.mozSlice;
            return slice.apply(this, arguments);
        },

        _BitrateTimer: function () {
            this.timestamp = ((Date.now) ? Date.now() : (new Date()).getTime());
            this.loaded = 0;
            this.bitrate = 0;
            this.getBitrate = function (now, loaded, interval) {
                var timeDiff = now - this.timestamp;
                if (!this.bitrate || !interval || timeDiff > interval) {
                    this.bitrate = (loaded - this.loaded) * (1000 / timeDiff) * 8;
                    this.loaded = loaded;
                    this.timestamp = now;
                }
                return this.bitrate;
            };
        },

        _isXHRUpload: function (options) {
            return !options.forceIframeTransport &&
                ((!options.multipart && $.support.xhrFileUpload) ||
                $.support.xhrFormDataFileUpload);
        },

        _getFormData: function (options) {
            var formData;
            if ($.type(options.formData) === 'function') {
                return options.formData(options.form);
            }
            if ($.isArray(options.formData)) {
                return options.formData;
            }
            if ($.type(options.formData) === 'object') {
                formData = [];
                $.each(options.formData, function (name, value) {
                    formData.push({name: name, value: value});
                });
                return formData;
            }
            return [];
        },

        _getTotal: function (files) {
            var total = 0;
            $.each(files, function (index, file) {
                total += file.size || 1;
            });
            return total;
        },

        _initProgressObject: function (obj) {
            var progress = {
                loaded: 0,
                total: 0,
                bitrate: 0
            };
            if (obj._progress) {
                $.extend(obj._progress, progress);
            } else {
                obj._progress = progress;
            }
        },

        _initResponseObject: function (obj) {
            var prop;
            if (obj._response) {
                for (prop in obj._response) {
                    if (obj._response.hasOwnProperty(prop)) {
                        delete obj._response[prop];
                    }
                }
            } else {
                obj._response = {};
            }
        },

        _onProgress: function (e, data) {
            if (e.lengthComputable) {
                var now = ((Date.now) ? Date.now() : (new Date()).getTime()),
                    loaded;
                if (data._time && data.progressInterval &&
                        (now - data._time < data.progressInterval) &&
                        e.loaded !== e.total) {
                    return;
                }
                data._time = now;
                loaded = Math.floor(
                    e.loaded / e.total * (data.chunkSize || data._progress.total)
                ) + (data.uploadedBytes || 0);
                // Add the difference from the previously loaded state
                // to the global loaded counter:
                this._progress.loaded += (loaded - data._progress.loaded);
                this._progress.bitrate = this._bitrateTimer.getBitrate(
                    now,
                    this._progress.loaded,
                    data.bitrateInterval
                );
                data._progress.loaded = data.loaded = loaded;
                data._progress.bitrate = data.bitrate = data._bitrateTimer.getBitrate(
                    now,
                    loaded,
                    data.bitrateInterval
                );
                // Trigger a custom progress event with a total data property set
                // to the file size(s) of the current upload and a loaded data
                // property calculated accordingly:
                this._trigger(
                    'progress',
                    $.Event('progress', {delegatedEvent: e}),
                    data
                );
                // Trigger a global progress event for all current file uploads,
                // including ajax calls queued for sequential file uploads:
                this._trigger(
                    'progressall',
                    $.Event('progressall', {delegatedEvent: e}),
                    this._progress
                );
            }
        },

        _initProgressListener: function (options) {
            var that = this,
                xhr = options.xhr ? options.xhr() : $.ajaxSettings.xhr();
            // Accesss to the native XHR object is required to add event listeners
            // for the upload progress event:
            if (xhr.upload) {
                $(xhr.upload).bind('progress', function (e) {
                    var oe = e.originalEvent;
                    // Make sure the progress event properties get copied over:
                    e.lengthComputable = oe.lengthComputable;
                    e.loaded = oe.loaded;
                    e.total = oe.total;
                    that._onProgress(e, options);
                });
                options.xhr = function () {
                    return xhr;
                };
            }
        },

        _isInstanceOf: function (type, obj) {
            // Cross-frame instanceof check
            return Object.prototype.toString.call(obj) === '[object ' + type + ']';
        },

        _initXHRData: function (options) {
            var that = this,
                formData,
                file = options.files[0],
                // Ignore non-multipart setting if not supported:
                multipart = options.multipart || !$.support.xhrFileUpload,
                paramName = $.type(options.paramName) === 'array' ?
                    options.paramName[0] : options.paramName;
            options.headers = $.extend({}, options.headers);
            if (options.contentRange) {
                options.headers['Content-Range'] = options.contentRange;
            }
            if (!multipart || options.blob || !this._isInstanceOf('File', file)) {
                options.headers['Content-Disposition'] = 'attachment; filename="' +
                    encodeURI(file.name) + '"';
            }
            if (!multipart) {
                options.contentType = file.type || 'application/octet-stream';
                options.data = options.blob || file;
            } else if ($.support.xhrFormDataFileUpload) {
                if (options.postMessage) {
                    // window.postMessage does not allow sending FormData
                    // objects, so we just add the File/Blob objects to
                    // the formData array and let the postMessage window
                    // create the FormData object out of this array:
                    formData = this._getFormData(options);
                    if (options.blob) {
                        formData.push({
                            name: paramName,
                            value: options.blob
                        });
                    } else {
                        $.each(options.files, function (index, file) {
                            formData.push({
                                name: ($.type(options.paramName) === 'array' &&
                                    options.paramName[index]) || paramName,
                                value: file
                            });
                        });
                    }
                } else {
                    if (that._isInstanceOf('FormData', options.formData)) {
                        formData = options.formData;
                    } else {
                        formData = new FormData();
                        $.each(this._getFormData(options), function (index, field) {
                            formData.append(field.name, field.value);
                        });
                    }
                    if (options.blob) {
                        formData.append(paramName, options.blob, file.name);
                    } else {
                        $.each(options.files, function (index, file) {
                            // This check allows the tests to run with
                            // dummy objects:
                            if (that._isInstanceOf('File', file) ||
                                    that._isInstanceOf('Blob', file)) {
                                formData.append(
                                    ($.type(options.paramName) === 'array' &&
                                        options.paramName[index]) || paramName,
                                    file,
                                    file.uploadName || file.name
                                );
                            }
                        });
                    }
                }
                options.data = formData;
            }
            // Blob reference is not needed anymore, free memory:
            options.blob = null;
        },

        _initIframeSettings: function (options) {
            var targetHost = $('<a></a>').prop('href', options.url).prop('host');
            // Setting the dataType to iframe enables the iframe transport:
            options.dataType = 'iframe ' + (options.dataType || '');
            // The iframe transport accepts a serialized array as form data:
            options.formData = this._getFormData(options);
            // Add redirect url to form data on cross-domain uploads:
            if (options.redirect && targetHost && targetHost !== location.host) {
                options.formData.push({
                    name: options.redirectParamName || 'redirect',
                    value: options.redirect
                });
            }
        },

        _initDataSettings: function (options) {
            if (this._isXHRUpload(options)) {
                if (!this._chunkedUpload(options, true)) {
                    if (!options.data) {
                        this._initXHRData(options);
                    }
                    this._initProgressListener(options);
                }
                if (options.postMessage) {
                    // Setting the dataType to postmessage enables the
                    // postMessage transport:
                    options.dataType = 'postmessage ' + (options.dataType || '');
                }
            } else {
                this._initIframeSettings(options);
            }
        },

        _getParamName: function (options) {
            var fileInput = $(options.fileInput),
                paramName = options.paramName;
            if (!paramName) {
                paramName = [];
                fileInput.each(function () {
                    var input = $(this),
                        name = input.prop('name') || 'files[]',
                        i = (input.prop('files') || [1]).length;
                    while (i) {
                        paramName.push(name);
                        i -= 1;
                    }
                });
                if (!paramName.length) {
                    paramName = [fileInput.prop('name') || 'files[]'];
                }
            } else if (!$.isArray(paramName)) {
                paramName = [paramName];
            }
            return paramName;
        },

        _initFormSettings: function (options) {
            // Retrieve missing options from the input field and the
            // associated form, if available:
            if (!options.form || !options.form.length) {
                options.form = $(options.fileInput.prop('form'));
                // If the given file input doesn't have an associated form,
                // use the default widget file input's form:
                if (!options.form.length) {
                    options.form = $(this.options.fileInput.prop('form'));
                }
            }
            options.paramName = this._getParamName(options);
            if (!options.url) {
                options.url = options.form.prop('action') || location.href;
            }
            // The HTTP request method must be "POST" or "PUT":
            options.type = (options.type ||
                ($.type(options.form.prop('method')) === 'string' &&
                    options.form.prop('method')) || ''
                ).toUpperCase();
            if (options.type !== 'POST' && options.type !== 'PUT' &&
                    options.type !== 'PATCH') {
                options.type = 'POST';
            }
            if (!options.formAcceptCharset) {
                options.formAcceptCharset = options.form.attr('accept-charset');
            }
        },

        _getAJAXSettings: function (data) {
            var options = $.extend({}, this.options, data);
            this._initFormSettings(options);
            this._initDataSettings(options);
            return options;
        },

        // jQuery 1.6 doesn't provide .state(),
        // while jQuery 1.8+ removed .isRejected() and .isResolved():
        _getDeferredState: function (deferred) {
            if (deferred.state) {
                return deferred.state();
            }
            if (deferred.isResolved()) {
                return 'resolved';
            }
            if (deferred.isRejected()) {
                return 'rejected';
            }
            return 'pending';
        },

        // Maps jqXHR callbacks to the equivalent
        // methods of the given Promise object:
        _enhancePromise: function (promise) {
            promise.success = promise.done;
            promise.error = promise.fail;
            promise.complete = promise.always;
            return promise;
        },

        // Creates and returns a Promise object enhanced with
        // the jqXHR methods abort, success, error and complete:
        _getXHRPromise: function (resolveOrReject, context, args) {
            var dfd = $.Deferred(),
                promise = dfd.promise();
            context = context || this.options.context || promise;
            if (resolveOrReject === true) {
                dfd.resolveWith(context, args);
            } else if (resolveOrReject === false) {
                dfd.rejectWith(context, args);
            }
            promise.abort = dfd.promise;
            return this._enhancePromise(promise);
        },

        // Adds convenience methods to the data callback argument:
        _addConvenienceMethods: function (e, data) {
            var that = this,
                getPromise = function (args) {
                    return $.Deferred().resolveWith(that, args).promise();
                };
            data.process = function (resolveFunc, rejectFunc) {
                if (resolveFunc || rejectFunc) {
                    data._processQueue = this._processQueue =
                        (this._processQueue || getPromise([this])).then(
                            function () {
                                if (data.errorThrown) {
                                    return $.Deferred()
                                        .rejectWith(that, [data]).promise();
                                }
                                return getPromise(arguments);
                            }
                        ).then(resolveFunc, rejectFunc);
                }
                return this._processQueue || getPromise([this]);
            };
            data.submit = function () {
                if (this.state() !== 'pending') {
                    data.jqXHR = this.jqXHR =
                        (that._trigger(
                            'submit',
                            $.Event('submit', {delegatedEvent: e}),
                            this
                        ) !== false) && that._onSend(e, this);
                }
                return this.jqXHR || that._getXHRPromise();
            };
            data.abort = function () {
                if (this.jqXHR) {
                    return this.jqXHR.abort();
                }
                this.errorThrown = 'abort';
                that._trigger('fail', null, this);
                return that._getXHRPromise(false);
            };
            data.state = function () {
                if (this.jqXHR) {
                    return that._getDeferredState(this.jqXHR);
                }
                if (this._processQueue) {
                    return that._getDeferredState(this._processQueue);
                }
            };
            data.processing = function () {
                return !this.jqXHR && this._processQueue && that
                    ._getDeferredState(this._processQueue) === 'pending';
            };
            data.progress = function () {
                return this._progress;
            };
            data.response = function () {
                return this._response;
            };
        },

        // Parses the Range header from the server response
        // and returns the uploaded bytes:
        _getUploadedBytes: function (jqXHR) {
            var range = jqXHR.getResponseHeader('Range'),
                parts = range && range.split('-'),
                upperBytesPos = parts && parts.length > 1 &&
                    parseInt(parts[1], 10);
            return upperBytesPos && upperBytesPos + 1;
        },

        // Uploads a file in multiple, sequential requests
        // by splitting the file up in multiple blob chunks.
        // If the second parameter is true, only tests if the file
        // should be uploaded in chunks, but does not invoke any
        // upload requests:
        _chunkedUpload: function (options, testOnly) {
            options.uploadedBytes = options.uploadedBytes || 0;
            var that = this,
                file = options.files[0],
                fs = file.size,
                ub = options.uploadedBytes,
                mcs = options.maxChunkSize || fs,
                slice = this._blobSlice,
                dfd = $.Deferred(),
                promise = dfd.promise(),
                jqXHR,
                upload;
            if (!(this._isXHRUpload(options) && slice && (ub || mcs < fs)) ||
                    options.data) {
                return false;
            }
            if (testOnly) {
                return true;
            }
            if (ub >= fs) {
                file.error = options.i18n('uploadedBytes');
                return this._getXHRPromise(
                    false,
                    options.context,
                    [null, 'error', file.error]
                );
            }
            // The chunk upload method:
            upload = function () {
                // Clone the options object for each chunk upload:
                var o = $.extend({}, options),
                    currentLoaded = o._progress.loaded;
                o.blob = slice.call(
                    file,
                    ub,
                    ub + mcs,
                    file.type
                );
                // Store the current chunk size, as the blob itself
                // will be dereferenced after data processing:
                o.chunkSize = o.blob.size;
                // Expose the chunk bytes position range:
                o.contentRange = 'bytes ' + ub + '-' +
                    (ub + o.chunkSize - 1) + '/' + fs;
                // Process the upload data (the blob and potential form data):
                that._initXHRData(o);
                // Add progress listeners for this chunk upload:
                that._initProgressListener(o);
                jqXHR = ((that._trigger('chunksend', null, o) !== false && $.ajax(o)) ||
                        that._getXHRPromise(false, o.context))
                    .done(function (result, textStatus, jqXHR) {
                        ub = that._getUploadedBytes(jqXHR) ||
                            (ub + o.chunkSize);
                        // Create a progress event if no final progress event
                        // with loaded equaling total has been triggered
                        // for this chunk:
                        if (currentLoaded + o.chunkSize - o._progress.loaded) {
                            that._onProgress($.Event('progress', {
                                lengthComputable: true,
                                loaded: ub - o.uploadedBytes,
                                total: ub - o.uploadedBytes
                            }), o);
                        }
                        options.uploadedBytes = o.uploadedBytes = ub;
                        o.result = result;
                        o.textStatus = textStatus;
                        o.jqXHR = jqXHR;
                        that._trigger('chunkdone', null, o);
                        that._trigger('chunkalways', null, o);
                        if (ub < fs) {
                            // File upload not yet complete,
                            // continue with the next chunk:
                            upload();
                        } else {
                            dfd.resolveWith(
                                o.context,
                                [result, textStatus, jqXHR]
                            );
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        o.jqXHR = jqXHR;
                        o.textStatus = textStatus;
                        o.errorThrown = errorThrown;
                        that._trigger('chunkfail', null, o);
                        that._trigger('chunkalways', null, o);
                        dfd.rejectWith(
                            o.context,
                            [jqXHR, textStatus, errorThrown]
                        );
                    });
            };
            this._enhancePromise(promise);
            promise.abort = function () {
                return jqXHR.abort();
            };
            upload();
            return promise;
        },

        _beforeSend: function (e, data) {
            if (this._active === 0) {
                // the start callback is triggered when an upload starts
                // and no other uploads are currently running,
                // equivalent to the global ajaxStart event:
                this._trigger('start');
                // Set timer for global bitrate progress calculation:
                this._bitrateTimer = new this._BitrateTimer();
                // Reset the global progress values:
                this._progress.loaded = this._progress.total = 0;
                this._progress.bitrate = 0;
            }
            // Make sure the container objects for the .response() and
            // .progress() methods on the data object are available
            // and reset to their initial state:
            this._initResponseObject(data);
            this._initProgressObject(data);
            data._progress.loaded = data.loaded = data.uploadedBytes || 0;
            data._progress.total = data.total = this._getTotal(data.files) || 1;
            data._progress.bitrate = data.bitrate = 0;
            this._active += 1;
            // Initialize the global progress values:
            this._progress.loaded += data.loaded;
            this._progress.total += data.total;
        },

        _onDone: function (result, textStatus, jqXHR, options) {
            var total = options._progress.total,
                response = options._response;
            if (options._progress.loaded < total) {
                // Create a progress event if no final progress event
                // with loaded equaling total has been triggered:
                this._onProgress($.Event('progress', {
                    lengthComputable: true,
                    loaded: total,
                    total: total
                }), options);
            }
            response.result = options.result = result;
            response.textStatus = options.textStatus = textStatus;
            response.jqXHR = options.jqXHR = jqXHR;
            this._trigger('done', null, options);
        },

        _onFail: function (jqXHR, textStatus, errorThrown, options) {
            var response = options._response;
            if (options.recalculateProgress) {
                // Remove the failed (error or abort) file upload from
                // the global progress calculation:
                this._progress.loaded -= options._progress.loaded;
                this._progress.total -= options._progress.total;
            }
            response.jqXHR = options.jqXHR = jqXHR;
            response.textStatus = options.textStatus = textStatus;
            response.errorThrown = options.errorThrown = errorThrown;
            this._trigger('fail', null, options);
        },

        _onAlways: function (jqXHRorResult, textStatus, jqXHRorError, options) {
            // jqXHRorResult, textStatus and jqXHRorError are added to the
            // options object via done and fail callbacks
            this._trigger('always', null, options);
        },

        _onSend: function (e, data) {
            if (!data.submit) {
                this._addConvenienceMethods(e, data);
            }
            var that = this,
                jqXHR,
                aborted,
                slot,
                pipe,
                options = that._getAJAXSettings(data),
                send = function () {
                    that._sending += 1;
                    // Set timer for bitrate progress calculation:
                    options._bitrateTimer = new that._BitrateTimer();
                    jqXHR = jqXHR || (
                        ((aborted || that._trigger(
                            'send',
                            $.Event('send', {delegatedEvent: e}),
                            options
                        ) === false) &&
                        that._getXHRPromise(false, options.context, aborted)) ||
                        that._chunkedUpload(options) || $.ajax(options)
                    ).done(function (result, textStatus, jqXHR) {
                        that._onDone(result, textStatus, jqXHR, options);
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        that._onFail(jqXHR, textStatus, errorThrown, options);
                    }).always(function (jqXHRorResult, textStatus, jqXHRorError) {
                        that._onAlways(
                            jqXHRorResult,
                            textStatus,
                            jqXHRorError,
                            options
                        );
                        that._sending -= 1;
                        that._active -= 1;
                        if (options.limitConcurrentUploads &&
                                options.limitConcurrentUploads > that._sending) {
                            // Start the next queued upload,
                            // that has not been aborted:
                            var nextSlot = that._slots.shift();
                            while (nextSlot) {
                                if (that._getDeferredState(nextSlot) === 'pending') {
                                    nextSlot.resolve();
                                    break;
                                }
                                nextSlot = that._slots.shift();
                            }
                        }
                        if (that._active === 0) {
                            // The stop callback is triggered when all uploads have
                            // been completed, equivalent to the global ajaxStop event:
                            that._trigger('stop');
                        }
                    });
                    return jqXHR;
                };
            this._beforeSend(e, options);
            if (this.options.sequentialUploads ||
                    (this.options.limitConcurrentUploads &&
                    this.options.limitConcurrentUploads <= this._sending)) {
                if (this.options.limitConcurrentUploads > 1) {
                    slot = $.Deferred();
                    this._slots.push(slot);
                    pipe = slot.then(send);
                } else {
                    this._sequence = this._sequence.then(send, send);
                    pipe = this._sequence;
                }
                // Return the piped Promise object, enhanced with an abort method,
                // which is delegated to the jqXHR object of the current upload,
                // and jqXHR callbacks mapped to the equivalent Promise methods:
                pipe.abort = function () {
                    aborted = [undefined, 'abort', 'abort'];
                    if (!jqXHR) {
                        if (slot) {
                            slot.rejectWith(options.context, aborted);
                        }
                        return send();
                    }
                    return jqXHR.abort();
                };
                return this._enhancePromise(pipe);
            }
            return send();
        },

        _onAdd: function (e, data) {
            var that = this,
                result = true,
                options = $.extend({}, this.options, data),
                files = data.files,
                filesLength = files.length,
                limit = options.limitMultiFileUploads,
                limitSize = options.limitMultiFileUploadSize,
                overhead = options.limitMultiFileUploadSizeOverhead,
                batchSize = 0,
                paramName = this._getParamName(options),
                paramNameSet,
                paramNameSlice,
                fileSet,
                i,
                j = 0;
            if (!filesLength) {
                return false;
            }
            if (limitSize && files[0].size === undefined) {
                limitSize = undefined;
            }
            if (!(options.singleFileUploads || limit || limitSize) ||
                    !this._isXHRUpload(options)) {
                fileSet = [files];
                paramNameSet = [paramName];
            } else if (!(options.singleFileUploads || limitSize) && limit) {
                fileSet = [];
                paramNameSet = [];
                for (i = 0; i < filesLength; i += limit) {
                    fileSet.push(files.slice(i, i + limit));
                    paramNameSlice = paramName.slice(i, i + limit);
                    if (!paramNameSlice.length) {
                        paramNameSlice = paramName;
                    }
                    paramNameSet.push(paramNameSlice);
                }
            } else if (!options.singleFileUploads && limitSize) {
                fileSet = [];
                paramNameSet = [];
                for (i = 0; i < filesLength; i = i + 1) {
                    batchSize += files[i].size + overhead;
                    if (i + 1 === filesLength ||
                            ((batchSize + files[i + 1].size + overhead) > limitSize) ||
                            (limit && i + 1 - j >= limit)) {
                        fileSet.push(files.slice(j, i + 1));
                        paramNameSlice = paramName.slice(j, i + 1);
                        if (!paramNameSlice.length) {
                            paramNameSlice = paramName;
                        }
                        paramNameSet.push(paramNameSlice);
                        j = i + 1;
                        batchSize = 0;
                    }
                }
            } else {
                paramNameSet = paramName;
            }
            data.originalFiles = files;
            $.each(fileSet || files, function (index, element) {
                var newData = $.extend({}, data);
                newData.files = fileSet ? element : [element];
                newData.paramName = paramNameSet[index];
                that._initResponseObject(newData);
                that._initProgressObject(newData);
                that._addConvenienceMethods(e, newData);
                result = that._trigger(
                    'add',
                    $.Event('add', {delegatedEvent: e}),
                    newData
                );
                return result;
            });
            return result;
        },

        _replaceFileInput: function (data) {
            var input = data.fileInput,
                inputClone = input.clone(true),
                restoreFocus = input.is(document.activeElement);
            // Add a reference for the new cloned file input to the data argument:
            data.fileInputClone = inputClone;
            $('<form></form>').append(inputClone)[0].reset();
            // Detaching allows to insert the fileInput on another form
            // without loosing the file input value:
            input.after(inputClone).detach();
            // If the fileInput had focus before it was detached,
            // restore focus to the inputClone.
            if (restoreFocus) {
                inputClone.focus();
            }
            // Avoid memory leaks with the detached file input:
            $.cleanData(input.unbind('remove'));
            // Replace the original file input element in the fileInput
            // elements set with the clone, which has been copied including
            // event handlers:
            this.options.fileInput = this.options.fileInput.map(function (i, el) {
                if (el === input[0]) {
                    return inputClone[0];
                }
                return el;
            });
            // If the widget has been initialized on the file input itself,
            // override this.element with the file input clone:
            if (input[0] === this.element[0]) {
                this.element = inputClone;
            }
        },

        _handleFileTreeEntry: function (entry, path) {
            var that = this,
                dfd = $.Deferred(),
                errorHandler = function (e) {
                    if (e && !e.entry) {
                        e.entry = entry;
                    }
                    // Since $.when returns immediately if one
                    // Deferred is rejected, we use resolve instead.
                    // This allows valid files and invalid items
                    // to be returned together in one set:
                    dfd.resolve([e]);
                },
                successHandler = function (entries) {
                    that._handleFileTreeEntries(
                        entries,
                        path + entry.name + '/'
                    ).done(function (files) {
                        dfd.resolve(files);
                    }).fail(errorHandler);
                },
                readEntries = function () {
                    dirReader.readEntries(function (results) {
                        if (!results.length) {
                            successHandler(entries);
                        } else {
                            entries = entries.concat(results);
                            readEntries();
                        }
                    }, errorHandler);
                },
                dirReader, entries = [];
            path = path || '';
            if (entry.isFile) {
                if (entry._file) {
                    // Workaround for Chrome bug #149735
                    entry._file.relativePath = path;
                    dfd.resolve(entry._file);
                } else {
                    entry.file(function (file) {
                        file.relativePath = path;
                        dfd.resolve(file);
                    }, errorHandler);
                }
            } else if (entry.isDirectory) {
                dirReader = entry.createReader();
                readEntries();
            } else {
                // Return an empy list for file system items
                // other than files or directories:
                dfd.resolve([]);
            }
            return dfd.promise();
        },

        _handleFileTreeEntries: function (entries, path) {
            var that = this;
            return $.when.apply(
                $,
                $.map(entries, function (entry) {
                    return that._handleFileTreeEntry(entry, path);
                })
            ).then(function () {
                return Array.prototype.concat.apply(
                    [],
                    arguments
                );
            });
        },

        _getDroppedFiles: function (dataTransfer) {
            dataTransfer = dataTransfer || {};
            var items = dataTransfer.items;
            if (items && items.length && (items[0].webkitGetAsEntry ||
                    items[0].getAsEntry)) {
                return this._handleFileTreeEntries(
                    $.map(items, function (item) {
                        var entry;
                        if (item.webkitGetAsEntry) {
                            entry = item.webkitGetAsEntry();
                            if (entry) {
                                // Workaround for Chrome bug #149735:
                                entry._file = item.getAsFile();
                            }
                            return entry;
                        }
                        return item.getAsEntry();
                    })
                );
            }
            return $.Deferred().resolve(
                $.makeArray(dataTransfer.files)
            ).promise();
        },

        _getSingleFileInputFiles: function (fileInput) {
            fileInput = $(fileInput);
            var entries = fileInput.prop('webkitEntries') ||
                    fileInput.prop('entries'),
                files,
                value;
            if (entries && entries.length) {
                return this._handleFileTreeEntries(entries);
            }
            files = $.makeArray(fileInput.prop('files'));
            if (!files.length) {
                value = fileInput.prop('value');
                if (!value) {
                    return $.Deferred().resolve([]).promise();
                }
                // If the files property is not available, the browser does not
                // support the File API and we add a pseudo File object with
                // the input value as name with path information removed:
                files = [{name: value.replace(/^.*\\/, '')}];
            } else if (files[0].name === undefined && files[0].fileName) {
                // File normalization for Safari 4 and Firefox 3:
                $.each(files, function (index, file) {
                    file.name = file.fileName;
                    file.size = file.fileSize;
                });
            }
            return $.Deferred().resolve(files).promise();
        },

        _getFileInputFiles: function (fileInput) {
            if (!(fileInput instanceof $) || fileInput.length === 1) {
                return this._getSingleFileInputFiles(fileInput);
            }
            return $.when.apply(
                $,
                $.map(fileInput, this._getSingleFileInputFiles)
            ).then(function () {
                return Array.prototype.concat.apply(
                    [],
                    arguments
                );
            });
        },

        _onChange: function (e) {
            var that = this,
                data = {
                    fileInput: $(e.target),
                    form: $(e.target.form)
                };
            this._getFileInputFiles(data.fileInput).always(function (files) {
                data.files = files;
                if (that.options.replaceFileInput) {
                    that._replaceFileInput(data);
                }
                if (that._trigger(
                        'change',
                        $.Event('change', {delegatedEvent: e}),
                        data
                    ) !== false) {
                    that._onAdd(e, data);
                }
            });
        },

        _onPaste: function (e) {
            var items = e.originalEvent && e.originalEvent.clipboardData &&
                    e.originalEvent.clipboardData.items,
                data = {files: []};
            if (items && items.length) {
                $.each(items, function (index, item) {
                    var file = item.getAsFile && item.getAsFile();
                    if (file) {
                        data.files.push(file);
                    }
                });
                if (this._trigger(
                        'paste',
                        $.Event('paste', {delegatedEvent: e}),
                        data
                    ) !== false) {
                    this._onAdd(e, data);
                }
            }
        },

        _onDrop: function (e) {
            e.dataTransfer = e.originalEvent && e.originalEvent.dataTransfer;
            var that = this,
                dataTransfer = e.dataTransfer,
                data = {};
            if (dataTransfer && dataTransfer.files && dataTransfer.files.length) {
                e.preventDefault();
                this._getDroppedFiles(dataTransfer).always(function (files) {
                    data.files = files;
                    if (that._trigger(
                            'drop',
                            $.Event('drop', {delegatedEvent: e}),
                            data
                        ) !== false) {
                        that._onAdd(e, data);
                    }
                });
            }
        },

        _onDragOver: getDragHandler('dragover'),

        _onDragEnter: getDragHandler('dragenter'),

        _onDragLeave: getDragHandler('dragleave'),

        _initEventHandlers: function () {
            if (this._isXHRUpload(this.options)) {
                this._on(this.options.dropZone, {
                    dragover: this._onDragOver,
                    drop: this._onDrop,
                    // event.preventDefault() on dragenter is required for IE10+:
                    dragenter: this._onDragEnter,
                    // dragleave is not required, but added for completeness:
                    dragleave: this._onDragLeave
                });
                this._on(this.options.pasteZone, {
                    paste: this._onPaste
                });
            }
            if ($.support.fileInput) {
                this._on(this.options.fileInput, {
                    change: this._onChange
                });
            }
        },

        _destroyEventHandlers: function () {
            this._off(this.options.dropZone, 'dragenter dragleave dragover drop');
            this._off(this.options.pasteZone, 'paste');
            this._off(this.options.fileInput, 'change');
        },

        _setOption: function (key, value) {
            var reinit = $.inArray(key, this._specialOptions) !== -1;
            if (reinit) {
                this._destroyEventHandlers();
            }
            this._super(key, value);
            if (reinit) {
                this._initSpecialOptions();
                this._initEventHandlers();
            }
        },

        _initSpecialOptions: function () {
            var options = this.options;
            if (options.fileInput === undefined) {
                options.fileInput = this.element.is('input[type="file"]') ?
                        this.element : this.element.find('input[type="file"]');
            } else if (!(options.fileInput instanceof $)) {
                options.fileInput = $(options.fileInput);
            }
            if (!(options.dropZone instanceof $)) {
                options.dropZone = $(options.dropZone);
            }
            if (!(options.pasteZone instanceof $)) {
                options.pasteZone = $(options.pasteZone);
            }
        },

        _getRegExp: function (str) {
            var parts = str.split('/'),
                modifiers = parts.pop();
            parts.shift();
            return new RegExp(parts.join('/'), modifiers);
        },

        _isRegExpOption: function (key, value) {
            return key !== 'url' && $.type(value) === 'string' &&
                /^\/.*\/[igm]{0,3}$/.test(value);
        },

        _initDataAttributes: function () {
            var that = this,
                options = this.options,
                data = this.element.data();
            // Initialize options set via HTML5 data-attributes:
            $.each(
                this.element[0].attributes,
                function (index, attr) {
                    var key = attr.name.toLowerCase(),
                        value;
                    if (/^data-/.test(key)) {
                        // Convert hyphen-ated key to camelCase:
                        key = key.slice(5).replace(/-[a-z]/g, function (str) {
                            return str.charAt(1).toUpperCase();
                        });
                        value = data[key];
                        if (that._isRegExpOption(key, value)) {
                            value = that._getRegExp(value);
                        }
                        options[key] = value;
                    }
                }
            );
        },

        _create: function () {
            this._initDataAttributes();
            this._initSpecialOptions();
            this._slots = [];
            this._sequence = this._getXHRPromise(true);
            this._sending = this._active = 0;
            this._initProgressObject(this);
            this._initEventHandlers();
        },

        // This method is exposed to the widget API and allows to query
        // the number of active uploads:
        active: function () {
            return this._active;
        },

        // This method is exposed to the widget API and allows to query
        // the widget upload progress.
        // It returns an object with loaded, total and bitrate properties
        // for the running uploads:
        progress: function () {
            return this._progress;
        },

        // This method is exposed to the widget API and allows adding files
        // using the fileupload API. The data parameter accepts an object which
        // must have a files property and can contain additional options:
        // .fileupload('add', {files: filesList});
        add: function (data) {
            var that = this;
            if (!data || this.options.disabled) {
                return;
            }
            if (data.fileInput && !data.files) {
                this._getFileInputFiles(data.fileInput).always(function (files) {
                    data.files = files;
                    that._onAdd(null, data);
                });
            } else {
                data.files = $.makeArray(data.files);
                this._onAdd(null, data);
            }
        },

        // This method is exposed to the widget API and allows sending files
        // using the fileupload API. The data parameter accepts an object which
        // must have a files or fileInput property and can contain additional options:
        // .fileupload('send', {files: filesList});
        // The method returns a Promise object for the file upload call.
        send: function (data) {
            if (data && !this.options.disabled) {
                if (data.fileInput && !data.files) {
                    var that = this,
                        dfd = $.Deferred(),
                        promise = dfd.promise(),
                        jqXHR,
                        aborted;
                    promise.abort = function () {
                        aborted = true;
                        if (jqXHR) {
                            return jqXHR.abort();
                        }
                        dfd.reject(null, 'abort', 'abort');
                        return promise;
                    };
                    this._getFileInputFiles(data.fileInput).always(
                        function (files) {
                            if (aborted) {
                                return;
                            }
                            if (!files.length) {
                                dfd.reject();
                                return;
                            }
                            data.files = files;
                            jqXHR = that._onSend(null, data);
                            jqXHR.then(
                                function (result, textStatus, jqXHR) {
                                    dfd.resolve(result, textStatus, jqXHR);
                                },
                                function (jqXHR, textStatus, errorThrown) {
                                    dfd.reject(jqXHR, textStatus, errorThrown);
                                }
                            );
                        }
                    );
                    return this._enhancePromise(promise);
                }
                data.files = $.makeArray(data.files);
                if (data.files.length) {
                    return this._onSend(null, data);
                }
            }
            return this._getXHRPromise(false, data && data.context);
        }

    });

}));
/*
 * jQuery Iframe Transport Plugin
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2011, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/* global define, require, window, document */


(function (factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        // Register as an anonymous AMD module:
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // Node/CommonJS:
        factory(require('jquery'));
    } else {
        // Browser globals:
        factory(window.jQuery);
    }
}(function ($) {
    'use strict';

    // Helper variable to create unique names for the transport iframes:
    var counter = 0;

    // The iframe transport accepts four additional options:
    // options.fileInput: a jQuery collection of file input fields
    // options.paramName: the parameter name for the file form data,
    //  overrides the name property of the file input field(s),
    //  can be a string or an array of strings.
    // options.formData: an array of objects with name and value properties,
    //  equivalent to the return data of .serializeArray(), e.g.:
    //  [{name: 'a', value: 1}, {name: 'b', value: 2}]
    // options.initialIframeSrc: the URL of the initial iframe src,
    //  by default set to "javascript:false;"
    $.ajaxTransport('iframe', function (options) {
        if (options.async) {
            // javascript:false as initial iframe src
            // prevents warning popups on HTTPS in IE6:
            /*jshint scripturl: true */
            var initialIframeSrc = options.initialIframeSrc || 'javascript:false;',
            /*jshint scripturl: false */
                form,
                iframe,
                addParamChar;
            return {
                send: function (_, completeCallback) {
                    form = $('<form style="display:none;"></form>');
                    form.attr('accept-charset', options.formAcceptCharset);
                    addParamChar = /\?/.test(options.url) ? '&' : '?';
                    // XDomainRequest only supports GET and POST:
                    if (options.type === 'DELETE') {
                        options.url = options.url + addParamChar + '_method=DELETE';
                        options.type = 'POST';
                    } else if (options.type === 'PUT') {
                        options.url = options.url + addParamChar + '_method=PUT';
                        options.type = 'POST';
                    } else if (options.type === 'PATCH') {
                        options.url = options.url + addParamChar + '_method=PATCH';
                        options.type = 'POST';
                    }
                    // IE versions below IE8 cannot set the name property of
                    // elements that have already been added to the DOM,
                    // so we set the name along with the iframe HTML markup:
                    counter += 1;
                    iframe = $(
                        '<iframe src="' + initialIframeSrc +
                            '" name="iframe-transport-' + counter + '"></iframe>'
                    ).bind('load', function () {
                        var fileInputClones,
                            paramNames = $.isArray(options.paramName) ?
                                    options.paramName : [options.paramName];
                        iframe
                            .unbind('load')
                            .bind('load', function () {
                                var response;
                                // Wrap in a try/catch block to catch exceptions thrown
                                // when trying to access cross-domain iframe contents:
                                try {
                                    response = iframe.contents();
                                    // Google Chrome and Firefox do not throw an
                                    // exception when calling iframe.contents() on
                                    // cross-domain requests, so we unify the response:
                                    if (!response.length || !response[0].firstChild) {
                                        throw new Error();
                                    }
                                } catch (e) {
                                    response = undefined;
                                }
                                // The complete callback returns the
                                // iframe content document as response object:
                                completeCallback(
                                    200,
                                    'success',
                                    {'iframe': response}
                                );
                                // Fix for IE endless progress bar activity bug
                                // (happens on form submits to iframe targets):
                                $('<iframe src="' + initialIframeSrc + '"></iframe>')
                                    .appendTo(form);
                                window.setTimeout(function () {
                                    // Removing the form in a setTimeout call
                                    // allows Chrome's developer tools to display
                                    // the response result
                                    form.remove();
                                }, 0);
                            });
                        form
                            .prop('target', iframe.prop('name'))
                            .prop('action', options.url)
                            .prop('method', options.type);
                        if (options.formData) {
                            $.each(options.formData, function (index, field) {
                                $('<input type="hidden"/>')
                                    .prop('name', field.name)
                                    .val(field.value)
                                    .appendTo(form);
                            });
                        }
                        if (options.fileInput && options.fileInput.length &&
                                options.type === 'POST') {
                            fileInputClones = options.fileInput.clone();
                            // Insert a clone for each file input field:
                            options.fileInput.after(function (index) {
                                return fileInputClones[index];
                            });
                            if (options.paramName) {
                                options.fileInput.each(function (index) {
                                    $(this).prop(
                                        'name',
                                        paramNames[index] || options.paramName
                                    );
                                });
                            }
                            // Appending the file input fields to the hidden form
                            // removes them from their original location:
                            form
                                .append(options.fileInput)
                                .prop('enctype', 'multipart/form-data')
                                // enctype must be set as encoding for IE:
                                .prop('encoding', 'multipart/form-data');
                            // Remove the HTML5 form attribute from the input(s):
                            options.fileInput.removeAttr('form');
                        }
                        form.submit();
                        // Insert the file input fields at their original location
                        // by replacing the clones with the originals:
                        if (fileInputClones && fileInputClones.length) {
                            options.fileInput.each(function (index, input) {
                                var clone = $(fileInputClones[index]);
                                // Restore the original name and form properties:
                                $(input)
                                    .prop('name', clone.prop('name'))
                                    .attr('form', clone.attr('form'));
                                clone.replaceWith(input);
                            });
                        }
                    });
                    form.append(iframe).appendTo(document.body);
                },
                abort: function () {
                    if (iframe) {
                        // javascript:false as iframe src aborts the request
                        // and prevents warning popups on HTTPS in IE6.
                        // concat is used to avoid the "Script URL" JSLint error:
                        iframe
                            .unbind('load')
                            .prop('src', initialIframeSrc);
                    }
                    if (form) {
                        form.remove();
                    }
                }
            };
        }
    });

    // The iframe transport returns the iframe content document as response.
    // The following adds converters from iframe to text, json, html, xml
    // and script.
    // Please note that the Content-Type for JSON responses has to be text/plain
    // or text/html, if the browser doesn't include application/json in the
    // Accept header, else IE will show a download dialog.
    // The Content-Type for XML responses on the other hand has to be always
    // application/xml or text/xml, so IE properly parses the XML response.
    // See also
    // https://github.com/blueimp/jQuery-File-Upload/wiki/Setup#content-type-negotiation
    $.ajaxSetup({
        converters: {
            'iframe text': function (iframe) {
                return iframe && $(iframe[0].body).text();
            },
            'iframe json': function (iframe) {
                return iframe && $.parseJSON($(iframe[0].body).text());
            },
            'iframe html': function (iframe) {
                return iframe && $(iframe[0].body).html();
            },
            'iframe xml': function (iframe) {
                var xmlDoc = iframe && iframe[0];
                return xmlDoc && $.isXMLDoc(xmlDoc) ? xmlDoc :
                        $.parseXML((xmlDoc.XMLDocument && xmlDoc.XMLDocument.xml) ||
                            $(xmlDoc.body).html());
            },
            'iframe script': function (iframe) {
                return iframe && $.globalEval($(iframe[0].body).text());
            }
        }
    });

}));
var RepositoryLoading = {
	element : function(){
		this.loading 	  = $("<div />", {class: "c-loading"});
		this.loadingInner = $("<span />", {class: "c-loading__inner"}).appendTo(this.loading);		
	},
	init: function(){
		this.element();
		this.loading.appendTo( $(".workspace") );
		RepositoryLoading.close();		
	},
	open : function() {
		this.loading.show();
	},
	close : function() { 
		this.loading.hide();
	}
};

RepositoryLoading.init();
var FolderAndFiles = function (item) {
	this.item = $(item);
	//
	this.interfaceElements = {
		detailBox 		: $(".rp_details"), // interfaceElements.detailBox
		wrapperBox		: $(".rp_content"), // interfaceElements.wrapperBox
		detailBoxInner  : $(".rp_details__inner"),
	}
	//
	this.UI();
	this.observe();
	//
	// this.click = 0;
	// prevent doubleclick
	this.timer;
	this.prevent = false;
	return this;
};
FolderAndFiles.prototype.UI = function () {
	this.UIelements = "input[type='checkbox'] , input.toggle_multiselection , button[data-action] , div.folder , div.file";
};
FolderAndFiles.prototype.observe = function () {
	//
	this.item.on("mouseenter", $.proxy(this.setClass, this));
	this.item.on("mouseleave", $.proxy(this.deleteClass, this));
	//
	this.item.on("dblclick doubletap", $.proxy(this.gotoFolder, this));
	this.item.on("click tap", $.proxy(this.foundDetails, this));
};
FolderAndFiles.prototype.setClass = function (event) {
	// console.info( "setClass ", event)
	if (event.type == "mouseenter") {
		
		if( this.item.hasClass("file") ){
			repository.activeData.isSelectFiles ? this.item.addClass("is-hover") : ""
		}
		if( this.item.hasClass("folder") ){
			repository.activeData.isSelectFolders ? this.item.addClass("is-hover") : ""
		}
		
	}
};
FolderAndFiles.prototype.deleteClass = function (event) {
	
	if (event.type == "mouseleave") {

		if( this.item.hasClass("file") ){
			repository.activeData.isSelectFiles ? this.item.removeClass("is-hover") : ""
		}
		if( this.item.hasClass("folder") ){
			repository.activeData.isSelectFolders ? this.item.removeClass("is-hover") : ""
		}

	}

	if (event.type == "click") {
		if( this.item.hasClass("file") ){
			repository.activeData.isSelectFiles ? this.item.removeClass("is-selected") : ""
		}
		if( this.item.hasClass("folder") ){
			repository.activeData.isSelectFolders ? this.item.removeClass("is-selected") : ""
		}
	}
};
FolderAndFiles.prototype.gotoFolder = function (event) {
	console.info( "gotoFolder ", event, $(event.currentTarget).hasClass("rp_item--folder"))
	if ( $(event.currentTarget).hasClass("rp_item--folder") ) {
		
		RepositoryLoading.open(); // loading

		var $item = $(event.currentTarget);
		//
		globalVar.Uid_Repository = $item.data("uidrepository") === undefined ? "" : $item.data("uidrepository");
		globalVar.Uid_Parent     = $item.data("uid") === undefined ? "" : $item.data("uid");
		//
		repository.toggleloading("open");
		repository.breadcumbs.ajaxCall(event);
		repository.requestRefresh('navigateToFolder');

		// repository.breadcumbs("add", $(event.currentTarget) );
	}
};
FolderAndFiles.prototype.clickEvent = function(event){
	
	var self = this;

	if ( event.type == "click" ){

		this.timer = setTimeout(function() {

		  if (!self.prevent) {
			// click
			self.foundDetails(event)
		  }

		  self.prevent = false;

		}, 300);

	}else if( event.type == "dblclick" ){

		clearTimeout(this.timer);
		this.prevent = true;
		self.gotoFolder(event);

	}
	
};
FolderAndFiles.prototype.foundDetails = function ( event ) {

	event.stopPropagation();

	var 
		self = this,
		$target = $( event.currentTarget )
	;

	if(event.shiftKey && !repository.methods.currentAction){
		repository.ismultiselection = true;

		var firstElemIndex =  $(".rp_item.is-selected").first().index();
		var index = $target.index();
		selectOtherElements(firstElemIndex, index);

	}else{

		if( ! repository.multiselection.prop("checked") ){
			repository.ismultiselection = false;
		}

	}

	if (this.item.hasClass("is-selected")) {
		this.deleteClass(event);
	} else {
		
		if( this.item.hasClass("rp_item--file") ){
			repository.activeData.isSelectFiles ? this.item.addClass("is-selected") : ""
		}
		if( this.item.hasClass("rp_item--folder") ){
			repository.activeData.isSelectFolders ? this.item.addClass("is-selected") : ""
		}

	}

	if (!repository.ismultiselection) {

		$(".rp_item").not(this.item).removeClass("is-selected");

	}

	function selectOtherElements(a, b){
		if(a > b){
			startIndex = b;
			lastIndex = a;
		}else{
			startIndex = a;
			lastIndex = b;
		}
		$(".rp_item").each( function(i,el){
			if ( i > startIndex && i < lastIndex ){
				$(el).addClass("is-selected");
			}
		});
	}
	// controllo che elementi sono stati selezionati
	repository.controlElementonPage();
	this.popolateDetail($target);

	// console.log(elementsSelected); // stampo gli elementi selezionati
	// se clicco fuori resetto i selezionati

	$(document).one("click", function (event) {
		
		if ( !$(event.currentTarget).is(self.UIelements) && !$(event.target).is(self.UIelements) ) {
			$(".rp_item").removeClass("is-selected");
			repository.controlElementonPage();
			var
				fileBox = $(".rp_file__details"),
				undefinedBox = $(".rp_undefined__details"),
				folderBox = $(".rp_folder__details")
			;

			fileBox.hide();
			undefinedBox.show();
			folderBox.hide();
			// self.popolateDetail( event.currentTarget );
			
			if(repository.methods.currentAction){
				// $('.workspace').attr("data-details", "");
				// self.UIdetailList = 
			}
			
		}
	});
};
FolderAndFiles.prototype.popolateDetail = function (event) {

	var UID = event.attr("data-uid") || "" ;
	var self = this;
	//diocane

	this.interfaceElements.detailBoxInner.addClass("has--loading");

	if (!repository.ismultiselection){

		$.ajax({
			url: globalVar.wsUrl + repository.methods.getRepositoryItem,
			data: "uid_CmsNlsContext="+ globalVar.CmsNlsContext +"&uid_CmsUsers="+ globalVar.CmsUsers + "&uid_Repository="+globalVar.Uid_Repository+"&uid="+ UID ,
			method: "POST",
			success: function(data){

				var info = data[0];
				var fileBox = $(".rp_file__details");
				var undefinedBox = $(".rp_undefined__details");
				var folderBox = $(".rp_folder__details");
				var folderHtml = $(".rp_folder__content");
				var fileHtml = $(".rp_file__content");
				var fileDetails = $(".rp_file__details");

				if (typeof info != "undefined"){

					if ( info.typo === "file"){
						folderBox.hide();
						undefinedBox.hide();
						fileDetails.find(".rp_preview").css( {
							"background-image" : "url(" + encodeURI(info.thumb) + ")"
						});
						fileDetails.find("a").attr( "href", info.original );
						fileHtml.html("");
						$.each( info.details, function(i,definitionlist){
							fileHtml.append( createDefinitionList(definitionlist) );
						});

						fileBox.show();
					}

					if ( info.typo == "folder"){
						fileBox.hide();
						undefinedBox.hide();
						folderHtml.html("");
						$.each( info.details, function(i,definitionlist){
							folderHtml.append( createDefinitionList(definitionlist) );
						});

						folderBox.show();
					}

				}else{					

					undefinedBox.show();
					folderBox.hide();
					fileBox.hide();

				}

				self.interfaceElements.detailBoxInner.removeClass("has--loading");

			},
			error: function(){

				self.interfaceElements.detailBoxInner.removeClass("has--loading");

			}
		});

	}else{
		// se mi trovo in multiselezione io ricevo un array con gli oggetti passati a repository        
		$(".rp_folder__details .rp_folder__content").html("");

		for (var i = 0; i < repository.selectedArray.length; i++) {
			var itemHtml = createListFolder(repository.selectedArray[i]);
			$(".rp_folder__details .rp_folder__content").append(itemHtml);
		}

		self.interfaceElements.detailBoxInner.removeClass("has--loading");
	}    

	function createListFolder(item) {
		var type = item.hasClass("rp_item--file") ? "file" : "folder";
		var text = item.find(".rp_name").text();
		var html = $("<div />", { "text": text , "class": type });

		return html;
	};

	function createDefinitionList(value) {
		var 
			HtmlItem = $("<dl />", { "class": "detail-row" }),
			HtmlDt = $("<dt />"),
			HtmlDd = $("<dd />")
		;

		HtmlDt.html(value.label);
		HtmlDd.html(value.value);
		HtmlItem.append(HtmlDt, HtmlDd);

		return HtmlItem;
	};

};

var repository, uploadfiles, breadcumbs;

var Repository = function (NlsContext, CmsUsers) {    

	this.interfaceElements = {
		Workspace : $(".rp_wrap"),                  		// interfaceElements.Workspace
		Details : $(".rp_details"),                         // interfaceElements.Details
		RepositoryContainerInner : $(".rp_content__inner"), // interfaceElements.BtnMultiselection
		RepositoryContainer : $(".rp_content"),     		// interfaceElements.RepositoryContainer
		RepositoryDiv : $("#Panel_Repository"),     		// interfaceElements.RepositoryDiv
		MainDocument : $(document.documentElement), 		// interfaceElements.MainDocument
		Window : $(window), 								// interfaceElements.Window
	};

	this.methods = {
		deleteItems: "Delete",
		moveItems: "Move",
		rename: "Rename",
		createfolder: "CreateFolder",
		listFolder: "List",
		searchFull: "Search",
		getRepositoryItem: "Detail",
	};

	this.icons = {
		folder : "fa-folder-o",
		images : "fa-file-image-o",
		pdf    : "fa-file-pdf-o",
		archive: "fa-file-archive-o",
		text   : "fa-file-text-o",
		excel  : "fa-file-excel-o",
		movie  : "fa-file-video-o"
	};

	this.activeData = {
		isSelectFiles : true,
		isSelectFolders : true,
	};



	this.loading = $(".loading-container");
	this.methods.currentAction = false;

	this.UI();
	this.toggleloading("open");

	var self = this;

	$(window).on("load", function () {
		$.ajax({
			url: globalVar.wsUrl + self.methods.listFolder,
			type: 'POST',
			dataType: 'json',
			data: 'uid_CmsNlsContext=' + globalVar.CmsNlsContext + '&uid_CmsUsers=' + globalVar.CmsUsers + '&Uid_Repository=' + globalVar.Uid_Repository + '&Uid_Parent=' + globalVar.Uid_Parent,
			success: function (data) {
				self.createWrap(data);
				self.items = $(".rp_item");
			},
			error: function (error) {
				// genericmodal
			}
		});
	});

	this.breadcumbs = new Breadcumbs();

	this.changeTypeSelection();
	this.observe();

};
// Duplicata da estendere LOAD
Repository.prototype.openRepository = function(event,_){
	
	event.preventDefault();

	var _ = this;

	this.selectOnlyForm = false;
		
	if(typeof event.data != "undefined"){

		if(event.data.from == "crop"){

			if( $(event.currentTarget).parents(".inform-upload").length ){
				this.fromcrop = $(event.currentTarget).parents(".inform-upload").data("configuration");
			}else{
				this.fromcrop = false;

			}
		
		}

		if(event.data.from == "folder"){

			this.activeData = {
				isSelectFiles : true,
				isSelectFolders : true,
				onlyFolder : true
			};
			this.selectedBtn.hide();
			this.selectOnlyForm = $(event.currentTarget).data();
			this.selectedFolderBtn.attr("disabled", null).show();

		}else{
			this.selectedBtn.show();
		}

	}

	this.interfaceElements.RepositoryDiv.attr("data-state","open");
	this.interfaceElements.MainDocument.addClass("repository-is-open");

	setTimeout(function(){
		_.setDimensions();
	},250)	

	return false;

};
Repository.prototype.closeRepository = function(event){

	event.preventDefault();

	this.interfaceElements.RepositoryDiv.attr("data-state", "");
	this.interfaceElements.MainDocument.removeClass("repository-is-open");

	this.activeData = {
		isSelectFiles : true,
		isSelectFolders : true,
	};


	return false;
};
Repository.prototype.requestRefresh = function (method ,name ,uid) {

		var self = this,
			data = "",
			wsMethod,
			uidItem
		;

		if (method == "navigateToFolder") {
			wsMethod = self.methods.listFolder;
			data = 'uid_CmsNlsContext=' + window.globalVar.CmsNlsContext + '&Uid_CmsUsers=' + window.globalVar.CmsUsers + '&Uid_Repository=' + window.globalVar.Uid_Repository + '&Uid_Parent=' + window.globalVar.Uid_Parent;
		}

		if (method == "CreateFolder") {
			wsMethod = self.methods.createfolder;
			data = 'uid_CmsNlsContext=' + window.globalVar.CmsNlsContext + '&Uid_CmsUsers=' + window.globalVar.CmsUsers + '&Uid_Repository=' + window.globalVar.Uid_Repository + '&Uid_Parent=' + window.globalVar.Uid_Parent + '&name=' + name;
		}

		if (method == "move") {
			wsMethod = self.methods.moveItems;
			data = 'uid_CmsNlsContext=' + window.globalVar.CmsNlsContext + '&Uid_CmsUsers=' + window.globalVar.CmsUsers + '&Uid_Repository=' + window.globalVar.Uid_Repository + '&Uid_Parent=' +  globalVar.Uid_Parent + '&uid_Items=' + self.prepareData();
		}

		if (method == "delete") {
			wsMethod = self.methods.deleteItems;
			data = 'uid_CmsNlsContext=' + window.globalVar.CmsNlsContext + '&Uid_CmsUsers=' + window.globalVar.CmsUsers + '&Uid_Repository=' + window.globalVar.Uid_Repository + '&Uid_Parent=' + window.globalVar.Uid_Parent + '&uid_Items=' + self.prepareData();
		}

		if (method == "rename") {
			wsMethod = self.methods.rename;
			uidItem = uid;
			data = 'uid_CmsNlsContext=' + window.globalVar.CmsNlsContext + '&Uid_CmsUsers=' + window.globalVar.CmsUsers + '&Uid_Repository=' + window.globalVar.Uid_Repository + '&Uid_Parent=' + window.globalVar.Uid_Parent + '&uid_Item=' + uidItem + '&name=' + name;
		}
		if (method == "searchFull") {
			wsMethod = self.methods.searchFull;
			data = 'uid_CmsNlsContext=' + window.globalVar.CmsNlsContext + '&Uid_CmsUsers=' + window.globalVar.CmsUsers + '&Uid_Repository=' + window.globalVar.Uid_Repository + '&Uid_Parent=' + window.globalVar.Uid_Parent + '&text=' + name;
		}

		$.ajax({
			url: globalVar.wsUrl + wsMethod,
			type: 'POST',
			dataType: 'json',
			data: data,
			success: function (data) {
				if( method == "move" ){ 
					$("#modal-move").modal('hide'); self.correlated.hide(); 
				} 
				if( method == "delete" ){ 
					$("#modal-delete").modal('hide'); 
				};
			if( method == "searchFull"){
				self.breadcumbs.breadcumbsContainer.html("<div class='search-query'>" + window.globalVar.LabelSearch + "<em>" + name + "</em></div>");
			}
			if( data.response == null || data.response == 'undefined' ){

				self.createWrap(data);
				RepositoryLoading.close();
				self.items = $(".rp_item");

			}else{
				alert(data.response);
			}

			self.controlElementonPage(); // ricontrollo il numero di elementi selezionati

			RepositoryLoading.close();

		},
		error: function (error) {
			// genericmodal
			RepositoryLoading.close();
		}
	});
	
};
Repository.prototype.toggleloading = function (state) {
	var self = this;
	this.loading_timeout = setTimeout(function(){
		// if (state == "close") {
		//     self.loading.removeClass("loading-active");
		//     self.loading.addClass("loading-inactive");
		// }
		// if (state == "open") {
		//     self.loading.removeClass("loading-inactive");
		//     self.loading.addClass("loading-active");
		// }
	}, 500);
}
Repository.prototype.UI = function () {

	// this.UIdetailList           = $(".details-list"); // dettagli in lista
	// this.UIdetailBox            = $(".detail-box"); // dettagli in lista
	// this.UIpreview              = this.UIdetailBox.find(".preview"); // dettagli in lista
	// this.UIdetails              = this.UIdetailBox.find(".details"); // dettagli in lista

	this.modalDelete            = $("#modal-delete");
	this.modalMove              = $("#modal-move");
	this.modalGeneric           = $("#modal-generic");
	this.modalCrop              = $("#modal-crop");

	this.multiselection = $(".toggle_multiselection");

	// Buttons
	this.moveBtn                = $("[data-action='js-move']");
	this.moveConfirm            = $("[data-action='js-confirmmove']");
	this.deleteBtn              = $("[data-action='js-delete']");
	this.uploadBtn              = $("[data-action='js-uploadfile']");
	this.renameBtn              = $("[data-action='js-renamefile']");
	this.createBtn              = $("[data-action='js-create-folder']");
	this.closeRepoBtn           = $("[data-action='js-close-repository']");
	this.selectedBtn            = $("[data-action='js-selected']");
	this.selectedFolderBtn		= $("[data-action='js-selected-folder']");
	this.resetBtn               = $("[data-action='js-reset-actions']");
	this.modalBtnMoveConfirm    = $("[data-action='js-confirm-move']");
	this.modalBtnDeleteConfirm  = $("[data-action='js-confirm-delete']");
	this.closeDetails           = $("[data-action='show-hide-detail']");

	this.correlated             = $('[data-correlated="js-move"]');
	this.searchBar              = $(".search-input");
	this.searchBtn              = $("[data-action='js-search']");
	this.filterBar              = $("[data-action='js-inpage-search']");

	this.openRepoBtn            = $("[data-action='js-open-repository']");
	this.openRepoFolder         = $('[data-action="js-uploadfolder"]');

	this.activeData = {
		isSelectFiles : true,
		isSelectFolders : true,
		onlyFolder : false
	};
	
	
	// Disable Buttons
	this.moveConfirm.attr("disabled", "disabled").hide();
	this.selectedFolderBtn.attr("disabled", "disabled").hide();
};
Repository.prototype.observe = function () {

	this.interfaceElements.Window.on("load resize orientationchange", $.proxy( this.setDimensions , this ) )

	this.multiselection.on("change", $.proxy(this.changeTypeSelection, this));
	this.createBtn.on("click", $.proxy(this.createNewFolder, this));
	this.renameBtn.on("click", $.proxy(this.renameFolder, this));
	this.moveConfirm.on("click", function(){ $("#modal-move").modal(); });
	this.modalBtnMoveConfirm.on("click", $.proxy(this.confirmActions, this));
	this.modalBtnDeleteConfirm.on("click", $.proxy(this.confirmActions, this));
	
	this.openRepoBtn.on("click", {from: "crop"} , $.proxy(this.openRepository, this ));
	this.openRepoFolder.on("click", {from: "folder"} , $.proxy(this.openRepository, this ));

	this.closeRepoBtn.on("click", $.proxy(this.closeRepository, this ));

	this.resetBtn.on("click", $.proxy(this.resetAction, this));
	this.filterBar.on("keyup", $.proxy(this.searchInPage, this));
	this.searchBtn.on("click", $.proxy(this.searchBoxAction, this));
	this.searchBar.on("keypress", $.proxy(this.searchBoxAction, this));

	this.selectedBtn.on("click", $.proxy(this.activateCrop, this) );
	this.selectedFolderBtn.on("click", $.proxy(this.controlFolder, this) );
	this.correlated.hide();    

	this.moveBtn.on("click", {action:"move"}, $.proxy(this.prepareAction, this));
	this.deleteBtn.on("click",{action:"delete"}, $.proxy(this.prepareAction, this));

	/* toggle class for open e close details */
	this.closeDetails.on("click", $.proxy(this.openDetails, this) );

};
Repository.prototype.setDimensions = function(){
	var 
		oT,
		hH = $(".rp_header").outerHeight(true),
		sH = $(".wrapper-subheader").outerHeight(true),
		totalH
	;



	if( $("html").hasClass("repository-is-open") ){
		oT = $(".navbar").outerHeight(true);



	}else{
		oT = $(".rp_header").offset().top;
	}

	totalH = this.interfaceElements.Window.outerHeight() - (oT + hH + sH + 25);

	this.interfaceElements.Details.css({
		height : totalH,
		position: "relative"
	});
	this.interfaceElements.RepositoryContainerInner.css({
		height : totalH
	});
	this.interfaceElements.RepositoryContainer.css({
		height : totalH
	});
};
Repository.prototype.openDetails = function(event){

    event.preventDefault();

    if( this.interfaceElements.RepositoryContainer.attr("data-detail") == "open" ){
        this.interfaceElements.RepositoryContainer.attr("data-detail", null);
        window.sessionStorage.setItem("detailsshow", false)
    }else{
        this.interfaceElements.RepositoryContainer.attr("data-detail","open");   
        window.sessionStorage.setItem("detailsshow", true)
    }

};
Repository.prototype.activateCrop = function(){
	
	var self = this, selectedElement, image_up_block;
	selectedElement = $(".rp_item.is-selected");

	if( selectedElement.hasClass("rp_item--file") ){

		var fakeimg =  new Image();
			fakeimg.src = selectedElement.data("originalpath");
		
		fakeimg.onload = function(){
			self.fromcrop.imageWidth = this.width;
			self.fromcrop.imageHeight = this.height;
		}

		$("#box_image").attr("src" , null);
		$("#box_image").attr("src" ,  selectedElement.data("originalpath") );

		if( this.fromcrop.croptype == "true" ) {
			
			// apro la modale
			$("#modal-crop").trigger("openModalCrop", { "inputValues": this.fromcrop });

		}
		else{
			var 
				image_up_block = $("#" + self.fromcrop.inputUrlID ).parents(".inform-upload"),
				item           = selectedElement.data("item-options"),
				imagePath      = selectedElement.data("originalpath"),
                image          = $("<img />", { 
                    "class" : "inform-image",
                    "src" : imagePath    
                });
			;
            image_up_block.find(".inform-thumb").find("img").remove();
			image_up_block.find(".inform-thumb").append( image );
			image_up_block.find(".inform-name").text( item[0].value );
            image_up_block.find(".btn.cancel").show();

			$("#" + self.fromcrop.inputUrlID ).val( imagePath );


            console.log( self.fromcrop.instance )

			repository.closeRepoBtn.trigger("click");
		}
	}else{

		console.warn("non è un file");

	}

};
Repository.prototype.resetSelection = function () {
	this.selectedElement = "";
};
Repository.prototype.confirmSelection = function () {
	this.selectedElement = this.selectedArray;
	return this.selectedArray;
};
Repository.prototype.searchInPage = function(event){

	if ( event.keyCode === 13 ){
		
		event.preventDefault();

		var 
			term = $( event.currentTarget ).val(),
			termLower = term.toLowerCase()
		;

		if ( term.length >= 1 ){

			$(".rp_item").each(function(i, el){
				var textToconfront = $(el).find(".rp_name").text().toLowerCase();

				if( textToconfront.indexOf(termLower) != -1 ){
					$(el).show();
				}else{
					$(el).hide();
				}
			});

		}else{
			$(".rp_item").show();
		}

	}else{

		var 
			term = $( event.currentTarget ).val(),
			termLower = term.toLowerCase()
		;

		if ( term.length >= 3 ){

			$(".rp_item").each(function(i, el){
				var textToconfront = $(el).find(".rp_name").text().toLowerCase();

				if( textToconfront.indexOf(termLower) != -1 ){
					$(el).show();
				}else{
					$(el).hide();
				}
			});

		}else{
			$(".rp_item").show();
		}
		
	}
	

};
Repository.prototype.controlFolder = function(ev){

	if(this.activeData.onlyFolder){

		var $element = this.confirmSelection();
		var textPath = 	$("#" + this.selectOnlyForm.id ).parents(".inform-folder-upload").find(".inform-folder-name");
		var $uid = $("#" + this.selectOnlyForm.folderUid );
		var $folder = $("#" + this.selectOnlyForm.folderUrl );

		var data = $element[0].data();
			

		console.log(data.objectData)
		if( $element[0].hasClass("rp_item--folder") ){
		    
			$uid.val( data.uid ).change();
			$folder.val( data.objectData.path ).change();
			textPath.text( data.objectData.path );

			repository.closeRepoBtn.trigger("click");

			this.activeData.onlyFolder = false;

		}else{
			return false;
		}


	}

};
Repository.prototype.prepareData = function(){
	var itemListPiped = "";

	for(var q = 0; q < this.selectedElement.length ; q++){
		if( q == 0 ){
			itemListPiped += this.selectedElement[q].data("uid");    
		}else{
			itemListPiped += "|" + this.selectedElement[q].data("uid");    
		}
	}

	return itemListPiped;
};
Repository.prototype.prepareAction = function(event){
	this.resetBtn.attr("disabled",null);
	this.methods.currentAction = event.data.action;
	this.confirmSelection();

	if( event.data.action == "move"){
		this.moveFiles(event);
		this.uidStartingFolder = globalVar.Uid_Parent;
	}
	if( event.data.action == "delete"){
		$("#modal-delete").modal();
	}
};
Repository.prototype.resetAction = function(){
	this.resetBtn.attr("disabled","disabled");
	
	if(this.methods.currentAction == "move"){

		this.moveBtn.attr("disabled", null).show();
		this.moveConfirm.attr("disabled", "disabled").hide();

		this.correlated.hide();


	}
	this.methods.currentAction = false;
	this.resetSelection();
};
Repository.prototype.confirmActions = function(){

	if(this.methods.currentAction == "move"){
		this.requestRefresh(this.methods.currentAction, "", this.uidStartingFolder);
		this.moveBtn.attr("disabled", null).show();
	}
	if(this.methods.currentAction == "delete"){
		this.requestRefresh(this.methods.currentAction);
	}

};
Repository.prototype.controlElementonPage = function () {
	var self = this;

	this.selectedArray = [];

	this.items.each(function (i, el) {

		if ( $(el).hasClass("is-selected") ) {
			self.selectedArray.push( $(el) );
		}

	});

	if(this.selectedArray.length > 1){
		this.renameBtn.attr("disabled","disabled"); // setto disabled il pulsante rename se più di un elemento
		this.moveBtn.attr("disabled",null);
		this.deleteBtn.attr("disabled",null);
	}else if(this.selectedArray.length == 1){
		this.renameBtn.attr("disabled",null); // setto il pulsante rename attivo se è un elemento
		this.moveBtn.attr("disabled",null);
		this.deleteBtn.attr("disabled",null);
	}else{
		// setto il pulsante rename attivo se è un elemento
		this.moveBtn.attr("disabled","disabled");
		this.deleteBtn.attr("disabled","disabled");
		this.renameBtn.attr("disabled","disabled");
	}

	return this.selectedArray;
};
Repository.prototype.renameFolder = function(event){

	var self = this,

		$folderToRename = $(".rp_item--folder.is-selected, .rp_item--file.is-selected").first(),
		$itemName = $folderToRename.find(".rp_name"),
		_oldName = $itemName.text(),
		uidItem = $folderToRename.data("uid")
	;

	$folderToRename.addClass("is-inedit"); // aggiungo il modificatore per mostrare il testo continuo
	removeAllHandler();
	$itemName.attr("contenteditable", true);
	$itemName.focus();




	$itemName.on("keypress", function(event){
        event.stopPropagation();
		if (event.type == "keypress"){
			if ( event.keyCode === 13 && ( $itemName.text() != " " || $itemName.text() != _oldName ) ){

                $itemName.blur();

			}
            
		}

	});

    $itemName.on("blur", function(event){
        event.stopPropagation();
        if ( $itemName.text() != " " || $itemName.text() != _oldName ){
            // invia
            self.requestRefresh("rename", $itemName.text(), uidItem );
            // FolderAndFiles.popolateDetail( event );
            callDetails( self.items ,event );
            removeAllHandler();
        }else{
            $itemName.text( _oldName );
            removeAllHandler();
        }

    });
    
    function callDetails (){
        console.log(arguments[0], typeof arguments )
        $.each(arguments[0], function (a,b) {
            console.log(a,b)
            if( $(b).hasClass("is-selected") ){
                $(this).trigger("click");
            }
        });
        
    }
	function removeAllHandler(){
		$itemName.attr("contenteditable", null);
		$itemName.off("blur");
        $itemName.off("keypress");
		$folderToRename.removeClass("is-inedit"); // tolgo il modificatore
	}

};
Repository.prototype.createWrap = function (data) {
	var 
		self = this,
		itemlist = ""
	;

	this.interfaceElements.Workspace.html("");

	var isFirst = false;

	for (var i = 0 ; i < data.length; i++) {

		var $item   = $("<div />", { "class": "rp_item" }),
			$inner  = $("<div />", { "class": "rp_item__inner" }),
			$name   = $("<div />", { "class": "rp_name" }),
			$image  = $("<div />", { "class": "rp_preview" }),
			$iconbox= $("<div />", { "class": "rp_icon" }),
			$icon   = $("<i />",   { "class": "fa" }),
			$footer = $("<div />", { "class": "rp_item__caption" })
		;

		if (data[i].details != null) {

			for (var j = 0; j < data[i].details.length; j++) {
				$item.data("item-options", data[i].details);
				$item.data("originalpath", data[i].original);
			}

		};

		if (data[i].typo == "folder") {

			if (i == 0 && data[i].name == "/.." ) {
				$item.removeClass("rp_item").addClass("rp_item--folder rp_item--first")
			} else {
				$item.addClass("rp_item--folder");
			}

			// extension
			$icon.addClass( self.icons.folder );

			$name.html(data[i].name);
			$name.attr("title", data[i].name);
			$iconbox.append($icon);
			$footer.append($iconbox).append($name);

			$footer.appendTo($inner);

		};

		if (data[i].typo == "file") {

			$item.addClass("rp_item--file");

			if( data[i].extension != null){

				switch( data[i].extension ){
					case "png" :
					case "jpg" :
					case "JPG" :
					case "JPEG":
					case "jpeg":
					case "gif" :
					case "bmp" :
					case "tiff":
						$icon.addClass( self.icons.images );
						break;
					case "pdf" :
						$icon.addClass( self.icons.pdf );
						break;
					case "zip" :
						$icon.addClass( self.icons.archive );
						break;
					case "doc" :
						$icon.addClass( self.icons.text );
						break;
					case "xls" :
						$icon.addClass( self.icons.excel );
						break;
					case "mov" :
						$icon.addClass( self.icons.movie );
						break;
				}

			}

			if (data[i].thumb != "" || data[i].thumb != null) {
				$image.css({ "background-image": "url(" + encodeURI( data[i].thumb ) + ")" });
				$inner.append($image)
			}else{
				$inner.append($icon)
			}

			$name.html(data[i].name);
			$iconbox.append($icon);
			$footer.append($iconbox);
			$footer.append($name);
			$inner.append($footer);

			if(!isFirst){
				isFirst = true;
				$hr = $("<hr />",{ class: "separator" });
				$hr.before($item);
			}

		};
		$item.data("object-data", data[i] );
		$item.attr("data-uid", data[i].Uid );
		$item.attr("data-uidrepository", data[i].UidRepository);
		$inner.appendTo($item); // was $col

		self.items = new FolderAndFiles($item);
		self.interfaceElements.Workspace.append($item); // was $col

		RepositoryLoading.close(); // chiudo il loading
	}
};
Repository.prototype.searchForDuplicated = function(name){

	var arrayNames = [];

	function controlText(name){
		$(".rp_item--folder").not(".add").each(function(i, el){ 
			arrayNames.push( $(el).find(".rp_name").text() );
		});
	}

	if( $.inArray( arrayNames, name ) != -1 ){
		var j = 0;
		j++
		name = name + j.toString();
		controlText( name );
	}

	return name;
};
Repository.prototype.createNewFolder = function(event){

	var self = this;

	function retriveName(event){

		var namefolder = "";

		if( event.type == "keypress" ){
			if( event.keyCode === 13 && ( $(event.currentTarget).text() != "my folder" || $(event.currentTarget).text() != " ") ){
				// namefolder = $(event.currentTarget).text();
				// cerco i duplicati
				namefolder = self.searchForDuplicated( $(event.currentTarget).text() );
				self.requestRefresh( self.methods.createfolder, namefolder );

			}

		}else{
			if( $(event.currentTarget).text() != "my folder" ){
				// namefolder = $(event.currentTarget).text();

				namefolder = self.searchForDuplicated( $(event.currentTarget).text() );
				self.requestRefresh(self.methods.createfolder, namefolder);
			}else{

			}

			

		}
	};

	var $item;

	function createFolder () {

			$item   = $( "<div />", {"class":"rp_item folder add"} );
			$folder = $( "<div />", {"class":"rp_item__inner"} ).appendTo( $item ),
			$icon   = $( "<div />", {"class":"rp_icon"} ).appendTo( $folder ),
			$ico    = $( "<i />",   {"class":"fa fa-folder-o"} ).appendTo( $icon );

		var $itemN  = $("<div />", {"class":"rp_name", "text": "my folder", 

			blur: function(event){
				retriveName(event);
				$item.remove();
			},
			keypress: function(event){
				retriveName(event);
			}

		}).attr("contenteditable", true).appendTo($folder);

		return $item;

	}

	$(".rp_item").first().before( createFolder() );
	$(".rp_item").first().find(".rp_name").focus();

};
Repository.prototype.searchBoxAction = function(event){

	var 
		self = this,
		stringChart = this.searchBar.val()
	;



	if( stringChart.length > 2 && stringChart.length < 30 ){
		if (event.type == "click"){
			this.requestRefresh("searchFull",stringChart);
			this.searchBar.val("");
		}
		if (event.type == "keypress") {
		    event.stopPropagation();		   
		    if (event.keyCode == 13) {

		        self.searchBtn.focus();
		        self.searchBtn.trigger("click");
		        self.searchBtn.blur();

		        RepositoryLoading.open();
		        return false;
			}
		} 

	}
};
Repository.prototype.changeTypeSelection = function (event) {
	if ( this.multiselection.prop("checked") ) {
		this.ismultiselection = true;
	} else {
		this.ismultiselection = false;
	}
};
Repository.prototype.moveFiles = function (event) {
	this.correlated.show();
	// nascondi bottone
	this.moveBtn.attr("disabled", "disabled").hide();
	// visualizza cofnerma bottone
	this.moveConfirm.attr("disabled", null).show();
};
var Breadcumbs = function(){
	this.ajaxCall();
	this.observe();
	this.breadcumbsWrap =  $(".rp_breadcumbs");
	this.breadcumbsContainer = $("<ul />", {"class":"breadcrumb" });
};
Breadcumbs.prototype.observe = function() {
	$("document , body").on("click", ".rp_breadcumbs ul li", $.proxy( this.gotoFolder, this ) );
	//$("body").on('mouseenter', '.rp_breadcumbs ol li', $.proxy( this.gotoFolder, this ));
};
Breadcumbs.prototype.popolateSubThree = function(event){

	var target = $(event.currentTarget),
		temporaryUid = target.data("uid"),
		temporaryRepository = target.data("uidrepository")
	;

	$.ajax({
		url: globalVar.wsUrl + self.methods.listFolder,
		type: 'POST',
		dataType: 'json',
		data: 'uid_CmsNlsContext=' + globalVar.CmsNlsContext + '&uid_CmsUsers=' + globalVar.CmsUsers + '&Uid_Repository=' + globalVar.Uid_Repository + '&Uid_Parent=' + globalVar.Uid_Parent,
		success: function (data) {
			self.createWrap(data);
			self.items = $(".rp_item");  
		},
		error: function (error) {
			// genericmodal
		}
	});
};
Breadcumbs.prototype.gotoFolder = function (event) {

	if ( !$(event.currentTarget).hasClass("active") ) {

		RepositoryLoading.open();

		globalVar.Uid_Repository = $(event.currentTarget).data("uidrepository") === undefined ? "" : $(event.currentTarget).data("uidrepository");
		globalVar.Uid_Parent = $(event.currentTarget).data("uid") === undefined ? "" : $(event.currentTarget).data("uid");

		repository.breadcumbs.ajaxCall(event);
		repository.requestRefresh('navigateToFolder');
	}
};
Breadcumbs.prototype.createObjArray = function(type, el){
	this.breadcumbsArray = [];
	if( type == "obj" ){
		for (var i = 0; i < el.length; i++){

			var breadcumbObj = {};

				breadcumbObj.name = el[i].name;
				breadcumbObj.uid = el[i].Uid;
				breadcumbObj.repository = el[i].UidRepository;

			this.breadcumbsArray.push(breadcumbObj);
		}
	}else{

		breadcumbObj.name = el.find(".rp_name").text();
		breadcumbObj.uid = el.data("uid");

		this.breadcumbsArray.push(breadcumbObj);
	}

	this.createHtml();
};
Breadcumbs.prototype.addObjToArray = function(){
	this.breadcumbsArray.push();
};
Breadcumbs.prototype.removeObjToArray = function(index){

};
Breadcumbs.prototype.createHtml = function(){
	this.breadcumbsWrap.html("");
	this.breadcumbsContainer.html("");

	for (var j = 0; j < this.breadcumbsArray.length; j++ ){
		var
			breadcumbsElements = $("<li />"),
			breadcumbsRootIcon = $("<i />", {"class":"menu-icon glyphicon glyphicon-folder-open" })
		;

		if (j === 0) {
			breadcumbsElements.addClass( "active" );
		} 
		if (j === (this.breadcumbsArray.length - 1)){
			breadcumbsElements
				.attr("data-uidrepository", this.breadcumbsArray[j].repository)
				.attr("data-uid", this.breadcumbsArray[j].uid)
				.append(breadcumbsRootIcon)
				.append( "<a href='#'>" + this.breadcumbsArray[j].name + "</a>");

		}else{
			breadcumbsElements
				.attr("data-uidrepository", this.breadcumbsArray[j].repository)
				.attr("data-uid", this.breadcumbsArray[j].uid)
				.append( "<a href='#'>" + this.breadcumbsArray[j].name + "</a>");
		}

		this.breadcumbsContainer.prepend(breadcumbsElements)
	}

	this.breadcumbsContainer.appendTo(this.breadcumbsWrap);    

};
Breadcumbs.prototype.ajaxCall = function() {
	var self = this;
	$.ajax({

		url: globalVar.wsUrl + "Breadcumbs",
		type: 'POST',
		dataType: 'json',
		data: 'uid_CmsNlsContext=' + globalVar.CmsNlsContext + '&uid_CmsUsers=' + globalVar.CmsUsers + '&Uid_Repository=' + globalVar.Uid_Repository + '&Uid_Parent=' + globalVar.Uid_Parent,

		success: function (data) {
			self.createObjArray("obj", data);
		},
		error: function (error) {
				// genericmodal

		}
	});
};
var UploadFiles = function (destination) {
    this.wrapper = $(destination);
    this.wrapList = this.wrapper.find(".wrapper-uploadfiles--content");
    
    this.btn = $('[data-action="js-uploadfile"]');
    this.btnDel = $('[data-action="js-delete-all"]');
    this.submit = $('[data-action="js-send-files"]');
    this.input = $('.file-upload');
    this.content = $("<div />");
    this.isSubmit = false;

    this.filesArray = [];
    this.contentArray = [];
    this.arrayfinal = [];

    var old_file = "";
    var self = this;
    var i = 0;

    this.input.fileupload({
        "dataType": 'json',
        "singleFileUploads": true,
        "autoUpload": false,
        "dropZone": self.input,
        "sequentialUploads": true,
        "url": globalVar.wsHandler,
        add: function (e, data) {
            i++;

            data.url = globalVar.wsHandler + '?UidCmsRepository=' + globalVar.Uid_Repository + '&UidCmsRepositoryFolder=' + globalVar.Uid_Parent;

            var originalname = data.files[0].name;
            var originalsize = data.files[0].size;
            var j = 0;

            if (self.contentArray.length == 0) {
                self.contentArray.push(data); // aggiungo l'oggetto completo restituitomi dall'upload
            } else {      
                for (var q = 0; q < self.contentArray.length; q++) {
                    var name = self.contentArray[q].files[0].name;

                    if (originalname != self.contentArray[q].files[0].name && originalsize != self.contentArray[q].files[0].size) {
                        j++;
                    }
                }
                if (j == self.contentArray.length) {
                    self.contentArray.push(data); // aggiungo il dato all'earray degli elementi
                }

            }
            // quando finisco l'upload
            if (i === data.originalFiles.length) {
                console.log("here");
                i = 0;
                events.emit("finishSelection", self.contentArray);        
            }

        },
        progress: function (e, data) {
            var progress = parseInt(data._progress.loaded / data._progress.total * 100, 10);

            $('.wrapper-uploadfiles--content').find(".list-item").eq(i).find('.progress-bar').css('width', progress + '%');

            if (progress == 100) {
                i++;
            }

            if (i == self.contentArray.length) {
                events.emit("finishUpload")
                i = 0;
            }
        },
        done: function(e,data){
            repository.requestRefresh("navigateToFolder"); // console.log(e,data);
        }
    });

    events.on("finishSelection", function (data) {
        self.printList(data);
    });

    events.on("finishUpload", function () {

        setTimeout(function () {
            self.wrapper.slideUp(250, function () {
                self.deleteAllFiles(); // dopo aver nascosto svuoto
            });
        }, 1000); // end Upload
        
    });

    this.observe();

};
UploadFiles.prototype.printList = function (arrayList) {
    var size, listato, self = this;

    this.wrapList.html("");

    for (var q = 0; q < arrayList.length; q++) {

        var li = $("<li />", { "class": "list-item" });

        var textContainer = $("<div />", { "class": "list-text" }).appendTo(li);
        var text = $("<p />", { "class": "small" }).appendTo(textContainer);

        var progressContainer = $("<div />", { "class": "list-progress" }).appendTo(li);
        var progress = $("<div />", { "class": "progress progress-striped active" }).appendTo(progressContainer)
        var progressBar = $("<div />", { "class": "progress-bar progress-bar-success" }).appendTo(progress);
            progressBar.attr("aria-valuenow", "").attr("aria-valuemin", "").attr("aria-valuemax", "");
        var progressInc = $("<span />", { "class": "sr-only" }).appendTo(progressBar);

        var cancel = $("<div />", { "class": "list-btn" }).appendTo(li);
        var cancelBtn = $("<button />", { "class": "btn btn-danger", "type": "button", click: function (event) { cancelFile(event); }, "text": "x" }).attr("data-inc", q).attr("data-action", "js-remove-file").appendTo(cancel);

            listato = arrayList[q].files[0];

            size = parseFloat(listato.size / 1048576).toFixed(2);

            text.text(listato.name + " " + size);
            cancelBtn.data("element_" + [q], arrayList[q])

            this.wrapList.append(li).css({ "display": "none" }).fadeIn(250);

        if (q + 1 == arrayList.length) {

            self.wrapper.slideDown(250);
            inputReset(self.input);
            self.input.change();

        }
    };

    function cancelFile(event) {

        var element = $(event.currentTarget).parents(".list-item");
        var index = $(event.currentTarget).data("inc");

        // var removed = self.arrayfinal.splice( index , 1);
        var removed = self.contentArray.splice(index, 1);
        element.remove();
        if ($(".list-item").length == 0) {
            self.contentArray = [];
            self.wrapper.slideUp(250);
            
        }

    };
};
UploadFiles.prototype.observe = function () {

    var self = this;
    // this.btn.on("click", function(){ self.input.trigger("click") });
    this.btnDel.on("click", $.proxy(this.deleteAllFiles, this));
    this.submit.on("click", $.proxy(this.submitAllFiles, this));
    //this.input.on("click",function(){ inputReset(this) });

    // this.input.on("change", function(event){
    //  self.init.call(null,self, event);
    // });

    //this.fileupload.on("fileuploadchange", function(event,el){ self.observeFileInput(event,el) });
    //this.fileupload.on("fileuploadprogress", function(e, data){ console.log(data) });
    // this.fileupload.on("fileuploadprogressall", function(e,data){ console.log(data) });
};
UploadFiles.prototype.deleteAllFiles = function () {
    $(".list-item").remove();
    this.contentArray = [];

    this.wrapper.slideUp(250);
};
UploadFiles.prototype.submitAllFiles = function () {
    var self = this;
    var j = 0;

    if (self.contentArray.length != 0) {

        for (j; j < self.contentArray.length; j++) {
            self.contentArray[j].submit();
        }

    } else {
        console.warn("array vuoto");
    }
}

repository = new Repository();
uploadfiles = new UploadFiles(".wrapper-uploadfiles");


function inputReset(el) {
    $(el).prop("value", "").change();
}
;
var PopUpBlock = function(button){
    this.handler = $(button);
    this.block = this.handler.parents(".form-group");
    this.blockInfo  = this.block.find(".tf-info_content");
    this.blockMaster  = this.block.find(".tf-master_content");

    this.events();  
    
};
PopUpBlock.prototype.events = function(){
    this.handler.on("click", $.proxy(this.toggleSingle , this) )
};
PopUpBlock.prototype.toggleAll = function(){

};
PopUpBlock.prototype.toggleSingle = function(){
    if( this.handler.hasClass("tf_master") ){
        if (this.blockMaster.hasClass("hide")){
            this.blockMaster.removeClass("hide");
            this.blockInfo.addClass("hide");
            this.block.find(".tf_help").removeClass("is-active")
        }else{
            this.blockMaster.addClass("hide");
        }
    }else{
        if (this.blockInfo.hasClass("hide")){
            this.blockInfo.removeClass("hide");
            this.blockMaster.addClass("hide");
            this.block.find(".tf_master").removeClass("is-active");

        }else{
            this.blockInfo.addClass("hide");
        }

    }
    if( this.handler.hasClass("is-active") ){
        this.handler.removeClass("is-active");
    }else{
        this.handler.addClass("is-active");
    }
};
var SwitchRadio = function(){
    this.radios = $(".panel-right").find('[type="radio"]');
    this.events();
};
SwitchRadio.prototype.events = function(){
    this.radios.on("change", $.proxy(this.controlActive , this) )
    this.radios.eq(0).prop("checked", true).change();
};
SwitchRadio.prototype.controlActive = function(){
    this.radios.each(function(i,el){
        if($(el).prop("checked")){
            if($(el).val() == "1"){
                $(".rp_wrap").addClass("view-list");
            }else{
                $(".rp_wrap").removeClass("view-list");
            }
        }
    });
}
;
var UploadFromFolder = function(btn){
	this.button = $(btn);
	this.configuration = {};
	this.createObject();
};
UploadFromFolder.prototype.createObject = function() {
	
	this.configuration.id = this.button.attr("id");
	this.configuration.folderUid = this.button.attr("data-folder");
	this.configuration.folderUrl = this.button.attr("data-folderurl");

	this.button.data( this.configuration );

};




















NotificationError.init();

if(window.sessionStorage){

	if( window.sessionStorage.getItem("detailsshow") == "false" ){

		$(".rp_content").attr("data-detail", null);

	}else{

		$(".rp_content").attr("data-detail", "open");

	}

	$(".rp_folder__details").hide();
	$(".rp_file__details").hide();

}

if ($(".table-th").length) {

	$(window).on("scroll", function (e) {
		var tableH = $(".table-th").offset().top

		if (window.scrollY == tableH || window.scrollY >= tableH) {
			$(".table-th").addClass("blocked")
			.css("top", window.scrollY);
		}else if ( window.scrollY == $(".table-th").parent().offset().top + $(".table-th").parent().innerHeight() ){
			$(".table-th")
				.removeClass("blocked")
				.css("top", 'auto');
		}

	});

};

if($(".tf_master").length){
	$(".tf_master").each(function(i,help){
		new PopUpBlock(this);
	});
};

if($(".tf_help").length ){
	$(".tf_help").each(function(i,help){
		new PopUpBlock(this);
	});
};

if($(".panel-right").length){
	new SwitchRadio();
};

if($('[data-action="js-uploadfolder"]').length){
	$('[data-action="js-uploadfolder"]').each(function(i,btn){
		new UploadFromFolder(btn);
	});
}

;
