using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Factories;
using LeagueSoldierDeathTeam.Site.Models;
using Microsoft.Web.Mvc;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class BaseController : Controller
	{
		protected IAppContext AppContext { get; private set; }

		protected HttpContextBase HttpContextBase
		{
			get
			{
				return ContextFactory.GetHttpContext();
			}
		}

		protected ServiceFactoryBase ServiceFactory { get; private set; }

		private readonly IAccountService _accountService;

		private bool _disposeAppContext;

		public BaseController(ServiceFactoryBase serviceFactory)
		{
			if (serviceFactory == null)
				throw new ArgumentNullException("serviceFactory");
			ServiceFactory = serviceFactory;

			_accountService = serviceFactory.CreateAccountService();
		}

		public JsonResult GetBackgroundFiles()
		{
			var backgrounds = new List<BackgroundModel>();
			var path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, Constants.BackgroundDirectoryPath);

			if (Directory.Exists(path))
			{
				var images = Directory.GetFiles(path, "*.jpg");
				backgrounds.AddRange(images.Select(image => new BackgroundModel
				{
					Src = string.Concat(Constants.BackgroundDirectoryPath, image.Substring(image.LastIndexOf('\\')))
				}));
			}

			return Json(backgrounds, JsonRequestBehavior.AllowGet);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!disposing)
				return;

			if (AppContext == null)
				return;

			if (_disposeAppContext)
				Classes.AppContext.Current = null;
			AppContext = null;
		}

		protected override void OnAuthorization(AuthorizationContext authorizationContext)
		{
			base.OnAuthorization(authorizationContext);

			AppContext = Classes.AppContext.Current;
			if (AppContext != null)
				return;

			_disposeAppContext = true;
			AppContext = new AppContext();
			Classes.AppContext.Current = AppContext;

			if (AppContext.CurrentUser != null)
				Execute(() => _accountService.UpdateLastActivity(AppContext.CurrentUser.Id));

			if (AppContext.CurrentUser == null && authorizationContext.HttpContext.User != null && authorizationContext.HttpContext.Request.IsAuthenticated)
			{
				if (SessionManager.Get<UserData>(SessionKeys.User) != null)
					AppContext.CurrentUser = SessionManager.Get<UserData>(SessionKeys.User);
				else
				{
					var userIdentity = authorizationContext.HttpContext.User.Identity;
					Execute(() => AppContext.CurrentUser = _accountService.GetUser(userIdentity.Name));
				}

				if (AppContext.CurrentUser != null)
					SessionManager.Set(SessionKeys.User, AppContext.CurrentUser);
			}
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			Logger.WriteEmergency(filterContext.Exception);
			base.OnException(filterContext);
		}

		protected bool Execute(Action action)
		{
			try
			{
				action();
				return true;
			}
			catch (ArgumentNullException exception)
			{
				ModelState.AddModelError("Error", exception.Message);
			}
			catch (ArgumentException exception)
			{
				ModelState.AddModelError("Error", exception.Message);
			}
			catch (EntityCommandExecutionException exception)
			{
				AnalyzeSqlException(exception);
			}
			catch (UpdateException exception)
			{
				AnalyzeSqlException(exception);
			}
			catch (Exception exception)
			{
				Logger.WriteEmergency(exception);
				ModelState.AddModelError("Error", "Произошла внутренняя ошибка.");
			}
			return false;
		}

		protected T Execute<T>(Func<T> func)
		{
			try
			{
				return func();
			}
			catch (ArgumentNullException exception)
			{
				ModelState.AddModelError("Error", exception.Message);
			}
			catch (ArgumentException exception)
			{
				ModelState.AddModelError("Error", exception.Message);
			}
			catch (EntityCommandExecutionException exception)
			{
				AnalyzeSqlException(exception);
			}
			catch (UpdateException exception)
			{
				AnalyzeSqlException(exception);
			}
			catch (Exception exception)
			{
				Logger.WriteEmergency(exception);
				ModelState.AddModelError("Error", "Произошла внутренняя ошибка.");
			}
			return default(T);
		}

		protected bool ModelIsValid { get { return ModelState.IsValid; } }

		public static RedirectToRouteResult RedirectToAction<T>(Expression<Action<T>> action, RouteValueDictionary values = null)
			where T : Controller
		{
			var body = action.Body as MethodCallExpression;
			if (body == null)
				throw new ArgumentException("Expression must be a method call.");
			if (body.Object != action.Parameters[0])
				throw new ArgumentException("Method call must target lambda argument.");

			var actionName = body.Method.Name;
			var attributes = body.Method.GetCustomAttributes(typeof(ActionNameAttribute), false);
			if (attributes.Length > 0)
			{
				var actionNameAttr = (ActionNameAttribute)attributes[0];
				actionName = actionNameAttr.Name;
			}

			var controllerName = typeof(T).Name;
			if (controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
				controllerName = controllerName.Remove(controllerName.Length - 10, 10);

			var defaults = LinkBuilder.BuildParameterValuesFromExpression(body) ?? new RouteValueDictionary();

			values = values ?? new RouteValueDictionary();
			values.Add("controller", controllerName);
			values.Add("action", actionName);

			foreach (var pair in defaults.Where(p => p.Value != null))
				values.Add(pair.Key, pair.Value);

			return new RedirectToRouteResult(values);
		}

		private void AnalyzeSqlException(Exception exception)
		{
			var innerException = exception.InnerException as SqlException;
			if (innerException != null && innerException.Number == 50000)
				ModelState.AddModelError("Error", innerException.Errors.Count > 0 ? innerException.Errors[0].Message : innerException.Message);
			else
			{
				Logger.WriteEmergency(innerException);
				ModelState.AddModelError("Error", "Произошла внутренняя ошибка.");
			}
		}
	}
}