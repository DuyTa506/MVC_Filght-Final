using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.DAL.Entities;
using EaseFlight.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    //Each Action must only run 1 time
    public class GenerateDataController : Controller
    {
        #region Properties
        private IAirportService AirportService { get; set; }
        private ISeatClassService SeatClassService { get; set; }
        private IPlaneService PlaneService { get; set; }
        private IPlaneSeatClassService PlaneSeatClassService { get; set; }
        private IPlaneAirportService PlaneAirportService { get; set; }
        private IFlightService FlightService { get; set; }
        #endregion

        #region Constructors
        public GenerateDataController(IAirportService airportService, ISeatClassService seatClassService,
            IPlaneService planeService, IPlaneSeatClassService planeSeatClassService,
            IPlaneAirportService planeAirportService, IFlightService flightService)
        {
            this.AirportService = airportService;
            this.SeatClassService = seatClassService;
            this.PlaneService = planeService;
            this.PlaneAirportService = planeAirportService;
            this.PlaneSeatClassService = planeSeatClassService;
            this.FlightService = flightService;
        }
        #endregion

        #region Actions

        //Plane 1 -> 7 : Flight in Viet Nam
        //Plane 8 -> 14 : Flight from Viet Nam to other region
        //Plane 15 -> 20: Flight in other region
        public ActionResult GeneratePlaneAirport()
        {
            var planeList = this.PlaneService.FindAll().ToList();
            var airportList = this.AirportService.FindAll().ToList();
            var airportVietNam = airportList.Where(airport => airport.Country.Name.Equals(Constant.CONST_DB_NAME_VIETNAM)).ToList();
            var airportRegion = airportList.Where(airport => !airport.Country.Name.Equals(Constant.CONST_DB_NAME_VIETNAM)).ToList();
            var planeIndex = 1;

            foreach (var plane in planeList)
            {
                var result = new List<AirportModel>();

                if (planeIndex >= 1 && planeIndex <= 7)
                    result = RandomAirport(airportVietNam, 10);
                else if (planeIndex >= 8 && planeIndex <= 14)
                {
                    var result1 = RandomAirport(airportVietNam, 10);
                    result = result1.Concat(RandomAirport(airportRegion, 10)).OrderByDescending(airport => airport.Name).ToList();
                }
                else
                    result = RandomAirport(airportRegion, 20);

                foreach(var aiport in result)
                {
                    var planeAirport = new PlaneAirportModel {
                        AirportID = aiport.ID,
                        PlaneID = plane.ID
                    };

                    this.PlaneAirportService.Insert(planeAirport);
                }
                ++planeIndex;
            }

            return View("Done");
        }

        public ActionResult GenerateFlight()
        {
            var planeList = this.PlaneService.FindAll().Where(plane => plane.PlaneAirports.Count > 0).OrderBy(plane => plane.ID).ToList();
            var minutes = new List<int> { 40, 45, 50, 55, 60 };
            var minutes2 = new List<int> { 30, 35, 40, 45, 50, 55, 60 };

            foreach (var plane in planeList)
            {
                var departureDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1); //Day 1 next month
                var arrivalDate = new DateTime();
                var departure = new AirportModel();
                var arrival = new AirportModel();

                while (departureDate.Day <= 5) //Generate 5 days
                {
                    var flight = new FlightModel
                    {
                        PlaneID = plane.ID,
                        DepartureDate = departureDate.AddMinutes(minutes2[new Random().Next(minutes2.Count())]),
                        Status = "Ready"
                    };

                    var airports = plane.PlaneAirports.ToList();
                    var randomAirport = new Random();
                    var index1 = 0;
                    var index2 = 0;
                    do
                    {
                        index1 = randomAirport.Next(airports.Count());
                        index2 = randomAirport.Next(airports.Count());

                        if (arrival != null)
                        {
                            if (arrival.ID != airports[index2].AirportID) break;
                            else continue;
                        }

                        if (index1 != index2) break;

                    } while (true);

                    if (arrival.ID == 0)
                        departure = this.AirportService.Find(airports[index1].AirportID);
                    else departure = arrival;

                    arrival = this.AirportService.Find(airports[index2].AirportID);
                    if (departure.ID == arrival.ID) continue;
                    arrivalDate = flight.DepartureDate.Value;

                    if (departure.Country.Name.Equals(arrival.Country.Name)) //In country
                    {
                        flight.Price = new Random().Next(20, 51);
                    }
                    else if (departure.Country.Region.Equals(arrival.Country.Region)) //In Region
                    {
                        arrivalDate = arrivalDate.AddMinutes(new Random().Next(20, 60));
                        flight.Price = new Random().Next(30, 1000);
                    }
                    else //Diff Region
                    {
                        arrivalDate = arrivalDate.AddMinutes(new Random().Next(20, 60));
                        flight.Price = new Random().Next(200, 3000);
                    }

                    arrivalDate = arrivalDate.AddMinutes(minutes[new Random().Next(minutes.Count())]);

                    flight.ArrivalDate = arrivalDate;

                    //insert flight get ID
                    this.FlightService.Insert(flight);
                    var flightId = this.FlightService.FindAll().Last().ID;

                    this.PlaneAirportService.UpdateDepartureOrArrival(plane.ID, departure.ID, flightId, true);
                    this.PlaneAirportService.UpdateDepartureOrArrival(plane.ID, arrival.ID, flightId, false);

                    //Reset value
                    departureDate = arrivalDate;
                }
            }

            return View("Done");
        }

        [HttpGet]
        public ActionResult ViewDataGenerate()
        {
            var planeAirportList = this.PlaneAirportService.FindAll();
            var flightList = this.FlightService.FindAll();

            ViewData["planeAirportList"] = planeAirportList;
            ViewData["flightList"] = flightList;

            return View();
        }

        [HttpGet]
        public ActionResult ViewDataDuplicate()
        {
            var flightList = this.FlightService.FindAll().Where(flight => flight.Departure.ID == flight.Arrival.ID);

            ViewData["flightList"] = flightList;

            return View("ViewDataGenerate");
        }
        #endregion

        #region Private Functions
        private List<AirportModel> RandomAirport(List<AirportModel> list, int range)
        {
            var random = new Random();
            var result = new List<AirportModel>();

            for (var i = 0; i < range; ++i)
            {
                var index = random.Next(list.Count());

                if (result.IndexOf(list[index]) != -1)
                    --i;
                else result.Add(list[index]);
            }

            return result;
        }
        #endregion
    }
}