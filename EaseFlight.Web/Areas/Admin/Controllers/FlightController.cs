using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class FlightController : BaseController
    {
        #region Properties
        private IFlightService FlightService { get; set; }
        private IPlaneService PlaneService { get; set; }
        private IPlaneAirportService PlaneAirportService { get; set; }
        #endregion

        #region Constructor
        public FlightController(IFlightService flightService, IPlaneService planeService,
            IPlaneAirportService planeAirportService)
        {
            this.FlightService = flightService;
            this.PlaneService = planeService;
            this.PlaneAirportService = planeAirportService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Index()
        {
            this.FlightService.UpdateFlightDone();
            ViewData["flights"] = this.FlightService.FindAll();
            ViewData["planes"] = this.PlaneService.FindAll().Where(plane => plane.PlaneAirports.Count > 1);

            return View();
        }

        [HttpPost]
        public JsonResult GetAirport(string planeId)
        {
            var result = new JsonResult { ContentType = "text" };
            var airports = this.PlaneAirportService.FindByPlane(int.Parse(planeId)).ToList();
            var data = new List<string>();

            airports.ForEach(air => data.Add(air.AirportID.ToString() + ":" + air.Airport.Name));
            result.Data = new { airport = data};

            return result;
        }

        [HttpPost]
        public ActionResult AddFlight(FormCollection collection)
        {
            var departDateString = collection.Get("flightDate").Split('-')[0].Trim();
            var arrivalDateString = collection.Get("flightDate").Split('-')[1].Trim();
            var departId = int.Parse(collection.Get("depart"));
            var arrivalId = int.Parse(collection.Get("arrival"));
            var planeId = int.Parse(collection.Get("plane"));
            var arrivalDate = DateTime.ParseExact(arrivalDateString, "dd/MM/yyyy hh:mm tt", null);
            var departDate = DateTime.ParseExact(departDateString, "dd/MM/yyyy hh:mm tt", null);

            if(!CheckDateTimeAvailable(planeId, departDate, arrivalDate))
            {
                TempData["msg"] = "error-Add flight failed because the flight time was already available";
                return RedirectToAction("Index");
            }

            var flight = new FlightModel
            {
                PlaneID = planeId,
                DepartureDate = departDate,
                ArrivalDate = arrivalDate,
                Price = double.Parse(collection.Get("price")),
                Status = Constant.CONST_FLIGHT_STATUS_READY
            };

            var flightId = this.FlightService.Insert(flight);
            var planeAirportDepart = this.PlaneAirportService.Find(planeId, departId);
            var planeAirportArrival = this.PlaneAirportService.Find(planeId, arrivalId);
            var departAirportList = planeAirportDepart.DepartureOrArrival.Split('-').ToList();
            var arrivalAirportList = planeAirportArrival.DepartureOrArrival.Split('-').ToList();

            this.PlaneAirportService.UpdateDepartureOrArrival(planeId, departId, flightId, true);
            this.PlaneAirportService.UpdateDepartureOrArrival(planeId, arrivalId, flightId, false);

            TempData["msg"] = "success-Flight added successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateFlight(FormCollection collection)
        {
            var departDateString = collection.Get("flightDate").Split('-')[0].Trim();
            var arrivalDateString = collection.Get("flightDate").Split('-')[1].Trim();
            var departId = int.Parse(collection.Get("depart"));
            var arrivalId = int.Parse(collection.Get("arrival"));
            var planeId = int.Parse(collection.Get("plane"));
            var flightId = int.Parse(collection.Get("flightId"));
            var arrivalDate = DateTime.ParseExact(arrivalDateString, "dd/MM/yyyy hh:mm tt", null);
            var departDate = DateTime.ParseExact(departDateString, "dd/MM/yyyy hh:mm tt", null);

            if (!CheckDateTimeAvailable(planeId, departDate, arrivalDate, flightId))
            {
                TempData["msg"] = "error-Flight update failed because the flight time was already available";

                return RedirectToAction("Index");
            }

            var currentFlight = this.FlightService.Find(flightId);

            currentFlight.PlaneID = planeId;
            currentFlight.DepartureDate = departDate;
            currentFlight.ArrivalDate = arrivalDate;
            currentFlight.Price = double.Parse(collection.Get("price"));

            this.FlightService.Update(currentFlight);
            this.PlaneAirportService.ChangeDepartureOrArrival(planeId, departId, flightId, true);
            this.PlaneAirportService.ChangeDepartureOrArrival(planeId, arrivalId, flightId, false);

            TempData["msg"] = "success-Flight update successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteFlight(string id)
        {
            var flightId = int.Parse(id);
            var currentFlight = this.FlightService.Find(flightId);

            if(currentFlight != null)
            {
                var result = this.FlightService.Delete(flightId);

                if (result == 0)
                    TempData["msg"] = "error-Flight delete failed because there were tickets booked";
                else if(result == -1)
                    TempData["msg"] = "error-Flight delete failed because flight online";
                else
                {
                    this.PlaneAirportService.DeleteDepartureOrArrival(currentFlight.PlaneID.Value, currentFlight.Departure.ID, currentFlight.Arrival.ID, flightId);
                    TempData["msg"] = "success-Flight delete successfully";
                }
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangeStatus(string id, string status)
        {
            var currentFlight = this.FlightService.Find(int.Parse(id));

            if(currentFlight != null)
            {
                if (status.Equals("Ready"))
                    currentFlight.Status = Constant.CONST_FLIGHT_STATUS_READY;
                else if (status.Equals("Online"))
                    currentFlight.Status = Constant.CONST_FLIGHT_STATUS_ONLINE;
                else if (status.Equals("Delay"))
                    currentFlight.Status = Constant.CONST_FLIGHT_STATUS_DELAY;

                this.FlightService.Update(currentFlight);
                TempData["msg"] = "success-Flight changed status successfully";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Private Funtions
        private bool CheckDateTimeAvailable(int planeId, DateTime departDate, DateTime arrivalDate, int flightId = -1)
        {
            var flights = this.FlightService.FindByDateAndPlane(planeId, departDate.Date).ToList();

            foreach(var flight in flights)
            {
                if (flightId == flight.ID) continue;

                if ((departDate >= flight.DepartureDate && departDate <= flight.ArrivalDate) || (arrivalDate >= flight.DepartureDate && arrivalDate <= flight.ArrivalDate)
                    || (departDate <= flight.DepartureDate && arrivalDate >= flight.ArrivalDate))
                    return false;
            }
                

            return true;
        }
        #endregion
    }
}