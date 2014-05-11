﻿(function () {
	site.layout =
	{
		settings: {
			urls: {
				login: '',
				register: ''
			},
			vars: {
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
			$.vegas('slideshow', {
				delay: 10000,
				backgrounds: [
					{ src: '/Images/Background/bf1.jpg', fade: 2000 },
					{ src: '/Images/Background/bf2.jpg', fade: 5000 },
					{ src: '/Images/Background/bf3.jpg', fade: 5000 },
					{ src: '/Images/Background/bf4.jpg', fade: 5000 }
				]
			})('overlay', {
				src: '/Images/13.png'
			});
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