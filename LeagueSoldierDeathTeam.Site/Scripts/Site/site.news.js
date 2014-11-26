(function () {
	site.news =
	{
		settings: {
			urls: {

			},
			vars: {
				form: null
			},
			elements: {

			}
		},

		initEdit: {
			init: function (settings) {
				$.extend(true, site.news.settings, settings);

				site.news.initEdit.initEditor();
				site.news.initEdit.initPlatforms();
			},

			initPlatforms: function () {
				$(".platform-tag").each(function () {
					var $this = $(this);
					var icon = $this.find("span[data-type=platform-icon]");

					$this.off("click").on("click", function () {
						if ($this.data("active")) {
							$this.removeClass("active");
							$this.data("active", false);
							icon.removeClass("platform-icon");
						} else {
							$this.addClass("active");
							$this.data("active", true);
							icon.addClass("platform-icon");
						}
					});
				});
			},

			initEditor: function () {
				var $description = $("#Description");

				if ($description != undefined) {
					$description.markItUp(mySettings);
					site.news.initEdit.parseBBCode($description);
				}
			},

			parseBBCode: function (description) {
				var $descriptionPreview = $(".description-preview");
				var $previewLink = $(".preview-link");

				$previewLink.off("click").on("click", function () {
					if ($descriptionPreview != undefined) {
						var htmlContent = $.fn.bbcodeParser(description.val());
						if (htmlContent != undefined && htmlContent.trim() != '') {
							$descriptionPreview.html(htmlContent);
							$descriptionPreview.fadeIn("fast");

							$.fn.slideSpoiler($descriptionPreview);
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
				$.extend(true, site.news.settings, settings);
				site.news.initView.parseBBCode();
			},

			parseBBCode: function () {
				var $newsDescription = $('.news-description');

				if ($newsDescription != undefined) {
					var htmlContent = $.fn.bbcodeParser($newsDescription.html());
					$newsDescription.html(htmlContent);

					$.fn.slideSpoiler($newsDescription);
				}
			},
		}
	};
})();