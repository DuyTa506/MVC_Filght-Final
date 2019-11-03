using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface ISeatMapService
    {
        IEnumerable<SeatMapModel> FindAll();
        List<string> GetFullSeatCode(int planeId, int seatClassId = -1);
        SeatClassModel FindBySeatCode(string seatCode, int planeId);
        List<string> GenerateSeatCodeTicket(int planeId, int seatClassId, int flightId, int range);
        List<string> GetAllSeatCodeFlight(int flightId);
    }
}