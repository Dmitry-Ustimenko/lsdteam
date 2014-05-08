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
	$.fn.applyDatepicker = function () {
		var container = $(this);

		container.find('input[data-format=date]').each(function () {
			var dateInput = $(this);
			dateInput.keydown(function (e) {
				return ((e.keyCode > 47) && (e.keyCode < 58) && (e.keyCode < 48) && (e.keyCode > 57));
			});

			if (dateInput.val() == "")
				dateInput.datepicker({
					format: "dd.mm.yyyy",
					language: "ru"
				});
			else
				dateInput.datepicker('setValue', dateInput.val());
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
			},
			youtube: {
				color: 'red'
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

(function ($) {
	$.fn.alertMessage = function (title, message) {
		var alert = $('#alertOverlay');
		alert.find("[data-type=modal-title]").html(title);
		alert.find("[data-type=modal-message]").html(message);

		alert.on('show.bs.modal', function () {
			$(this).removeClass('fadeOutDown').addClass('animated fadeInDown');
		});

		alert.on('hide.bs.modal', function () {
			$(this).removeClass('fadeInDown').addClass('animated fadeOutDown');
		});

		alert.modal('show');

		alert.find("#btnOk").on("click", function () { alert.closeOverlay(); });
		alert.find(".close").on("click", function () { alert.closeOverlay(); });
	};
})(jQuery);

(function ($) {
	$.fn.loadOverlay = function (openDialogUrl, openDialogParams, saveFormUrl, callBack, onLoadCallBack, onCloseOverlayCallBack) {
		var sender = this;
		$(sender).loadData(openDialogUrl, openDialogParams, function () {
			$(sender).modal().show();
			initDialog(sender, saveFormUrl, callBack, onLoadCallBack, onCloseOverlayCallBack);
		});

		function initDialog(dialog, saveUrl, callBackFunction, onLoadCallBackEvent, onCloseOverlayCallBackEvent) {
			var $overlay = dialog;
			var $dialog = $overlay;
			$.validator.unobtrusive.parse($dialog.find("form"));

			if (typeof (onLoadCallBackEvent) == 'function')
				onLoadCallBackEvent($overlay);

			$dialog.find("#btnSave").off("click").on("click", function () {
				var form = $dialog.find("form");
				if ($(form).length == 0) {
					if (typeof (onCloseOverlayCallBackEvent) == 'function')
						onCloseOverlayCallBackEvent();
					$overlay.closeOverlay();
					if (typeof (callBackFunction) == 'function')
						callBackFunction();
				} else {
					$.fn.initValidationSummary($dialog);
					if (form.valid())
						tacc.ajax.post(saveUrl, form.serializeArray(), function (data) {
							if (typeof (onCloseOverlayCallBackEvent) == 'function')
								onCloseOverlayCallBackEvent();
							$overlay.closeOverlay();
							if (typeof (callBackFunction) == 'function')
								callBackFunction(data);
						}, $dialog);
				}
			});

			$dialog.find("#btnCancel").on("click", function () { $overlay.closeOverlay(); });
			$dialog.find(".close").on("click", function () { $overlay.closeOverlay(); });
		}
	};
})(jQuery);

(function ($) {
	$.fn.closeOverlay = function () { $(this).modal('hide').html(""); };
})(jQuery);

(function ($) {
	$.fn.loadData = function (url, dataParam, callbackParam, callbackError) {
		var target = this;
		site.ajax.post(url, dataParam, function (data) {
			try {
				var json = $.parseJSON(data);
				if (json != undefined) {
					for (var key in json) {
						if (key == "ReturnUrl") {
							if (json[key] != null) {
								window.location.href = json[key];
								return;
							}
						}
					}

					var returnUrl = $.fn.GetQueryParamValue("ReturnUrl");
					if (returnUrl != undefined)
						window.location.href = returnUrl.split("%2F").join("/");
					else
						window.location.href = "/";
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