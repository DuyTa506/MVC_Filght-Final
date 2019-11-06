using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class FlightRepository : BaseRepository, IFlightRepository
    {
        #region Constructors
        public FlightRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<Flight> FindAll()
        {
            var result = this.UnitOfWork.DBContext.Flights;

            return result;
        }

        public int Insert(Flight flight)
        {
            this.UnitOfWork.DBContext.Flights.Add(flight);

            return flight.ID;
        }

        public Flight Find(int id)
        {
            var result = this.UnitOfWork.DBContext.Flights.Find(id);

            return result;
        }

        public IEnumerable<Flight> FindByTicket(int ticketId, bool roundTrip)
        {
            var result = this.UnitOfWork.DBContext.TicketFlights.Where(ticketFlight => 
                ticketFlight.TicketID == ticketId && ticketFlight.RoundTrip == roundTrip).Select(ticketFlight => ticketFlight.Flight);

            return result;
        }
        #endregion
    }
}