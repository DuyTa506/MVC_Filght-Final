using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface ITicketService
    {
        IEnumerable<TicketModel> FindAll();
        int Insert(TicketModel ticket);
        TicketModel Find(int id);
        IEnumerable<TicketModel> FindByAccount(int accountId);
        int Update(TicketModel ticket);
    }
}