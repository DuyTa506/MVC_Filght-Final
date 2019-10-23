using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

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
        #endregion
    }
}