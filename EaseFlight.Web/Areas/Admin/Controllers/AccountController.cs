using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.Common.Utilities;
using EaseFlight.Web.WebUtilities;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        #region Properties
        private IAccountService AccountService { get; set; }
        #endregion

        #region Constructors
        public AccountController(IAccountService accountService)
        {
            this.AccountService = accountService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Login(string msg)
        {
            if (SessionUtility.IsSessionAlive())
                return RedirectToAction("Index", "Home");

            ViewData["msg"] = msg;

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
                    SessionUtility.SetAuthenticationToken(userModel, 60);

                    return RedirectToAction("Index", "Home");
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
        #endregion
    }
}