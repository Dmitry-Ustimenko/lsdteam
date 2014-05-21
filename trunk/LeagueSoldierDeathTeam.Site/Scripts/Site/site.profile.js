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
					tabContentChangePassword: '#tab-content-change-pass',
					progressbar: '#progressbar',
					progresslabel: '#progresslabel'
				}
			},

			initProfileInfo: {
				init: function () {
					site.profile.initProfileInfo.initUploadContainer();
				},

				initUploadContainer: function () {
					var $clearInput = $(".clear-input");
					var $photoUploadFile = $(site.profile.settings.elements.photoUploadFile);
					var $photoUploadFileName = $(site.profile.settings.elements.photoUploadFileName);

					$clearInput.off("click").on("click", function () {
						$photoUploadFileName.val("");
						$photoUploadFile.val("");
						$clearInput.hide();
						$(site.profile.settings.elements.progressbar).hide();
					});

					$photoUploadFile.on('change', function () {
						$photoUploadFileName.val($(this).val().replace(/\\/g, '/').replace(/.*\//, ''));
						$clearInput.show();
					});

					var $userUploadPhoto = $(".user-upload-photo");
					if ($userUploadPhoto.find(".validation-summary-errors").length)
						$userUploadPhoto.fadeIn();

					$(".edit-icon").on("click", function () {
						if ($userUploadPhoto.is(":hidden"))
							$userUploadPhoto.fadeIn();
						else
							$userUploadPhoto.fadeOut();
					});

					$(".delete-icon").on("click", function () {
						var $this = $(this);
						$.fn.confirmOverlay("Удаление фото", "Вы действительно хотите удалить фото?", function () {
							var form = $this.closest("form");
							form.submit();
						});
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
									$.fn.alertOverlay("Смена пароля", "Пароль был успешно изменен.");
								},
								function () {
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
									$("[data-type=auth-name]").html($tabContentMain.find("[data-type=user-name]").val());
									$.fn.alertOverlay("Обновление профиля", "Основные данные были успешно обновлены.");
								},
								function () {
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
									$.fn.alertOverlay("Обновление профиля", "Дополнительные данные были успешно обновлены.");
									$.fn.initCheckbox();
								},
								function () {
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
									$.fn.alertOverlay("Обновление профиля", "Связные данные были успешно обновлены.");
									$.fn.initCheckbox();
								},
								function () {
									site.profile.initEditInfo.initContentBind();
									$.fn.initCheckbox();
								});
						}
					});
				}
			}
		};
})();