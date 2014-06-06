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
									$.fn.alertOverlay("Успешная активация", "Аккаунт успешно активирован.");
								}, function () {
									$.fn.alertOverlay("Ошибка активации", "При активации аккаунта возникли проблемы.");
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
									$.fn.alertOverlay("Успешная активация", "Письмо для активации успешно отправлено.");
								}, function () {
									$.fn.alertOverlay("Ошибка активации", "При активации аккаунта возникли проблемы.");
								});
						});
					});
				},

				banUser: function () {

				},

				deleteUser: function () {

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