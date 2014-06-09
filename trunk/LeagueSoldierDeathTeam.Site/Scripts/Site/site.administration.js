(function () {
	site.administation =
		{
			settings: {
				urls: {
					deleteUser: '',
					activateUser: '',
					banUser: '',
					sendMessageForActivate: ''
				},
				elements: {
					tabs: '#tabs',
					users: '#users'
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
					site.administation.users.activateUser();
					site.administation.users.sendMessageForActivate();
					site.administation.users.banUser();
					site.administation.users.deleteUser();
				},

				activateUser: function () {
					$('[data-action=activation]').off("click").on('click', function () {
						var $this = $(this);
						$.fn.confirmOverlay("Активация аккаунта", "Подтвердите активацию этого аккаунта", function () {
							var userId = $this.data("id");
							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.activateUser, { userId: userId },
								function () {
									site.administation.users.init();
								});
						});
					});
				},

				sendMessageForActivate: function () {
					$('[data-action=message]').off("click").on('click', function () {
						var $this = $(this);
						$.fn.confirmOverlay("Письмо активации", "Подтвердите активацию этого аккаунта", function () {
							var userId = $this.data("id");
							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.sendMessageForActivate, { userId: userId },
								function () {
									site.administation.users.init();
								});
						});
					});
				},

				banUser: function () {
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
							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.banUser, { userId: userId, isBanned: isBanned },
								function () {
									site.administation.users.init();
								});
						});
					});
				},

				deleteUser: function () {
					$('[data-action=delete]').off("click").on('click', function () {
						var $this = $(this);
						$.fn.confirmOverlay("Удаление аккаунта", "Вы действительно хотите удалить аккаунт?", function () {
							var userId = $this.data("id");
							site.administation.users.refresh(site.administation.settings.elements.users, site.administation.settings.urls.deleteUser, { userId: userId },
								function () {
									site.administation.users.init();
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

				},
			}
		};
})();