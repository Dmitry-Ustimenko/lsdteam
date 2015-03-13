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
					form: null
				},
				elements: {
					content: "#content",
					messageTypeId: "#MessageTypeId",
					messageType: "#MessageType",
					form: "#form",
					pager: "#pager"
				}
			},

			init: function (settings) {
				$.extend(true, site.messages.settings, settings);

				var $form = $(site.messages.settings.elements.form);
				site.messages.settings.vars.form = $form.find("form");

				site.messages.changeType();
				site.messages.refreshGrid();
			},

			refreshGrid: function () {
				var type = $(site.messages.settings.elements.messageTypeId);
				var messages = $("[data-type=message]");

				$.fn.initCheckbox();
				site.messages.initPager();
				site.messages.initCheckboxes(messages);
				site.messages.saveAsDraft(type, messages);
				site.messages.saveAsRead(type, messages);
				site.messages.deleteMessages(type, messages);
			},

			changeType: function () {
				var $form = $(site.messages.settings.elements.form);
				site.messages.settings.vars.form = $form.find("form");

				$form.on("change", function () {
					site.messages.refresh(site.messages.settings.elements.content, site.messages.settings.urls.refreshGrid,
							$.fn.serializeParams(site.messages.settings.vars.form),
							function () {
								site.messages.refreshGrid();
							});
				});
			},

			initPager: function () {
				$(site.messages.settings.elements.pager).pager(function (pageId) {
					site.messages.refresh(site.messages.settings.elements.content, site.messages.settings.urls.refreshGrid,
						$.fn.serializeParams(site.messages.settings.vars.form, [{ name: "Pager.PageId", value: pageId }]),
						function () {
							site.messages.refreshGrid();
						});
				});
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

			initGlobalMessageCount: function () {
				if ($(site.messages.settings.elements.messageType).val() == "Inbox") {
					var globalMessageCount = $("[data-type=global-message-count]");
					var ureadCount = $("#ureadMessages").val();

					globalMessageCount.text(ureadCount);
					if (ureadCount == '0')
						globalMessageCount.closest("sup").addClass('gray');
				}
			},

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
							{ typeId: type.val(), messageIds: array.join(",") },
							function () {
								site.messages.refreshGrid();
								site.messages.initGlobalMessageCount();
							});
						});
					}
				});
			},

			saveAsDraft: function (type, messages) {
				$('[data-action=saveMessages]').off("click").on('click', function () {
					var array = [];
					messages.each(function () {
						var $this = $(this);
						if ($this.is(":checked"))
							array.push($this.data("id"));
					});

					if (array.length != 0) {
						$.fn.confirmOverlay("Сохранение сообщений", "Подтвердите сохранение отмеченных сообщений", function () {
							site.messages.refresh(site.messages.settings.elements.content, site.messages.settings.urls.saveAsDraft,
							{ typeId: type.val(), messageIds: array.join(",") },
							function () {
								site.messages.refreshGrid();
								site.messages.initGlobalMessageCount();
							});
						});
					}
				});
			},

			deleteMessages: function (type, messages) {
				$('[data-action=deleteMessages]').off("click").on('click', function () {
					var array = [];
					messages.each(function () {
						var $this = $(this);
						if ($this.is(":checked"))
							array.push($this.data("id"));
					});

					if (array.length != 0) {
						$.fn.confirmOverlay("Удаление сообщений", "Подтвердите удаление отмеченных сообщений", function () {
							site.messages.refresh(site.messages.settings.elements.content, site.messages.settings.urls.deleteMessages,
							{ typeId: type.val(), messageIds: array.join(",") },
							function () {
								site.messages.refreshGrid();
								site.messages.initGlobalMessageCount();
							});
						});
					}
				});
			},

			refresh: function (content, url, params, callback, callbackError) {
				$(content).loadData(url, params, callback, callbackError);
			}
		};
})();