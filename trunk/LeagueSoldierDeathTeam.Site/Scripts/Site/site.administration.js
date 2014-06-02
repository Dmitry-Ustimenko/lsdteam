(function () {
	site.administation =
		{
			settings: {
				urls: {
				},
				elements: {
				}
			},

			roleManagement: {
				init: function (settings) {
					$.extend(true, site.administation.settings, settings);

					$("#tabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
				},
			}
		};
})();