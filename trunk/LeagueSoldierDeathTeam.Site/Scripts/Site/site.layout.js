(function () {
	site.layout =
	{
		settings: {
			urls: {
				login: '',
				register: '',
				backgroundImages: ''
			},
			vars: {
				backgrounds: null
			},
			elements: {
				login: '#login',
				register: '#register',
				loginTab: '#login-tab',
				registerTab: '#register-tab'
			}
		},

		init: function (settings) {
			$.extend(true, site.layout.settings, settings);
			var $loginForm = $(site.layout.settings.elements.login);
			var $registerForm = $(site.layout.settings.elements.register);

			site.layout.backgroundSlider();
			site.layout.clearSocialHash();
			site.layout.topPage();
			site.layout.fixedMenu();
			site.layout.initTabs($loginForm, $registerForm);
			site.layout.initLoginForm($loginForm);
			site.layout.initRegisterForm($registerForm);
		},

		backgroundSlider: function () {
			$(document).ajaxStart($.unblockUI);
			site.ajax.post(site.layout.settings.urls.backgroundImages, null, function (data) {
				var backgrounds = [];
				var json = shuffle($.parseJSON(data));

				for (var i = 0; i < json.length; i++) {
					if (i == 0)
						backgrounds.push({ src: json[i]["Src"], fade: 500 });
					else
						backgrounds.push({ src: json[i]["Src"], fade: 5000 });
				}
				if (json.length)
					backgrounds.push({ src: json[0]["Src"], fade: 5000 });

				var isStart = true;
				$('body').bind('vegaswalk', function (e, bg, step) {
					if (step == 0 && !isStart)
						$.vegas("next");
					isStart = false;
				});

				$.vegas('slideshow', {
					delay: 10000,
					backgrounds: backgrounds
				})('overlay', {
					src: '/Images/13.png'
				});
			});
			$(document).ajaxStart($.blockUI);

			function shuffle(array) {
				var temp, index;
				var counter = array.length;

				while (counter > 0) {
					index = Math.floor(Math.random() * counter);
					counter--;

					temp = array[counter];
					array[counter] = array[index];
					array[index] = temp;
				}

				return array;
			}
		},

		clearSocialHash: function () {
			if (window.location.hash && window.location.hash === "#_=_") {
				if (Modernizr.history) {
					window.history.pushState("", document.title, window.location.pathname);
				} else {
					var scroll = {
						top: document.body.scrollTop,
						left: document.body.scrollLeft
					};
					window.location.hash = "";
					document.body.scrollTop = scroll.top;
					document.body.scrollLeft = scroll.left;
				}
			}
		},

		topPage: function () {
			var $pageUpArrow = $("div.arrow-page-up");
			$pageUpArrow.on("click", function () {
				$("html, body").animate({ scrollTop: 0 }, "normal");
				return false;
			});

			$(window).scroll(function () {
				if ($(this).scrollTop() == 0)
					$pageUpArrow.fadeOut("normal");
				else
					$pageUpArrow.fadeIn("normal");
			});
		},

		fixedMenu: function () {
			var $sectionHeader = $(".section-header");
			var $sectionMenu = $(".section-menu");

			var fixedHeight = 0;
			var sectionHeaderHeight = $sectionHeader.height() + 4;
			$sectionMenu.css("top", sectionHeaderHeight);

			$(window).scroll(function () {
				var top = $(this).scrollTop();
				if (top + fixedHeight < sectionHeaderHeight)
					$sectionMenu.css('top', (sectionHeaderHeight - top));
				else
					$sectionMenu.css('top', fixedHeight);
			});
		},

		initTabs: function (loginForm, registerForm) {
			var socialArrow = loginForm.find("div.down-arrow");
			var socialList = loginForm.find("div.social-list");

			socialArrow.on("click", function () {
				if (socialList.is(":hidden")) {
					socialList.fadeIn("normal");
				}
				else
					socialList.fadeOut("normal");

				$(this).toggleClass("up-arrow");
			});

			var $loginTab = $(site.layout.settings.elements.loginTab);
			var $registerTab = $(site.layout.settings.elements.registerTab);
			var btnLogin = $loginTab.find("#btn-login");
			var btnRegister = $registerTab.find("#btn-register");

			$(document).click(function (event) {
				var $eventTarget = $(event.target);

				if ($eventTarget.closest(site.layout.settings.elements.loginTab).length) {
					loginForm.fadeIn("fast");
					btnLogin.addClass("active-tab");
				}
				else {
					loginForm.fadeOut("fast");
					btnLogin.removeClass("active-tab");
				}

				if ($eventTarget.closest(site.layout.settings.elements.registerTab).length) {
					registerForm.fadeIn("fast");
					btnRegister.addClass("active-tab");
				}
				else {
					registerForm.fadeOut("fast");
					btnRegister.removeClass("active-tab");
				}

				event.stopPropagation ? event.stopPropagation() : (event.cancelBubble = true);
			});
		},

		initLoginForm: function (loginForm) {
			loginForm.find("input[type=button]").on("click", function () {
				var form = loginForm.find("form");
				if (form.valid()) {
					loginForm.loadData(site.layout.settings.urls.login, $.fn.serializeParams(form), null,
						function () {
							$.fn.initValidationSummary(loginForm);
							site.layout.initLoginForm(loginForm);
						});
				}
			});
		},

		initRegisterForm: function (registerForm) {
			registerForm.find("input[type=button]").on("click", function () {
				var form = registerForm.find("form");
				if (form.valid()) {
					registerForm.loadData(site.layout.settings.urls.register, $.fn.serializeParams(form), null,
						function () {
							$.fn.initValidationSummary(registerForm);
							site.layout.initRegisterForm(registerForm);
						});
				}
			});
		}
	};
})();