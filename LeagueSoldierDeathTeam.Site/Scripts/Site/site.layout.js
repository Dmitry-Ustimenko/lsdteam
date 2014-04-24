(function () {
	site.layout =
	{
		settings: {
			urls: {
				login: '',
				register: ''
			},
			vars: {
			},
			elements: {
				login: '#login',
				register: '#register',
				loginTab: '#login-tab',
				registerTab: '#register-tab'
			}
		},

		init: function (settings) {
			$.extend(true, site.layout.settings, settings);
			var $loginForm = $(site.layout.settings.elements.login);
			var $registerForm = $(site.layout.settings.elements.register);

			site.layout.initTabs($loginForm, $registerForm);
			site.layout.initLoginForm($loginForm);
			site.layout.initRegisterForm($registerForm);
		},

		initTabs: function (loginForm, registerForm) {
			var socialArrow = loginForm.find("div.down-arrow");
			var socialList = loginForm.find("div.social-list");

			socialArrow.on("click", function () {
				if (socialList.is(":hidden")) {
					socialList.fadeIn("normal");
				}
				else
					socialList.fadeOut("normal");

				$(this).toggleClass("up-arrow");
			});

			var $loginTab = $(site.layout.settings.elements.loginTab);
			var $registerTab = $(site.layout.settings.elements.registerTab);
			var btnLogin = $loginTab.find("#btn-login");
			var btnRegister = $registerTab.find("#btn-register");

			$(document).click(function (event) {
				var $eventTarget = $(event.target);

				if ($eventTarget.closest(site.layout.settings.elements.loginTab).length) {
					loginForm.fadeIn("fast");
					btnLogin.addClass("active-tab");
				}
				else {
					loginForm.fadeOut("fast");
					btnLogin.removeClass("active-tab");
				}

				if ($eventTarget.closest(site.layout.settings.elements.registerTab).length) {
					registerForm.fadeIn("fast");
					btnRegister.addClass("active-tab");
				}
				else {
					registerForm.fadeOut("fast");
					btnRegister.removeClass("active-tab");
				}

				event.stopPropagation ? event.stopPropagation() : (event.cancelBubble = true);
			});
		},

		initLoginForm: function (loginForm) {
			loginForm.find("input[type=button]").on("click", function () {
				var form = loginForm.find("form");
				if (form.valid()) {
					loginForm.loadData(site.layout.settings.urls.login, $.fn.serializeParams(form), null,
						function () {
							$.fn.initValidationSummary(loginForm);
							site.layout.initLoginForm(loginForm);
						});
				}
			});
		},

		initRegisterForm: function (registerForm) {
			registerForm.find("input[type=button]").on("click", function () {
				var form = registerForm.find("form");
				if (form.valid()) {
					registerForm.loadData(site.layout.settings.urls.register, $.fn.serializeParams(form), null,
						function () {
							$.fn.initValidationSummary(registerForm);
							site.layout.initRegisterForm(registerForm);
						});
				}
			});
		}
	};
})();