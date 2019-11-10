using EaseFlight.BLL.Interfaces;
using EaseFlight.BLL.Services;
using EaseFlight.Common.Constants;
using EaseFlight.Models.EntityModels;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class PlaneController : BaseController
    {
        #region Propertities
        private IPlaneService PlaneService { set; get; }
        private ISeatMapService SeatMapService { set; get; }
        private IAirportService AirportService { set; get; }
        private IPlaneSeatClassService PlaneSeatClassService { set; get; }
        private IPlaneAirportService PlaneAirportService { set; get; }
        #endregion

        #region Constructor
        public PlaneController(IPlaneService planeService, ISeatMapService seatMapService, IAirportService airportService, IPlaneSeatClassService planeSeatClassService, IPlaneAirportService planeAirportService)
        {
            this.PlaneService = planeService;
            this.AirportService = airportService;
            this.SeatMapService = seatMapService;
            this.PlaneSeatClassService = planeSeatClassService;
            this.PlaneAirportService = planeAirportService;
        }
        #endregion

        #region Actions
        // GET: Admin/PlaneAirport
        [HttpGet]
        public ActionResult Index()
        {
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

            return RedirectToAction("Index");
        }
        //EDIT PLANE
        [HttpPost]
        public ActionResult EditPlane(FormCollection collection)
        {



            return RedirectToAction("Index");
        }

        #endregion
    }
}