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
					site.message.initEdit.parseBBCode($description);
				}
			},
			
			parseBBCode: function (description) {
				var $descriptionPreview = $(".description-preview");
				var $previewLink = $(".preview-link");

				$previewLink.off("click").on("click", function() {
					if ($descriptionPreview != undefined) {
						var htmlContent = $.fn.bbcodeParser(description.val());
						if (htmlContent != undefined && htmlContent.trim() != '') {
							$descriptionPreview.html(htmlContent);
							$descriptionPreview.fadeIn("fast");
							
							$.fn.slideSpoiler($descriptionPreview);
						}
					}
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
					var htmlContent = $.fn.bbcodeParser($messageDescription.html());
					$messageDescription.html(htmlContent);
					
					$.fn.slideSpoiler();
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