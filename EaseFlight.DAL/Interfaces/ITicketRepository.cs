using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> FindAll();
        int Insert(Ticket ticket);
        Ticket Find(int id);
        IEnumerable<Ticket> FindByAccount(int accountId);
        void Update(Ticket ticket);
    }
}