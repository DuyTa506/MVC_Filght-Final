using EaseFlight.BLL.Interfaces;
using EaseFlight.Models.CustomModel;
using EaseFlight.Web.WebUtilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    public class FlightController : Controller
    {
        #region Properties
        private IAirportService AirportService { get; set; }
        private ISeatClassService SeatClassService { get; set; }
        private IFlightService FlightService { get; set; }
        #endregion

        #region Constructors
        public FlightController(IAirportService airportService, ISeatClassService seatClassService,
            IFlightService flightService)
        {
            this.AirportService = airportService;
            this.SeatClassService = seatClassService;
            this.FlightService = flightService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Find()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Find(FormCollection collection)
        {
            var result = new JsonResult { ContentType = "text" };

            try
            {
                var departure = this.AirportService.Find(int.Parse(collection.Get("departure")));
                var arrival = this.AirportService.Find(int.Parse(collection.Get("arrival")));
                var departureDate = DateTime.ParseExact(collection.Get("date"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var returnDate = string.IsNullOrEmpty(collection.Get("return")) ? new DateTime()
                    : DateTime.ParseExact(collection.Get("return"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var seatClass = this.SeatClassService.Find(int.Parse(collection.Get("seat")));
                var adult = int.Parse(collection.Get("adult"));
                var child = int.Parse(collection.Get("child"));
                var infant = int.Parse(collection.Get("infant"));
                var roundTrip = bool.Parse(collection.Get("roundtrip"));

                if (departure == null || arrival == null || seatClass == null || (roundTrip && returnDate.Year == 1)
                    || (roundTrip && returnDate.Date < departureDate.Date) || adult < 1 || (adult + child + infant) > 10 || adult < (child + infant))
                    result.Data = new { type = "error" };
                else
                {
                    var searchList = this.FlightService.FindFlight(departure, arrival, departureDate);
                    var data = new List<SearchFlightModel>();
                    var totalSeat = adult + child;

                    foreach(var search in searchList)
                    {
                        var check = true;
                        foreach(var flight in search.FlightList)
                        {
                            var totalSeatClass = seatClass.PlaneSeatClasses.Where(planeSeat => planeSeat.PlaneID == flight.PlaneID)
                                .Select(planeSeat => planeSeat.Capacity).FirstOrDefault();
                            var totalTicket = flight.TicketFlights.Where(ticketflight => 
                                SeatMapUtility.FindBySeatCode(ticketflight.SeatCode, flight.PlaneID.Value).ID == seatClass.ID).Count();

                            if(totalSeat > (totalSeatClass - totalTicket))
                            {
                                check = false; break;
                            }
                        }

                        if (check) data.Add(search);
                    }
                    
                    result.Data = new { type = "success", result = RenderViewToString(this.ControllerContext , "_FlightResultPartial", data) };
                }
            }
            catch
            {
                result.Data = new { type = "error" };
            }

            return result;
        }

        private static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        #endregion
    }
}