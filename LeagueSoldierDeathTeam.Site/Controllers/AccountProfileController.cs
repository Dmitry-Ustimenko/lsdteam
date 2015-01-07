using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Business.Classes.Extensions;
using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
using LeagueSoldierDeathTeam.Site.Classes.Extensions;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Models.AccountProfile;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[Authorize]
	public class AccountProfileController : BaseController
	{
		#region Private Fields

		private readonly IAccountService _accountService;

		private readonly IAccountProfileService _accountProfileService;

		#endregion

		#region Constructors

		public AccountProfileController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{
			_accountService = serviceFactory.CreateAccountService();
			_accountProfileService = serviceFactory.CreateAccountProfileService();
		}

		#endregion

		#region Actions

		#region Profile Info

		[HttpGet]
		[AllowAnonymous]
		[Route("user-profile-info/{userId:int}")]
		public ActionResult ProfileInfo(int userId)
		{
			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);

			if (ModelIsValid)
				return View(model);
			return RedirectToAction<HomeController>(o => o.Index());
		}

		[HttpGet]
		public ActionResult EditPhoto()
		{
			return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(CurrentUser.Id));
		}

		[HttpPost]
		public ActionResult EditPhoto(int userId, HttpPostedFileBase photoUploadFile)
		{
			Private(userId, true);

			if (ModelIsValid)
			{
				if (photoUploadFile != null && photoUploadFile.FileName != null)
				{
					try
					{
						if (photoUploadFile.ContentLength > 102400)
							ModelState.AddModelError(string.Empty, "Размер файла не выше 100кб");

						if (!Constants.AcceptImage.Contains(photoUploadFile.ContentType))
							ModelState.AddModelError(string.Empty, "Требуемые форматы: *.jpg, *.jpeg, *.png, *.gif");

						if (ModelIsValid)
						{
							var image = Image.FromStream(photoUploadFile.InputStream);
							if (image.Width > 200 || image.Height > 200)
								ModelState.AddModelError(string.Empty, "Разрешение фото не должно превышать 200х200");
						}

						if (ModelIsValid)
						{
							var user = Execute(() => _accountService.GetUser(userId));
							var oldPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, user.PhotoPath);

							var fileName = string.Concat(StringGeneration.Generate(20), Path.GetExtension(photoUploadFile.FileName));
							var path = Path.Combine(Server.MapPath(Constants.PhotoDirectoryPath), Path.GetFileName(fileName));
							while (System.IO.File.Exists(path))
								fileName = string.Concat(StringGeneration.Generate(20), Path.GetExtension(photoUploadFile.FileName));

							photoUploadFile.SaveAs(path);

							Execute(() => _accountService.UpdateUserPhoto(userId, string.Concat(Constants.PhotoDirectoryPath, fileName)));

							if (CurrentUser.IsMe(userId))
								Execute(() => CurrentUser = _accountService.GetUser(userId));

							if (ModelIsValid)
							{
								if (System.IO.File.Exists(oldPath))
									System.IO.File.Delete(oldPath);

								return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(userId));
							}
						}
					}
					catch (Exception)
					{
						ModelState.AddModelError(string.Empty, "Ошибка при сохранении файла");
					}
				}
				else
					ModelState.AddModelError(string.Empty, "Файл не выбран");
			}

			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);
			return View("ProfileInfo", model);
		}

		[HttpGet]
		public ActionResult DeletePhoto()
		{
			return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(CurrentUser.Id));
		}

		[HttpPost]
		public ActionResult DeletePhoto(int userId)
		{
			Private(userId, true);

			if (ModelIsValid)
			{
				var user = Execute(() => _accountService.GetUser(userId));
				var path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, user.PhotoPath);

				Execute(() => _accountService.DeleteUserPhoto(userId));

				if (CurrentUser.IsMe(userId))
					Execute(() => CurrentUser = _accountService.GetUser(userId));

				if (ModelIsValid && System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);
					return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(userId));
				}
			}

			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);
			return View("ProfileInfo", model);
		}

		#endregion

		#region Edit Profile

		[HttpGet]
		[Route("edit-user-profile/{userId:int}")]
		public ActionResult EditProfile(int userId)
		{
			var model = new EditProfileModel { UserId = userId };
			FillEditProfileModel(model);

			if (ModelIsValid)
				return View(model);
			return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(CurrentUser.Id));
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult EditMainInfo(EditMainInfoModel model)
		{
			if (ModelIsValid)
			{
				var data = new UserInfoData
				{
					UserId = model.UserId,
					UserName = model.UpdateUserName,
					UserEmail = model.UpdateUserEmail,
					FirstName = model.FirstName,
					LastName = model.LastName,
					SexId = model.SexId,
					ShowUserEmail = model.ShowEmail
				};

				Execute(() => _accountService.UpdateMainInfo(data));

				if (CurrentUser.IsMe(model.UserId))
					Execute(() => CurrentUser = _accountService.GetUser(model.UserId));
			}
			return View("_EditMainInfoPartial", model);
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult EditAdvanceInfo(EditAdvanceInfoModel model)
		{
			if (ModelIsValid)
			{
				var dateBirth = default(DateTime?);
				if (!string.IsNullOrWhiteSpace(model.DateBirth))
				{
					DateTime date;
					if (DateTime.TryParse(model.DateBirth, CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
						dateBirth = date;
					else
						ModelState.AddModelError(string.Empty, "Неверный формат даты.");
				}

				if (ModelIsValid)
				{
					var data = new UserInfoData
					{
						UserId = model.UserId,
						AboutMe = model.AboutMe,
						Activity = model.Activity,
						DateBirth = dateBirth,
						Country = model.Country,
						Town = model.Town,
						Street = model.Street,
						HomeNumber = model.HomeNum
					};

					Execute(() => _accountService.UpdateAdvanceInfo(data));
				}
			}

			return View("_EditAdvanceInfoPartial", model);
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult EditBindInfo(EditBindInfoModel model)
		{
			if (ModelIsValid)
			{
				var data = new UserInfoData
				{
					UserId = model.UserId,
					SiteLink = model.SiteLink,
					Icq = model.Icq,
					Skype = model.Skype,
					BattleLog = model.BattleLog,
					Steam = model.Steam
				};

				Execute(() => _accountService.UpdateBindInfo(data));
			}

			return View("_EditBindInfoPartial", model);
		}

		#endregion

		#region Change Password

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelIsValid)
				Execute(() => _accountService.ChangePassword(model.OldPassword, model.NewPassword, model.UserId));
			return View("_ChangePasswordPartial", model);
		}

		#endregion

		#region Message

		[HttpGet]
		[Route("messages")]
		public ActionResult Messages()
		{
			if (!Request.QueryString.AllKeys.Contains("type"))
				return View(new UserMessagesModel());

			MessageTypeEnum messageType;
			return View(Enum.TryParse(Request.QueryString["type"], out messageType)
				? new UserMessagesModel { MessageTypeId = (int)messageType }
				: new UserMessagesModel());
		}

		[AjaxOrChildActionOnly]
		public ActionResult MessagesData(UserMessagesModel model)
		{
			FillUserMessagesModel(model);
			return View(model);
		}

		[HttpGet]
		[Route("view-message/{id:int?}")]
		public ActionResult ViewMessage(int? id)
		{
			if (id.HasValue)
			{
				var model = Execute(() => _accountProfileService.GetUserMessage(CurrentUser.Id, id.GetValueOrDefault()));

				if (model == null)
					return RedirectToAction<AccountProfileController>(o => o.Messages());

				if (model.Type == MessageTypeEnum.Inbox && !model.IsRead)
				{
					Execute(() => _accountProfileService.SaveAsRead(CurrentUser.Id, new[] { id.GetValueOrDefault() }));
					Execute(() => CurrentUser.InboxMessageCount = _accountProfileService.GetUserMessageCount(CurrentUser.Id));
				}

				return View(model);
			}

			return RedirectToAction<AccountProfileController>(o => o.Messages());
		}

		[HttpGet]
		[Route("create-message")]
		public ActionResult CreateMessage()
		{
			return View("EditMessage", Request.QueryString.AllKeys.Contains("name")
				? new UserMessageModel { RecipientName = Request.QueryString["name"] }
				: new UserMessageModel());
		}

		[HttpGet]
		[Route("edit-message/{id:int?}")]
		public ActionResult EditMessage(int? id)
		{
			if (id.HasValue)
			{
				var model = new UserMessageModel { MessageId = id.Value };

				if (FillUserMessageModel(model, false))
					return View(model);
				return RedirectToAction<AccountProfileController>(o => o.CreateMessage());
			}

			return RedirectToAction<AccountProfileController>(o => o.CreateMessage());
		}

		[HttpGet]
		[Route("reply-message/{id:int?}")]
		public ActionResult ReplyMessage(int? id)
		{
			if (id.HasValue)
			{
				var model = new UserMessageModel { MessageId = id.Value };

				if (FillUserMessageModel(model, false))
					return View("EditMessage", model);
				return RedirectToAction<AccountProfileController>(o => o.CreateMessage());
			}

			return RedirectToAction<AccountProfileController>(o => o.CreateMessage());
		}

		[HttpGet]
		[Route("reply-message-quote/{id:int?}")]
		public ActionResult ReplyMessageWithQuote(int? id)
		{
			if (id.HasValue)
			{
				var model = new UserMessageModel { MessageId = id.Value };

				if (FillUserMessageModel(model, true))
					return View("EditMessage", model);
				return RedirectToAction<AccountProfileController>(o => o.CreateMessage());
			}

			return RedirectToAction<AccountProfileController>(o => o.CreateMessage());
		}

		[HttpPost]
		[Route("edit-message/{id:int?}")]
		public ActionResult EditMessage(UserMessageModel model)
		{
			if (ModelIsValid)
			{
				var data = model.CopyTo();
				data.SenderId = CurrentUser.Id;

				Execute(() => _accountProfileService.SaveMessage(data));

				if (ModelIsValid)
				{
					return RedirectToAction<AccountProfileController>(o => o.Messages(),
						new RouteValueDictionary(new Dictionary<string, object> { { "type", EnumEx.GetName(MessageTypeEnum.Sent) } }));
				}
			}

			return View(model);
		}

		[AjaxOrChildActionOnly]
		[HttpPost]
		public ActionResult SaveMessagesAsRead(int typeId, string messageIds)
		{
			Execute(() => _accountProfileService.SaveAsRead(CurrentUser.Id, ParseMessageIds(messageIds)));

			var model = new UserMessagesModel { MessageTypeId = typeId };
			FillUserMessagesModel(model);

			return ModelIsValid
				? (ActionResult)View("MessagesData", model)
				: JsonErrorResult();
		}

		[AjaxOrChildActionOnly]
		[HttpPost]
		public ActionResult SaveMessagesAsDraft(int typeId, string messageIds)
		{
			Execute(() => _accountProfileService.SaveAsDraft(CurrentUser.Id, ParseMessageIds(messageIds)));

			var model = new UserMessagesModel { MessageTypeId = typeId };
			FillUserMessagesModel(model);

			return ModelIsValid
				? (ActionResult)View("MessagesData", model)
				: JsonErrorResult();
		}

		[AjaxOrChildActionOnly]
		[HttpPost]
		public ActionResult SaveMessageAsDraft(int id)
		{
			Execute(() => _accountProfileService.SaveAsDraft(CurrentUser.Id, new[] { id }));

			return ModelIsValid
			? Json(new
			{
				ReturnUrl = WebBuilder.BuildActionUrl<AccountProfileController>(o => o.Messages(),
					new RouteValueDictionary(new Dictionary<string, object> { { "type", EnumEx.GetName(MessageTypeEnum.Draft) } }))
			}) : JsonErrorResult();
		}

		[AjaxOrChildActionOnly]
		[HttpPost]
		public ActionResult DeleteMessages(int typeId, string messageIds)
		{
			Execute(() => _accountProfileService.DeleteMessages(CurrentUser.Id, ParseMessageIds(messageIds)));

			var model = new UserMessagesModel { MessageTypeId = typeId };
			FillUserMessagesModel(model);

			return ModelIsValid
				? (ActionResult)View("MessagesData", model)
				: JsonErrorResult();
		}

		[AjaxOrChildActionOnly]
		[HttpPost]
		public ActionResult DeleteMessage(int id)
		{
			Execute(() => _accountProfileService.DeleteMessages(CurrentUser.Id, new[] { id }));
			return ModelIsValid ? Json(new { ReturnUrl = WebBuilder.BuildActionUrl<AccountProfileController>(o => o.Messages()) }) : JsonErrorResult();
		}

		#endregion

		#endregion

		#region Internal Implementation

		private static IEnumerable<int> ParseMessageIds(string messageIds)
		{
			var selectedMessages = new List<int>();
			foreach (var item in messageIds.Split(','))
			{
				int messageId;
				if (int.TryParse(item, out messageId))
					selectedMessages.Add(messageId);
			}
			return selectedMessages;
		}

		private void FillUserProfileModel(UserProfileModel model)
		{
			var userProfile = Execute(() => _accountService.GetUserProfile(model.UserId));
			model.CopyFrom(userProfile);
		}

		private void FillEditProfileModel(EditProfileModel model)
		{
			var userProfile = Execute(() => _accountService.GetUserProfile(model.UserId));
			model.CopyFrom(userProfile);
		}

		private void FillUserMessagesModel(UserMessagesModel model)
		{
			var pagerData = (Execute(() => _accountProfileService.GetUserMessages(CurrentUser.Id, model.MessageTypeId, model.Pager.PageId, model.Pager.PageSize))
				?? new PageData<UserMessageData>());
			
			model.CopyFrom(pagerData);
		}

		private bool FillUserMessageModel(UserMessageModel model, bool isQuoteMessage)
		{
			var message = Execute(() => _accountProfileService.GetUserMessage(CurrentUser.Id, model.MessageId.GetValueOrDefault()));

			if (message != null && (message.Type == MessageTypeEnum.Inbox || message.Type == MessageTypeEnum.Sent))
			{
				model.MessageId = message.Type == MessageTypeEnum.Sent ? model.MessageId : default(int);
				model.Title = message.Type == MessageTypeEnum.Inbox ? StringGeneration.QuoteTitleBuilder(message.Title) : message.Title;
				model.Description = isQuoteMessage ? StringGeneration.QuoteDescriptionBuilder(message.Description, message.SenderName) : message.Description;
				model.RecipientName = message.SenderName;
				return true;
			}

			return false;
		}

		#endregion
	}
}