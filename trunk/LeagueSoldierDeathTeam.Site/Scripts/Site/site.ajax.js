$.blockUI.defaults.message = "Please wait ...";
$.blockUI.defaults.css.border = 'none';
$.blockUI.defaults.css.padding = '15px';
$.blockUI.defaults.css.backgroundColor = '#000';
$.blockUI.defaults.css.opacity = .5;
$.blockUI.defaults.css.color = '#fff';
$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

(function () {
	site.ajax =
	{
		post: function (url, dataParam, callbackParam, sender) {
			site.ajax.send(url, dataParam, callbackParam, sender);
		},

		get: function (url, dataParam, callbackParam, sender) {
			site.ajax.send(url, dataParam, callbackParam, sender);
		},

		send: function (url, dataParam, callbackParam, sender) {
			var defaultDataType = 'html';
			var defaultMethod = jQuery.post;
			var paramObj = typeof (dataParam) == 'object' ? dataParam : {};
			var callBackFunc = typeof (callbackParam) == 'function' ? callbackParam : (typeof (dataParam) == 'function' ? dataParam : null);
			var dataType = typeof (callbackParam) == 'string' ? callbackParam : defaultDataType;
			window.status = "Please wait...";
			document.body.style.cursor = "wait";
			defaultMethod(url, paramObj, function (data, textStatus, jqxhr) {
				if (jqxhr.status == 204) {
					window.location.reload();
					return;
				}

				window.status = "Done";
				document.body.style.cursor = "default";
				if (data != 'Unauthorized') {
					var status = "";
					if (dataType == 'json')
						data = jQuery.parseJSON(data);

					var message = "";
					var title = "Error";
					try {
						if (data.indexOf('{"Status') == 0) {
							var res = jQuery.parseJSON(data);
							if (res.Status != "undefined")
								status = res.Status;
							if (res.Message != "undefined")
								message = res.Message;
							if (res.Title != "undefined")
								title = res.Title;
						}
					} catch (e) {
					}

					if (status == "Failed") {
						if (sender) {
							var vs = $(sender).find("div[class*=validation-summary]");
							if (vs.length > 0) {
								vs.addClass("validation-summary-errors");
								vs.removeClass("validation-summary-valid");
								var ul = vs.find("ul");
								if (ul.length > 0) {
									ul.html("");
									ul.append("<li>" + message + "</li>");
								} else {
									vs.html("");
									vs.append("<ul></ul>");
									ul.append("<li>" + message + "</li>");
								}
							} else
								alertMessage(title, message);
						} else
							alertMessage(title, message);
					} else if (callBackFunc != null)
						callBackFunc(data, textStatus, jqxhr);
				} else
					alertMessage("Error", "Unauthorized");
			}, 'html');

			function alertMessage(title, message) {
				$.fn.alertOverlay(title, message);
			}
		}
	};
})();

