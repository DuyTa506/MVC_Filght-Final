using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

namespace EaseFlight.DAL.Repositories
{
    public class AirportRepository : BaseRepository, IAirportRepository
    {
        #region Constructors
        public AirportRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<Airport> FindAll()
        {
            var result = this.UnitOfWork.DBContext.Airports;

            return result;
        }
        #endregion
    }
}