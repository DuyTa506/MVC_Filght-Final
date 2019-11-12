using EaseFlight.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IFlightRepository
    {
        IEnumerable<Flight> FindAll();
        int Insert(Flight flight);
        Flight Find(int id);
        IEnumerable<Flight> FindByTicket(int ticketId ,bool roundTrip);
        void Update(Flight flight);
        IEnumerable<Flight> FindByDateAndPlane(int planeId, DateTime departDate);
        int Delete(int flightId);
    }
}