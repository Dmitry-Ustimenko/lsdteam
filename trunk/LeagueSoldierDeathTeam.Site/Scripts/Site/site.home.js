(function () {
	site.home =
		{
			settings: {
				urls: {
				},
				vars: {
				},
				elements: {
					slider: '#galleria',
					calendar: '#calendar'
				}
			},

			init: function () {
				$.fn.slider(site.home.settings.elements.slider);

				$(site.home.settings.elements.calendar).datepicker({
					language: "ru",
					weekStart: 1,
					todayHighlight: true,
					activateSwitch: false
				});
			},
		};
})();