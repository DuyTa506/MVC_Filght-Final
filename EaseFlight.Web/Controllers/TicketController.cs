using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.Common.EmailSenders;
using EaseFlight.Models.CustomModel;
using EaseFlight.Models.EntityModels;
using EaseFlight.Web.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    public class TicketController : Controller
    {
        #region Properties
        private IPassengerTypeService PassengerTypeService { get; set; }
        private ITicketService TicketService { get; set; }
        private IPassengerTicketService PassengerTicketService { get; set; }
        private ITicketFlightService TicketFlightService { get; set; }
        private ISeatMapService SeatMapService { get; set; }
        private IAccountService AccountService { get; set; }
        private IFlightService FlightService { get; set; }
        private IAirportService AirportService { get; set; }
        #endregion

        #region Constructors
        public TicketController(IPassengerTypeService passengerTypeService, ITicketService ticketService,
            IPassengerTicketService passengerTicketService, ITicketFlightService ticketFlightService,
            ISeatMapService seatMapService, IAccountService accountService, IFlightService flightService,
            IAirportService airportService)
        {
            this.PassengerTypeService = passengerTypeService;
            this.TicketService = ticketService;
            this.PassengerTicketService = passengerTicketService;
            this.TicketFlightService = ticketFlightService;
            this.SeatMapService = seatMapService;
            this.AccountService = accountService;
            this.FlightService = flightService;
            this.AirportService = airportService;
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            var loggedUser = SessionUtility.GetLoggedUser();

            if (loggedUser == null)
                return RedirectToAction("Index", "Home");

            var ticketList = this.TicketService.FindByAccount(loggedUser.ID);
            var model = new List<TicketHistoryModel>();

            foreach(var ticket in ticketList)
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

            return View(model.OrderByDescending(m => m.Ticket.ID).ToList());
        }

        [HttpPost]
        public JsonResult SavePassenger(string json)
        {
            var passengerJsonObject = JsonConvert.DeserializeObject<JObject>(json);
            var passengerList = passengerJsonObject.Properties().Select(p => p.Value).ToList();
            var modelList = new List<PassengerTicketModel>();
            var itsYou = true;
            
            foreach(var passenger in passengerList)
            {
                var passport = GetValueFromJson(passenger, "IDCardOrPassport");
                var passengerType = GetValueFromJson(passenger, "id");

                if (passengerType.Equals("UpdateAccount"))
                    continue;

                var passengerModel = new PassengerTicketModel
                {
                    FirstName = GetValueFromJson(passenger, "FirstName"),
                    LastName = GetValueFromJson(passenger, "LastName"),
                    Gender = !GetValueFromJson(passenger, "Gender").Equals("0"),
                    Birthday = DateTime.ParseExact(GetValueFromJson(passenger, "Birthday"), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                };

                if (!string.IsNullOrEmpty(passport))
                {
                    passengerModel.IDCardOrPassport = passport;
                    passengerModel.DateIssueOrExpiry = DateTime.ParseExact(GetValueFromJson(passenger, "DateIssueOrExpiry"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    passengerModel.PlaceIssue = string.Concat(GetValueFromJson(passenger, "Nationality"), string.IsNullOrEmpty(GetValueFromJson(passenger, "City"))?"": ", " + GetValueFromJson(passenger, "City"));
                }

                if (passengerType.Contains("Adult"))
                    passengerModel.PassengerTypeID = this.PassengerTypeService.FindByName(Constant.CONST_DB_NAME_ADULT).ID;
                else if (passengerType.Contains("Child"))
                    passengerModel.PassengerTypeID = this.PassengerTypeService.FindByName(Constant.CONST_DB_NAME_CHILD).ID;
                else if (passengerType.Contains("Infant"))
                    passengerModel.PassengerTypeID = this.PassengerTypeService.FindByName(Constant.CONST_DB_NAME_INFANT).ID;

                if (passengerType.Equals("Adult1")) //Check if not have account is a passenger
                    itsYou = false;

                modelList.Add(passengerModel);
            }

            if (itsYou)
            {
                var loggedUser = SessionUtility.GetLoggedUser();
                modelList.Add(new PassengerTicketModel()
                {
                    FirstName = loggedUser.FirstName,
                    LastName = loggedUser.LastName,
                    Gender = loggedUser.Gender,
                    Birthday = loggedUser.Birthday,
                    IDCardOrPassport = loggedUser.IDCardOrPassport,
                    DateIssueOrExpiry = loggedUser.DateIssueOrExpiry,
                    PlaceIssue = loggedUser.PlaceIssue,
                    PassengerTypeID = this.PassengerTypeService.FindByName(Constant.CONST_DB_NAME_ADULT).ID
                }); ;
            }

            SessionUtility.SetPassengerSession(modelList);

            return new JsonResult { ContentType = "text" };
        }

        [HttpGet]
        public ActionResult PaymentPaypal()
        {
            if (SessionUtility.GetBookingSession() == null || SessionUtility.GetPassengerSession() == null)
                return RedirectToAction("Index", "Home");

            APIContext apiContext = PaypalUtility.GetAPIContext();

            try
            {
                var payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    var baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Ticket/PaymentPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = PaypalUtility.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //Get id payment for refund
                    var boookingSession = SessionUtility.GetBookingSession();
                    boookingSession.PaymentID = createdPayment.id;
                    SessionUtility.SetBookingSession(boookingSession);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                            paypalRedirectUrl = lnk.href;
                    }

                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = PaypalUtility.ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                        return View("Failed");
                }
            }
            catch
            {
                return View("Failed");
            }

            return RedirectToAction("BookingSuccess");
        }

        public ActionResult BookingSuccess()
        {
            var loggedUser = SessionUtility.GetLoggedUser();
            var booking = SessionUtility.GetBookingSession();
            var passengerList = SessionUtility.GetPassengerSession();
            var seatCodeSuccess = new List<string>();

            if (booking == null || passengerList == null)
                return RedirectToAction("Index", "Home");

            var percent = booking.PassengerType.Where(type => type.Name.Equals(Constant.CONST_DB_NAME_INFANT)).Select(type => type.Discount.Value).FirstOrDefault();
            var priceInfant = Math.Round(booking.Price - (booking.Price / 100) * percent, 2);
            var percent2 = booking.PassengerType.Where(type => type.Name.Equals(Constant.CONST_DB_NAME_CHILD)).Select(type => type.Discount.Value).FirstOrDefault();
            var priceChild = Math.Round(booking.Price - (booking.Price / 100) * percent2, 2);
            var totalPrice = (booking.Price * booking.Adult) + (booking.Child > 0 ? priceChild * booking.Child : 0) + (booking.Infant > 0 ? priceInfant * booking.Infant : 0);

            //Save Ticket
            var ticket = new TicketModel 
            { 
                AccountID = loggedUser.ID,
                Price = totalPrice,
                CreateDate = DateTime.Now,
                PaymentID = booking.PaymentID,
                Status = Constant.CONST_DB_TICKET_STATUS_SUCCESS
            };

            var ticketId = this.TicketService.Insert(ticket);
            
            //Save Passenger Ticket
            foreach(var passenger in passengerList)
            {
                passenger.TicketID = ticketId;
                this.PassengerTicketService.Insert(passenger);
            }

            //Save Ticket Flight
            var order = 1;
            
            foreach(var flight in booking.DepartFlight)
            {
                var seatCode = this.SeatMapService.GenerateSeatCodeTicket(flight.PlaneID.Value, booking.SeatClass.ID
                    , flight.ID, booking.Adult + booking.Child);

                if (order == 1) seatCodeSuccess = seatCode;

                var ticketFlight = new TicketFlightModel
                {
                    TicketID = ticketId,
                    FlightID = flight.ID,
                    SeatCode = string.Join(",", seatCode),
                    RoundTrip = false,
                    Order = order++
                };

                this.TicketFlightService.Insert(ticketFlight);
            }

            //Return flight
            if(booking.ReturnFlight.Count() != 0)
            {
                order = 1;
                foreach (var flight in booking.ReturnFlight)
                {
                    var seatCode = this.SeatMapService.GenerateSeatCodeTicket(flight.PlaneID.Value, booking.SeatClass.ID
                        , flight.ID, booking.Adult + booking.Child);
                    var ticketFlight = new TicketFlightModel
                    {
                        TicketID = ticketId,
                        FlightID = flight.ID,
                        SeatCode = string.Join(",", seatCode),
                        RoundTrip = true,
                        Order = order++
                    };

                    this.TicketFlightService.Insert(ticketFlight);
                }
            }

            SessionUtility.RemoveBookingSession();
            SessionUtility.RemovePassengerSession();

            var model = new BookingSuccessModel
            {
                TicketId = ticketId,
                PaymentId = ticket.PaymentID.Split('-')[1],
                Customer = loggedUser.FirstName + " " + loggedUser.LastName,
                DepartDate = booking.DepartFlight.First().DepartureDate.Value,
                Flight = booking.Departure.City + " to " + booking.Arrival.City
                    + (booking.ReturnFlight.Count() > 0 ? " (Round trip)" : string.Empty),
                Passenger = booking.Adult + " Adult, " + booking.Child + " Child, " + booking.Infant + " Infant",
                SeatCode = string.Join(", ", seatCodeSuccess),
                Price = totalPrice
            };

            var baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            var currentUser = this.AccountService.Find(loggedUser.ID);

            EmailSender.SendMailBookingSuccess(currentUser.Email, baseUrl + "Ticket", model.PaymentId, 
                model.DepartDate.ToString("dd/MM/yyyy hh:mm tt"), model.Flight, model.Passenger, model.SeatCode, model.Price.ToString(), ticketId);
            SessionUtility.SetAuthenticationToken(currentUser, 60);

            return View(model);
        }

        [HttpPost]
        public JsonResult RefundTicket(string ticketId, string admin = null)
        {
            var result = new JsonResult { ContentType = "text" };
            var currentTicket = this.TicketService.Find(int.Parse(ticketId));

            if(currentTicket == null)
            {
                result.Data = new { type = "error" };
                return result;
            }

            var firstFlight = this.FlightService.FindByTicket(currentTicket.ID).First();
            var departFlight = this.FlightService.FindByTicket(currentTicket.ID).ToList();
            var returnFlight = this.FlightService.FindByTicket(currentTicket.ID, true).ToList();
            var flightTicket = departFlight.First().Departure.City + " to " + departFlight.Last().Arrival.City + (returnFlight.Count() > 0 ? " (Roundtrip)" : "");

            var refundPrice = currentTicket.Price.Value / 2;
            APIContext apiContext = PaypalUtility.GetAPIContext();

            if ((firstFlight.DepartureDate.Value - DateTime.Now).TotalDays > 30 && !currentTicket.Status.Equals(Constant.CONST_DB_TICKET_STATUS_RETURN))
                if (PaypalUtility.RefundPayment(apiContext, currentTicket.PaymentID, refundPrice))
                {
                    result.Data = new { type = "success", refund = string.Format("{0:0.00}", refundPrice) };
                    currentTicket.Status = Constant.CONST_DB_TICKET_STATUS_RETURN;
                    currentTicket.UpdateDate = DateTime.Now;
                    currentTicket.Description = "Refund $" + string.Format("{0:0.00}", refundPrice);

                    EmailSender.SendMailReturnTicket(currentTicket.Account.Email, flightTicket, currentTicket.Price.ToString(), string.Format("{0:0.00}", refundPrice), admin);
                    this.TicketService.Update(currentTicket);
                }
                else result.Data = new { type = "error" };
            else result.Data = new { type = "error" };

            return result;
        }
        #endregion

        #region Private Functions
        private string GetValueFromJson(JToken token, string key)
        {
            return token.Where(p => p.SelectToken("name").Value<string>().Equals(key))
                    .Select(p => p.SelectToken("value").Value<string>()).FirstOrDefault();
        }
        #endregion
    }
}