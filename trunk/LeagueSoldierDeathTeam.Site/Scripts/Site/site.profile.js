(function () {
	site.profile =
		{
			settings: {
				urls: {
					mainContent: '',
					changePassword: '',
					editMainInfo: ''
				},
				vars: {
				},
				elements: {
					photoUploadFile: '#PhotoUploadFile',
					photoUploadFileName: '#PhotoUploadFileName',
					tabContentMain: '#tab-content-main',
					tabContentAdvance: '#tab-content-advance',
					tabContentBind: '#tab-content-bind',
					tabContentChangePassword: '#tab-content-change-pass'
				}
			},

			init: function (settings) {
				$.extend(true, site.profile.settings, settings);

				site.profile.initChangePassword();
				site.profile.initContentMain();
				site.profile.initContentAdvance();
				site.profile.initContentBind();
			},

			initChangePassword: function () {
				var $tabContentChangePassword = $(site.profile.settings.elements.tabContentChangePassword);
				$tabContentChangePassword.find("input[type=button]").on("click", function () {
					var form = $tabContentChangePassword.find("form");
					if (form.valid()) {
						$tabContentChangePassword.loadData(site.profile.settings.urls.changePassword, $.fn.serializeParams(form),
							function () {
								site.profile.initChangePassword();
								$.fn.alertMessage("Смена пароля", "Пароль был успешно изменен.");
							},
							function () {
								$.fn.initValidationSummary($tabContentChangePassword);
								site.profile.initChangePassword();
							});
					}
				});
			},

			initContentMain: function () {
				var $tabContentMain = $(site.profile.settings.elements.tabContentMain);

				$tabContentMain.find(site.profile.settings.elements.photoUploadFile).on('change', function () {
					$tabContentMain.find(site.profile.settings.elements.photoUploadFileName).val($(this).val().replace(/\\/g, '/').replace(/.*\//, ''));
				});

				$tabContentMain.find("input[type=button]").on("click", function () {
					var form = $tabContentMain.find("form");
					if (form.valid()) {
						$tabContentMain.loadData(site.profile.settings.urls.editMainInfo, $.fn.serializeParams(form),
							function () {
								site.profile.initContentMain();
								$.fn.alertMessage("Обновление профиля", "Данные успешно обновлены.");
							},
							function () {
								$.fn.initValidationSummary($tabContentMain);
								site.profile.initContentMain();
							});
					}
				});
			},

			initContentAdvance: function () {
				var $tabContentAdvance = $(site.profile.settings.elements.tabContentAdvance);
				$tabContentAdvance.applyDatepicker();
			},

			initContentBind: function () {
				var $tabContentBind = $(site.profile.settings.elements.tabContentBind);
			}
		};
})();