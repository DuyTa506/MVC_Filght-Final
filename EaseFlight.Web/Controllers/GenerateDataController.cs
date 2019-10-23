using EaseFlight.BLL.Interfaces;
using EaseFlight.DAL.Entities;
using EaseFlight.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    //Each Action will only run 1 time
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
        public ActionResult GeneratePlaneAirport(int number)
        {
            var planeList = this.PlaneService.FindAll().ToList();
            var airportList = this.AirportService.FindAll().ToList();

            foreach(var plane in planeList)
            {
                var result = new List<AirportModel>();
                var random = new Random();

                for (var i = 0; i < number; ++i)
                {
                    var index = random.Next(airportList.Count());

                    if (result.IndexOf(airportList[index]) != -1)
                        --i;
                    else result.Add(airportList[index]);
                }

                for(var i = 0; i < number; ++i)
                {
                    var planeAirport = new PlaneAirportModel {
                        AirportID = result[i].ID,
                        PlaneID = plane.ID
                    };

                    this.PlaneAirportService.Insert(planeAirport);
                }
            }

            return View("Done");
        }

        public ActionResult GenerateFlight()
        {
            var planeList = this.PlaneService.FindAll().ToList();
            var minutes = new List<int> {0, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            foreach (var plane in planeList)
            {
                var departureDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1); //Day 1 next month
                var arrivalDate = new DateTime();
                var departure = new AirportModel();
                var arrival = new AirportModel();

                while (departureDate.Month != DateTime.Now.Month + 2) //Generate one month
                {
                    var flight = new FlightModel
                    {
                        PlaneID = plane.ID,
                        DepartureDate = departureDate.AddHours(new Random().Next(0, 13))
                        .AddMinutes(minutes[new Random().Next(minutes.Count())]),
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
                        if (arrival != null && arrival.ID != airports[index2].AirportID) break;
                        if (index1 != index2) break;

                    } while (true);

                    if (arrival.ID == 0)
                        departure = this.AirportService.Find(airports[index1].AirportID);
                    else departure = arrival;

                    arrival = this.AirportService.Find(airports[index2].AirportID);
                    arrivalDate = flight.DepartureDate.Value;

                    if (departure.Country.Name.Equals(arrival.Country.Name)) //In country
                    {
                        arrivalDate = arrivalDate.AddHours(new Random().Next(1, 4));
                        flight.Price = new Random().Next(20, 51);
                    }
                    else if (departure.Country.Region.Equals(arrival.Country.Region)) //In Region
                    {
                        arrivalDate = arrivalDate.AddHours(new Random().Next(3, 8));
                        flight.Price = new Random().Next(30, 2000);
                    }
                    else //Diff Region
                    {
                        arrivalDate = arrivalDate.AddHours(new Random().Next(5, 12));
                        flight.Price = new Random().Next(200, 5000);
                    }

                    arrivalDate = arrivalDate.AddMinutes(minutes[new Random().Next(minutes.Count())]);

                    flight.ArrivalDate = arrivalDate;

                    //insert flight get ID
                    this.FlightService.Insert(flight);
                    var flightId = this.FlightService.FindAll().Last().ID;

                    UpdateDepartureOrArrival(plane.ID, departure.ID, flightId, true);
                    UpdateDepartureOrArrival(plane.ID, arrival.ID, flightId, false);

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
        #endregion

        #region Private Funtions
        private void UpdateDepartureOrArrival(int planeId, int airportId, int flightId, bool departure)
        {
            var result = new List<string>();
            var planeAirport = this.PlaneAirportService.Find(planeId, airportId);
            var departureOrArrival = planeAirport.DepartureOrArrival;

            if (departureOrArrival != null)
                result = departureOrArrival.Split('-').ToList();
            if(departure)
                result.Add(flightId + ".d");
            else result.Add(flightId + ".a");

            planeAirport.DepartureOrArrival = string.Join("-", result);

            this.PlaneAirportService.Update(planeAirport);
        }
        #endregion
    }
}