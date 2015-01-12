(function () {
	site.layout =
	{
		settings: {
			urls: {
				login: '',
				register: ''
			},
			vars: {
				backgrounds: null
			},
			elements: {
				login: '#login',
				register: '#register',
				loginTab: '#login-tab',
				registerTab: '#register-tab',
				checkbox: 'input[type=checkbox]',
			}
		},

		init: function (settings) {
			$.extend(true, site.layout.settings, settings);
			var $loginForm = $(site.layout.settings.elements.login);
			var $registerForm = $(site.layout.settings.elements.register);

			$.fn.initCheckbox();
			$.fn.checkCommentHash();
			//site.layout.backgroundSlider();
			site.layout.clearSocialHash();
			site.layout.topPage();
			site.layout.fixedMenu();
			site.layout.fixedProfile();
			site.layout.initProfile();
			site.layout.initLoginTab($loginForm);
			site.layout.initRegisterTab($registerForm);
			site.layout.initLoginForm($loginForm);
			site.layout.initRegisterForm($registerForm);
		},

		initErrorPage: function (settings) {
			$.extend(true, site.layout.settings, settings);

			//site.layout.backgroundSlider();
		},

		backgroundSlider: function () {
			var backgrounds = [];
			var json = shuffle(site.layout.settings.vars.backgrounds);

			for (var i = 0; i < json.length; i++) {
				if (i == 0)
					backgrounds.push({ src: json[i]["Src"], fade: 500 });
				else
					backgrounds.push({ src: json[i]["Src"], fade: 5000 });
			}

			var isStart = true;
			$('body').bind('vegaswalk', function (e, bg, step) {
				if (step == 0 && !isStart)
					$.vegas("next");
				isStart = false;
			});

			$.vegas('slideshow', {
				delay: 15000,
				backgrounds: backgrounds
			})('overlay', {
				src: '/Images/13.png'
			});

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

		fixedProfile: function () {
			var defaultTop = 35;

			var $profileMini = $(".profile-info-mini");
			$profileMini.css('top', defaultTop);

			$(window).scroll(function () {
				var scrollTop = $(this).scrollTop();
				$profileMini.css('top', scrollTop + defaultTop);
			});
		},

		initProfile: function () {
			var $profileMini = $(".profile-mini");
			var $profileInfoMini = $(".profile-info-mini");

			var cookieName = "ProfileStatus";
			var cookieOptions = { expires: 365, path: "/" };

			if ($.fn.cookieGet(cookieName) == undefined)
				$.fn.cookieSet(cookieName, true, cookieOptions);

			if ($.fn.cookieGet(cookieName) == "true") {
				$profileMini.find("span").addClass("hide-profile-mini");
				$profileInfoMini.show();
			} else {
				$profileMini.find("span").addClass("open-profile-mini");
				$profileInfoMini.hide();
			}

			$profileMini.off("click").on("click", function () {
				if ($profileInfoMini.is(":hidden")) {
					$(this).find("span").removeClass("open-profile-mini").addClass("hide-profile-mini");
					$profileInfoMini.fadeIn("normal");
					$.fn.cookieSet(cookieName, true, cookieOptions);
				} else {
					$(this).find("span").removeClass("hide-profile-mini").addClass("open-profile-mini");
					$profileInfoMini.fadeOut("normal");
					$.fn.cookieSet(cookieName, false, cookieOptions);
				}
			});

			$profileInfoMini.find(".hide-profile").off("click").on("click", function () {
				$profileMini.find("span").removeClass("hide-profile-mini").addClass("open-profile-mini");
				$profileInfoMini.fadeOut("normal");
				$.fn.cookieSet(cookieName, false, cookieOptions);
			});
		},

		initLoginTab: function (loginForm) {
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

			var btnLogin = $(site.layout.settings.elements.loginTab).find("#btn-login");
			$(document).click(function (event) {
				var $eventTarget = $(event.target);
				site.layout.enterPressFormEvent(loginForm.find("form"));

				if ($eventTarget.closest(site.layout.settings.elements.loginTab).length) {
					loginForm.fadeIn("fast");
					btnLogin.addClass("active-tab");
				}
				else {
					loginForm.fadeOut("fast");
					btnLogin.removeClass("active-tab");
				}

				event.stopPropagation ? event.stopPropagation() : (event.cancelBubble = true);
			});
		},

		initRegisterTab: function (registerForm) {
			var btnRegister = $(site.layout.settings.elements.registerTab).find("#btn-register");
			$(document).click(function (event) {
				var $eventTarget = $(event.target);
				site.layout.enterPressFormEvent(registerForm.find("form"));

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

		enterPressFormEvent: function (form) {
			var btn = form.find("input[type=button]");
			form.off("keypress").keypress(function (e) {
				var code = e.keyCode || e.which;
				if (code === 13) {
					e.preventDefault();
					btn.click();
				}
			});
		},

		initLoginForm: function (loginForm) {
			loginForm.find("input[type=button]").on("click", function () {
				var form = loginForm.find("form");
				if (form.valid()) {
					loginForm.loadData(site.layout.settings.urls.login, $.fn.serializeParams(form), function () {
						window.location.href = "/";
					}, function () {
						site.layout.initLoginForm(loginForm);
						site.layout.initLoginTab(loginForm);

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
							site.layout.initRegisterForm(registerForm);
							site.layout.initRegisterTab(registerForm);
						});
				}
			});
		}
	};
})();