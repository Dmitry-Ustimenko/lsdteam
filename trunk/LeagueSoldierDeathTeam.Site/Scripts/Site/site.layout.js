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
				register: '#register'
			}
		},

		init: function (settings) {
			$.extend(true, site.layout.settings, settings);
			var $loginForm = $(site.layout.settings.elements.login);
			var $registerForm = $(site.layout.settings.elements.register);

			site.layout.initLoginForm($loginForm);
			site.layout.initRegisterForm($registerForm);
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