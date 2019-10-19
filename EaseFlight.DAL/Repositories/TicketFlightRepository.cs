using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

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
        #endregion
    }
}