(function () {
	site.administation =
		{
			settings: {
				urls: {
					changeRole: '',
					deleteUser: '',
					activateUser: '',
					banUser: '',
					sendMessageForActivate: '',
					filterUsers: '',
					filterRoles: ''
				},
				elements: {
					tabs: '#tabs',
					users: '#users-content',
					containment: '#containment'
				}
			},

			init: function (settings) {
				$.extend(true, site.administation.settings, settings);

				$(site.administation.settings.elements.tabs).tabs().addClass("ui-tabs-vertical ui-helper-clearfix");

				site.administation.users.init();
				site.administation.roleManagement.init();
			},

			users: {
				init: function () {
					var sortName = $(".sort-name");
					var searchInput = $(".search input");

					site.administation.users.initFilter(sortName, searchInput);

					site.administation.users.activateUser(sortName, searchInput);
					site.administation.users.sendMessageForActivate(sortName, searchInput);
					site.administation.users.banUser(sortName, searchInput);
					site.administation.users.deleteUser(sortName, searchInput);
				},

				initFilter: function (sortName, searchInput) {
					var sortChangeable = $(".sort-changeable");
					var sortDropdown = $(".sort-dropdown");

					sortChangeable.width(sortName.width());

					sortName.off("click").on("click", function () {
						if (sortDropdown.is(":hidden"))
							sortDropdown.fadeIn("fast");
						else
							sortDropdown.fadeOut("fast");
					});

					$(document).click(function (event) {
						var $eventTarget = $(event.target);
						if (!$eventTarget.closest(".sort-changeable").length)
							sortDropdown.fadeOut("fast");
						event.stopPropagation ? event.stopPropagation() : (event.cancelBubble = true);
					});

					sortDropdown.find("li").each(function () {
						var $this = $(this);
						$this.off("click").on("click", function () {
							sortName.text($this.text());
							sortName.data("val", $this.data("val"));
							sortChangeable.width($this.width());
							sortDropdown.fadeOut("fast");

							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.filterUsers,
								{ sortFilter: $this.data("val"), term: searchInput.val() },
								function () {
									site.administation.users.init();
								});
						});
					});

					var clearBtn = $(".clear-btn");
					clearBtn.off("click").on("click", function () {
						searchInput.val("");
						clearBtn.hide();
						searchBtn.click();
					});

					var searchBtn = $(".search-btn");
					searchBtn.off("click").on("click", function () {
						site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.filterUsers,
								{ sortFilter: sortName.data("val"), term: searchInput.val() },
								function () {
									site.administation.users.init();
								});
					});

					searchInput.keyup(function (e) {
						var $this = $(this);
						if (e.which == 13) {
							searchBtn.click();
							e.preventDefault();
						}

						if ($this.val() != "")
							clearBtn.show();
						else
							clearBtn.hide();
					});
				},

				activateUser: function (sortName, searchInput) {
					$('[data-action=activation]').off("click").on('click', function () {
						var $this = $(this);
						$.fn.confirmOverlay("Активация аккаунта", "Подтвердите активацию этого аккаунта", function () {
							var userId = $this.data("id");
							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.activateUser,
								{ userId: userId, sortFilter: sortName.data("val"), term: searchInput.val() },
								function () {
									site.administation.users.init();
								});
						});
					});
				},

				sendMessageForActivate: function (sortName, searchInput) {
					$('[data-action=message]').off("click").on('click', function () {
						var $this = $(this);
						$.fn.confirmOverlay("Письмо активации", "Подтвердите активацию этого аккаунта", function () {
							var userId = $this.data("id");
							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.sendMessageForActivate,
								{ userId: userId, sortFilter: sortName.data("val"), term: searchInput.val() },
								function () {
									site.administation.users.init();
								});
						});
					});
				},

				banUser: function (sortName, searchInput) {
					$('[data-action=ban]').off("click").on('click', function () {
						var $this = $(this);
						var isBanned = $this.data("banned");
						var userId = $this.data("id");

						var title = "Блокировка аккаунта";
						var message = "Вы действительно хотите забанить аккаунт?";

						if (isBanned == "True") {
							title = "Разблокировка аккаунта";
							message = "Подтвердите разблокировку аккаунта";
						}

						$.fn.confirmOverlay(title, message, function () {
							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.banUser,
								{ userId: userId, isBanned: isBanned, sortFilter: sortName.data("val"), term: searchInput.val() },
								function () {
									site.administation.users.init();
								});
						});
					});
				},

				deleteUser: function (sortName, searchInput) {
					$('[data-action=delete]').off("click").on('click', function () {
						var $this = $(this);
						$.fn.confirmOverlay("Удаление аккаунта", "Вы действительно хотите удалить аккаунт?", function () {
							var userId = $this.data("id");
							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.deleteUser,
								{ userId: userId, sortFilter: sortName.data("val"), term: searchInput.val() },
								function () {
									site.administation.users.init();
									$(site.administation.settings.elements.containment).find('.draggable[data-id=' + userId + ']').remove();
								});
						});
					});
				},

				refresh: function (content, url, params, callback, callbackError) {
					$(content).loadData(url, params, callback, callbackError);
				}
			},

			roleManagement: {
				init: function () {
					var searchInput = $(".role-search input");
					var searchBtn = $(".role-search-btn");

					site.administation.roleManagement.initFilter(searchInput, searchBtn);
					site.administation.roleManagement.initSortable(searchBtn);
				},

				initSortable: function (searchBtn) {
					$('.role-container').each(function () {
						var $this = $(this);

						$this.sortable({
							connectWith: ".role-container",
							opacity: 0.9,
							revert: 400,
							placeholder: 'placeholder',
							receive: function (event, ui) {
								var $sender = $(ui.sender),
									$item = (ui.item),
									$changedSender = $(event.target);

								var changedRole = $changedSender.data("name"),
									currentRole = $sender.data("name");

								$.fn.confirmOverlay("Смена прав доступа", "Подтвердите смену категории прав доступа c '" + currentRole + "' на '" + changedRole + "'", function () {
									var userId = $item.data("id");
									var roleId = $changedSender.data("id");
									site.ajax.post(site.administation.settings.urls.changeRole, { roleId: roleId, userId: userId }, function () {
										searchBtn.click();
									}, function () {
										$sender.sortable("cancel");
									});
								}, function () {
									$sender.sortable("cancel");
								});
							},
							containment: "#containment"
						}).disableSelection();

						$this.droppable({
							hoverClass: "drop"
						});
					});
				},

				initFilter: function (searchInput, searchBtn) {
					var clearBtn = $(".role-clear-btn");

					clearBtn.off("click").on("click", function () {
						searchInput.val("");
						clearBtn.hide();
						searchBtn.click();
					});

					searchBtn.off("click").on("click", function () {
						site.administation.roleManagement.refresh(site.administation.settings.elements.containment, site.administation.settings.urls.filterRoles,
								{ term: searchInput.val() },
								function () {
									site.administation.roleManagement.init();
								});
					});

					searchInput.keyup(function (e) {
						var $this = $(this);
						if (e.which == 13) {
							searchBtn.click();
							e.preventDefault();
						}

						if ($this.val() != "")
							clearBtn.show();
						else
							clearBtn.hide();
					});
				},

				refresh: function (content, url, params, callback, callbackError) {
					$(content).loadData(url, params, callback, callbackError);
				}
			}
		};
})();