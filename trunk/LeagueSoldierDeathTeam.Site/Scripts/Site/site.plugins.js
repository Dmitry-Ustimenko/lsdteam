(function ($) {
	$.fn.serializeParams = function (form) {
		return $(form).serializeArray();
	};
})(jQuery);

(function ($) {
	$.fn.load = function (url, dataParam, callbackParam) {
		var target = this;
		site.ajax.post(url, dataParam, function (data) {
			$(target).html(data);
			var form = target.find("form");
			if (form != undefined) {
				$(form).removeData("validator");
				$(form).removeData("unobtrusiveValidation");
				$.validator.unobtrusive.parse($(form));
			}
			if ($(data).find("div.validation-summary-errors").length > 0)
				return;
			if (typeof (callbackParam) == 'function')
				callbackParam(data);
		});
	};
})(jQuery);

(function ($) {
	$.fn.slider = function (sliderId) {
		var slideshowTransitions = [{
			$Duration: 250,
			$Delay: 50,
			$Cols: 7,
			$Rows: 5,
			$Formation: $JssorSlideshowFormations$.$FormationRandom,
			$Assembly: 250,
			$Opacity: 2
		}];

		return new $JssorSlider$(sliderId, {
			$AutoPlay: true,
			$AutoPlayInterval: 4000,
			$SlideDuration: 1000,
			$DragOrientation: 1,

			$ArrowNavigatorOptions: {
				$Class: $JssorArrowNavigator$,
				$ChanceToShow: 2,
				$AutoCenter: 2
			},

			$ThumbnailNavigatorOptions: {
				$Class: $JssorThumbnailNavigator$,
				$ChanceToShow: 2,

				$SpacingX: 10,
				$SpacingY: 3,
				$DisplayPieces: 4,

				//$ArrowNavigatorOptions: {
				//	$Class: $JssorArrowNavigator$,
				//	$ChanceToShow: 2,
				//	$AutoCenter: 2
				//}
			},

			$SlideshowOptions: {
				$Class: $JssorSlideshowRunner$,
				$Transitions: slideshowTransitions,
				$TransitionsOrder: 0,
				$ShowLink: true
			}
		});
	};
})(jQuery);