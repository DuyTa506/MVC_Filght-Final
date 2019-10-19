using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

namespace EaseFlight.DAL.Repositories
{
    public class PassengerTypeRepository : BaseRepository, IPassengerTypeRepository
    {
        #region Constructors
        public PassengerTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<PassengerType> FindAll()
        {
            var result = this.UnitOfWork.DBContext.PassengerTypes;

            return result;
        }
        #endregion
    }
}