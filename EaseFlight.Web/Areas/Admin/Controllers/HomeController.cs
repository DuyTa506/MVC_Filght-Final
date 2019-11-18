using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using System;
using System.Collections.Generic;
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
            var tickets = this.TicketService.FindAll();
            var ticketSuccess = new List<int>();
            var ticketReturn = new List<int>();
            var ticketRoundtrip = new List<int>();

            for(var i = 1; i <= DateTime.Now.Month; ++i)
            {
                ticketSuccess.Add(tickets.Where(ticket => ticket.CreateDate.Value.Month == i && ticket.Status.Equals(Constant.CONST_DB_TICKET_STATUS_SUCCESS)).Count());
                ticketReturn.Add(tickets.Where(ticket => ticket.CreateDate.Value.Month == i && ticket.Status.Equals(Constant.CONST_DB_TICKET_STATUS_RETURN)).Count());
                ticketRoundtrip.Add(tickets.Where(ticket => ticket.CreateDate.Value.Month == i && ticket.TicketFlights.Where(flight => flight.RoundTrip.Value).Count() > 0).Count());
            }

            ViewData["flights"] = this.FlightService.FindAll();
            ViewData["tickets"] = tickets;
            ViewData["planes"] = this.PlaneService.FindAll();
            ViewData["accounts"] = this.AccountService.FindAll();
            ViewData["ticketSuccess"] = ticketSuccess;
            ViewData["ticketReturn"] = ticketReturn;
            ViewData["ticketRoundtrip"] = ticketRoundtrip;

            return View();
        }
        #endregion
    }
}