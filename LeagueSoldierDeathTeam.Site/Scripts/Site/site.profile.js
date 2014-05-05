(function () {
	site.profile =
		{
			settings: {
				urls: {
				},
				vars: {
				},
				elements: {
					photoUploadFile: '#PhotoUploadFile',
					photoUploadFileName: '#PhotoUploadFileName',
					tabContentMain: '#tab-content-main',
					tabContentAdvance: '#tab-content-advance',
					tabContentBind: '#tab-content-bind',
					tabContentChangePass: 'tab-content-change-pass'
				}
			},

			init: function () {
				$(site.profile.settings.elements.photoUploadFile).on('change', function () {
					$(site.profile.settings.elements.photoUploadFileName).val($(this).val().replace(/\\/g, '/').replace(/.*\//, ''));
				});

				$(site.profile.settings.elements.tabContentAdvance).applyDatepicker();
			},
		};
})();