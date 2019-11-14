using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.Models.CustomModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class TicketController : BaseController
    {
        #region Properties
        private ITicketService TicketService { get; set; }
        private IFlightService FlightService { get; set; }
        private IAirportService AirportService { get; set; }
        private ISeatMapService SeatMapService { get; set; }
        private ITicketFlightService TicketFlightService { get; set; }
        private IPassengerTicketService PassengerTicketService { get; set; }
        #endregion

        #region Constructor
        public TicketController(ITicketService ticketService, IFlightService flightService, IPassengerTicketService passengerTicketService,
            IAirportService airportService, ISeatMapService seatMapService, ITicketFlightService ticketFlightService)
        {
            this.TicketService = ticketService;
            this.FlightService = flightService;
            this.AirportService = airportService;
            this.SeatMapService = seatMapService;
            this.TicketFlightService = ticketFlightService;
            this.PassengerTicketService = passengerTicketService;
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            var tickets = this.TicketService.FindAll();

            var model = new List<TicketHistoryModel>();

            foreach (var ticket in tickets)
            {
                var departFlight = this.FlightService.FindByTicket(ticket.ID).ToList();
                var returnFlight = this.FlightService.FindByTicket(ticket.ID, true).ToList();
                var passengers = this.PassengerTicketService.FindByTicket(ticket.ID).ToList();
                var firstTicketFlight = this.TicketFlightService.FindByTicket(ticket.ID).First();

                model.Add(new TicketHistoryModel
                {
                    Ticket = ticket,
                    From = this.AirportService.Find(departFlight.First().Departure.ID),
                    To = this.AirportService.Find(departFlight.Last().Arrival.ID),
                    SeatClass = this.SeatMapService.FindBySeatCode(firstTicketFlight.SeatCode.Split(',').First(), firstTicketFlight.Flight.PlaneID.Value),
                    DepartFlight = departFlight,
                    ReturnFlight = returnFlight,
                    Passengers = passengers,
                    TicketFlightList = this.TicketFlightService.FindByTicket(ticket.ID).ToList()
                });
            }

            ViewData["tickets"] = model;

            return View();
        }

        [HttpPost]
        public JsonResult UpdatePassenger(FormCollection collection)
        {
            var result = new JsonResult { ContentType = "text" };
            var currentPassenger = this.PassengerTicketService.Find(int.Parse(collection.Get("passengerId")));

            currentPassenger.FirstName = collection.Get("firstname");
            currentPassenger.LastName = collection.Get("lastname");
            currentPassenger.Gender = collection.Get("title").Equals("1") ? true : false;
            currentPassenger.Birthday = DateTime.ParseExact(collection.Get("birthday"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            currentPassenger.IDCardOrPassport = collection.Get("passport");
            currentPassenger.PlaceIssue = string.Concat(collection.Get("nationality"), string.IsNullOrEmpty(collection.Get("city")) ? "" : ", " + collection.Get("city"));

            if (string.IsNullOrEmpty(collection.Get("expiry")))
                currentPassenger.DateIssueOrExpiry = null;
            else currentPassenger.DateIssueOrExpiry = DateTime.ParseExact(collection.Get("expiry"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            this.PassengerTicketService.Update(currentPassenger);

            return result;
        }

        [HttpPost]
        public JsonResult CancelTicket(string ticketId)
        {
            var result = new JsonResult { ContentType = "text" };
            var currentTicket = this.TicketService.Find(int.Parse(ticketId));

            if (currentTicket == null || currentTicket.Status.Equals(Constant.CONST_DB_TICKET_STATUS_CANCEL))
            {
                result.Data = new { type = "error" };
                return result;
            }

            currentTicket.Status = Constant.CONST_DB_TICKET_STATUS_CANCEL;
            result.Data = new { type = "success" };
            this.TicketService.Update(currentTicket);

            return result;
        }

        [HttpPost]
        public JsonResult DeleteTicket(string ticketId)
        {
            var result = new JsonResult { ContentType = "text" };
            var currentTicket = this.TicketService.Find(int.Parse(ticketId));

            if(currentTicket != null && currentTicket.Status.Equals(Constant.CONST_DB_TICKET_STATUS_CANCEL))
            {

                result.Data = new { type = "success" };
            }

            return result;
        }
        #endregion
    }
}