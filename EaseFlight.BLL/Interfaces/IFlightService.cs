using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IFlightService
    {
        IEnumerable<FlightModel> FindAll();
        int Insert(FlightModel flight);
    }
}