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
		post: function (url, params, callback) {
			site.ajax.send("POST", url, params, callback);
		},

		get: function (url, params, callback) {
			site.ajax.send("GET", url, params, callback);
		},

		send: function (type, url, dataParams, callback) {
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

					if (typeof (callback) == 'function') {
						callback(data);
					}
				},
				error: function (jqxhr) {
					window.status = "Done";
					document.body.style.cursor = "default";

					if (jqxhr.status == 404)
						alertMessage("Ошибка (404)", "Страница не найдена.");
					else
						alertMessage("Ошибка (" + jqxhr.status + ")", "При обработке запроса произошла ошибка.");
				}
			});

			function alertMessage(title, message) {
				$.fn.alertOverlay(title, message);
			}
		}
	};
})();

