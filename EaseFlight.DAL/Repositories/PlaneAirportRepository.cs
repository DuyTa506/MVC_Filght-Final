using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

namespace EaseFlight.DAL.Repositories
{
    public class PlaneAirportRepository : BaseRepository, IPlaneAirportRepository
    {
        #region Constructors
        public PlaneAirportRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<PlaneAirport> FindAll()
        {
            var result = this.UnitOfWork.DBContext.PlaneAirports;

            return result;
        }
        #endregion
    }
}