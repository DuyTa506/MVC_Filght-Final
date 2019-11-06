using EaseFlight.Web.App_Start;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "SA AD")]
    public class BaseController : Controller
    {
       
    }
}