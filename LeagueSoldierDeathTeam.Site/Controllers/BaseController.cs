using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.LoggedUser;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Business.LoggedUser;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
using LeagueSoldierDeathTeam.Site.Classes.Factories;
using LeagueSoldierDeathTeam.Site.Models;
using Microsoft.Web.Mvc;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class BaseController : Controller
	{
		#region Private Fields

		private readonly IAccountService _accountService;

		private readonly IAccountProfileService _accountProfileService;

		private readonly IResourceService _resourceService;

		private bool _disposeAppContext;

		private readonly ILoggedUserProvider _loggedUserProvider;

		#endregion

		#region Properties

		protected ServiceFactoryBase ServiceFactory { get; private set; }

		protected IAppContextCustom AppContextCustomCustom { get; private set; }

		protected HttpContextBase HttpContextBase
		{
			get { return ContextFactory.GetHttpContext(); }
		}

		protected UserData CurrentUser
		{
			get { return AppContextCustomCustom.CurrentUser; }
			set { AppContextCustomCustom.CurrentUser = value; }
		}

		protected bool ModelIsValid
		{
			get { return ModelState.IsValid; }
		}

		#endregion

		#region Constructors

		public BaseController(ILoggedUserProvider loggedUserProvider, ServiceFactoryBase serviceFactory)
		{
			if (serviceFactory == null)
				throw new ArgumentNullException("serviceFactory");
			ServiceFactory = serviceFactory;

			if (loggedUserProvider == null)
				throw new ArgumentNullException("loggedUserProvider");
			_loggedUserProvider = loggedUserProvider;

			_accountService = serviceFactory.CreateAccountService();
			_accountProfileService = serviceFactory.CreateAccountProfileService();
			_resourceService = serviceFactory.CreateResourceService();
		}


		#endregion

		#region Common Actions

		#region Comments

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult GetEditCommentContainer(int? id)
		{
			var comment = Execute(() => _resourceService.GetComment(id.GetValueOrDefault())) ?? new CommentData();
			var model = new CommentEditModel { Id = comment.Id, Description = comment.Description };

			return ModelIsValid
				? (ActionResult)View("_CommentEditPartial", model)
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult EditComment(CommentEditModel model)
		{
			if (model.Description.Length < 2)
				return JsonErrorResult("Слишком короткий комментарий");

			var data = new CommentData
			{
				Id = model.Id,
				Description = model.Description
			};

			var comment = Execute(() => _resourceService.SaveComment(data));

			return ModelIsValid
				? (ActionResult)View("_CommentDataItemPartial", new CommentItemModel { CommentItem = comment })
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult IncCommentRate(int? commentId)
		{
			var rate = Execute(() => _resourceService.IncCommentRate(commentId.GetValueOrDefault()));

			return ModelIsValid
				? (ActionResult)View("_CommentRatePartial", new CommentData
				{
					Id = commentId.GetValueOrDefault(),
					Rate = rate,
					IsInc = true
				}) : JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult DecCommentRate(int? commentId)
		{
			var rate = Execute(() => _resourceService.DecCommentRate(commentId.GetValueOrDefault()));

			return ModelIsValid
				? (ActionResult)View("_CommentRatePartial", new CommentData
				{
					Id = commentId.GetValueOrDefault(),
					Rate = rate,
					IsInc = false
				}) : JsonErrorResult();
		}

		#endregion

		#endregion

		#region Public Methods

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

		#endregion

		#region Protected Methods

		protected override void OnAuthorization(AuthorizationContext authorizationContext)
		{
			base.OnAuthorization(authorizationContext);

			AppContextCustomCustom = Classes.AppContextCustom.Current;
			if (AppContextCustomCustom != null)
				return;

			_disposeAppContext = true;
			AppContextCustomCustom = new AppContextCustom();
			Classes.AppContextCustom.Current = AppContextCustomCustom;

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

				_loggedUserProvider.LoggedUser = new LoggedUser { CurrentUserId = CurrentUser.Id };
			}
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			Logger.WriteEmergency(filterContext.Exception);
			base.OnException(filterContext);
		}

		protected void Private(int userId, bool adminAccess = false)
		{
			if (adminAccess && (CurrentUser.IsMainAdmin || CurrentUser.IsAdmin))
				return;

			if (!CurrentUser.IsMe(userId))
				ModelState.AddModelError(string.Empty, "Недостаточно прав");
		}

		protected JsonResult JsonRedirectToAction(string url)
		{
			return Json(new BaseModel { ReturnUrl = url }, JsonRequestBehavior.AllowGet);
		}

		protected JsonResult JsonErrorResult(string message = null)
		{
			var errors = GetModelStateErrors(ModelState);
			if (!string.IsNullOrWhiteSpace(errors))
			{
				message = errors;
			}

			return string.IsNullOrWhiteSpace(message)
				? Json(new { Status = Constants.ErrorStatus }, JsonRequestBehavior.AllowGet)
				: Json(new { Status = Constants.ErrorStatus, Message = message }, JsonRequestBehavior.AllowGet);
		}

		public static string GetModelStateErrors(ModelStateDictionary modelState)
		{
			return string.Join("; ", modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
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
				ModelState.AddModelError(GetParamName(exception.ParamName), GetErrorMessage(exception.Message));
			}
			catch (ArgumentException exception)
			{
				ModelState.AddModelError(GetParamName(exception.ParamName), GetErrorMessage(exception.Message));
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
				ModelState.AddModelError(GetParamName(exception.ParamName), GetErrorMessage(exception.Message));
			}
			catch (ArgumentException exception)
			{
				ModelState.AddModelError(GetParamName(exception.ParamName), GetErrorMessage(exception.Message));
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

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!disposing)
				return;

			if (AppContextCustomCustom == null)
				return;

			if (_disposeAppContext)
				Classes.AppContextCustom.Current = null;
			AppContextCustomCustom = null;
		}

		#endregion

		#region Internel Implementation

		private static string GetParamName(string paramName)
		{
			return !string.IsNullOrWhiteSpace(paramName) ? paramName : "Error";
		}

		private static string GetErrorMessage(string message)
		{
			var paramNameIndex = message.IndexOf("Parameter name:", StringComparison.Ordinal);
			return paramNameIndex > 0 ? message.Substring(0, paramNameIndex) : message;
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

		#endregion
	}
}