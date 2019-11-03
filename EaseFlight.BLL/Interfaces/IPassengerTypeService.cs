using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IPassengerTypeService
    {
        IEnumerable<PassengerTypeModel> FindAll();
        PassengerTypeModel FindByName(string name);
    }
}