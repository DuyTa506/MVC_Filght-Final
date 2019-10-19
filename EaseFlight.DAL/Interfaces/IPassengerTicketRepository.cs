using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IPassengerTicketRepository
    {
        IEnumerable<PassengerTicket> FindAll();
    }
}