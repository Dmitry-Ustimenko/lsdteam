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
	$.fn.serializeParams = function (form, additionalParams) {
		var params = $(form).serializeArray();

		if (additionalParams != undefined && typeof (additionalParams) == 'object') {
			for (var i = 0; i < additionalParams.length; i++) {
				var param = additionalParams[i];
				if (typeof (param == 'object') && param.name != undefined && param.value != undefined) {
					params.push({
						name: param.name,
						value: param.value
					});
				}
			}
		}

		return params;
	};
})(jQuery);

(function ($) {
	$.fn.sortDropdown = function (sortName, callback) {
		var sortChangeable = $(".sort-changeable");
		var sortDropdown = $(".sort-dropdown");

		sortName.off("click").on("click", function () {
			if (sortDropdown.is(":hidden"))
				sortDropdown.fadeIn("fast");
			else
				sortDropdown.fadeOut("fast");
		});

		$(document).click(function (event) {
			var $eventTarget = $(event.target);
			if (!$eventTarget.closest(".sort-changeable").length)
				sortDropdown.fadeOut("fast");
			event.stopPropagation ? event.stopPropagation() : (event.cancelBubble = true);
		});

		sortDropdown.find("li").each(function () {
			var $this = $(this);
			$this.off("click").on("click", function () {
				sortName.text($this.text());
				sortName.data("val", $this.data("val"));
				sortChangeable.width($this.width());
				sortDropdown.fadeOut("fast");

				if (typeof (callback) == "function") {
					callback(sortName);
				}
			});
		});
	};
})(jQuery);

(function ($) {
	$.fn.lenghtLimit = function (txtBox, lengthBox, lenght) {
		if (txtBox != undefined && lengthBox != undefined) {
			txtBox.on('input keyup paste cut', function () {
				if (typeof (lenght) == 'number') {
					var leftCharacters = lenght - txtBox.val().length;
					if (leftCharacters < 0) {
						lengthBox.addClass('darkred');
					} else {
						lengthBox.removeClass('darkred');
					}

					lengthBox.html(leftCharacters);
				}
			});
		}
	};
})(jQuery);


(function ($) {
	$.fn.clock = function (containerClockId) {
		startTime();

		function startTime() {
			var today = new Date();
			var h = today.getHours();
			var m = today.getMinutes();
			var s = today.getSeconds();
			m = checkTime(m);
			s = checkTime(s);
			$(containerClockId).html(h + ":" + m + ":" + s);
			setTimeout(function () { startTime(); }, 500);
		}

		function checkTime(i) {
			// add zero in front of numbers < 10
			if (i < 10) {
				i = "0" + i;
			};
			return i;
		}
	};
})(jQuery);

(function ($) {
	$.fn.pager = function (callback) {
		var container = $(this);
		container.find('span[data-page]').on('click', function () {
			var page = $(this);
			if (page.hasClass('currentPage') || page.hasClass('disabledPrevNext'))
				return;
			if (typeof (callback) == 'function')
				callback(parseInt(page.data('page')));
		});
	};
})(jQuery);

(function ($) {
	$.fn.bbcodeParser = function (container, content) {
		var htmlContent = XBBCODE.process({
			text: content,
			addInLineBreaks: true
		}).html;

		htmlContent = htmlContent.replace(/&#91;hr&#93;/g, "<hr/>").replace(/\[hr\]/g, "<hr/>");

		if (container != undefined) {
			container.html(htmlContent);
			$.fn.slideSpoiler(container);
		}
	};
})(jQuery);

(function ($) {
	$.fn.bbcodeCustomParser = function (container, content) {
		var htmlContent = XBBCODECustom.process({
			text: content,
			addInLineBreaks: true
		}).html;

		htmlContent = htmlContent.replace(/&#91;hr&#93;/g, "<hr/>").replace(/\[hr\]/g, "<hr/>");

		if (container != undefined) {
			container.html(htmlContent);
			$.fn.slideSpoiler(container);
		}
	};
})(jQuery);

(function ($) {
	$.fn.checkCommentHash = function () {
		if (window.location.hash != undefined && window.location.hash.trim() != '') {
			if (window.location.hash == '#comments-header') {
				$.fn.animateScrollTop(window.location.hash + "-hash", 0);
			} else {
				var rgx = new RegExp('^#comment-+[0-9]+$', "i");
				if (rgx.test(window.location.hash)) {
					$.fn.animateScrollTop(window.location.hash + "-hash", 0);
				}
			}
		}
	};
})(jQuery);

(function ($) {
	$.fn.animateScrollTop = function (element, speed) {
		var spd = 'normal';
		if (typeof (speed) == 'number' || speed == 'slow' || speed == 'normal' || speed == 'fast') {
			spd = speed;
		}

		$('html, body').animate({
			scrollTop: $(element).offset().top - $('table.menu').height()
		}, spd);
	};
})(jQuery);

(function ($) {
	$.fn.slideSpoiler = function (content) {
		var $spoilers;
		if (content != undefined) {
			$spoilers = content.find(".xbbcode-spoiler");
		} else {
			$spoilers = $('.xbbcode-spoiler');
		}

		$spoilers.each(function () {
			var spoiler = $(this);

			if (spoiler != undefined) {
				var header = spoiler.find('> .xbbcode-spoiler-head');
				var slide = spoiler.find('> .xbbcode-spoiler-slide');
				var footer = slide.find('> .xbbcode-spoiler-footer');

				var headerIcon = header.find('[data-id=header-icon]');

				header.off('click').on('click', function () {
					slideFun();
				});

				footer.off('click').on('click', function () {
					slideFun();
				});

				function slideFun() {
					if (slide.is(':visible')) {
						slide.slideUp();
					} else {
						slide.slideDown();
					}
					headerIcon.toggleClass('xbbcode-open-icon xbbcode-close-icon');
				}
			}
		});
	};
})(jQuery);

(function ($) {
	$.fn.cookieSet = function (name, value, options) {
		options = options || {};
		var exp = options.expires;

		if (typeof exp == 'number' && exp) {
			var date = new Date();
			date.setDate(date.getDate() + exp);
			exp = options.expires = date;
		}

		if (exp && exp.toUTCString)
			options.expires = exp.toUTCString();

		value = encodeURIComponent(value);

		var updatedCookie = name + '=' + value;
		for (var propName in options) {
			updatedCookie += '; ' + propName;
			var propValue = options[propName];
			if (propValue !== true)
				updatedCookie += '=' + propValue;
		}

		document.cookie = updatedCookie;
	};
})(jQuery);

(function ($) {
	$.fn.cookieGet = function (name) {
		var matches = document.cookie.match(new RegExp(
			'(?:^|; )' + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + '=([^;]*)'
		));
		return matches ? decodeURIComponent(matches[1]) : undefined;
	};
})(jQuery);

(function ($) {
	$.fn.cookieDelete = function (name) {
		$.fn.cookieSet(name, '', { expires: -1 });
	};
})(jQuery);

(function ($) {
	$.fn.initCheckbox = function () {
		$(site.layout.settings.elements.checkbox).each(function () {
			var $this = $(this);
			if ($this.is(":checked"))
				$this.parent().addClass('checked');
			$this.off("click").on("click", function () {
				if ($(this).is(":checked"))
					$(this).parent().addClass('checked');
				else
					$(this).parent().removeClass('checked');
			});
		});
	};
})(jQuery);

(function ($) {
	$.fn.applyDatepicker = function () {
		var container = $(this);

		container.find('input[data-format=date]').each(function () {
			var dateInput = $(this);
			dateInput.keydown(function (e) {
				return ((e.keyCode > 47) && (e.keyCode < 58) && (e.keyCode < 48) && (e.keyCode > 57));
			}).datepicker({
				format: "dd.mm.yyyy",
				language: "ru",
				autoclose: true,
				weekStart: 1
			});
		});
	};
})(jQuery);

(function ($) {
	$.fn.slider = function (sliderId) {
		Galleria.loadTheme('/Scripts/Plugins/galleria.classic.js');
		Galleria.run(sliderId, {
			autoplay: 2500,
			transition: 'fade',
			transitionSpeed: 750,
			imageCrop: true,
			thumbCrop: 'height',
			idleMode: false,
			showInfo: false,
			showCounter: false,
			showThumbNav: true,
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
	$.fn.alertOverlay = function (title, message) {
		var alert = $('#alertOverlay');
		alert.find("[data-type=modal-title]").html(title);
		alert.find("[data-type=modal-message]").html(message);

		alert.showOverlay();

		alert.find("#btnOk").off("click").on("click", function () { alert.closeOverlay(); });
		alert.find(".close").off("click").on("click", function () { alert.closeOverlay(); });
	};
})(jQuery);

(function ($) {
	$.fn.confirmOverlay = function (title, message, callback, callbackClose) {
		var confirm = $('#confirmOverlay');
		confirm.find("[data-type=modal-title]").html(title);
		confirm.find("[data-type=modal-message]").html(message);

		confirm.showOverlay();

		confirm.find("#btnOk").off("click").on("click", function () {
			if (typeof (callback) == 'function')
				callback();
			confirm.closeOverlay();
		});

		confirm.find("#btnCancel").off("click").on("click", function () {
			if (typeof (callbackClose) == 'function')
				callbackClose();
			confirm.closeOverlay();
		});

		confirm.find(".close").off("click").on("click", function () {
			if (typeof (callbackClose) == 'function')
				callbackClose();
			confirm.closeOverlay();
		});
	};
})(jQuery);

(function ($) {
	$.fn.promptOverlay = function (title, message, callback, callbackClose) {
		var confirm = $('#promptOverlay');
		confirm.find("[data-type=modal-title]").html(title);
		confirm.find("[data-type=modal-message]").html(message);

		confirm.showOverlay();

		confirm.find("#btnOk").off("click").on("click", function () {
			if (typeof (callback) == 'function')
				callback(confirm.find("#txtValue").val());
			confirm.closeOverlay();
		});

		confirm.find("#btnCancel").off("click").on("click", function () {
			if (typeof (callbackClose) == 'function')
				callbackClose();
			confirm.closeOverlay();
		});

		confirm.find(".close").off("click").on("click", function () {
			if (typeof (callbackClose) == 'function')
				callbackClose();
			confirm.closeOverlay();
		});
	};
})(jQuery);

(function ($) {
	$.fn.showOverlay = function () {
		var overlay = $(this);
		overlayEvents(overlay);
		overlay.modal().show();
	};

	$.fn.closeOverlay = function () {
		$(this).modal('hide');
	};

	function overlayEvents(overlay) {
		overlay.off('show.bs.modal').on('show.bs.modal', function () {
			overlay.removeClass('fadeOutDown').addClass('fadeInDown');
		}).on('hide.bs.modal', function (e) {
			e.preventDefault();
			overlay.removeClass('fadeInDown').addClass('fadeOutDown');
			setTimeout("$('#" + overlay.prop('id') + "').modal('hide')", 480);
			overlay.unbind('hide.bs.modal');
		});
	};
})(jQuery);

(function ($) {
	$.fn.loadOverlay = function (openDialogUrl, openDialogParams, saveFormUrl, callBack, onLoadCallBack, onCloseOverlayCallBack) {
		var sender = this;
		$(sender).loadData(openDialogUrl, openDialogParams, function () {
			$(sender).showOverlay();
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
						site.ajax.post(saveUrl, form.serializeArray(), function (data) {
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
	$.fn.loadData = function (url, params, callback, callbackError) {
		var target = this;
		site.ajax.post(url, params, function (data) {
			$(target).html(data);

			var form = target.find("form");
			if (form != undefined) {
				$(form).removeData("validator");
				$(form).removeData("unobtrusiveValidation");
				$.validator.unobtrusive.parse($(form));
			}

			if ($(data).find("div.validation-summary-errors").length) {
				if (typeof (callbackError) == 'function')
					callbackError();
				$.fn.initValidationSummary(target);
				return;
			}

			if (typeof (callback) == 'function')
				callback(data);
		}, function (data) {
			if (typeof (callbackError) == 'function')
				callbackError(data);
		});
	};
})(jQuery);

(function ($) {
	$.fn.validateUploadFile = function (file, options) {
		if (options == undefined)
			return undefined;

		if (options.size != undefined && file.size > options.size)
			return "Размер файла не выше " + options.size / 1024 + "кб";

		return undefined;
	};
})(jQuery);

(function ($) {
	$.fn.progressBar = function (uploadBtn, url) {
		$(uploadBtn).click(function () {
			var $progressbar = $(site.profile.settings.elements.progressbar);
			var $validationSummary = $(".validation-summary-errors");

			var files = $(site.profile.settings.elements.photoUploadFile).get(0).files;
			if (files.length) {
				var $progresslabel = $(site.profile.settings.elements.progresslabel);
				$validationSummary.hide();

				$progressbar.fadeIn("fast");
				$progressbar.progressbar({
					max: 100,
					change: function () {
						$progresslabel.text($progressbar.progressbar("value") + "%");
					},
					complete: function () {
						$progressbar.find(".ui-progressbar-value").width(196);
						$progresslabel.text("Файл загружен");
					},
				});

				var data = new FormData();
				data.append("UploadFile", files[0]);

				var xhr = new XMLHttpRequest();
				xhr.upload.addEventListener("progress", function (e) {
					if (e.lengthComputable) {
						var progress = Math.round(e.loaded * 100 / e.total);
						$progressbar.progressbar("value", progress);
					}
				}, false);

				xhr.open("POST", url, true);
				xhr.onreadystatechange = function () {
					if (xhr.readyState == 4) {
						if (xhr.status == 200) {
							// success
						}
					}
				};
				xhr.send(data);

				return false;
			} else {
				$progressbar.hide();
				$validationSummary.show();
			}
		});
	};
})(jQuery);