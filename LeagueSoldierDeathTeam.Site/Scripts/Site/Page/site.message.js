﻿(function () {
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
					site.message.initEdit.parseBBCode($description);
				}
			},

			parseBBCode: function (description) {
				var $descriptionPreview = $(".description-preview");
				var $previewLink = $(".preview-link");

				$previewLink.off("click").on("click", function () {
					if ($descriptionPreview != undefined) {
						$.fn.bbcodeParser($descriptionPreview, description.val());

						if ($descriptionPreview.html() != undefined && $descriptionPreview.html().trim() != '') {
							$descriptionPreview.fadeIn("fast");
						}
					}
				});

				description.keypress(function (e) {
					e = e || window.event;

					if (e.shiftKey && (e.which == 13 || e.keyCode == 13)) {
						$previewLink.click();
						return false;
					}

					return true;
				});
			}
		},

		initView: {
			init: function (settings) {
				$.extend(true, site.message.settings, settings);
				site.message.initView.saveAsDraft();
				site.message.initView.deleteMessage();
				site.message.initView.parseBBCode();
			},

			parseBBCode: function () {
				var $messageDescription = $('.message-description');

				if ($messageDescription != undefined) {
					$.fn.bbcodeParser($messageDescription, $messageDescription.html());
				}
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