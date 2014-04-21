(function ($) {
	$.fn.serializeParams = function (form) {
		return $(form).serializeArray();
	};
})(jQuery);

(function ($) {
	$.fn.loadData = function (url, dataParam, callbackParam) {
		var target = this;
		site.ajax.post(url, dataParam, function (data) {
			$(target).html(data);
			var form = target.find("form");
			if (form != undefined) {
				$(form).removeData("validator");
				$(form).removeData("unobtrusiveValidation");
				$.validator.unobtrusive.parse($(form));
			}
			if ($(data).find("div.validation-summary-errors").length > 0)
				return;
			if (typeof (callbackParam) == 'function')
				callbackParam(data);
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