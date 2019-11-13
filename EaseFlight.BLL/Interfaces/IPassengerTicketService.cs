using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IPassengerTicketService
    {
        IEnumerable<PassengerTicketModel> FindAll();
        int Insert(PassengerTicketModel passengerTicket);
        IEnumerable<PassengerTicketModel> FindByTicket(int ticketId);
        PassengerTicketModel Find(int passengerTicketId);
        int Update(PassengerTicketModel passengerTicket);
    }
}