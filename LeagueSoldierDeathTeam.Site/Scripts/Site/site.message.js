(function () {
	site.message =
	{
		settings: {
			urls: {
				saveAsDraft: '',
				deleteMessage: '',
				redirectAction: ''
			},
			vars: {
				form: null
			},
			elements: {

			}
		},

		initEdit: {
			init: function (settings) {
				$.extend(true, site.message.settings, settings);

				site.message.initEdit.initEditor();
			},

			initEditor: function () {
				var $description = $("#Description");

				if ($description != undefined) {
					$description.markItUp(mySettings);
				}
			}
		},

		initView: {
			init: function (settings) {
				$.extend(true, site.message.settings, settings);
				site.message.initView.saveAsDraft();
				site.message.initView.deleteMessage();
			},

			saveAsDraft: function () {
				$('[data-action=saveMessage]').off("click").on('click', function () {
					var id = $(this).data("id");
					$.fn.confirmOverlay("Сохранение сообщения", "Подтвердите сохранение сообщения", function () {
						site.ajax.post(site.message.settings.urls.saveAsDraft, { id: id });
					});
				});
			},

			deleteMessage: function () {
				$('[data-action=deleteMessage]').off("click").on('click', function () {
					var id = $(this).data("id");
					$.fn.confirmOverlay("Удаление сообщения", "Подтвердите удаление сообщения", function () {
						site.ajax.post(site.message.settings.urls.deleteMessage, { id: id });
					});
				});
			}
		}
	};
})();