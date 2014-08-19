(function () {
	site.messages =
		{
			settings: {
				urls: {
					saveAsRead: '',
					saveAsDraft: '',
					deleteMessages: '',
					refreshGrid: ''
				},
				vars: {
				},
				elements: {
					content: "#content",
					messageTypeId: "#MessageTypeId",
					form: "#form"
				},
				attributes: {
				}
			},

			init: function (settings) {
				$.extend(true, site.messages.settings, settings);
				site.messages.changeType();
				site.messages.refreshGrid();
			},

			changeType: function () {
				$(site.messages.settings.elements.form).on("change", function () {
					site.messages.refresh(site.messages.settings.elements.content, site.messages.settings.urls.refreshGrid,
							$.fn.serializeParams(site.messages.settings.vars.form),
							function () {
								site.messages.refreshGrid();
							});
				});
			},

			refreshGrid: function () {
				var type = $(site.messages.settings.elements.messageTypeId);
				var messages = $("[data-type=message]");

				$.fn.initCheckbox();
				site.messages.initCheckboxes(messages);
				site.messages.saveAsDraft();
				site.messages.saveAsRead(type, messages);
				site.messages.deleteMessages();
			},

			initCheckboxes: function (messages) {
				var allSelect = $("[data-action=allSelect]");
				var allSelectCheckbox = allSelect.find("input[type=checkbox]");
				var selectRead = $("[data-action=select-read]");
				var selectUnread = $("[data-action=select-unread]");

				function unselect() {
					if (allSelectCheckbox.is(":checked")) {
						allSelectCheckbox.prop("checked", false);
						allSelectCheckbox.parent().removeClass('checked');
					}
				}

				allSelect.off("click").on("click", function () {
					messages.each(function () {
						var $this = $(this);

						if (allSelectCheckbox.is(":checked")) {
							if (!$this.is(":checked"))
								$this.click();
						} else {
							if ($this.is(":checked"))
								$this.click();
						}
					});
				});

				selectRead.off("click").on("click", function () {
					unselect();

					messages.each(function () {
						var $this = $(this);
						if ($this.data("read") == "True") {
							if (!$this.is(":checked"))
								$this.click();
						} else {
							if ($this.is(":checked"))
								$this.click();
						}
					});
				});

				selectUnread.off("click").on("click", function () {
					unselect();

					messages.each(function () {
						var $this = $(this);
						if ($this.data("read") == "False") {
							if (!$this.is(":checked"))
								$this.click();
						} else {
							if ($this.is(":checked"))
								$this.click();
						}
					});
				});
			},

			//initMessageCount: function (type) {
			//	if (type.val() == "Inbox")
			//		$("[data-type=global-message-count]").text($("[data-type=message-count]").text());
			//},

			saveAsRead: function (type, messages) {
				$('[data-action=readMessages]').off("click").on('click', function () {
					var array = [];
					messages.each(function () {
						var $this = $(this);
						if ($this.is(":checked") && $this.data("read") == "False")
							array.push($this.data("id"));
					});

					if (array.length != 0) {
						$.fn.confirmOverlay("Подтверждение", "Подтвердите действие", function () {
							site.messages.refresh(site.messages.settings.elements.content, site.messages.settings.urls.saveAsRead,
							{ type: type.val(), messageIds: array.join(",") },
							function () {
								site.messages.refreshGrid();
							});
						});
					}
				});
			},

			saveAsDraft: function () {

			},

			deleteMessages: function () {

			},

			refresh: function (content, url, params, callback, callbackError) {
				$(content).loadData(url, params, callback, callbackError);
			}
		};
})();