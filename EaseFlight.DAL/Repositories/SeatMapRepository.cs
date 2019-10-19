using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

namespace EaseFlight.DAL.Repositories
{
    public class SeatMapRepository : BaseRepository, ISeatMapRepository
    {
        #region Constructors
        public SeatMapRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<SeatMap> FindAll()
        {
            var result = this.UnitOfWork.DBContext.SeatMaps;

            return result;
        }
        #endregion
    }
}