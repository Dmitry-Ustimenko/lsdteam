(function () {
	site.news =
	{
		settings: {
			urls: {
				refreshGrid: '',
				deleteNews: '',
				addNewsComment: '',
				refreshNewsFeed: '',
				deleteNewsComment: ''
			},
			vars: {
				form: null
			},
			elements: {
				platformIds: '#HiddenPlatformIds',
				imageUploadFile: '#ImageUploadFile',
				imageUploadFileName: '#ImageUploadFileName',
				submitBtn: '#submitBtn',
				form: '#form',
				content: '#content',
				pager: '#pager',
				commentDescription: '#CommentDescription',
				comments: '#comments'
			}
		},

		initGrid: {
			init: function (settings) {
				$.extend(true, site.news.settings, settings);

				var $form = $(site.news.settings.elements.form);
				site.news.settings.vars.form = $form.find("form");

				site.news.initGrid.initNewsGrid();
				site.news.initGrid.initFilter($form);
			},

			initNewsGrid: function () {
				site.news.initGrid.initPager();
			},

			initPager: function () {
				$(site.news.settings.elements.pager).pager(function (pageId) {
					site.news.initGrid.refresh(site.news.settings.elements.content, site.news.settings.urls.refreshGrid,
						$.fn.serializeParams(site.news.settings.vars.form, [{ name: "Pager.PageId", value: pageId }]),
						function () {
							site.news.initGrid.initNewsGrid();
						});
				});
			},

			initFilter: function (form) {
				form.on("change", function () {
					site.news.initGrid.refresh(site.news.settings.elements.content, site.news.settings.urls.refreshGrid,
							$.fn.serializeParams(site.news.settings.vars.form),
							function () {
								site.news.initGrid.initNewsGrid();
							});
				});
			},

			refresh: function (content, url, params, callback, callbackError) {
				$(content).loadData(url, params, callback, callbackError);
			}
		},

		initEdit: {
			init: function (settings) {
				$.extend(true, site.news.settings, settings);

				site.news.initEdit.initEditor();
				site.news.initEdit.initPlatforms();
				site.news.initEdit.initAnnotationLimit();
				site.news.initEdit.submitForm();
				site.news.initEdit.initUploadContainer();
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

			initAnnotationLimit: function () {
				$.fn.lenghtLimit($("[data-type=annotation]"), $(".annotation-length"), 150);
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
			},

			initUploadContainer: function () {
				var $clearInput = $(".clear-input");
				var $imageUploadFile = $(site.news.settings.elements.imageUploadFile);
				var $imageUploadFileName = $(site.news.settings.elements.imageUploadFileName);

				$clearInput.off("click").on("click", function () {
					$imageUploadFileName.val("");
					$imageUploadFile.val("");
					$clearInput.hide();
				});

				$imageUploadFile.on('change', function () {
					$imageUploadFileName.val($(this).val().replace(/\\/g, '/').replace(/.*\//, ''));
					$clearInput.show();
				});

				$(site.news.settings.elements.submitBtn).on("click", function () {
					var $this = $(this);
					var form = $this.closest("form");
					var validationSummary = form.find("[class^=validation-summary-]");
					validationSummary.removeClass("validation-summary-errors").addClass("validation-summary-valid");
					validationSummary.find("ul").html("");
					if (form.valid()) {
						var files = $imageUploadFile.get(0).files;
						if (files.length) {
							var message = $.fn.validateUploadFile(files[0], { size: 102400 });
							if (message != undefined) {
								validationSummary.removeClass("validation-summary-valid").addClass("validation-summary-errors");
								validationSummary.find("ul").append("<li>" + message + "</li>");
								return false;
							}
						}

						return true;
					}
					return false;
				});
			},

			submitForm: function () {
				var form = $("form");
				form.on('submit', function (e) {
					var $this = $(this);
					var platformIds = [];

					$(".platform-tag").each(function () {
						var platform = $(this);

						if (platform.data("active")) {
							platformIds.push(platform.data("id"));
						}
					});

					if ($this.valid()) {
						$(site.news.settings.elements.platformIds).val(platformIds.join(","));
					}
				});
			}
		},

		initView: {
			init: function (settings) {
				$.extend(true, site.news.settings, settings);
				site.news.initView.parseBBCode();
				site.news.initView.initComments();
				site.news.initView.deleteNews();
			},

			deleteNews: function () {
				$('[data-action=delete-news]').off('click').on('click', function () {
					var $this = $(this);
					$.fn.confirmOverlay("Удаление новости", "Вы действительно хотите удалить новость", function () {
						var id = $this.data("id");
						site.ajax.post(site.news.settings.urls.deleteNews, { id: id });
					});
				});
			},

			initComments: function () {
				$.fn.refreshCommentFeed(site.news.settings.urls.refreshNewsFeed, site.news.settings.urls.deleteNewsComment);
				$.fn.initNewComment(site.news.settings.urls.addNewsComment, site.news.settings.urls.deleteNewsComment);
				$.fn.initCommentFeed(site.news.settings.urls.deleteNewsComment);
			},

			parseBBCode: function () {
				var $newsDescription = $('.view-news-description');

				if ($newsDescription != undefined) {
					var htmlContent = $.fn.bbcodeParser($newsDescription.html());
					$newsDescription.html(htmlContent);

					$.fn.slideSpoiler($newsDescription);
				}
			},
		}
	};
})();