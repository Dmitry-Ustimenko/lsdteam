(function () {
	site.messages =
		{
			settings: {
				urls: {
					saveAsRead: '',
					saveAsDraft: '',
					deleteMessages: '',
				},
				vars: {
				},
				elements: {
					content: "#content",
					messageType: "#MessageType"
				},
				attributes: {
				}
			},

			init: function (settings) {
				$.extend(true, site.messages.settings, settings);

				var type = $(site.messages.settings.elements.messageType);
				var messages = $("[data-type=message]");

				site.messages.initCheckboxes(messages);
				site.messages.changeType();
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

			changeType: function () {

			},

			saveAsRead: function (type, messages) {
				$('[data-action=readMessages]').off("click").on('click', function () {
					$.fn.confirmOverlay("Подтверждение", "Подтвердите действие", function () {
						var array = [];
						messages.each(function () {
							var $this = $(this);
							if ($this.is(":checked"))
								array.push($this.data("id"));
						});

						if (array.length != 0) {
							site.messages.refresh(site.messages.settings.elements.content, site.messages.settings.urls.saveAsRead,
							{ type: type.val(), messageIds: array.join(",") },
							function () {
								site.messages.init();
							});
						}
					});
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