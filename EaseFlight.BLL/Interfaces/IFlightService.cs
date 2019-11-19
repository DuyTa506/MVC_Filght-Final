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
        IEnumerable<FlightModel> FindByTicket(int ticketId, bool roundTrip = false);
        int Update(FlightModel flight);
        IEnumerable<FlightModel> FindByDateAndPlane(int planeId, DateTime departDate);
        int Delete(int flightId);
        void UpdateFlightDone();
    }
}