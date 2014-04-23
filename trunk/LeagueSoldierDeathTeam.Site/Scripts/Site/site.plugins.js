(function ($) {
	$.fn.GetQueryParamValue = function (param) {
		var queryString = window.location.search.substring(1);
		var variables = queryString.split('&');
		for (var i = 0; i < variables.length; i++) {
			var variable = variables[i].split('=');
			if (variable[0] == param)
				return variable[1];
		}
		return undefined;
	};
})(jQuery);

(function ($) {
	$.fn.serializeParams = function (form) {
		return $(form).serializeArray();
	};
})(jQuery);

(function ($) {
	$.fn.loadData = function (url, dataParam, callbackParam, callbackError) {
		var target = this;
		site.ajax.post(url, dataParam, function (data) {
			try {
				var json = $.parseJSON(data);
				if (json != undefined) {
					var returnUrl = $.fn.GetQueryParamValue("ReturnUrl");
					if (returnUrl != undefined)
						window.location.href = returnUrl.split("%2F").join("/");
					else
						window.location.href = window.location.pathname;
				}
				else
					window.location.href = "/";
			}
			catch (e) {
				$(target).html(data);
				var form = target.find("form");
				if (form != undefined) {
					$(form).removeData("validator");
					$(form).removeData("unobtrusiveValidation");
					$.validator.unobtrusive.parse($(form));
				}
				if ($(data).find("div.validation-summary-errors").length > 0) {
					if (typeof (callbackError) == 'function')
						callbackError();
					return;
				}

				if (typeof (callbackParam) == 'function')
					callbackParam(data);
			}
		});
	};
})(jQuery);

(function ($) {
	$.fn.slider = function (sliderId) {
		Galleria.loadTheme('/Scripts/Plugins/galleria.classic.js');
		Galleria.run(sliderId, {
			autoplay: 2500,
			transition: 'fade',
			transitionSpeed: 1000,
			imageCrop: true,
			thumbCrop: 'height',
			idleMode: false,
			showInfo: false,
			showCounter: false,
			pauseOnInteraction: false,
			imagePan: true,
			extend: function () {
				var gallery = this;
				$(sliderId).mouseenter(this.proxy(function () {
					gallery.pause();
				})).mouseleave(this.proxy(function () {
					gallery.play(2500);
				}));
			}
		});
	};
})(jQuery);

(function ($) {
	$.fn.initValidationSummary = function (container) {
		var form = container.find("form");
		$.validator.unobtrusive.parse(form);
		form.unbind("invalid-form.validate", form.validate().settings.invalidHandler);
		form.validate().settings.invalidHandler = $.proxy(function (event, validator) {
			var vs = form.find("div[data-valmsg-summary=true]");
			var ul = vs.find("ul");
			vs.addClass("validation-summary-errors");
			vs.removeClass("validation-summary-valid");
			if (ul.length > 0) {
				ul.html("");
				for (var name in validator.errorList)
					ul.append("<li>" + validator.errorList[name].message + "</li>");
			}
			else {
				vs.html("");
				vs.append("<ul></ul>");
				ul = vs.find("ul");
				for (var error in validator.errorList)
					ul.append("<li>" + validator.errorList[error].message + "</li>");
			}
		}, form);
		form.bind("invalid-form.validate", form.validate().settings.invalidHandler);
	};
})(jQuery);