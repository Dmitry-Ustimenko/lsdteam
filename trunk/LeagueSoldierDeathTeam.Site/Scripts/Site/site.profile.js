(function () {
	site.profile =
		{
			settings: {
				urls: {
					mainContent: '',
					changePassword: '',
					editMainInfo: '',
					editAdvanceInfo: '',
					editBindInfo: ''
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
								$.fn.initCheckbox();
								$.fn.alertMessage("Смена пароля", "Пароль был успешно изменен.");
							},
							function () {
								$.fn.initValidationSummary($tabContentChangePassword);
								site.profile.initChangePassword();
								$.fn.initCheckbox();
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
								$.fn.initCheckbox();
								$.fn.alertMessage("Обновление профиля", "Основные данные были успешно обновлены.");
							},
							function () {
								$.fn.initValidationSummary($tabContentMain);
								site.profile.initContentMain();
								$.fn.initCheckbox();
							});
					}
				});
			},

			initContentAdvance: function () {
				var $tabContentAdvance = $(site.profile.settings.elements.tabContentAdvance);
				$tabContentAdvance.applyDatepicker();

				//$tabContentAdvance.find("input[data-type]").each(function () {
				//	var $this = $(this);
				//	var bindElem = $tabContentAdvance.find("input[data-type=" + $this.data("bind") + "]");

				//	if ($this.val() == "")
				//		$this.closest(".clearfix").hide();
				//	else
				//		$this.closest(".clearfix").show();
				//});

				//$tabContentAdvance.find("input[data-type]").on("change", function () {
				//	var $this = $(this);
				//	var bindElem = $tabContentAdvance.find("input[data-type=" + $this.data("bind") + "]");

				//	if ($this.val() == "") {
				//		bindElem.closest(".clearfix").fadeOut("fast");
				//		bindElem.val("");
				//	} else {
				//		bindElem.closest(".clearfix").fadeIn("");
				//	}
				//});

				$tabContentAdvance.find("input[type=button]").on("click", function () {
					var form = $tabContentAdvance.find("form");
					if (form.valid()) {
						$tabContentAdvance.loadData(site.profile.settings.urls.editAdvanceInfo, $.fn.serializeParams(form),
							function () {
								site.profile.initContentAdvance();
								$.fn.alertMessage("Обновление профиля", "Дополнительные данные были успешно обновлены.");
								$.fn.initCheckbox();
							},
							function () {
								$.fn.initValidationSummary($tabContentAdvance);
								site.profile.initContentAdvance();
								$.fn.initCheckbox();
							});
					}
				});
			},

			initContentBind: function () {
				var $tabContentBind = $(site.profile.settings.elements.tabContentBind);
				$tabContentBind.find("input[type=button]").on("click", function () {
					var form = $tabContentBind.find("form");
					if (form.valid()) {
						$tabContentBind.loadData(site.profile.settings.urls.editBindInfo, $.fn.serializeParams(form),
							function () {
								site.profile.initContentBind();
								$.fn.alertMessage("Обновление профиля", "Связные данные были успешно обновлены.");
								$.fn.initCheckbox();
							},
							function () {
								$.fn.initValidationSummary($tabContentBind);
								site.profile.initContentBind();
								$.fn.initCheckbox();
							});
					}
				});
			}
		};
})();