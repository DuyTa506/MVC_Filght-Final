using EaseFlight.BLL.Interfaces;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        #region Properties
        private IFlightService FlightService { get; set; }
        private IAccountService AccountService { get; set; }
        private IPlaneService PlaneService { get; set; }
        private ITicketService TicketService { get; set; }
        #endregion

        #region Constructors
        public HomeController(IFlightService flightService, IAccountService accountService,
            IPlaneService planeService, ITicketService ticketService)
        {
            this.AccountService = accountService;
            this.PlaneService = planeService;
            this.TicketService = ticketService;
            this.FlightService = flightService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["flights"] = this.FlightService.FindAll();
            ViewData["tickets"] = this.TicketService.FindAll();
            ViewData["planes"] = this.PlaneService.FindAll();
            ViewData["accounts"] = this.AccountService.FindAll();

            return View();
        }
        #endregion
    }
}