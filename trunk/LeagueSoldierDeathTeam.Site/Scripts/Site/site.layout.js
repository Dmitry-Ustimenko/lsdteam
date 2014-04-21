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
			site.layout.settings.vars.form = $loginForm.find("form");
			$loginForm.find("input[type=button]").on("click", function () {
				//var form = site.layout.settings.vars.form;
				//$.validator.unobtrusive.parse(site.layout.settings.vars.form);
				//form.unbind("invalid-form.validate", form.validate().settings.invalidHandler);
				//form.validate().settings.invalidHandler = $.proxy(function (event, validator) {
				//	var vs = $dialog.find("div[data-valmsg-summary=true]");
				//	var ul = vs.find("ul");
				//	vs.addClass("validation-summary-errors");
				//	vs.removeClass("validation-summary-valid");
				//	if (ul.length > 0) {
				//		ul.html("");
				//		for (var name in validator.errorList)
				//			ul.append("<li>" + validator.errorList[name].message + "</li>");
				//	}
				//	else {
				//		vs.html("");
				//		vs.append("<ul></ul>");
				//		ul = vs.find("ul");
				//		for (var error in validator.errorList)
				//			ul.append("<li>" + validator.errorList[error].message + "</li>");
				//	}
				//}, form);
				//form.bind("invalid-form.validate", form.validate().settings.invalidHandler);
				if (form.valid()) {
					$loginForm.loadData(site.layout.settings.urls.login, $.fn.serializeParams(form), null);
				}
			});
		}
	};
})();