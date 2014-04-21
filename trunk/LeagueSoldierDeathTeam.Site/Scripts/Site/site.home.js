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
					slider: '#galleria',
				}
			},

			init: function () {
				$.fn.slider(site.home.settings.attributes.slider);
			},
		};
})();