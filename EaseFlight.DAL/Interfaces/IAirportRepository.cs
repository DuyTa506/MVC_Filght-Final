using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IAirportRepository
    {
        IEnumerable<Airport> FindAll();
        Airport Find(int id);
    }
}
