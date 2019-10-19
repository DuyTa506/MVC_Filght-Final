using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IPlaneAirportRepository
    {
        IEnumerable<PlaneAirport> FindAll();
    }
}