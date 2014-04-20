(function () {
	site.home =
		{
			settings: {
				urls: {
				},
				vars: {
				},
				elements: {
				},
				attributes: {
					sliderId: 'slider',
				}
			},

			init: function () {
				$.fn.slider(site.home.settings.attributes.sliderId);
			},
		};
})();