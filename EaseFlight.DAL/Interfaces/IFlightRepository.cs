using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IFlightRepository
    {
        IEnumerable<Flight> FindAll();
        int Insert(Flight flight);
    }
}