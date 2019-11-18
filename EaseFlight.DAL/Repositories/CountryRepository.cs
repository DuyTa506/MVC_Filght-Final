using EaseFlight.Common.Utilities;
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

        public int Insert(Country country)
        {
            this.UnitOfWork.DBContext.Countries.Add(country);

            return country.ID;
        }

        public void Update(Country country)
        {
            var currentCountry = this.UnitOfWork.DBContext.Countries.Find(country.ID);

            if (currentCountry != null)
                CommonMethods.CopyObjectProperties(country,currentCountry);
        }

        public Country Find(int id)
        {
            var country = this.UnitOfWork.DBContext.Countries.Find(id);

            return country;
        }

        public void Delete(int countryId)
        {
            var country = this.UnitOfWork.DBContext.Countries.Find(countryId);

            if (country.Airports.Count == 0)
            {
                this.UnitOfWork.DBContext.Countries.Remove(country);
                this.UnitOfWork.SaveChanges();
            }
        }
        #endregion
    }
}