using EaseFlight.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ActionResult Find(FormCollection collection)
        {
            var departure = this.AirportService.Find(int.Parse(collection.Get("idAirportDeparture")));
            var arrival = this.AirportService.Find(int.Parse(collection.Get("idAirportArrival")));
            var departureDate = DateTime.ParseExact(collection.Get("departureDate"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var returnDate = string.IsNullOrEmpty(collection.Get("returnDate")) ? new DateTime()
                : DateTime.ParseExact(collection.Get("returnDate"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var seatClass = this.SeatClassService.Find(int.Parse(collection.Get("idSeatClass")));
            var adult = int.Parse(collection.Get("adult"));
            var child = int.Parse(collection.Get("child"));
            var infant = int.Parse(collection.Get("infant"));
            var roundTrip = bool.Parse(collection.Get("roundTrip"));

            if (departure == null || arrival == null)
                return View();

            this.FlightService.FindFlight(departure, arrival, departureDate);

            return View();
        }

        public JsonResult GetMap()
        {
            var result = new JsonResult { ContentType = "text" };

            //seatClassOrder 1: Business. seatClassOrder 2: Economy. seatClassOrder -1: Get all
            var data = SeatCode(-1);
            var seatClass = FindBySeatCode("17F", data);

            return result;
        }

        private class SeatClass
        {
            public string name { get; set; }
            public int capacity { get; set; }
            public int order { get; set; }
        }

        private List<string> SeatCode(int seatClassOrder)
        {
            var data = new List<string>();
            var rowWithoutChair = new List<string>();

            //Data Seat Code VietName Airline Airbus 330 type 1
            var characterColumnDB = "A-C-D-E-F-G-H-K";
            var rowWithoutChairDB = "1.E.F-2.E.F-3.E.F-4-23.D.E.F.G-33.E-34.E-35.E-36.E-37.A.C.E.H.K"; //Row 1 without chair 1C 1D 1E, row 5 and 6 dont have any chair
            var capacity = 269;
            var listSeatClass = new List<SeatClass> {
                new SeatClass
                {
                    name = "business", capacity = 18, order = 1
                },
                new SeatClass
                {
                    name = "economy", capacity = 251, order = 2
                }
            };
            //listSeatClass sort by order (1- 2 -3)

            //End Data

            var characterColumn = characterColumnDB.Split('-');
            var x = rowWithoutChairDB.Split('-').ToList();
            foreach (var t in x)
            {
                var u = t.Split('.'); var i = 0;

                if (u.Length == 1)
                {
                    characterColumn.ToList().ForEach(column => rowWithoutChair.Add(u[0] + column));
                }
                else while (++i < u.Length)
                        rowWithoutChair.Add(u[0] + u[i]);
            }

            var rows = (capacity + rowWithoutChair.Count) / characterColumn.Length;
            var index = 0;

            //Get Seat Code
            for (var i = 1; i <= rows; ++i)
            {
                foreach (var column in characterColumn)
                {
                    var seat = string.Concat(i, column);
                    if (rowWithoutChair.IndexOf(seat) == -1)
                        data.Add(seat);
                    if (data.Count == listSeatClass[index].capacity && seatClassOrder != -1)
                    {
                        if (listSeatClass[index].order == seatClassOrder)
                        {
                            i = rows + 1; break;
                        }
                        else
                        {
                            data.Clear();
                            ++index;
                        }

                    }
                }
            }

            return data;
        }

        private SeatClass FindBySeatCode(string seatCode, List<string> data)
        {
            //Data
            var listSeatClass = new List<SeatClass> {
                new SeatClass
                {
                    name = "business", capacity = 18, order = 1
                },
                new SeatClass
                {
                    name = "economy", capacity = 251, order = 2
                }
            };
            //listSeatClass sort by order (1- 2 -3)

            //End Data

            var index = data.IndexOf(seatCode);

            if (index == -1) return new SeatClass();

            var first = 0;

            for (var i = 0; i < listSeatClass.Count; ++i)
            {
                var last = first + listSeatClass[i].capacity - 1;

                if (index >= first && index <= last)
                    return listSeatClass[i];
                else first = listSeatClass[i].capacity;
            }

            return new SeatClass();
        }
        #endregion
    }
}