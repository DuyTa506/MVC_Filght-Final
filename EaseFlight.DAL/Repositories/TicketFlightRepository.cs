using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class TicketFlightRepository : BaseRepository, ITicketFlightRepository
    {
        #region Constructors
        public TicketFlightRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<TicketFlight> FindAll()
        {
            var result = this.UnitOfWork.DBContext.TicketFlights;

            return result;
        }

        public int Insert(TicketFlight ticketFlight)
        {
            this.UnitOfWork.DBContext.TicketFlights.Add(ticketFlight);

            return ticketFlight.TicketID;
        }

        public IEnumerable<TicketFlight> FindByFlight(int flightId)
        {
            var result = this.UnitOfWork.DBContext.TicketFlights.Where(ticketFlight => ticketFlight.FlightID == flightId);

            return result;
        }

        public IEnumerable<TicketFlight> FindByTicket(int ticketId)
        {
            var result = this.UnitOfWork.DBContext.TicketFlights.Where(ticketFlight => ticketFlight.TicketID == ticketId);

            return result;
        }
        #endregion
    }
}