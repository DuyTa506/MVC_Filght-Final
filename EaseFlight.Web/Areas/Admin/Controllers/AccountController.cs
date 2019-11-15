using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.Common.Utilities;
using EaseFlight.Models.EntityModels;
using EaseFlight.Web.WebUtilities;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        #region Properties
        private IAccountService AccountService { get; set; }
        private IAccountTypeService AccountTypeService { get; set; }
        #endregion

        #region Constructors
        public AccountController(IAccountService accountService, IAccountTypeService accountTypeService)
        {
            this.AccountService = accountService;
            this.AccountTypeService = accountTypeService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Login(string msg, string callUrl)
        {
            if (SessionUtility.IsSessionAlive())
                return RedirectToAction("Index", "Home");

            ViewData["msg"] = msg;
            ViewData["callUrl"] = callUrl;

            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var userModel = this.AccountService.FindByUsername(collection.Get("username"));

            if (userModel != null && !string.IsNullOrEmpty(userModel.Password) 
                && EncryptionUtility.BcryptCheckPassword(collection.Get("password"), userModel.Password))
            {
                if (userModel.Status.Value)
                {
                    var callBackUrl = collection.Get("callUrl");

                    SessionUtility.SetAuthenticationToken(userModel, 60);

                    if (!string.IsNullOrEmpty(callBackUrl))
                        return RedirectToAction(callBackUrl.Split('/')[1], callBackUrl.Split('/')[0]);
                    else return RedirectToAction("Index", "Home");

                }else return RedirectToAction("Login", new { msg = Constant.CONST_MESSAGE_LOGIN_DISABLE });
            }

            return RedirectToAction("Login" , new { msg = Constant.CONST_MESSAGE_LOGIN_INVALID});
        }

        [HttpGet]
        public ActionResult Logout()
        {
            SessionUtility.Logout();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewData["account"] = this.AccountService.FindAll().Where(account => account.AccountType.Name.Equals(Constant.CONST_ROLE_USER));

            return View();
        }

        [HttpPost]
        public ActionResult AddAccount(FormCollection collection)
        {
            var account = new AccountModel
            {
                FirstName = collection.Get("firstname"),
                LastName = collection.Get("lastname"),
                Gender = collection.Get("title").Equals("1") ? true : false,
                Birthday = DateTime.ParseExact(collection.Get("birthday"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Username = collection.Get("username"),
                Address = collection.Get("address"),
                Phone = collection.Get("phone"),
                Email = collection.Get("email"),
                Password = EncryptionUtility.BcryptHashPassword(Constant.CONST_PASSWORD_DEFAULT),
                AccountTypeID = this.AccountTypeService.FindByName(Constant.CONST_ROLE_USER).ID,
                Status = true
            };

            this.AccountService.Insert(account);
            TempData["msg"] = "success-Account added successfully";

            return RedirectToAction("Index");
        }
        #endregion
    }
}