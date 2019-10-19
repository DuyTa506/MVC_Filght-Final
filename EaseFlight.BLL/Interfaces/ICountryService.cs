using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface ICountryService
    {
        IEnumerable<CountryModel> FindAll();
    }
}