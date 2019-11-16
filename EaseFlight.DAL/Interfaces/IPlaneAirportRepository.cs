using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IPlaneAirportRepository
    {
        IEnumerable<PlaneAirport> FindAll();
        void Insert(PlaneAirport planeAirport);
        PlaneAirport Find(int planeId, int airportId);
        void Update(PlaneAirport planeAirport);
        IEnumerable<PlaneAirport> FindByPlane(int planeId);
        void Delete(int planeid, int planeAirportId);
    }
}