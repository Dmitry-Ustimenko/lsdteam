﻿(function () {
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
				filterRoles: '',
				loginAs: '',
				refreshUsersGrid: '',
				refreshUserRolesGrid: '',
			},
			elements: {
				tabs: '#tabs',
				users: '#usersContent',
				roles: '#roleContent',
				rolePager: '#rolePager',
				userPager: '#userPager'
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
				site.administation.users.refreshGrid(sortName, searchInput);
			},

			refreshGrid: function (sortName, searchInput) {
				site.administation.users.loginAs();
				site.administation.users.initPager(sortName, searchInput);
				site.administation.users.activateUser(sortName, searchInput);
				site.administation.users.sendMessageForActivate(sortName, searchInput);
				site.administation.users.banUser(sortName, searchInput);
				site.administation.users.deleteUser(sortName, searchInput);
			},

			initPager: function (sortName, searchInput) {
				$(site.administation.settings.elements.userPager).pager(function (pageId) {
					site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.refreshUsersGrid,
						[{ name: "SortType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }, { name: "Pager.PageId", value: pageId }],
						function () {
							site.administation.users.refreshGrid(sortName, searchInput);
						});
				});
			},

			initFilter: function (sortName, searchInput) {
				$.fn.sortDropdown(sortName, function (name) {
					site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.refreshUsersGrid,
						[{ name: "SortType", value: name.data("val") }, { name: "Term", value: searchInput.val() }],
						function () {
							site.administation.users.refreshGrid(name, searchInput);
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
					site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.refreshUsersGrid,
							[{ name: "SortType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }],
							function () {
								site.administation.users.refreshGrid(sortName, searchInput);
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
							[{ name: "SortType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }, { name: "userId", value: userId }],
							function () {
								site.administation.users.refreshGrid(sortName, searchInput);
							});
					});
				});
			},

			sendMessageForActivate: function (sortName, searchInput) {
				$('[data-action=message]').off("click").on('click', function () {
					var $this = $(this);
					$.fn.confirmOverlay("Письмо активации", "Подтвердите отправку письма активации", function () {
						var userId = $this.data("id");
						site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.sendMessageForActivate,
							[{ name: "SortType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }, { name: "userId", value: userId }],
							function () {
								site.administation.users.refreshGrid(sortName, searchInput);
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
							[{ name: "SortType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }, { name: "userId", value: userId }, { name: "isBanned", value: isBanned }],
							function () {
								site.administation.users.refreshGrid(sortName, searchInput);
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
							[{ name: "SortType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }, { name: "userId", value: userId }],
							function () {
								site.administation.users.refreshGrid(sortName, searchInput);
							});
					});
				});
			},

			loginAs: function () {
				$('[data-action=login-as]').off("click").on('click', function () {
					var $this = $(this);
					$.fn.confirmOverlay("Релогин", "Вы действительно хотите залогинится под этим аккаунтом?", function () {
						var userId = $this.data("id");
						site.ajax.post(site.administation.settings.urls.loginAs, { userId: userId });
					});
				});
			},

			refresh: function (content, url, params, callback, callbackError) {
				$(content).loadData(url, params, callback, callbackError);
			}
		},

		roleManagement: {
			init: function () {
				var sortName = $(".sort-role-name");
				var searchInput = $(".role-search input");

				site.administation.roleManagement.initFilter(sortName, searchInput);
				site.administation.roleManagement.initPager(sortName, searchInput);
				site.administation.roleManagement.changeRole(sortName, searchInput);
			},

			initFilter: function (sortName, searchInput) {
				var sortChangeable = $(".sort-role-changeable");
				var sortDropdown = $(".sort-role-dropdown");

				sortName.off("click").on("click", function () {
					if (sortDropdown.is(":hidden"))
						sortDropdown.fadeIn("fast");
					else
						sortDropdown.fadeOut("fast");
				});

				$(document).click(function (event) {
					var $eventTarget = $(event.target);
					if (!$eventTarget.closest(".sort-role-changeable").length)
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

						site.administation.roleManagement.refresh(site.administation.settings.elements.roles, site.administation.settings.urls.refreshUserRolesGrid,
							[{ name: "RoleType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }],
							function () {
								site.administation.roleManagement.init();
							});
					});
				});

				var clearBtn = $(".role-clear-btn");
				clearBtn.off("click").on("click", function () {
					searchInput.val("");
					clearBtn.hide();
					searchBtn.click();
				});

				var searchBtn = $(".role-search-btn");
				searchBtn.off("click").on("click", function () {
					site.administation.roleManagement.refresh(site.administation.settings.elements.roles, site.administation.settings.urls.refreshUserRolesGrid,
							[{ name: "RoleType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }],
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

			initPager: function (sortName, searchInput) {
				$(site.administation.settings.elements.rolePager).pager(function (pageId) {
					site.administation.roleManagement.refresh(site.administation.settings.elements.roles, site.administation.settings.urls.refreshUserRolesGrid,
							[{ name: "RoleType", value: sortName.data("val") }, { name: "Term", value: searchInput.val() }, { name: "Pager.PageId", value: pageId }],
							function () {
								site.administation.roleManagement.init();
							});
				});
			},

			changeRole: function (sortName, searchInput) {
				$("select[data-type=role]").each(function () {
					var $this = $(this);
					var currentValue = $this.val();

					$this.on("change", function () {
						$.fn.confirmOverlay("Смена роли", "Подтвердите смену роли для данного аккаунта", function () {
							site.administation.roleManagement.refresh(site.administation.settings.elements.roles, site.administation.settings.urls.changeRole,
								[{ name: "userId", value: $this.data("user-id") }, { name: "roleId", value: $this.val() },
									{ name: "RoleType", value: "None" }, { name: "Term", value: $this.data("user-name") }],
							function () {
								site.administation.roleManagement.init();
								searchInput.val($this.data("user-name"));
								$(".role-clear-btn").show();
							});
						}, function () {
							$this.val(currentValue);
						});
					});
				});
			},

			refresh: function (content, url, params, callback, callbackError) {
				$(content).loadData(url, params, callback, callbackError);
			}

			//initSortable: function (searchBtn) {
			//	$('.role-container').each(function () {
			//		var $this = $(this);

			//		$this.sortable({
			//			connectWith: ".role-container",
			//			opacity: 0.9,
			//			revert: 400,
			//			placeholder: 'placeholder',
			//			receive: function (event, ui) {
			//				var $sender = $(ui.sender),
			//					$item = (ui.item),
			//					$changedSender = $(event.target);

			//				var changedRole = $changedSender.data("name"),
			//					currentRole = $sender.data("name");

			//				$.fn.confirmOverlay("Смена прав доступа", "Подтвердите смену категории прав доступа c '" + currentRole + "' на '" + changedRole + "'", function () {
			//					var userId = $item.data("id");
			//					var roleId = $changedSender.data("id");
			//					site.ajax.post(site.administation.settings.urls.changeRole, { roleId: roleId, userId: userId }, function () {
			//						searchBtn.click();
			//					}, function () {
			//						$sender.sortable("cancel");
			//					});
			//				}, function () {
			//					$sender.sortable("cancel");
			//				});
			//			},
			//			containment: "#containment"
			//		}).disableSelection();

			//		$this.droppable({
			//			hoverClass: "drop"
			//		});
			//	});
			//},
		}
	};
})();