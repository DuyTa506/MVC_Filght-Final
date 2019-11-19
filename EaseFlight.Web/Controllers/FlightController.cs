using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.Models.CustomModel;
using EaseFlight.Models.EntityModels;
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
        private ICountryService CountryService { get; set; }
        private IPassengerTypeService PassengerTypeService { get; set; }
        private ISeatMapService SeatMapService { get; set; }
        #endregion

        #region Constructors
        public FlightController(IAirportService airportService, ISeatClassService seatClassService,
            IFlightService flightService, ICountryService countryService, IPassengerTypeService passengerTypeService,
            ISeatMapService seatMapService)
        {
            this.AirportService = airportService;
            this.SeatClassService = seatClassService;
            this.FlightService = flightService;
            this.CountryService = countryService;
            this.PassengerTypeService = passengerTypeService;
            this.SeatMapService = seatMapService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Find()
        {
            var airports = this.AirportService.FindAll();
            var regions = this.CountryService.FindAll().Select(country => country.Region).Distinct();
            var passengers = this.PassengerTypeService.FindAll();
            var airportRegion = new List<AirportRegionModel>
            {
                new AirportRegionModel
                {
                    Region = Constant.CONST_DB_NAME_VIETNAM,
                    Airports = airports.Where(airport => airport.Country.Name.Equals(Constant.CONST_DB_NAME_VIETNAM))
                        .OrderBy(airport => airport.Name)
                }
            };

            foreach (var region in regions)
            {
                var airportList = airports.Where(airport => airport.Country.Region.Equals(region)
                && !airport.Country.Name.Equals(Constant.CONST_DB_NAME_VIETNAM));

                if (airportList.Count() != 0)
                    airportRegion.Add(new AirportRegionModel
                    {
                        Region = region,
                        Airports = airportList
                    });
            }

            ViewData["airports"] = airportRegion;
            ViewData["passengers"] = passengers;
            ViewData["seatClassList"] = this.SeatClassService.FindAll();
            SessionUtility.RemoveBookingSession();

            return View();
        }

        [HttpPost]
        public JsonResult GetSearchValue(FormCollection collection)
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
                var adultPassenger = int.Parse(collection.Get("adult"));
                var childPassenger = int.Parse(collection.Get("child"));
                var infantPassenger = int.Parse(collection.Get("infant"));
                var isRoundTrip = bool.Parse(collection.Get("roundtrip"));

                result.Data = new 
                {
                    departure = departure.ID,
                    arrival = arrival.ID,
                    date = departureDate.ToString("yyyy-MM-dd"),
                    returnDate = returnDate.ToString("yyyy-MM-dd"),
                    seat = seatClass.Name.Replace(" ",""),
                    adult = adultPassenger,
                    child = childPassenger,
                    infant = infantPassenger,
                    roundTrip = isRoundTrip
                };
            }
            catch
            {

            }

            return result;
        }

        [HttpPost]
        public JsonResult Find(FormCollection collection)
        {
            this.FlightService.UpdateFlightDone();
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
                var pageDepart = int.Parse(collection.Get("pageDepart"));
                var pageReturn = int.Parse(collection.Get("pageReturn"));

                if (departure == null || arrival == null || seatClass == null || (roundTrip && returnDate.Year == 1)
                    || (roundTrip && returnDate.Date < departureDate.Date) || adult < 1 || (adult + child + infant) > 10 || adult < (child + infant))
                    result.Data = new { type = "error" };
                else
                {
                    var searchDepartureList = this.FlightService.FindFlight(departure, arrival, departureDate);
                    var totalSeat = adult + child;
                    var returnData = new List<SearchFlightModel>();
                    var departureData = SearchFlight(searchDepartureList, totalSeat, seatClass).OrderBy(flight => flight.Price);

                    if (roundTrip)
                    {
                        var searchReturnList = this.FlightService.FindFlight(arrival, departure, returnDate);
                        returnData = SearchFlight(searchReturnList, totalSeat, seatClass).OrderBy(flight => flight.Price).ToList();
                    }

                    var model = new ResultFlightModel
                    {
                        DepartureData = departureData.Take(pageDepart).ToList(),
                        ReturnData = returnData.Take(pageReturn).ToList(),
                        From = departure,
                        To = arrival,
                        SeatClass = seatClass,
                        PageSize = new List<int> { departureData.Count(), returnData.Count()}
                    };

                    result.Data = new { type = "success", result = RenderViewToString(this.ControllerContext , "_FlightResultPartial", model) };
                }
            }
            catch
            {
                result.Data = new { type = "error" };
            }

            return result;
        }

        [HttpGet]
        public ActionResult Booking()
        {
            if (SessionUtility.GetBookingSession() == null || !SessionUtility.IsSessionAlive())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public JsonResult Booking(FormCollection collection)
        {
            var result = new JsonResult { ContentType = "text" };
            var departure = this.AirportService.Find(int.Parse(collection.Get("departure")));
            var arrival = this.AirportService.Find(int.Parse(collection.Get("arrival")));
            var seatClass = this.SeatClassService.Find(int.Parse(collection.Get("seat")));
            var adult = int.Parse(collection.Get("adult"));
            var child = int.Parse(collection.Get("child"));
            var infant = int.Parse(collection.Get("infant"));
            var price = double.Parse(collection.Get("price"));
            var roundTrip = bool.Parse(collection.Get("roundtrip"));
            var flightDepart = collection.Get("flightDepart").Split('.').ToList();
            var flightReturn = collection.Get("flightReturn").Split('.').ToList();
            var departList = new List<FlightModel>();
            var returnList = new List<FlightModel>();
            var passengerType = this.PassengerTypeService.FindAll().ToList();

            flightDepart.ForEach(id => departList.Add(this.FlightService.Find(int.Parse(id))));

            if(roundTrip)
                flightReturn.ForEach(id => returnList.Add(this.FlightService.Find(int.Parse(id))));

            var booking = new BookingModel
            {
                DepartFlight = departList,
                ReturnFlight = returnList,
                Departure = departure,
                Arrival = arrival,
                SeatClass = seatClass,
                PassengerType = passengerType,
                Adult = adult,
                Child = child,
                Infant = infant,
                Price = price
            };

            SessionUtility.SetBookingSession(booking);

            return result;
        }
        #endregion

        #region Private Funtions
        private List<SearchFlightModel> SearchFlight(IEnumerable<SearchFlightModel> searchList, int totalSeat, SeatClassModel seatClass)
        {
            var data = new List<SearchFlightModel>();

            foreach (var search in searchList)
            {
                var check = true;
                var priceSeat = 0.0;

                foreach (var flight in search.FlightList)
                {
                    var totalSeatClass = seatClass.PlaneSeatClasses.Where(planeSeat => planeSeat.PlaneID == flight.PlaneID)
                        .Select(planeSeat => planeSeat.Capacity).FirstOrDefault();
                    var totalTicket = flight.TicketFlights.Where(ticketflight => ticketflight.Ticket.Status.Equals(Constant.CONST_DB_TICKET_STATUS_SUCCESS)
                        && this.SeatMapService.FindBySeatCode(ticketflight.SeatCode, flight.PlaneID.Value).ID == seatClass.ID).Count();
                    priceSeat = seatClass.PlaneSeatClasses.Where(planeSeat => planeSeat.PlaneID == flight.PlaneID)
                        .Select(planeSeat => planeSeat.Price.Value).FirstOrDefault();

                    if (totalSeatClass == null)
                    {
                        check = false; break;
                    }

                    if (totalSeat > (totalSeatClass - totalTicket))
                    {
                        check = false; break;
                    }
                }

                if (check)
                {
                    var airportCode = new List<string>();
                    
                    for (var i = 1; i < search.FlightList.Count(); ++i)
                        airportCode.Add(search.FlightList[i].Departure.Name.Split('-').First());

                    search.Price += priceSeat;
                    search.AirportCode = airportCode;
                    data.Add(search);
                }
            }

            return data;
        }

        private string RenderViewToString(ControllerContext context, string viewName, object model)
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