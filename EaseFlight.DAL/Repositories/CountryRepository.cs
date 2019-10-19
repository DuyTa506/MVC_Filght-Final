using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

namespace EaseFlight.DAL.Repositories
{
    public class CountryRepository : BaseRepository, ICountryRepository
    {
        #region Constructors
        public CountryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<Country> FindAll()
        {
            var result = this.UnitOfWork.DBContext.Countries;

            return result;
        }
        #endregion
    }
}