(function () {
	site.comments =
		{
			settings: {
				urls: {
					addComment: '',
					getCommentDesription: '',
					editComment: '',
					deleteComment: '',
					refreshComments: ''
				},
				elements: {
					commentFeed: '#comment-feed',
					commentDescription: '#CommentDescription',
					editCommentDescription: '#Description',
					newComment: '#new-comment',
					commentsHeaderHash: '#comments-header-hash'
				}
			},

			init: function (settings) {
				$.extend(true, site.comments.settings, settings);

				site.comments.initCommentsFeed();
				site.comments.initNewComment();
				site.comments.initGoToCommentsHeader();
				site.comments.refreshCommentsFeed();
			},

			initCommentsFeed: function (commentId) {
				if (commentId == undefined)
					commentId = '';

				var $feed = $(site.comments.settings.elements.commentFeed);

				$feed.find(commentId + '.comment .description').each(function () {
					var $this = $(this);

					$.fn.bbcodeCustomParser($this, $this.html());
				});

				var hashLinks = $('.comment .comment-link');
				hashLinks.find('.comment-link-hash').each(function () {
					var $this = $(this);

					$this.off('click').on('click', function () {
						var input = $this.closest('.comment-link').find('.comment-link-input');
						if (input.is(':visible')) {
							input.hide();
						} else {
							hashLinks.find('.comment-link-input:visible').each(function () {
								$(this).hide();
							});

							input.fadeIn('fast').select()
								.off('click').on('click', function () {
									input.select();
								});
						}
					});
				});

				$(commentId + '.comment .comment-reply').each(function () {
					var $this = $(this);
					$this.off('click').on('click', function () {
						var newDescription = $(site.comments.settings.elements.commentDescription);
						newDescription.val($this.data("writer") + newDescription.val());

						$.fn.animateScrollTop(site.comments.settings.elements.newComment, 'fast');
						newDescription.focus();
					});
				});

				$(commentId + '.comment .comment-quote').each(function () {
					var $this = $(this);
					$this.off('click').on('click', function () {
						var newDescription = $(site.comments.settings.elements.commentDescription);
						newDescription.val(newDescription.val() + $this.data("description"));

						$.fn.animateScrollTop(site.comments.settings.elements.newComment, 'fast');
						newDescription.focus();
					});
				});

				site.comments.initDeleteComment(commentId);
				site.comments.initCommentRate(commentId);
				site.comments.initEditComment(commentId);
			},

			initDeleteComment: function (commentId) {
				$(commentId + '.comment [data-action=delete-comment]').each(function () {
					var $this = $(this);
					$this.off('click').on('click', function () {
						$.fn.confirmOverlay("Удаление комментария", "Вы действительно хотите удалить данный комментарий?", function () {
							$(site.comments.settings.elements.commentFeed).loadData(site.comments.settings.urls.deleteComment, { id: $this.data("id"), sortType: $('.sort-name').data("val") }, function () {
								site.comments.initCommentsFeed();
							});
						});
					});
				});
			},

			initEditComment: function (commentId) {
				$(commentId + '.comment [data-action=edit-comment]').each(function () {
					var $this = $(this);

					var commentDescriptionWrap = $this.closest(".comment-description-wrap");
					var commentViewWrapper = commentDescriptionWrap.find(".comment-view-wrapper");
					var commentEditWrapper = commentDescriptionWrap.find(".comment-edit-wrapper");

					$this.off('click').on('click', function () {
						commentEditWrapper.loadData(site.comments.settings.urls.getCommentDesription, { id: $this.data("id") }, function () {
							var editCommentBtn = commentDescriptionWrap.find('[data-type=edit]');
							var cancelCommentBtn = commentDescriptionWrap.find('[data-type=cancel]');
							var form = editCommentBtn.closest('form');

							cancelCommentBtn.off('click').on('click', function () {
								commentViewWrapper.show();
								commentEditWrapper.hide().html("");
							});

							editCommentBtn.off('click').on('click', function () {
								if (form.valid()) {
									var params = $.fn.serializeParams(form);
									var commentContainerId = "#comment-" + $this.data("id") + "-hash";

									$(commentContainerId).loadData(site.comments.settings.urls.editComment, params, function () {
										site.comments.initCommentsFeed(commentContainerId);
									});
								}
							});

							var description = commentDescriptionWrap.find('[data-type=edit-comment-description]');
							description.markItUp(myCommentSettings);

							commentViewWrapper.hide();
							commentEditWrapper.show();

							description[0].selectionStart = description.val().length;
							description[0].selectionEnd = description.val().length;
							description[0].focus();
						});
					});
				});
			},

			initCommentRate: function (commentId) {

			},

			initGoToCommentsHeader: function () {
				$('[data-action=go-to-comments-header]').off("click").on("click", function () {
					$.fn.animateScrollTop(site.comments.settings.elements.commentsHeaderHash, 'fast');
				});
			},

			initNewComment: function () {
				var $description = $(site.comments.settings.elements.commentDescription);
				var $addCommentBtn = $('[data-type=add]');
				var form = $addCommentBtn.closest('form');
				var $newCommentLink = $('.comments-new-comment-link');

				if ($description != undefined) {
					$description.markItUp(myCommentSettings);
					site.comments.initNewCommentPreview($description);
				}

				$addCommentBtn.off('click').on('click', function () {
					if (form.valid()) {
						var params = $.fn.serializeParams(form, [{ name: "SortType", value: $('.sort-name').data("val") }]);
						$(site.comments.settings.elements.commentFeed).loadData(site.comments.settings.urls.addComment, params, function () {
							$description.val('');
							site.comments.initCommentsFeed();

							$.fn.animateScrollTop(site.comments.settings.elements.commentsHeaderHash, 'fast');
						});
					}
				});

				$newCommentLink.off('click').on('click', function () {
					$.fn.animateScrollTop(site.comments.settings.elements.newComment, 'fast');
					$description.focus();
				});
			},

			initNewCommentPreview: function (description) {
				var $descriptionPreview = $('.description-preview');
				var $descriptionFooter = $('.markItUpFooter');
				var $previewLink = $('[data-type=preview]');

				$previewLink.off('click').on('click', function () {
					if ($descriptionPreview != undefined) {
						$.fn.bbcodeCustomParser($descriptionPreview, description.val());
						if ($descriptionPreview.html() != undefined && $descriptionPreview.html().trim() != '') {
							$descriptionPreview.fadeIn('fast');
							$descriptionFooter.fadeIn('fast');
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

			refreshCommentsFeed: function () {
				var $refreshCommentBtn = $('[data-type=refresh-comments]');

				$refreshCommentBtn.off('click').on('click', function () {
					$(site.comments.settings.elements.commentFeed).loadData(site.comments.settings.urls.refreshComments, { id: $refreshCommentBtn.data('content-id'), sortType: $('.sort-name').data("val") }, function () {
						site.comments.initCommentsFeed();

						setTimeout("$('.comments-refresh-complete').fadeIn('normal')", 100);
						setTimeout("$('.comments-refresh-complete').fadeOut('slow')", 2000);
					});
				});

				$.fn.sortDropdown($('.sort-name'), function (name) {
					$(site.comments.settings.elements.commentFeed).loadData(site.comments.settings.urls.refreshComments, { id: $refreshCommentBtn.data('content-id'), sortType: name.data("val") }, function () {
						site.comments.initCommentsFeed();

						setTimeout("$('.comments-refresh-complete').fadeIn('normal')", 100);
						setTimeout("$('.comments-refresh-complete').fadeOut('slow')", 2000);
					});
				});
			}
		};
})();