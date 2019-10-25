using EaseFlight.BLL.Interfaces;
using EaseFlight.Models.EntityModels;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.Web.WebUtilities
{
    public class SeatMapUtility
    {
        #region Properties
        private static ISeatClassService SeatClassService { get; set; }
        private static IPlaneService PlaneService { get; set; }
        private static IPlaneSeatClassService PlaneSeatClassService { get; set; }
        #endregion

        #region Constructors
        public SeatMapUtility(ISeatClassService seatClassService, IPlaneSeatClassService planeSeatClassService,
            IPlaneService planeService)
        {
            SeatClassService = seatClassService;
            PlaneSeatClassService = planeSeatClassService;
            PlaneService = planeService;
        }
        #endregion

        #region Functions
        public static List<string> GetSeatCode(int planeId, int seatClassId = -1)
        {
            var result = new List<string>();
            var rowWithoutChair = new List<string>();
            var plane = PlaneService.Find(planeId);
            var listSeatClass = PlaneSeatClassService.FindByPlane(planeId).ToList();
            var seatClassOrder = seatClassId == -1? -1: listSeatClass.Where(seat => seat.SeatClassID == seatClassId).Select(seat => seat.Order).FirstOrDefault();
            var characterColumn = plane.SeatMap.Columns.Split('-');
            var x = plane.SeatMap.RowWithoutSeat.Split('-').ToList();

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

        public static SeatClassModel FindBySeatCode(string seatCode, int planeId)
        {
            var listSeatClass = PlaneSeatClassService.FindByPlane(planeId).ToList();
            var data = GetSeatCode(planeId);
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
        #endregion
    }
}