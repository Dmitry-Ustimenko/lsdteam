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