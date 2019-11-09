using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IAirportService
    {
        IEnumerable<AirportModel> FindAll();
        AirportModel Find(int id);
        int Insert(AirportModel airport);
        void Update(AirportModel airport);
        void Delete(int airportId);
    }
}