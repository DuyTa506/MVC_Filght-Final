using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using EaseFlight.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.BLL.Services
{
    public class SeatMapService : BaseService, ISeatMapService
    {
        #region Properties
        private ISeatMapRepository SeatMapRepository { get; set; }
        private ISeatClassService SeatClassService { get; set; }
        private IPlaneService PlaneService { get; set; }
        private IPlaneSeatClassService PlaneSeatClassService { get; set; }
        private ITicketFlightService TicketFlightService { get; set; }
        #endregion

        #region Constructors
        public SeatMapService(IUnitOfWork unitOfWork,
            ISeatMapRepository seatMapRepository, ISeatClassService seatClassService, IPlaneSeatClassService planeSeatClassService,
            IPlaneService planeService, ITicketFlightService ticketFlightService) : base(unitOfWork)
        {
            this.SeatMapRepository = seatMapRepository;
            this.SeatClassService = seatClassService;
            this.PlaneSeatClassService = planeSeatClassService;
            this.PlaneService = planeService;
            this.TicketFlightService = ticketFlightService;
        }
        #endregion

        #region Functions
        public IEnumerable<SeatMapModel> FindAll()
        {
            var modelList = this.SeatMapRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }
        #endregion

        #region Seat Code Functions
        public List<string> GetFullSeatCode(int planeId, int seatClassId = -1)
        {
            var result = new List<string>();
            var rowWithoutChair = new List<string>();
            var plane = this.PlaneService.Find(planeId);
            var listSeatClass = this.PlaneSeatClassService.FindByPlane(planeId).ToList();
            var seatClassOrder = seatClassId == -1 ? -1 : listSeatClass.Where(seat => seat.SeatClassID == seatClassId).Select(seat => seat.Order).FirstOrDefault();
            var characterColumn = plane.SeatMap.Columns.Split('-');
            var x = plane.SeatMap.RowWithoutSeat != null? plane.SeatMap.RowWithoutSeat.Split('-').ToList(): new List<string>();

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

            var rows = (plane.SeatMap.Capacity.Value + rowWithoutChair.Count) / characterColumn.Length;
            var index = 0;

            //Get Seat Code
            for (var i = 1; i <= rows; ++i)
            {
                foreach (var column in characterColumn)
                {
                    var seat = string.Concat(i, column);
                    if (rowWithoutChair.IndexOf(seat) == -1)
                        result.Add(seat);
                    if (result.Count == listSeatClass[index].Capacity && seatClassOrder != -1)
                    {
                        if (listSeatClass[index].Order == seatClassOrder)
                        {
                            i = rows + 1; break;
                        }
                        else
                        {
                            result.Clear();
                            ++index;
                        }

                    }
                }
            }

            return result;
        }

        public SeatClassModel FindBySeatCode(string seatCode, int planeId)
        {
            var listSeatClass = this.PlaneSeatClassService.FindByPlane(planeId).ToList();
            var data = GetFullSeatCode(planeId);
            var index = data.IndexOf(seatCode);
            var first = 0;

            if (index == -1) return new SeatClassModel();

            for (var i = 0; i < listSeatClass.Count; ++i)
            {
                var last = first + listSeatClass[i].Capacity - 1;

                if (index >= first && index <= last)
                    return SeatClassService.Find(listSeatClass[i].SeatClassID);

                else first = listSeatClass[i].Capacity.Value;
            }

            return new SeatClassModel();
        }

        public List<string> GenerateSeatCodeTicket(int planeId, int seatClassId, int flightId, int range)
        {
            var result = new List<string>();
            var seatCodeList = GetFullSeatCode(planeId, seatClassId);
            var usedSeat = GetAllSeatCodeFlight(flightId);

            for (var i = 0; i < range; ++i)
            {
                while (true)
                {
                    var index = new Random().Next(seatCodeList.Count());

                    if (usedSeat.IndexOf(seatCodeList[index]) == -1 && result.IndexOf(seatCodeList[index]) == -1)
                    {
                        result.Add(seatCodeList[index]);
                        break;
                    }
                };
            }

            return result;
        }

        public List<string> GetAllSeatCodeFlight(int flightId)
        {
            var result = new List<string>();
            var seatCodeList = this.TicketFlightService.FindByFlight(flightId).Where(ticketFlight => 
                !ticketFlight.Ticket.Status.Equals(Constant.CONST_DB_TICKET_STATUS_RETURN)).Select(ticketFlight => ticketFlight.SeatCode);

            foreach (var seatCode in seatCodeList)
                seatCode.Split(',').ToList().ForEach(seat => result.Add(seat));

            return result;
        }
        #endregion

        #region Model Functions
        public SeatMapModel CreateViewModel(SeatMap model)
        {
            SeatMapModel viewModel = null;

            if (model != null)
            {
                viewModel = new SeatMapModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}