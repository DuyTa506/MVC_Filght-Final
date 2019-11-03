using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

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

        public PassengerType FindByName(string name)
        {
            var result = this.UnitOfWork.DBContext.PassengerTypes
                .Where(passengerType => passengerType.Name.Equals(name)).FirstOrDefault();

            return result;
        }
        #endregion
    }
}