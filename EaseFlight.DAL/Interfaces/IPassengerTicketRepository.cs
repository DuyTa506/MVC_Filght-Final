using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IPassengerTicketRepository
    {
        IEnumerable<PassengerTicket> FindAll();
        int Insert(PassengerTicket passenger);
        IEnumerable<PassengerTicket> FindByTicket(int ticketId);
        PassengerTicket Find(int passengerTicketId);
        void Update(PassengerTicket passengerTicket);
    }
}