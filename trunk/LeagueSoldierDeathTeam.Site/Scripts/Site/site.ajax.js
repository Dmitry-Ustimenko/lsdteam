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
		post: function (url, params, callback, callbackError) {
			site.ajax.send("POST", url, params, callback, callbackError);
		},

		get: function (url, params, callback, callbackError) {
			site.ajax.send("GET", url, params, callback, callbackError);
		},

		send: function (type, url, dataParams, callback, callbackError) {
			window.status = "Please wait...";
			document.body.style.cursor = "wait";

			var params = typeof (dataParams) == 'object' ? dataParams : {};

			$.ajax({
				type: type,
				url: url,
				data: params,
				success: function (data, textStatus, jqxhr) {
					window.status = "Done";
					document.body.style.cursor = "default";

					if (jqxhr.status == 204) {
						window.location.reload();
						return;
					}

					if (data.ReturnUrl != undefined) {
						window.location.href = data.ReturnUrl;
					}
					else {
						var returnUrl = $.fn.GetQueryParamValue("ReturnUrl");
						if (returnUrl != undefined) {
							window.location.href = returnUrl.split("%2F").join("/");
						}
						else {
							if (data.Status != undefined && data.Status == "Error") {
								if (typeof (callbackError) == 'function') {
									callbackError(data.Message != undefined ? data.Message : "При обработке запроса произошла ошибка.");
								}
							}
							else if (typeof (callback) == 'function') {
								callback(data);
							} else {
								window.location.href = "/";
							}
						}
					}
				},
				error: function (jqxhr) {
					window.status = "Done with error";
					document.body.style.cursor = "default";

					if (jqxhr.status == 404)
						alertMessage("Ошибка (404)", "Страница не найдена.");
					else
						alertMessage("Ошибка (" + jqxhr.status + ")", "При обработке запроса произошла ошибка.");

					if (typeof (callbackError) == 'function') {
						callbackError();
					}
				}
			});

			function alertMessage(title, message) {
				$.fn.alertOverlay(title, message);
			}
		}
	};
})();

