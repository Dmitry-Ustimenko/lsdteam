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

			initProfileInfo: {
				init: function () {
					$(site.profile.settings.elements.photoUploadFile).on('change', function () {
						$(site.profile.settings.elements.photoUploadFileName).val($(this).val().replace(/\\/g, '/').replace(/.*\//, ''));
					});

					site.profile.initProfileInfo.initUploadContainer();
				},

				initUploadContainer: function () {
					$(".edit-icon").on("click", function () {
						var $userUploadPhoto = $(".user-upload-photo");

						if ($userUploadPhoto.is(":hidden"))
							$userUploadPhoto.fadeIn();
						else
							$userUploadPhoto.fadeOut();
					});

					$(".delete-icon").on("click", function () {
						var answer = confirm("Вы действительно хотите удалить фото?");
						if (answer) {
							var form = $(this).closest("form");
							form.submit();
						}
					});
				}
			},

			initEditInfo: {
				init: function (settings) {
					$.extend(true, site.profile.settings, settings);

					site.profile.initEditInfo.initChangePassword();
					site.profile.initEditInfo.initContentMain();
					site.profile.initEditInfo.initContentAdvance();
					site.profile.initEditInfo.initContentBind();
				},

				initChangePassword: function () {
					var $tabContentChangePassword = $(site.profile.settings.elements.tabContentChangePassword);
					$tabContentChangePassword.find("input[type=button]").on("click", function () {
						var form = $tabContentChangePassword.find("form");
						if (form.valid()) {
							$tabContentChangePassword.loadData(site.profile.settings.urls.changePassword, $.fn.serializeParams(form),
								function () {
									site.profile.initEditInfo.initChangePassword();
									$.fn.initCheckbox();
									$.fn.alertMessage("Смена пароля", "Пароль был успешно изменен.");
								},
								function () {
									$.fn.initValidationSummary($tabContentChangePassword);
									site.profile.initEditInfo.initChangePassword();
									$.fn.initCheckbox();
								});
						}
					});
				},

				initContentMain: function () {
					var $tabContentMain = $(site.profile.settings.elements.tabContentMain);
					$tabContentMain.find("input[type=button]").on("click", function () {
						var form = $tabContentMain.find("form");
						if (form.valid()) {
							$tabContentMain.loadData(site.profile.settings.urls.editMainInfo, $.fn.serializeParams(form),
								function () {
									site.profile.initEditInfo.initContentMain();
									$.fn.initCheckbox();
									$.fn.alertMessage("Обновление профиля", "Основные данные были успешно обновлены.");
								},
								function () {
									$.fn.initValidationSummary($tabContentMain);
									site.profile.initEditInfo.initContentMain();
									$.fn.initCheckbox();
								});
						}
					});
				},

				initContentAdvance: function () {
					var $tabContentAdvance = $(site.profile.settings.elements.tabContentAdvance);
					$tabContentAdvance.applyDatepicker();

					$tabContentAdvance.find("input[data-type]").each(function () {
						var $this = $(this);
						var child = $tabContentAdvance.find("p[data-parent=" + $this.data("type") + "]");
						var removeChilds = $tabContentAdvance.find("p[data-remove*=" + $this.data("type") + "]");

						if ($this.val() == "")
							removeChilds.hide();
						else
							child.show();
					});

					$tabContentAdvance.find("input[data-type]").keyup(function () {
						var $this = $(this);
						var child = $tabContentAdvance.find("p[data-parent=" + $this.data("type") + "]");
						var removeChilds = $tabContentAdvance.find("p[data-remove*=" + $this.data("type") + "]");

						if ($this.val() == "") {
							removeChilds.each(function () {
								$(this).fadeOut("fast");
								$(this).find("input[type=text]").val("");
							});

						} else {
							child.fadeIn();
						}
					});

					$tabContentAdvance.find("input[type=button]").on("click", function () {
						var form = $tabContentAdvance.find("form");
						if (form.valid()) {
							$tabContentAdvance.loadData(site.profile.settings.urls.editAdvanceInfo, $.fn.serializeParams(form),
								function () {
									site.profile.initEditInfo.initContentAdvance();
									$.fn.alertMessage("Обновление профиля", "Дополнительные данные были успешно обновлены.");
									$.fn.initCheckbox();
								},
								function () {
									$.fn.initValidationSummary($tabContentAdvance);
									site.profile.initEditInfo.initContentAdvance();
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
									site.profile.initEditInfo.initContentBind();
									$.fn.alertMessage("Обновление профиля", "Связные данные были успешно обновлены.");
									$.fn.initCheckbox();
								},
								function () {
									$.fn.initValidationSummary($tabContentBind);
									site.profile.initEditInfo.initContentBind();
									$.fn.initCheckbox();
								});
						}
					});
				}
			}
		};
})();