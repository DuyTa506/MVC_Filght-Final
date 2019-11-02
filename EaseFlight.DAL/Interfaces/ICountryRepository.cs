using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface ICountryRepository
    {
        IEnumerable<Country> FindAll();
        int Insert(Country country);
        void Update(Country country);
        void Delete(int countryId);
        Country Find(int id);
    }
}