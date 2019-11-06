using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface ITicketFlightRepository
    {
        IEnumerable<TicketFlight> FindAll();
        int Insert(TicketFlight ticketFlight);
        IEnumerable<TicketFlight> FindByFlight(int flightId);
        IEnumerable<TicketFlight> FindByTicket(int ticketId);
    }
}