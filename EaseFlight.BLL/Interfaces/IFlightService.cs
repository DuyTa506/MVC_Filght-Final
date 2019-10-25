using EaseFlight.Models.CustomModel;
using EaseFlight.Models.EntityModels;
using System;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IFlightService
    {
        IEnumerable<FlightModel> FindAll();
        int Insert(FlightModel flight);
        FlightModel Find(int id);
        IEnumerable<SearchFlightModel> FindFlight(AirportModel departure, AirportModel arrival, DateTime departureDate);
    }
}