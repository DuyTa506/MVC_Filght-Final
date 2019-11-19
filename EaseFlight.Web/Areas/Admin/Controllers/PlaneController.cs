using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.Models.EntityModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class PlaneController : BaseController
    {
        #region Propertities
        private IPlaneService PlaneService { get; set; }
        private ISeatMapService SeatMapService { get; set; }
        private IAirportService AirportService { get; set; }
        private IPlaneSeatClassService PlaneSeatClassService { get; set; }
        private IPlaneAirportService PlaneAirportService { get; set; }
        private ITicketFlightService TicketFlightService { get; set; }
        private IFlightService FlightService { get; set; }
        #endregion

        #region Constructor
        public PlaneController(IPlaneService planeService, ISeatMapService seatMapService, IAirportService airportService, 
            IPlaneSeatClassService planeSeatClassService, IPlaneAirportService planeAirportService, ITicketFlightService ticketFlightService,
            IFlightService flightService)
        {
            this.PlaneService = planeService;
            this.AirportService = airportService;
            this.SeatMapService = seatMapService;
            this.PlaneSeatClassService = planeSeatClassService;
            this.PlaneAirportService = planeAirportService;
            this.TicketFlightService = ticketFlightService;
            this.FlightService = flightService;
        }
        #endregion

        #region Actions
        // GET: Admin/PlaneAirport
        [HttpGet]
        public ActionResult Index()
        {
            this.FlightService.UpdateFlightDone();

            ViewData["planes"] = this.PlaneService.FindAll();
            ViewData["seats"] = this.SeatMapService.FindAll();
            ViewData["airports"] = this.AirportService.FindAll();

            return View();
        }

        //ADD NEW PLANE+SEAT CLASS
        [HttpPost]
        public ActionResult AddNewPlane(FormCollection collection)
        {
            var plane = new PlaneModel
            {
                Airline = collection.Get("airline"),
                SeatMapID = int.Parse(collection.Get("planetype")),
                Status = Constant.CONST_PLANE_STATUS_READY
            };
            var planeID = this.PlaneService.Insert(plane);
            var firstclass = int.Parse(collection.Get("firstclass"));
            var business = int.Parse(collection.Get("business"));
            var economy = int.Parse(collection.Get("economy"));
            var order = 1;

            //for FirstClass
            if (firstclass > 0)
            {
                var planeSeatClass = new PlaneSeatClassModel
                {
                    PlaneID = planeID,
                    SeatClassID = Constant.CONST_DB_SEAT_CLASS_FIRSTCLASS_ID,
                    Capacity = firstclass,
                    Price = double.Parse(collection.Get("firstclassprice")),
                    Order = order++
                };

                this.PlaneSeatClassService.Insert(planeSeatClass);
            }

            //for Business
            if (business > 0)
            {
                var planeSeatClass = new PlaneSeatClassModel
                {
                    PlaneID = planeID,
                    SeatClassID = Constant.CONST_DB_SEAT_CLASS_BUSINESS_ID,
                    Capacity = business,
                    Price = double.Parse(collection.Get("businessprice")),
                    Order = order++
                };

                this.PlaneSeatClassService.Insert(planeSeatClass);
            }

            //for Economy
            if (economy > 0)
            {
                var planeSeatClass = new PlaneSeatClassModel
                {
                    PlaneID = planeID,
                    SeatClassID = Constant.CONST_DB_SEAT_CLASS_ECONOMY_ID,
                    Capacity = economy,
                    Price = double.Parse(collection.Get("economyprice")),
                    Order = order++
                };

                this.PlaneSeatClassService.Insert(planeSeatClass);
            }

            var air = collection.Get("airport").Split(',');

            foreach (var a in air)
            {
                var planeAirport = new PlaneAirportModel
                {
                    PlaneID = planeID,
                    AirportID = int.Parse(a)

                };

                this.PlaneAirportService.Insert(planeAirport);
            }
            TempData["msg"] = "success-Plane add successfully";

            return RedirectToAction("Index");
        }

        //Change Status
        [HttpGet]
        public ActionResult ChangeStatus(string id, string status)
        {
            var currentPlane = this.PlaneService.Find(int.Parse(id));

            if (currentPlane != null)
            {
                if (status.Equals("Ready"))
                    currentPlane.Status = Constant.CONST_PLANE_STATUS_READY;
                else if (status.Equals("Online"))
                    currentPlane.Status = Constant.CONST_PLANE_STATUS_ONLINE;
                else if (status.Equals("Repair"))
                    currentPlane.Status = Constant.CONST_PLANE_STATUS_REPAIR;

                this.PlaneService.Update(currentPlane);
                TempData["msg"] = "success-Plane changed status successfully";
            }

            return RedirectToAction("Index");
        }

        //EDIT PLANE
        [HttpPost]
        public ActionResult UpdatePlane(FormCollection collection)
        {
            var currentPlane = this.PlaneService.Find(int.Parse(collection.Get("planeid")));

            if (currentPlane != null && !currentPlane.Status.Equals(Constant.CONST_PLANE_STATUS_ONLINE))
            {
                if (CheckPlaneTicketBooked(currentPlane.Flights.Select(flight => flight.ID).ToList()))
                {
                    var airport = collection.Get("airport").Split(',').ToList();
                    var airportDB = currentPlane.PlaneAirports.Select(idairport => idairport.AirportID.ToString()).ToList();
                    var airportDelete = airportDB.Where(airportid => airport.IndexOf(airportid) == -1).ToList();
                    var airportAdd = airport.Where(airportid => airportDB.IndexOf(airportid) == -1).ToList();

                    foreach (var id in airportDelete)
                    {
                        var currentAirport = this.PlaneAirportService.Find(currentPlane.ID, int.Parse(id));

                        if (currentAirport.Plane.Flights.Count == 0)
                        {
                            this.PlaneAirportService.Delete(currentAirport.PlaneID, currentAirport.AirportID);
                        }
                        else
                        {
                            TempData["msg"] = "success-Plane changed status successfully";

                            return RedirectToAction("Index");
                        }
                    }

                    foreach (var air in airportAdd)
                    {
                        var planeAirport = new PlaneAirportModel
                        {
                            PlaneID = currentPlane.ID,
                            AirportID = int.Parse(air)

                        };

                        this.PlaneAirportService.Insert(planeAirport);
                    }

                    currentPlane.Airline = collection.Get("airline");
                    currentPlane.SeatMapID = int.Parse(collection.Get("planetype"));
                    this.PlaneService.Update(currentPlane);

                    var firstclass = this.PlaneSeatClassService.Find(currentPlane.ID, Constant.CONST_DB_SEAT_CLASS_FIRSTCLASS_ID);
                    var business = this.PlaneSeatClassService.Find(currentPlane.ID, Constant.CONST_DB_SEAT_CLASS_BUSINESS_ID);
                    var economy = this.PlaneSeatClassService.Find(currentPlane.ID, Constant.CONST_DB_SEAT_CLASS_ECONOMY_ID);

                    UpdatePlaneSeatClass(firstclass, currentPlane.ID, int.Parse(collection.Get("firstclass")), double.Parse(collection.Get("firstclassprice")), Constant.CONST_DB_SEAT_CLASS_FIRSTCLASS_ID);
                    UpdatePlaneSeatClass(business, currentPlane.ID, int.Parse(collection.Get("business")), double.Parse(collection.Get("businessprice")), Constant.CONST_DB_SEAT_CLASS_BUSINESS_ID);
                    UpdatePlaneSeatClass(economy, currentPlane.ID, int.Parse(collection.Get("economy")), double.Parse(collection.Get("economyprice")), Constant.CONST_DB_SEAT_CLASS_ECONOMY_ID);

                }
                else TempData["msg"] = "error-Update plane failed because plane has ticket booked";
            }
            else TempData["msg"] = "error-Update plane failed because plane is online";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DelelePlane(string planeid)
        {
            var currentPlane = this.PlaneService.Find(int.Parse(planeid));

            if (currentPlane.Flights.Count == 0)
            {
                var planeSeatClassList = currentPlane.PlaneSeatClasses.ToList();
                var planeAirportList = currentPlane.PlaneAirports.ToList();

                foreach (var seat in planeSeatClassList)
                    this.PlaneSeatClassService.Delete(currentPlane.ID, seat.SeatClassID);

                foreach (var planeair in planeAirportList)
                    this.PlaneAirportService.Delete(currentPlane.ID, planeair.AirportID);
            }

            if (this.PlaneService.Delete(int.Parse(planeid)) == 1)
                TempData["msg"] = "success-Delete plane successfully";
            else TempData["msg"] = "error-Delete plane failed";

            return new JsonResult { ContentType = "text" };
        }
        #endregion

        #region Private Fuctions
        private bool CheckPlaneTicketBooked(List<int> flightIdList)
        {
            foreach (var id in flightIdList)
            {
                var flightTicket = this.TicketFlightService.FindByFlight(id);

                if (flightTicket == null)
                    return false;
            }

            return true;
        }

        private void UpdatePlaneSeatClass(PlaneSeatClassModel seatClass, int planeId, int capacity, double price, int seatClassID)
        {
            if (seatClass != null)
            {
                seatClass.Capacity = capacity;
                seatClass.Price = price;
                this.PlaneSeatClassService.Update(seatClass);
            }
            else if (capacity > 0)
            {
                var order = this.PlaneSeatClassService.FindByPlane(planeId).Select(planeSeat => planeSeat.Order).Max();

                var planeSeatClass = new PlaneSeatClassModel
                {
                    PlaneID = planeId,
                    SeatClassID = seatClassID,
                    Capacity = capacity,
                    Price = price,
                    Order = (order == null) ? 1 : ++order
                };

                this.PlaneSeatClassService.Insert(planeSeatClass);

            }
        }
        #endregion
    }
}