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

			initCommentsFeed: function () {
				var $feed = $(site.comments.settings.elements.commentFeed);

				$feed.find('.comment .description').each(function () {
					var $this = $(this);
					$this.html($.fn.bbcodeCustomParser($this.html()));
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

				$('.comment-reply').each(function () {
					var $this = $(this);
					$this.off('click').on('click', function () {
						var newDescription = $(site.comments.settings.elements.commentDescription);
						newDescription.val($this.data("writer") + newDescription.val());

						$.fn.animateScrollTop(site.comments.settings.elements.newComment, 'fast');
						newDescription.focus();
					});
				});

				$('.comment-quote').each(function () {
					var $this = $(this);
					$this.off('click').on('click', function () {
						var newDescription = $(site.comments.settings.elements.commentDescription);
						newDescription.val(newDescription.val() + $this.data("description"));

						$.fn.animateScrollTop(site.comments.settings.elements.newComment, 'fast');
						newDescription.focus();
					});
				});

				site.comments.initDeleteComment();
				site.comments.initCommentRate();
				site.comments.initEditComment();
			},

			initDeleteComment: function () {
				$('[data-action=delete-comment]').each(function () {
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

			initEditComment: function () {
				$('[data-action=edit-comment]').each(function () {
					var $this = $(this);

					var commentDescriptionWrap = $this.closest(".comment-description-wrap");
					var commentViewWrapper = commentDescriptionWrap.find(".comment-view-wrapper");
					var commentEditWrapper = commentDescriptionWrap.find(".comment-edit-wrapper");

					$this.off('click').on('click', function () {
						commentEditWrapper.loadData(site.comments.settings.urls.getCommentDesription, { id: $this.data("id") }, function () {
							commentViewWrapper.hide();
							commentEditWrapper.show();

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
									$("#comment-" + $this.data("id") + "-hash").loadData(site.comments.settings.urls.editComment, params, function () {
										site.comments.initCommentsFeed();
									});
								}
							});
						});
					});
				});
			},

			initCommentRate: function () {

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
					$.fn.parseCommentPreviewBBCode($description);
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