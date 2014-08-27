using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
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

		private readonly IAccountProfileService _accountProfileService;

		private bool _disposeAppContext;

		protected UserData CurrentUser
		{
			get { return AppContext.CurrentUser; }
			set { AppContext.CurrentUser = value; }
		}

		public BaseController(ServiceFactoryBase serviceFactory)
		{
			if (serviceFactory == null)
				throw new ArgumentNullException("serviceFactory");
			ServiceFactory = serviceFactory;

			_accountService = serviceFactory.CreateAccountService();
			_accountProfileService = serviceFactory.CreateAccountProfileService();
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

			if (CurrentUser == null && authorizationContext.HttpContext.User != null && authorizationContext.HttpContext.Request.IsAuthenticated)
			{
				if (SessionManager.Get<UserData>(SessionKeys.User) != null)
					CurrentUser = SessionManager.Get<UserData>(SessionKeys.User);
				else
				{
					var userIdentity = authorizationContext.HttpContext.User.Identity;
					Execute(() => CurrentUser = _accountService.GetUser(userIdentity.Name));
				}
			}

			if (CurrentUser != null)
			{
				Execute(() => _accountService.UpdateLastActivity(CurrentUser.Id));
				Execute(() => CurrentUser.InboxMessageCount = _accountProfileService.GetUserMessageCount(CurrentUser.Id));
			}
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			Logger.WriteEmergency(filterContext.Exception);
			base.OnException(filterContext);
		}

		protected JsonResult JsonResult(string message = null)
		{
			return string.IsNullOrWhiteSpace(message)
				? Json(new { Status = Constants.ErrorStatus }, JsonRequestBehavior.AllowGet)
				: Json(new { Status = Constants.ErrorStatus, Message = message }, JsonRequestBehavior.AllowGet);
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
				ModelState.AddModelError("Error", Constants.ErrorMessage);
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
				ModelState.AddModelError("Error", Constants.ErrorMessage);
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

		protected void Private(int userId, bool adminAccess = false)
		{
			if (adminAccess && (CurrentUser.IsMainAdmin || CurrentUser.IsAdmin))
				return;

			if (!CurrentUser.IsMe(userId))
				ModelState.AddModelError(string.Empty, "Недостаточно прав");
		}

		private void AnalyzeSqlException(Exception exception)
		{
			var innerException = exception.InnerException as SqlException;
			if (innerException != null && innerException.Number == 50000)
				ModelState.AddModelError("Error", innerException.Errors.Count > 0 ? innerException.Errors[0].Message : innerException.Message);
			else
			{
				Logger.WriteEmergency(innerException);
				ModelState.AddModelError("Error", Constants.ErrorMessage);
			}
		}
	}
}