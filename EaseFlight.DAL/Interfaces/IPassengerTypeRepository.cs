using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IPassengerTypeRepository
    {
        IEnumerable<PassengerType> FindAll();
        PassengerType FindByName(string name);
    }
}