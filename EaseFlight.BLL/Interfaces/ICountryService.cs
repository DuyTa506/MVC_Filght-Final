using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface ICountryService
    {
        IEnumerable<CountryModel> FindAll();
        int Insert(CountryModel country);
        void Update(CountryModel country);
        void Delete(int countryId);
        CountryModel Find(int id);
    }
}