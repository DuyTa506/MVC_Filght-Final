using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface ITicketFlightRepository
    {
        IEnumerable<TicketFlight> FindAll();
    }
}