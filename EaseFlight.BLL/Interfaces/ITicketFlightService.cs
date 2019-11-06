using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface ITicketFlightService
    {
        IEnumerable<TicketFlightModel> FindAll();
        int Insert(TicketFlightModel ticketFlight);
        IEnumerable<TicketFlightModel> FindByFlight(int flightId);
        IEnumerable<TicketFlightModel> FindByTicket(int ticketId);
    }
}