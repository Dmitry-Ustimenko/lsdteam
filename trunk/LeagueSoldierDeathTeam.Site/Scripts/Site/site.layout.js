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
		}
	};
})();