using EaseFlight.Common.Constants;
using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class FlightRepository : BaseRepository, IFlightRepository
    {
        #region Constructors
        public FlightRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<Flight> FindAll()
        {
            var result = this.UnitOfWork.DBContext.Flights;

            return result;
        }

        public int Insert(Flight flight)
        {
            this.UnitOfWork.DBContext.Flights.Add(flight);
            this.UnitOfWork.SaveChanges();

            return flight.ID;
        }

        public Flight Find(int id)
        {
            var result = this.UnitOfWork.DBContext.Flights.Find(id);

            return result;
        }

        public IEnumerable<Flight> FindByTicket(int ticketId, bool roundTrip)
        {
            var result = this.UnitOfWork.DBContext.TicketFlights.Where(ticketFlight => 
                ticketFlight.TicketID == ticketId && ticketFlight.RoundTrip == roundTrip).Select(ticketFlight => ticketFlight.Flight);

            return result;
        }

        public void Update(Flight flight)
        {
            var currentFlight = this.UnitOfWork.DBContext.Flights.Find(flight.ID);

            if (currentFlight != null)
            {
                CommonMethods.CopyObjectProperties(flight, currentFlight);
            }
        }

        public IEnumerable<Flight> FindByDateAndPlane(int planeId, DateTime departDate)
        {
            var result = this.UnitOfWork.DBContext.Flights.Where(flight => flight.DepartureDate.Value.Year == departDate.Year 
                && flight.DepartureDate.Value.Month == departDate.Month && flight.DepartureDate.Value.Day == departDate.Day 
                && flight.PlaneID == planeId);

            return result;
        }

        public int Delete(int flightId)
        {
            var currentFlight = this.UnitOfWork.DBContext.Flights.Find(flightId);
            var result = 0;

            if (currentFlight.TicketFlights.Count == 0 && !currentFlight.Status.Equals(Constant.CONST_FLIGHT_STATUS_ONLINE))
            {
                this.UnitOfWork.DBContext.Flights.Remove(currentFlight);
                this.UnitOfWork.SaveChanges();
                result = 1;
            }

            if (currentFlight.Status.Equals(Constant.CONST_FLIGHT_STATUS_ONLINE))
                result = -1;

            return result;
        }
        #endregion
    }
}