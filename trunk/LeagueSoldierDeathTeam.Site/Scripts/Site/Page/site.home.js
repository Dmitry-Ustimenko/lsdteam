(function () {
	site.home =
		{
			settings: {
				urls: {
					refreshNews: ''
				},
				vars: {
				},
				elements: {
					slider: '#galleria',
					calendar: '#calendar',
					clock: '#clock',
					newsContent: '#news-content'
				}
			},

			init: function (settings) {
				$.extend(true, site.home.settings, settings);

				$.fn.slider(site.home.settings.elements.slider);
				$.fn.clock(site.home.settings.elements.clock);

				$(site.home.settings.elements.calendar).datepicker({
					language: "ru",
					weekStart: 1,
					todayHighlight: true,
					activateSwitch: false
				});

				site.home.initNewsFilter();
			},

			initNewsFilter: function () {
				$("[data-type=news-sort]").each(function () {
					var $this = $(this);

					$this.off("click").on("click", function () {
						site.home.refresh(site.home.settings.elements.newsContent, site.home.settings.urls.refreshNews, { newsSortId: $this.data("id") },
							function () {
								$("[data-type=news-sort].news-active").removeClass("news-active");
								$this.addClass("news-active");

								$.fn.initTooltip();
							});
					});
				});
			},

			refresh: function (content, url, params, callback, callbackError) {
				$(content).loadData(url, params, callback, callbackError);
			}
		};
})();