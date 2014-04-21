(function () {
	site.layout =
	{
		settings: {
			urls: {
				login: ''
			},
			vars: {
				form: null
			},
			elements: {
				login: '#login'
			}
		},

		init: function (settings) {
			$.extend(true, site.layout.settings, settings);
			var $loginForm = $(site.layout.settings.elements.login);
			site.layout.initLoginForm($loginForm);
		},

		initLoginForm: function (loginForm) {
			loginForm.find("input[type=button]").on("click", function () {
				var form = loginForm.find("form");
				if (form.valid()) {
					loginForm.loadData(site.layout.settings.urls.login, $.fn.serializeParams(form),
						function (data) {
							var s = data;
						},
						function () {
							$.fn.initValidationSummary(loginForm);
							site.layout.initLoginForm(loginForm);
						});
				}
			});
		}
	};
})();